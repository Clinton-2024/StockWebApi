using StockWebApi.Core.Dtos.Auth;

namespace StockWebApi.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeCashierAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> VerifyEmailAsync(string Token);
        Task<AuthServiceResponseDto> ForgotPasswordAsync(ForgotPasswordDto ForgotPasswordDto);
        Task<AuthServiceResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<AuthServiceResponseDto> ExistEmailAsync(ExistEmailDto data);
    }
}
