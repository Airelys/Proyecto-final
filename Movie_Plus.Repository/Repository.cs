using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _entity;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }
        public DbSet<T> GetAll()
        {
            return _entity;
        }

        public T Get(int id)
        {
            return _entity.FirstOrDefault(e => e.Id == id);
        }

        public void Insert(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _entity.Any(e => e.Id == id);
        }

        public void Remove(T entity)
        {
            _entity.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(ICollection<T> collection)
        {
            _context.RemoveRange(collection);
            _context.SaveChanges();
        }
    }
}
