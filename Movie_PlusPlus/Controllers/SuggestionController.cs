using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie_Plus.Services;
using Microsoft.AspNetCore.Authorization;
using Movie_Plus.Services.SuggestionsTypes;

namespace Movie_PlusPlus.Controllers
{
    public class SuggestionController : Controller
    {
        private ISuggestionService _SuggestionService;
        private IBuyTicketService _BuyTicketService;
        private IMovieService _MovieService;

        public SuggestionController(ISuggestionService SuggestionService, IBuyTicketService BuyTicketService, IMovieService MovieService)
        {
            _SuggestionService = SuggestionService;
            _BuyTicketService = BuyTicketService;
            _MovieService = MovieService;
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            ViewData["Suggestions"] = new SelectList(Enum.GetValues(typeof(_suggestions)).Cast<_suggestions>().ToList());

            return View();
        }

        [Authorize(Roles = "Basic_User,Admin,Manager")]
        public IActionResult Show(string Title)
        {
            if (Title != null)
            {
                try
                {
                    var sugestion = _SuggestionService.GetAny();

                    sugestion.Vigentsuggestion = Title;

                    _SuggestionService.UpdateVigentSuggestion(sugestion);

                }
                catch
                {
                    Suggestion s = new Suggestion() { Vigentsuggestion = Title };
                    _SuggestionService.InsertSuggestion(s);
                }
            }

            else
            {
                try
                {
                    Title = _SuggestionService.GetAny().Vigentsuggestion;
                }
                catch
                {
                    Suggestion s = new Suggestion() { Vigentsuggestion = _suggestions.Random.ToString() };
                    _SuggestionService.InsertSuggestion(s);
                }
            }


            ViewData["Suggestions"] = new SelectList(Enum.GetValues(typeof(_suggestions)).Cast<_suggestions>().ToList());

            return View(_SuggestionService.SuggestionMovies(Title, _MovieService.GetAllMovies(), _BuyTicketService.GetAllBuy_Tickets()));
        }
    }
}
