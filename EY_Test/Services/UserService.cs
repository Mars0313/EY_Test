using EY_Test.Data;
using EY_Test.Entity;
using EY_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace EY_Test.Services
{
    public class UserService
    {
        private readonly EYTestDbContext _context;
        public UserService(EYTestDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new MyException(400, "找不到使用者");
            }
            return user;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            { 
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task UpdateUser(int id, User user)
        {
            if (!UserExistsById(id))
            {
                throw new MyException(400, "找不到使用者id");
            }
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExistsById(user.Id))
                {
                    throw new MyException(400, "找不到使用者id");
                }
                else
                {
                    throw;
                }
            }
            
        }

        public async Task DeleteUser(int id)
        {
            try {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    throw new MyException(400, "找不到欲刪除使用者");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判斷使用者是否存在by id
        /// </summary>
        /// <param name="id">使用者id</param>
        /// <returns>true/false</returns>
        private bool UserExistsById(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        /// <summary>
        /// 判斷使用者是否存在by Name
        /// </summary>
        /// <param name="name">使用者Name</param>
        /// <returns>true/false</returns>
        public bool UserExistsByName(string name)
        {
            return _context.Users.Any(e => e.Name == name);
        }


    }
}
