using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Func<string, string> RemoveWhiteSpaces
        {
            get
            {
                return s => string.Concat(s.Where(c => !char.IsWhiteSpace(c))).ToLower();
            }
        }

        DbSet<T> GetAll();
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        bool Exists(int id);
        void Remove(T entity);
        void RemoveRange(ICollection<T> collection);
    }
}
