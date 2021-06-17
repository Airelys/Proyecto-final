using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Plus.Repository
{
    public class UserRepository<T> : IUserRepository<T> where T : ApplicationUser
    {
        ApplicationDbContext _context;
        DbSet<ApplicationUser> _users;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _users = context.Set<ApplicationUser>();
        }

        public async Task Create(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var result = await userManager.CreateAsync(user, "NewUser123!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, "Basic_User");

            await _context.SaveChangesAsync();

        }

        public ApplicationUser Get(string id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public DbSet<ApplicationUser> GetAll()
        {
            return _users;
        }

        public ApplicationUser GetByEmail(string Email)
        {
            return _users.FirstOrDefault(x => x.Email == Email);
        }

        public void Update(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }
    }
}
