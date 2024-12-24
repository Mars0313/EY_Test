using EY_Test.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EY_Test.Services
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        public JWTService(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public string GetJWTToken(string name)
        {

            if (!_userService.UserExistsByName(name))
            {
                throw new MyException(400, "未存在資料");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,name),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            // 3. 使用密鑰創建簽名並生成 JWT
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetValue<string>("Issuer"),
                audience: jwtSettings.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.GetValue<int>("ExpiryMinutes")),
                signingCredentials: credentials
            );

            // 4. 返回 JWT Token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
