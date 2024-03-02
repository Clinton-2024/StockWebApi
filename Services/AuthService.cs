using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockWebApi.Core.Dtos.Auth;
using StockWebApi.Core.Dtos.Mail;
using StockWebApi.Core.Entities;
using StockWebApi.Interfaces;
using StockWebApi.OtherObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StockWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly IConfiguration _configuration;
        private readonly IMail_Service _mailService;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger, IConfiguration configuration, IMail_Service mail_Service)
        {
            _userManager = userManager;
            _roleManger = roleManger;
            _configuration = configuration;
            _mailService = mail_Service;
        }

        public async Task<AuthServiceResponseDto> ExistEmailAsync(ExistEmailDto data)
        {
            var isExistEmail = await _userManager.FindByEmailAsync(data.Email);

            if (isExistEmail != null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = true,
                    Message = "User Name already exist"
                };
            }

            return new AuthServiceResponseDto()
            {
                IsSucceed = false,
                Message = "Email not exist"
            };
        }
                

        public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID",Guid.NewGuid().ToString()),
                new Claim("FirstName",user.FirstName),
                new Claim("LastName",user.LastName),
                new Claim("Email",user.Email),

            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = token
            };
        }

        

        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExisUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExisUser != null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Email already Exists"
                };


            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                VerificationToken = CreateRandomToken()
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Beacause: ";

                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }

                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };

            }

            // Add a Default USER Role to all users
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

            // send an email with the account confirmation link
            var requestMail = new MailRequestDto()
            {
                Subject = newUser.FirstName + ", please confirm your Ges-Stock account email address",
                ToEmail = newUser.Email,
                Body = "<h1>Hello, "+ newUser.FirstName + "</h1> </br> <p> Thank you for creating a Gest-Stock account! To confirm your email address, click the link below. </p>" + registerDto.SousDomaine + newUser.VerificationToken+"",
            };

            await _mailService.SendEmailAsync(requestMail);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User Created Successfully"
            };
        }

        
        public async Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManger.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManger.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isCashierRoleExists = await _roleManger.RoleExistsAsync(StaticUserRoles.CASHIER);
            bool isUserRoleExists = await _roleManger.RoleExistsAsync(StaticUserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists && isCashierRoleExists)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = true,
                    Message = "Roles Seeding is Already Done"
                };



            await _roleManger.CreateAsync(new IdentityRole(StaticUserRoles.CASHIER));
            await _roleManger.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManger.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManger.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));


            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Role seeding Done Successfully"
            };
        }

       

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["jwt:validissuer"],
                    audience: _configuration["jwt:validaudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<AuthServiceResponseDto> MakeCashierAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == updatePermissionDto.Email);


            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = true,
                    Message = "Invalid User name !!!!"
                };

            await _userManager.AddToRoleAsync(user, StaticUserRoles.CASHIER);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User is now an CASHIER"
            };
        }
        public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == updatePermissionDto.Email);


            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid User name !!!!"
                };

            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User is now an ADMIN"
            };
        }

        public async Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == updatePermissionDto.Email);

            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = true,
                    Message = "Invalid User name !!!!"
                };

            await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User is now an OWNER"
            };
        }

        public async Task<AuthServiceResponseDto> VerifyEmailAsync(string Token)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.VerificationToken == Token);

            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid token."
                };
            }

            user.VerifiedAt = DateTime.Now;
            user.EmailConfirmed = true;
            user.VerificationToken = string.Empty;
            await _userManager.UpdateAsync(user);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User verified. :)"
            };
        }

        public async Task<AuthServiceResponseDto> ForgotPasswordAsync(ForgotPasswordDto ForgotPasswordDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == ForgotPasswordDto.Email);

            if (user == null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "User not found."
                };
            }

            user.PasswordRestToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _userManager.UpdateAsync(user);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "You may now reset your password."
            };
        }

        public async Task<AuthServiceResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PasswordRestToken == resetPasswordDto.Token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Token"
                };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, resetPasswordDto.Password);
            if (isPasswordCorrect)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "change your password as it is similar to the old one."
                };
            }

            user.PasswordRestToken = string.Empty;
            user.ResetTokenExpires = null;
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!result.Succeeded)
            {
                string messageError = string.Empty;
                foreach (var error in result.Errors)
                {
                    messageError = messageError + "\t" + "#" + error.Code + " : " + error.Description;
                }

                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = messageError
                };
            }

            

            await _userManager.UpdateAsync(user);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Password successfully reset."
            };
        }

       
    }
}
