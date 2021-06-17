using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using Movie_Plus.Services;
using Movie_Plus.Services.Estadistic;
using Rotativa.AspNetCore;

namespace Movie_PlusPlus.Controllers
{
    public class EstadisticsController : Controller
    {
        IBuyTicketService _BuyTicketService;
        IEstadisticService _EstadisticService;
        IMovieService _MovieService;
        IPagerService<EstadisticViewModel> _PagerService;

        private static List<EstadisticViewModel> _estadistics;

        public EstadisticsController(IBuyTicketService BuyTicketService, IEstadisticService EstadisticService, 
                                     IMovieService MovieService, IPagerService<EstadisticViewModel> PagerService)
        {
            _BuyTicketService = BuyTicketService;
            _EstadisticService = EstadisticService;
            _MovieService = MovieService;
            _PagerService = PagerService;
        }

        // GET: Estadistics
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index(string _title, string _country, string _kindOfMovie, int _duration,
                                               DateTime _minDate, DateTime _maxDate, string _actor, int _ranking, int page = 1)
        {
            _estadistics = new List<EstadisticViewModel>();

            var _movieIds = _EstadisticService.MovieIds(_minDate, _maxDate, _BuyTicketService.GetAllBuy_Tickets());


            foreach (var item in _movieIds)
            {
                var _movie = _MovieService.GetMovie(item.Item1);

                EstadisticViewModel e = new EstadisticViewModel()
                {
                    Id = _movie.Id,
                    Title = _movie.Title,
                    Duration = _movie.Duration,
                    KindOfMovie = _movie.KindOfMovie,
                    Country = _movie.Country,
                    Actors = _movie.Actors,
                    Ranking = _movie.Ranking,
                    TotalOfEntrance = item.Item2
                };
                _estadistics.Add(e);
            }
            _estadistics = _EstadisticService.Filters(_title, _country, _kindOfMovie, _duration, _actor, _ranking, _estadistics).ToList();

            return View(_PagerService.GetPager(_estadistics, page));

        }

        public IActionResult ViewPDF()
        {
            return View(_estadistics);
        }

        public IActionResult PDF()
        {
            return new ViewAsPdf("ViewPDF", _estadistics)
            {
                FileName = "Estadistic.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
    }
}
