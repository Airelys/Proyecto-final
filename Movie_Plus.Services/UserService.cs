using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Plus.Services
{
    public class UserService : IUserService
    {
        private IUserRepository<ApplicationUser> _UserRepository;

        public UserService(IUserRepository<ApplicationUser> UserRepository)
        {
            _UserRepository = UserRepository;
        }

        public async Task CreateUser(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
           await  _UserRepository.Create(user, userManager);
        }

        public DbSet<ApplicationUser> GetAllUsers()
        {
            return _UserRepository.GetAll();
        }

        public ApplicationUser GetUser(string id)
        {
            return _UserRepository.Get(id);
        }

        public ApplicationUser GetUserByEmail(string Email)
        {
           return _UserRepository.GetByEmail(Email);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _UserRepository.Update(user);
        }
    }
}
