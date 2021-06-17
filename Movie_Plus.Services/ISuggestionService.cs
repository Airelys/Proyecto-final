using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface ISuggestionService
    {
        Suggestion GetAny();
        void UpdateVigentSuggestion(Suggestion suggestion);
        void InsertSuggestion(Suggestion suggestion);
        IEnumerable<Movie> SuggestionMovies(string suggestionType, DbSet<Movie> AllMovies, DbSet<Buy_Ticket> AllBuyTickets);
    }
}
