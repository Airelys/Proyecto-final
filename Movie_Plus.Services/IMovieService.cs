using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Plus.Services
{
    public interface IMovieService
    {
        DbSet<Movie> GetAllMovies();
        Movie GetMovie(int id);
        void InsertMovie(Movie movie);
        void UpdateMovie(Movie movie);
        bool ExistsMovie(int id);
        void DeleteMovie(Movie movie);
        IEnumerable<Movie> Filters(string _title, string _country, string _kindOfMovie, int _duration,
                                                  string _actor, int _ranking, List<Movie> _movies);
       bool DuplicateMovie(Movie movie);
    }
}
