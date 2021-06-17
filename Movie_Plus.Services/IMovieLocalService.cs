using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IMovieLocalService
    {
        DbSet<Movie_Local> GetAllMovie_Locals();
        Movie_Local GetMovie_Local(int id);
        void InsertMovie_Local(Movie_Local movieLocal);
        void UpdateMovie_Local(Movie_Local movieLocal);
        bool ExistsMovie_Local(int id);
        void DeleteMovie_Local(Movie_Local movieLocal);
        bool DuplicateMovieLocal(Movie_Local movieLocal);
    }
}
