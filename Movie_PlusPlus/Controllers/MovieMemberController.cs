using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using Movie_Plus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_PlusPlus.Controllers
{
    public class MovieMemberController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;

        private IUserService _UserService;

        public MovieMemberController(IUserService UserService, UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;

            _UserService = UserService;
        }
        public IActionResult Index()
        {
            if (User.IsInRole(Roles.Roles.Ticket_Agent.ToString()))
            {
                ViewData["IsTicketAgent"] = true;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string FirstName, string LastName, long IdentificationNumber, string Email)
        {
            if (User.IsInRole(Roles.Roles.Ticket_Agent.ToString()))
            {
                ViewData["IsTicketAgent"] = true;
            }

            if (FirstName == null || LastName == null)
            {
                ViewData["Error"] = "Write a valid FirstName/LastName";
                return View();
            }

            if (IdentificationNumber.ToString().Length != 11)
            {
                ViewData["Error"] = "Id must have 11 numbers";
                return View();
            }

            if (Email == null && User.IsInRole(Roles.Roles.Ticket_Agent.ToString()))
            {
                ViewData["Error"] = "Insert a valid Email";
                return View();
            }

            else if (Email == null)
            {
                var user = _UserService.GetUser(_UserManager.GetUserId(User));

                if (user.Code != null)
                {
                    TempData["msg"] = "You already are a member of this movie";
                    return RedirectToAction("Index", "Home");
                }
                user.Code = IdentificationNumber;
                user.Puntuation = 0;

                _UserService.UpdateUser(user);

                TempData["msg"] = "Success: You are a member of MoviePlus";
                return RedirectToAction("Index", "Home");
            }
               var userByEmail = _UserService.GetUserByEmail(Email);

                if (userByEmail != null)
                {
                    if (userByEmail.Code != null)
                    {
                        TempData["msg"] = userByEmail.UserName + " is already are a member of this movie";
                        return RedirectToAction("Index", "Home");
                    }
                    userByEmail.Code = IdentificationNumber;
                    userByEmail.Puntuation = 0;

                    _UserService.UpdateUser(userByEmail);

                    TempData["msg"] = "Success: " + userByEmail.UserName + " are a member of MoviePlus";
                    return RedirectToAction("Index", "Home");
                }

                var newUser = new ApplicationUser
                {
                    UserName = Email.Split('@')[0],
                    Email = Email,
                    FirstName = "Add First Name",
                    LastName = "Add Last Name",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Code = IdentificationNumber,
                    Puntuation = 0
                };

                await _UserService.CreateUser(newUser, _UserManager);


                TempData["msg"] = "New user was added to the system with Email: " + Email +
                    " and default password: NewUser123!. Password should be change as soon as posible.";

                return RedirectToAction("Index", "Home");
            

        }
    }
}
