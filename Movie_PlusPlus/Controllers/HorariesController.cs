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

namespace Movie_PlusPlus.Controllers
{
    public class HorariesController : Controller
    {
        IHoraryService _HoraryService;
        IMovieService _MovieService;
        IMovieLocalService _MovieLocalService;
        IPagerService<Horary> _PagerService;

        public HorariesController(IHoraryService HoraryService, IMovieService MovieService,
                                  IMovieLocalService MovieLocalService, IPagerService<Horary> PagerService)
        {
            _HoraryService = HoraryService;
            _MovieService = MovieService;
            _MovieLocalService = MovieLocalService;
            _PagerService = PagerService;
        }

        // GET: Horaries
        public IActionResult Index(int id, string _title, string _localName, DateTime _minDate, DateTime _maxDate,
                                   int _price, int _priceInPoints, int page = 1)
        {
            var horaries = _HoraryService.GetAllHoraries()
                .Include(h => h.Movie)
                .Include(h => h.Movie_Local).ToList();

            if (id != 0)
                horaries = horaries.Where(h => h.MovieId == id).ToList();

            horaries = _HoraryService.Filters(_title, _localName, _minDate, _maxDate, _price, _priceInPoints, horaries).ToList();

            return View(_PagerService.GetPager(horaries, page));

        }

        // GET: Horaries/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horary = _HoraryService.GetAllHoraries()
                .Include(h => h.Movie)
                .Include(h => h.Movie_Local)
                .FirstOrDefault(m => m.Id == id);

            if (horary == null)
            {
                return NotFound();
            }

            return View(horary);
        }

        // GET: Horaries/Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            ViewData["Movie"] = new SelectList(_MovieService.GetAllMovies(), "Title", "Title");
            ViewData["Movie_Local"] = new SelectList(_MovieLocalService.GetAllMovie_Locals(), "Local_Name", "Local_Name");
            return View();
        }

        // POST: Horaries/Create
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string Movie, string Movie_Local, DateTime Date, DateTime Time, int Price,
            int PriceInPoints, int PointsForBuying, int Id)
        {
            int MovieId = _MovieService.GetAllMovies().FirstOrDefault(x => x.Title == Movie).Id;
            int Movie_LocalId = _MovieLocalService.GetAllMovie_Locals().FirstOrDefault(x => x.Local_Name == Movie_Local).Id;


            if(Date.AddSeconds(Time.TimeOfDay.TotalSeconds) < DateTime.Now.AddHours(2))
                ModelState.AddModelError("", "Not valid Date.");

            Horary horary = new Horary()
            {
                MovieId = MovieId,
                Movie_LocalId = Movie_LocalId,
                Date = Date,
                Time = Time,
                Price = Price,
                PriceInPoints = PriceInPoints,
                PointsForBuying = PointsForBuying
            };

            if (ModelState.IsValid && _HoraryService.DuplicateHorary(horary))
                ModelState.AddModelError("", "There is already an horary with the same data.");


                if (ModelState.IsValid)
            {
                _HoraryService.InsertHorary(horary);

                return RedirectToAction(nameof(Index));
            }
            ViewData["Movie"] = new SelectList(_MovieService.GetAllMovies(), "Title", "Title", horary.MovieId);
            ViewData["Movie_Local"] = new SelectList(_MovieLocalService.GetAllMovie_Locals(), "Local_Name", "Local_Name", horary.Movie_LocalId);
            return View(horary);
        }

        // GET: Horaries/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horary = _HoraryService.GetHorary(id.Value);

            if (horary == null)
            {
                return NotFound();
            }

            ViewData["Movie"] = new SelectList(_MovieService.GetAllMovies(), "Title", "Title", horary.MovieId);
            ViewData["Movie_Local"] = new SelectList(_MovieLocalService.GetAllMovie_Locals(), "Local_Name", "Local_Name", horary.Movie_LocalId);

            return View(horary);
        }

        // POST: Horaries/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string Movie, string Movie_Local, DateTime Date, DateTime Time, int Price,
                                  int PriceInPoints, int PointsForBuying)
        {
            var horary = _HoraryService.GetHorary(id);

            int MovieId = _MovieService.GetAllMovies().FirstOrDefault(x => x.Title == Movie).Id;
            int Movie_LocalId = _MovieLocalService.GetAllMovie_Locals().FirstOrDefault(x => x.Local_Name == Movie_Local).Id;

            if (Date.AddSeconds(Time.TimeOfDay.TotalSeconds) < DateTime.Now.AddHours(2))
                ModelState.AddModelError("", "Not valid Date.");

            horary.MovieId = MovieId;
            horary.Movie_LocalId = Movie_LocalId;
            horary.Date = Date;
            horary.Time = Time;
            horary.Price = Price;
            horary.PriceInPoints = PriceInPoints;
            horary.PointsForBuying = PointsForBuying;

            if (ModelState.IsValid && _HoraryService.DuplicateHorary(horary))
                ModelState.AddModelError("", "There is already an horary with the same data.");

            if (ModelState.IsValid)
            {
                try
                {
                    _HoraryService.UpdateHorary(horary);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_HoraryService.ExistsHorary(horary.Id))
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


            ViewData["Movie"] = new SelectList(_MovieService.GetAllMovies(), "Title", "Title", horary.MovieId);
            ViewData["Movie_Local"] = new SelectList(_MovieLocalService.GetAllMovie_Locals(), "Local_Name", "Local_Name", horary.Movie_LocalId);

            return View(horary);
        }

        // GET: Horaries/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horary = _HoraryService.GetAllHoraries()
                .Include(h => h.Movie)
                .Include(h => h.Movie_Local)
                .FirstOrDefault(m => m.Id == id);

            if (horary == null)
            {
                return NotFound();
            }

            return View(horary);
        }

        // POST: Horaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var horary = _HoraryService.GetHorary(id);

            _HoraryService.DeleteHorary(horary);

            return RedirectToAction(nameof(Index));
        }

    }
}
