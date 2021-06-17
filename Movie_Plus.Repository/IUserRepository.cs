using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Plus.Repository
{
    public interface IUserRepository<T> where T: ApplicationUser
    {
        ApplicationUser Get(string id);
        DbSet<ApplicationUser> GetAll();
        ApplicationUser GetByEmail(string Email);
        void Update(ApplicationUser user);
        Task Create(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
