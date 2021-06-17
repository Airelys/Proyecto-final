using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class MovieLocalService : IMovieLocalService
    {
        private IRepository<Movie_Local> _MovieLocalRepository;

        public MovieLocalService(IRepository<Movie_Local> MovieLocalRepository)
        {
            _MovieLocalRepository = MovieLocalRepository;
        }

        public void DeleteMovie_Local(Movie_Local movieLocal)
        {
            _MovieLocalRepository.Remove(movieLocal);
        }

        public bool DuplicateMovieLocal(Movie_Local movieLocal)
        {
            return _MovieLocalRepository.GetAll().AsNoTracking().ToList()
                                        .Any(x => _MovieLocalRepository.RemoveWhiteSpaces(x.Local_Name) ==
                                                  _MovieLocalRepository.RemoveWhiteSpaces(movieLocal.Local_Name) &&
                                                  x.Id != movieLocal.Id);
        }

        public bool ExistsMovie_Local(int id)
        {
            return _MovieLocalRepository.Exists(id);
        }

        public DbSet<Movie_Local> GetAllMovie_Locals()
        {
            return _MovieLocalRepository.GetAll();
        }

        public Movie_Local GetMovie_Local(int id)
        {
            return _MovieLocalRepository.Get(id);
        }

        public void InsertMovie_Local(Movie_Local movieLocal)
        {
            _MovieLocalRepository.Insert(movieLocal);
        }

        public void UpdateMovie_Local(Movie_Local movieLocal)
        {
            _MovieLocalRepository.Update(movieLocal);
        }
    }
}
