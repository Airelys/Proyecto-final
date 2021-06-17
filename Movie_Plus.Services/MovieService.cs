using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Plus.Services
{
    public class MovieService : IMovieService
    {
        private IRepository<Movie> _MovieRepository;

        public MovieService(IRepository<Movie> MovieRepository)
        {
            _MovieRepository = MovieRepository;
        }

        public DbSet<Movie> GetAllMovies()
        {
            return _MovieRepository.GetAll();
        }
        public Movie GetMovie(int id)
        {
            return _MovieRepository.Get(id);
        }

        public void InsertMovie(Movie movie)
        {
            _MovieRepository.Insert(movie);
        }

        public void UpdateMovie(Movie movie)
        {
            _MovieRepository.Update(movie);
        }

        public bool ExistsMovie(int id)
        {
            return _MovieRepository.Exists(id);
        }

        public void DeleteMovie(Movie movie)
        {
            _MovieRepository.Remove(movie);
        }

        public IEnumerable<Movie> Filters(string _title, string _country, string _kindOfMovie, 
                                          int _duration, string _actor, int _ranking, List<Movie> _movies)
        {
            if (!string.IsNullOrEmpty(_title))
            {
                foreach (var item in _title.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _movies = _movies.Where(x => x.Title.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_country))
            {
                foreach (var item in _country.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _movies = _movies.Where(x => x.Country.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_kindOfMovie))
            {
                foreach (var item in _kindOfMovie.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _movies = _movies.Where(x => x.KindOfMovie.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (!string.IsNullOrEmpty(_actor))
            {
                foreach (var item in _actor.Split(new char[] { ' ' },
                         StringSplitOptions.RemoveEmptyEntries))
                {
                    _movies = _movies.Where(x => x.Actors.ToLower().Contains(item.ToLower())).ToList();
                }
            }

            if (_duration != 0)
            {
                _movies = _movies.Where(x => x.Duration <= _duration).ToList();
            }

            if (_ranking != 0)
            {
                _movies = _movies.Where(x => x.Ranking >= _ranking).ToList();
            }

            return _movies;
        }

        public bool DuplicateMovie(Movie movie)
        {
            return  _MovieRepository.GetAll().AsNoTracking().ToList()
                                             .Any(x => _MovieRepository.RemoveWhiteSpaces(x.Title) ==
                                             _MovieRepository.RemoveWhiteSpaces(movie.Title) &&
                                             x.Id != movie.Id);
        }
    }
}
