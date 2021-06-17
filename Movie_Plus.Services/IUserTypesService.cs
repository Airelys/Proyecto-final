using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IUserTypesService
    {
        DbSet<UserType> GetAllUserTypes();
        UserType GetUserType(int id);
        void InsertUserType(UserType userType);
        void UpdateUserTypes(UserType userType);
        bool ExistsUserType(int id);
        void DeleteUserType(UserType userType);
        bool DuplicateUserType(UserType userType);
    }
}
