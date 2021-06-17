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
    public class UserTypesController : Controller
    {
        IUserTypesService _UserTypesService;

        public UserTypesController(IUserTypesService UserTypesService)
        {
            _UserTypesService = UserTypesService;
        }

        // GET: UserTypes
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            return View(_UserTypesService.GetAllUserTypes().ToList());
        }

        // GET: UserTypes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = _UserTypesService.GetUserType(id.Value);

            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // GET: UserTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Type,Discount,Id")] UserType userType)
        {
            if (_UserTypesService.DuplicateUserType(userType))
                ModelState.AddModelError("", "There is already a UserType with the same type");

            if (ModelState.IsValid)
            {
                _UserTypesService.InsertUserType(userType);

                return RedirectToAction(nameof(Index));
            }

            return View(userType);
        }

        // GET: UserTypes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = _UserTypesService.GetUserType(id.Value);

            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // POST: UserTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Type,Discount,Id")] UserType userType)
        {
            if (id != userType.Id)
            {
                return NotFound();
            }

            if (_UserTypesService.DuplicateUserType(userType))
                ModelState.AddModelError("", "There is already a UserType with the same type");

            if (ModelState.IsValid)
            {
                try
                {
                    _UserTypesService.UpdateUserTypes(userType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_UserTypesService.ExistsUserType(userType.Id))
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

            return View(userType);
        }

        // GET: UserTypes/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = _UserTypesService.GetUserType(id.Value);

            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // POST: UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userType = _UserTypesService.GetUserType(id);

            _UserTypesService.DeleteUserType(userType);

            return RedirectToAction(nameof(Index));
        }
    }
}
