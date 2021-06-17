using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using Movie_Plus.Services.SuggestionsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class SuggestionService : ISuggestionService
    {
        private IRepository<Suggestion> _suggestionRepository;

        public SuggestionService(IRepository<Suggestion> suggestionRepository)
        {
            _suggestionRepository = suggestionRepository;
        }

        public Suggestion GetAny()
        {
            return _suggestionRepository.GetAll().First();
        }

        public void InsertSuggestion(Suggestion suggestion)
        {
            _suggestionRepository.Insert(suggestion);
        }

        public void UpdateVigentSuggestion(Suggestion suggestion)
        {
            _suggestionRepository.Update(suggestion);
        }

        public IEnumerable<Movie> SuggestionMovies(string suggestionType, DbSet<Movie> AllMovies, DbSet<Buy_Ticket> AllBuyTickets)
        {
            if (suggestionType == _suggestions.MoreViews.ToString())
            {
                var _moviesId = (from Buy in AllBuyTickets
                                 where Buy.PayCompleted == true
                                 group Buy by Buy.Horary.MovieId into g
                                 orderby g.Sum(x => x.NumberOfEntrance) descending
                                 select new { id = g.Key }).Take(10);

                var _movies = from mov in AllMovies
                              join ids in _moviesId on mov.Id equals ids.id
                              select mov;

                return _movies.ToList();
            }

            else if (suggestionType == _suggestions.Random.ToString())
            {
                List<Movie> _moviesList = AllMovies.ToList();
                var _moviesId = new List<int>();

                for (int i = 0; i < 10 && _moviesList.Count != 0; i++)
                {
                    Movie _movie = _moviesList[new Random().Next(_moviesList.Count())];

                    _moviesId.Add(_movie.Id);
                    _moviesList.Remove(_movie);
                }

                var _movies = from mov in AllMovies
                              where _moviesId.Contains(mov.Id)
                              select mov;

                return _movies.ToList();
            }

            else if (suggestionType == _suggestions.PropagandisticAndEconomic.ToString())
            {
                var _moviesId = (from  Mov in AllMovies
                                 where Mov.PropagandisticAndEconomics == true
                                 select new { id = Mov.Id }).Take(10);

                var _movies = from mov in AllMovies
                              join ids in _moviesId on mov.Id equals ids.id
                              select mov;

                return _movies.ToList();
            }
            else if (suggestionType == _suggestions.MostLiked.ToString())
            {
                var _moviesId = (from Buy in AllBuyTickets
                                 join Mov in AllMovies on Buy.Horary.MovieId equals Mov.Id
                                 where Buy.PayCompleted == true
                                 orderby Mov.Ranking descending
                                 select new { id = Mov.Id }).Distinct()
                               .Take(10);

                var _movies = from mov in AllMovies
                              join ids in _moviesId on mov.Id equals ids.id
                              select mov;

                return _movies.ToList();
            }

            return AllMovies.ToList().Take(10);
        }

    }
}
