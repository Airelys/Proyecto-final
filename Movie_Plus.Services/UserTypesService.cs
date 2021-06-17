using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class UserTypesService : IUserTypesService
    {
        private IRepository<UserType> _UserTypeRepository;

        public UserTypesService(IRepository<UserType> UserTypeRepository)
        {
            _UserTypeRepository = UserTypeRepository;
        }

        public void DeleteUserType(UserType userType)
        {
            _UserTypeRepository.Remove(userType);
        }

        public bool DuplicateUserType(UserType userType)
        {
            return _UserTypeRepository.GetAll().AsNoTracking().ToList()
                                      .Any(x => _UserTypeRepository.RemoveWhiteSpaces(x.Type) == 
                                                _UserTypeRepository.RemoveWhiteSpaces(userType.Type) &&
                                                x.Id != userType.Id);
        }

        public bool ExistsUserType(int id)
        {
            return _UserTypeRepository.Exists(id);
        }

        public DbSet<UserType> GetAllUserTypes()
        {
            return _UserTypeRepository.GetAll();
        }

        public UserType GetUserType(int id)
        {
            return _UserTypeRepository.Get(id);
        }

        public void InsertUserType(UserType userType)
        {
            _UserTypeRepository.Insert(userType);
        }

        public void UpdateUserTypes(UserType userType)
        {
            _UserTypeRepository.Update(userType);
        }
    }
}
