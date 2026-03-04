using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace LeadForge.Application.Interfaces;

public interface IAuthService
{
   Task<AuthResponse> LoginAsync(LoginRequest request);
   Task<AuthResponse> RefreshAsync(RefreshRequest request);
   Task<AuthResponse> RegisterAsync(RegisterRequest request);
}