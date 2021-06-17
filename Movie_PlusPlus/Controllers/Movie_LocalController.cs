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
    public class Movie_LocalController : Controller
    {
        IMovieLocalService _MovieLocalService;
        public Movie_LocalController(IMovieLocalService MovieLocalService)
        {
            _MovieLocalService = MovieLocalService;
        }

        // GET: Movie_Local
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            return View(_MovieLocalService.GetAllMovie_Locals().ToList());
        }

        // GET: Movie_Local/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Local = _MovieLocalService.GetMovie_Local(id.Value);

            if (movie_Local == null)
            {
                return NotFound();
            }

            return View(movie_Local);
        }

        // GET: Movie_Local/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie_Local/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Local_Name,Rows,Columns,Id")] Movie_Local movie_Local)
        {
            if (ModelState.IsValid && _MovieLocalService.DuplicateMovieLocal(movie_Local))
                ModelState.AddModelError("", "There is already a Movie Local with the same name.");

            if (ModelState.IsValid)
            {
                _MovieLocalService.InsertMovie_Local(movie_Local);

                return RedirectToAction(nameof(Index));
            }
            return View(movie_Local);
        }

        // GET: Movie_Local/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Local = _MovieLocalService.GetMovie_Local(id.Value);

            if (movie_Local == null)
            {
                return NotFound();
            }
            return View(movie_Local);
        }

        // POST: Movie_Local/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Local_Name,Rows,Columns,Id")] Movie_Local movie_Local)
        {
            if (id != movie_Local.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && _MovieLocalService.DuplicateMovieLocal(movie_Local))
                ModelState.AddModelError("", "There is already a Movie Local with the same name.");

            if (ModelState.IsValid)
            {
                try
                {
                    _MovieLocalService.UpdateMovie_Local(movie_Local);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_MovieLocalService.ExistsMovie_Local(movie_Local.Id))
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
            return View(movie_Local);
        }

        // GET: Movie_Local/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Local = _MovieLocalService.GetMovie_Local(id.Value);

            if (movie_Local == null)
            {
                return NotFound();
            }

            return View(movie_Local);
        }

        // POST: Movie_Local/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie_Local = _MovieLocalService.GetMovie_Local(id);

            _MovieLocalService.DeleteMovie_Local(movie_Local);

            return RedirectToAction(nameof(Index));
        }

    }
}
