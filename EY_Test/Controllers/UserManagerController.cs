using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EY_Test.Data;
using EY_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using EY_Test.Entity;
using EY_Test.Services;

namespace EY_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly EYTestDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly JWTService _JWTService;

        public UserManagerController(EYTestDbContext context, IConfiguration configuration, UserService userService, JWTService jwtService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
            _JWTService = jwtService;
        }
        /// <summary>
        /// 取得JWT Token
        /// </summary>
        /// <param name="email">使用者信箱</param>
        /// <returns>JWT token</returns>
        [HttpGet("GetJWTToken")]
        public IActionResult GetJWTToken(string name)
        {
            try
            {
                var token = _JWTService.GetJWTToken(name);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得使用者
        /// </summary>
        /// <returns>使用者List</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userService.GetUsers();
            //return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// 取出特定使用者
        /// </summary>
        /// <param name="id">使用者id</param>
        /// <returns>使用者資料</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                User user = await _userService.GetUser(id);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 建立使用者
        /// </summary>
        /// <param name="user">資料</param>
        /// <returns>user資料</returns>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            try
            {
                User resultUser = await _userService.CreateUser(user);
                return CreatedAtAction(nameof(GetUser), new { id = resultUser.Id }, resultUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 修改使用者資料
        /// </summary>
        /// <param name="id">userid</param>
        /// <param name="user">使用者資料</param>
        /// <returns>Object User</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            try
            {
                await _userService.UpdateUser(id, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NoContent();
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="id">使用者id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try {
                await _userService.DeleteUser(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NoContent();
        }


    }
}
