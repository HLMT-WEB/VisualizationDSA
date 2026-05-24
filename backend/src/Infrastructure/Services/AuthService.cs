using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;
using VisualizationDSA.Domain.Entities;
using VisualizationDSA.Domain.Exceptions;
using VisualizationDSA.Domain.Interfaces;

namespace VisualizationDSA.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);
            if (existingUsers.Any())
            {
                throw new ConflictException("Email này đã được đăng ký.");
            }

            var existingUsernames = await _unitOfWork.Users.FindAsync(u => u.Username == request.Username);
            if (existingUsernames.Any())
            {
                throw new ConflictException("Username này đã được sử dụng.");
            }

            var passwordHash = HashPassword(request.Password);
            var user = new User(request.Email, request.Username, passwordHash);
            await _unitOfWork.Users.AddAsync(user);

            var refreshToken = GenerateRefreshToken();
            user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(30));

            await _unitOfWork.CommitAsync();

            var accessToken = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                User = MapToUserDto(user)
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);
            var user = users.FirstOrDefault();

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new AuthenticationException("Email hoặc mật khẩu không đúng.");
            }

            user.RecordLogin();

            var refreshToken = GenerateRefreshToken();
            user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(30));

            await _unitOfWork.CommitAsync();

            var accessToken = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                User = MapToUserDto(user)
            };
        }

        public async Task<UserDto> GetCurrentUserAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User", userId);
            }

            return MapToUserDto(user);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.RefreshToken == refreshToken);
            var user = users.FirstOrDefault();

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new AuthenticationException("Refresh token không hợp lệ hoặc đã hết hạn.");
            }

            var newRefreshToken = GenerateRefreshToken();
            user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(30));
            await _unitOfWork.CommitAsync();

            var accessToken = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = accessToken,
                RefreshToken = newRefreshToken,
                User = MapToUserDto(user)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("level", user.CurrentLevel.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                TotalXP = user.TotalXP,
                CurrentLevel = user.CurrentLevel,
                StreakDays = user.StreakDays,
                CreatedAt = user.CreatedAt,
                Badges = new List<BadgeDto>()
            };
        }
    }
}
