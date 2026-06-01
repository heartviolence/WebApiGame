using Assets.Scripts.Shared.GameDatas;
using MessagePack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleWebApi.Service.Users;
using SampleWebApi.UserHealthPings;
using ServerShared.Shards;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        IConfiguration _configuration;
        ILogger _logger;
        UserRepository _repository;
        UserHealthPing _users;
        public LoginController(IConfiguration configuration, UserRepository UserRepository, ILogger<LoginController> logger, UserHealthPing users)
        {
            this._configuration = configuration;
            this._repository = UserRepository;
            this._logger = logger;
            this._users = users;
        }

        [HttpPost]
        public async Task<RegisterResponse> Register([FromBody] LoginRequest request)
        {
            if (await _repository.RegisterNewUser(request.Username, request.Password))
            {
                return new RegisterResponse() { IsSuccess = true };
            }
            return new RegisterResponse() { IsSuccess = false, Message = "유저가 이미존재함" };
        }

        [HttpPost]
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
        {
            var checkResult = await _repository.GetUserIdFromUsername(request.Username);
            if (checkResult.isExist)
            {
                await _repository.SnapShotData(checkResult.userId);
                await _repository.GrantItemToMailBox(checkResult.userId);
                _logger.LogInformation("유저 로그인성공, username: {Username},userId {UserId}", request.Username, checkResult.userId);
                return new LoginResponse() { IsSuccess = true, UserId = checkResult.userId, Token = LoginToken(request, checkResult.userId) };
            }

            _logger.LogInformation("유저 로그인실패, username: {Username}", request.Username);
            return new LoginResponse() { IsSuccess = false };
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> HealthPing()
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogInformation("토큰에서 Username을 찾지못함");
                return false;
            }

            if (_users.Lives.TryGetValue(username, out _))
            {
                _users.Lives[username] = DateTime.Now + TimeSpan.FromSeconds(30);
                return true;
            }

            return false;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IResult LoginTest()
        {
            return Results.Ok();
        }

        string LoginToken(LoginRequest request, int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim("userId", userId.ToString()),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
    }
}
