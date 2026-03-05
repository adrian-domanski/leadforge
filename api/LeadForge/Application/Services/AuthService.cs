using System.Security.Authentication;
using LeadForge.Application.Interfaces;
using LeadForge.Domain;
using LeadForge.Domain.Exceptions;
using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _db;

    public AuthService(ITokenService tokenService, AppDbContext db)
    {
        _tokenService = tokenService;
        _db = db;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var accessToken = _tokenService.GenerateAccessToken(user);

        var refreshToken = await CreateRefreshToken(user.Id);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest request)
    {
        var hashed = _tokenService.HashRefreshToken(request.RefreshToken);

        var existingToken = await _db.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x =>
            x.TokenHash == hashed);

        if (existingToken == null || existingToken.IsRevoked ||
            existingToken.ExpiresAt < DateTime.UtcNow)
            throw new InvalidRefreshTokenException();

        // Token rotation
        existingToken.IsRevoked = true;

        var newAccessToken = _tokenService.GenerateAccessToken(existingToken.User);

        var refreshToken = await CreateRefreshToken(existingToken.UserId);

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _db.Users
            .AnyAsync(x => x.Email == request.Email);

        if (existingUser)
            throw new AlreadyExistsException("User already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            Credits = 10
        };

        _db.Users.Add(user);

        var accessToken = _tokenService.GenerateAccessToken(user);

        var refreshToken = await CreateRefreshToken(user.Id);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<MeResponse> GetCurrentUserAsync()
    {

    }

    private async Task<string> CreateRefreshToken(Guid userId)
    {
        var refreshTokenPlain = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _tokenService.HashRefreshToken(refreshTokenPlain);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = refreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        return refreshTokenPlain;
    }
}