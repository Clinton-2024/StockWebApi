using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockWebApi.Core.Dtos.Auth;
using StockWebApi.Interfaces;

namespace StockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Route For Seeding my roles to DB
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> seedRoles()
        {
            var seedRoles = await _authService.SeedRolesAsync();
            if(seedRoles.IsSucceed)
            {
                return Ok(seedRoles);
            }
            return BadRequest(seedRoles);

        }

        // Route Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            var registerResult = await _authService.RegisterAsync(registerDto);

            if (registerResult.IsSucceed)
            {
                return Ok(registerResult);

            }
            return BadRequest(registerResult);
        }


        // Route Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.LoginAsync(loginDto);

            if (loginResult.IsSucceed)
            {
                return Ok(loginResult);

            }

            return Unauthorized(loginResult);
        }



        // Route -> make user -> admin
        [HttpPost]
        [Route("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeAdminAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
            {
                return Ok(operationResult);

            }

            return BadRequest(operationResult);
        }

        // Route -> make user -> cashier
        [HttpPost]
        [Route("make-cashier")]
        public async Task<IActionResult> MakeCashier([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeCashierAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
            {
                return Ok(operationResult);

            }

            return BadRequest(operationResult);
        }

        // Route -> make -> owner
        [HttpPost]
        [Route("make-owner")]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeOwnerAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
            {
                return Ok(operationResult);

            }

            return BadRequest(operationResult);
        }

        [HttpPost]
        [Route("Verify/{token}")]
        public async Task<IActionResult> Verify([FromRoute] string token)
        {
            var result = await _authService.VerifyEmailAsync(token);
            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);

            if (!result.IsSucceed)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await _authService.ResetPassword(resetPasswordDto);

            if (result.IsSucceed == false)
            {
                return BadRequest(result.Message);

            }

            return Ok(result.Message);

        }

        [HttpPost]
        [Route("EmailExist")]
        public async Task<IActionResult> ExistEmail([FromBody] ExistEmailDto data)
        {
            var Result = await _authService.ExistEmailAsync(data);
            if(Result.IsSucceed == false)
            {
                return BadRequest(Result);

            }
            return Ok(Result);
        }
    }
}
