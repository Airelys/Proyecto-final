using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Plus.Services
{
    public interface IUserService
    {
        ApplicationUser GetUser(string id);
        DbSet<ApplicationUser> GetAllUsers();
        ApplicationUser GetUserByEmail(string Email);
        void UpdateUser(ApplicationUser user);
        Task CreateUser(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
