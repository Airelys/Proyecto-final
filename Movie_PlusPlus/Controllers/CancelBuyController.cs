using Microsoft.AspNetCore.Authorization;
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
    public class CancelBuyController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;

        IUserService _UserService;
        IBuyTicketService _BuyTicketService;
        ICancelBuyService _CancelBuyService;
        IHoraryService _HoraryService;
        IReservedSeatsService _ReservedSeatsService;
        ICreditCardService _CreditCardService;
        IPagerService<Buy_Ticket> _PagerService;

        public CancelBuyController(IUserService UserService, IBuyTicketService BuyTicketService, 
                                   ICancelBuyService CancelBuyService, IHoraryService HoraryService,
                                   IReservedSeatsService ReservedSeatsService, ICreditCardService CreditCardService,
                                   IPagerService<Buy_Ticket> PagerService, UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;

            _UserService = UserService;
            _BuyTicketService = BuyTicketService;
            _CancelBuyService = CancelBuyService;
            _HoraryService = HoraryService;
            _ReservedSeatsService = ReservedSeatsService;
            _CreditCardService = CreditCardService;
            _PagerService = PagerService;
        }

        [Authorize(Roles = "Basic_User,Admin,Manager")]
        public IActionResult Index(string _code, string _title, string _localMovie, DateTime _minDate, DateTime _maxDate, int page = 1)
        {
            var user = _UserService.GetUser(_UserManager.GetUserId(User));

            var userBuys = _BuyTicketService.GetAllBuy_Tickets()
                .Include(u => u.Horary)
                .Include(u => u.Horary.Movie_Local)
                .Include(u => u.Horary.Movie)
                .Where(m => m.ApplicationUserId == user.Id && m.PayCompleted == true).ToList();

            userBuys = _CancelBuyService.Filters(_code, _title, _localMovie, _minDate, _maxDate, userBuys).ToList();

            return View(_PagerService.GetPager(userBuys, page));
        }
        public IActionResult Cancel(int id)
        {
            var Buy = _BuyTicketService.GetAllBuy_Tickets()
                .Include(u => u.Horary)
                .Include(u => u.CreditCard)
                .Include(u => u.ApplicationUser)
                .Include(u => u.Horary.Movie_Local)
                .Include(u => u.Horary.Movie)
                .Include(u => u.Reserved_Seats)
                .FirstOrDefault(m => m.Id == id);

            Buy.Horary.ReservedTickets -= (int)Buy.NumberOfEntrance;
            _HoraryService.UpdateHorary(Buy.Horary);

            _ReservedSeatsService.RemoveReservedSeats(Buy.Reserved_Seats);


            if (Buy.PayWithPoints.Value)
            {
                Buy.ApplicationUser.Puntuation += Buy.Payment;
                _UserService.UpdateUser(Buy.ApplicationUser);
            }

            else
            {
                Buy.CreditCard.Money += (int)Buy.Payment;
                _CreditCardService.UpdateCreditCardMoney(Buy.CreditCard);
            }

            _BuyTicketService.RemoveBuyTicket(Buy);

            TempData["Success"] = "Buy Canceled";

            return RedirectToAction(nameof(Index));
        }
    }
}
