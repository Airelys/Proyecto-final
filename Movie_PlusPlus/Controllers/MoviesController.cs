using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using Movie_Plus.Services;

namespace Movie_PlusPlus.Controllers
{
    public class MoviesController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;

        IMovieService _MovieService;
        IUserService _UserService;
        IBuyTicketService _BuyTicketService;
        IPagerService<Movie> _PagerService;

        private static int MovieId;
        public MoviesController(IMovieService MovieService, IUserService UserService, 
                                IBuyTicketService BuyTicketService, IPagerService<Movie> PagerService,
                                UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;

            _MovieService = MovieService;
            _UserService = UserService;
            _BuyTicketService = BuyTicketService;
            _PagerService = PagerService;
        }

        // GET: Movies
        public IActionResult Index(string _title, string _country, string _kindOfMovie, int _duration,
                                                                         string _actor, int _ranking, int page = 1)
        {
            var _movies = _MovieService.Filters(_title, _country, _kindOfMovie, _duration,
                                                _actor, _ranking, _MovieService.GetAllMovies().ToList());

            return View(_PagerService.GetPager(_movies.ToList(), page));
        }

        // GET: Movies/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _MovieService.GetMovie(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Duration,KindOfMovie,Country,Actors,Ranking,PropagandisticAndEconomics,Id")] Movie movie)
        {
            if (ModelState.IsValid && _MovieService.DuplicateMovie(movie))
                ModelState.AddModelError("", "There already a movie with the same Title.");

            if (ModelState.IsValid)
            {
                _MovieService.InsertMovie(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _MovieService.GetMovie(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Title,Duration,KindOfMovie,Country,Actors,Ranking,PropagandisticAndEconomics,Id")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && _MovieService.DuplicateMovie(movie))
                ModelState.AddModelError("", "There already a movie with the same Title.");

            if (ModelState.IsValid)
            {
                try
                {
                    _MovieService.UpdateMovie(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_MovieService.ExistsMovie(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _MovieService.GetMovie(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [Authorize(Roles = "Basic_User,Admin,Manager")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _MovieService.GetMovie(id);
            _MovieService.DeleteMovie(movie);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Basic_User,Admin,Manager")]
        public IActionResult RateMovie(int id)
        {
            MovieId = id;

            var user = _UserService.GetUser(_UserManager.GetUserId(User));

            var userBuys = _BuyTicketService.GetAllBuy_Tickets()
                .Include(u => u.Horary)
                .Include(u => u.Horary.Movie)
                .Where(m => m.ApplicationUserId == user.Id && 
                                m.Horary.MovieId == id && 
                                m.Horary.Date < DateTime.Now &&
                                m.PayCompleted == true);

            if(userBuys.Count() == 0)
            {
                TempData["Error"] = "You must see this movie in MoviePlus before rate it";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Numbers"] = new SelectList(new List<string> {"1", "2", "3", "4", "5", });
            ViewData["Movie"] = (_MovieService.GetMovie(MovieId)).Title;

            return View();
        }


        [Authorize(Roles = "Basic_User,Admin,Manager")]
        [HttpPost]
        public IActionResult RateMovie(string _rank) 
        {
            var movie = _MovieService.GetMovie(MovieId);

            movie.Ranking = (movie.Ranking + int.Parse(_rank)) / 2;
            _MovieService.UpdateMovie(movie);
            
            TempData["Success"] = "Thank you for rating";
            return RedirectToAction(nameof(Index));
        }
    }
}
