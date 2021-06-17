using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Repository;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie_Plus.Data;
using Microsoft.AspNetCore.Identity;
using Rotativa.AspNetCore;
using Movie_Plus.Services;
using Microsoft.AspNetCore.Authorization;

namespace Movie_PlusPlus.Controllers
{
    public class BuyEntranceController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;

        private static Buy_Ticket _ticket;

        IBuyEntranceService _BuyEntranceService;
        IUserTypesService _UserTypeService;
        IHoraryService _HoraryService;
        IBuyTicketService _BuyTicketService;
        ICreditCardService _CreditCardService;
        IUserService _UserService;
        IReservedSeatsService _ReservedSeatsService;

        private static bool[,] AllSeats;
        private static int seconds = 600;
        public BuyEntranceController(IBuyEntranceService BuyEntranceService, IUserTypesService UserTypesService,
                                     IHoraryService HoraryService, IBuyTicketService BuyTicketService,
                                     ICreditCardService CreditCardService, IUserService UserService,
                                     IReservedSeatsService ReservedSeatsService, UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
            _BuyEntranceService = BuyEntranceService;
            _UserTypeService = UserTypesService;
            _HoraryService = HoraryService;
            _BuyTicketService = BuyTicketService;
            _CreditCardService = CreditCardService;
            _UserService = UserService;
            _ReservedSeatsService = ReservedSeatsService;
        }


        public IActionResult Index(int id)
        {
            _BuyEntranceService.TenMinutesCheck(seconds);

            var _pendingTicket = _BuyEntranceService.GetUserPendingTicket(_UserManager.GetUserId(User), seconds);

            if (_pendingTicket != null)
            {
                _ticket = _pendingTicket;

                if (User.IsInRole(Roles.Roles.Ticket_Agent.ToString()))
                    return RedirectToAction(nameof(PayInTicketOffice));

                return RedirectToAction(nameof(Buy));
            }

            _ticket = new Buy_Ticket()
            {
                HoraryId = id,
                ApplicationUserId = _UserManager.GetUserId(User),
                Date = DateTime.Now,
                PayCompleted = false
            };

            ViewData["Total"] = 0;

            foreach (var item in _UserTypeService.GetAllUserTypes())
            {
                ViewData[item.Type] = 0;
            }

            return View(_UserTypeService.GetAllUserTypes());
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            var horary = _HoraryService.GetAllHoraries()
                .Include(h => h.Movie)
                .Include(h => h.Movie_Local)
                .FirstOrDefault(m => m.Id == _ticket.HoraryId);


            int Total = 0;
            int totalOfEnt = 0;


            foreach (var item in _UserTypeService.GetAllUserTypes())
            {

                int noEnt = int.Parse(form[item.Type].ToString());

                if (noEnt != 0)
                {
                    _ticket.VoucherUserTypes += item.Type + ": " + noEnt.ToString() + " entrances" + '\n';
                }

                totalOfEnt += noEnt;

                Total += noEnt * (horary.Price - item.Discount);

                ViewData[item.Type] = noEnt;
            }

            _ticket.Payment = Total;
            _ticket.NumberOfEntrance = totalOfEnt;

            if (totalOfEnt > horary.Movie_Local.Capacity - horary.ReservedTickets)
            {
                ViewData["Error"] = "NO CAPACITY";
                return View(_UserTypeService.GetAllUserTypes());
            }


            if (Total == 0)
                return View(_UserTypeService.GetAllUserTypes());

            return RedirectToAction(nameof(SelectSeats));
        }

        public IActionResult SelectSeats()
        {
            ViewData["Total"] = _ticket.Payment;

            _BuyEntranceService.TenMinutesCheck(seconds);

            var horary = _HoraryService.GetAllHoraries()
                .Include(h => h.Movie)
                .Include(h => h.Movie_Local)
                .FirstOrDefault(m => m.Id == _ticket.HoraryId);

            AllSeats = new bool[horary.Movie_Local.Rows, horary.Movie_Local.Columns];
            AllSeats = _BuyEntranceService.ShowMovieLocalSeats(AllSeats, horary.Id);

            ViewData["NoEnt"] = _ticket.NumberOfEntrance;

            return View(AllSeats);
        }

        [HttpPost]
        public IActionResult SelectSeats(IFormCollection form)
        {
            var horary = _HoraryService.GetAllHoraries()
                .Include(h => h.Movie)
                .Include(h => h.Movie_Local)
                .FirstOrDefault(m => m.Id == _ticket.HoraryId);

            if (form.Count() - 3 != _ticket.NumberOfEntrance)
            {
                ViewData["NoEnt"] = _ticket.NumberOfEntrance;
                ViewData["Error"] = "Number of seats dont match with number of entrances.";

                return View(AllSeats);
            }

            _ticket.Date = DateTime.Now;

            _BuyTicketService.InsertBuyTicket(_ticket);

            _ticket = _BuyEntranceService.InsertReservedSeats(_ticket, form);

            horary.ReservedTickets += _ticket.NumberOfEntrance.Value;
            _HoraryService.UpdateHorary(horary);

            if (form.ContainsKey("Buy"))
                return RedirectToAction(nameof(Buy));

            return RedirectToAction(nameof(PayInTicketOffice));

        }

        [Authorize(Roles = "Basic_User,Admin,Manager")]
        public IActionResult Buy()
        {
            _ticket = _BuyTicketService.GetAllBuy_Tickets()
                                                    .Include(t => t.Horary)
                                                    .Include(t => t.ApplicationUser)
                                                    .FirstOrDefault(t => t.Id == _ticket.Id);
            var horary = _ticket.Horary;
            var user = _ticket.ApplicationUser;

            int time = _BuyEntranceService.GetTime(_ticket.Date, seconds);

            if (time < 0) return RedirectToAction(nameof(Cancel));

            ViewData["Time"] = time;

            if (user.Code != null && user.Puntuation >= horary.PriceInPoints * _ticket.NumberOfEntrance)
            {
                TempData["IsMember"] = true;
            }

            return View();
        }

        [Authorize(Roles = "Basic_User,Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(long Number, int Code)
        {
            int time = _BuyEntranceService.GetTime(_ticket.Date, seconds);

            if (time < 0)
            {
                TempData["Error"] = "Your buy expired. Please try again";
                return RedirectToAction(nameof(Cancel));
            }

            ViewData["Time"] = time;

            if (Number.ToString().Length != 16)
            {
                ViewData["Error"] = "Invalid Operation: Credit Card must have 16 numbers";
                return View();
            }

            _ticket = _BuyTicketService.GetAllBuy_Tickets()
                .Include(b => b.Reserved_Seats)
                .Include(b => b.Horary)
                .Include(b => b.ApplicationUser)
                .FirstOrDefault(t => t.Id == _ticket.Id);

            var horary = _ticket.Horary;
            var user = _ticket.ApplicationUser;

            CreditCard _creditCard = new CreditCard()
            {
                ApplicationUserId = user.Id,
                Number = Number,
                Code = Code,
                Money = new Random().Next(0, 10000)
            };

            var exists = _CreditCardService.GetByNumber(Number);

            if (exists == null)
            {
                _CreditCardService.InsertCreditCard(_creditCard);
            }
            else
            {
                _creditCard = exists;
                if (exists.Code != Code)
                {
                    ViewData["Error"] = "Incorrect CreditCard code";
                    return View();
                }
            }

            if (_creditCard.Money < _ticket.Payment)
            {
                ViewData["Error"] = "NO CASH";
                return View();
            }
            else
            {
                _creditCard.Money -= _ticket.Payment.Value;
                _CreditCardService.UpdateCreditCardMoney(_creditCard);

                if (user.Code != null)
                {
                    user.Puntuation += horary.PointsForBuying * _ticket.NumberOfEntrance;
                    _UserService.UpdateUser(user);
                }

                _ticket.Date = DateTime.Now;

                _ticket.Voucher = "Date: " + _ticket.Date.ToString() + '\n' +
                      "User: " + user.FirstName + " " + user.LastName + '\n' +
                      "Number of entrances: " + _ticket.NumberOfEntrance.ToString() + '\n' +
                      "User Types: " + '\n' + _ticket.VoucherUserTypes +
                      "Price : " + _ticket.Payment.ToString() + '\n';

                _ticket.PayWithPoints = false;
                _ticket.CreditCardId = _creditCard.Id;
                _ticket.PayCompleted = true;

                string _code = _ticket.Id.ToString();
                _ticket.Voucher += "Pay Code: " + user.Id.Substring(0, 12 - _code.Length) + _code + '\n' +
                                    "Seats :" + '\n' + _ticket.VoucherSeats;

                _BuyTicketService.UpdateBuyTicket(_ticket);

                TempData["Success"] = "Successful Pay";
            }

            return RedirectToAction(nameof(ViewVoucher));
        }


        public IActionResult BuyByPoints()
        {
            if (DateTime.Now.Subtract(_ticket.Date).TotalSeconds > seconds)
            {
                TempData["Error"] = "Your buy expired. Please try again";
                return RedirectToAction(nameof(Cancel));
            }

            _ticket = _BuyTicketService.GetAllBuy_Tickets()
                .Include(b => b.Reserved_Seats)
                .Include(b => b.Horary)
                .Include(b => b.ApplicationUser)
                .FirstOrDefault(t => t.Id == _ticket.Id);

            var horary = _ticket.Horary;
            var user = _ticket.ApplicationUser;

            _ticket.Date = DateTime.Now;
            _ticket.Payment = _ticket.NumberOfEntrance * horary.PriceInPoints;

            _ticket.Voucher = "Date: " + _ticket.Date.ToString() + '\n' +
                      "User: " + user.FirstName + " " + user.LastName + '\n' +
                      "Number of entrances: " + _ticket.NumberOfEntrance.ToString() + '\n' +
                      "User Types: " + '\n' + _ticket.VoucherUserTypes +
                      "Points : " + _ticket.Payment.ToString() + '\n';

            _ticket.PayWithPoints = true;
            _ticket.PayCompleted = true;

            string _code = _ticket.Id.ToString();
            _ticket.Voucher += "Pay Code: " + user.Code.ToString().Substring(0, 12 - _code.Length) + _code + '\n' +
                                "Seats :" + '\n' + _ticket.VoucherSeats;

            _BuyTicketService.UpdateBuyTicket(_ticket);

            user.Puntuation -= _ticket.Payment;
            _UserService.UpdateUser(user);

            TempData["Success"] = "Success: " + user.Puntuation + "Points left";

            return RedirectToAction(nameof(ViewVoucher));
        }

        [Authorize(Roles = "Ticket_Agent")]
        public IActionResult PayInTicketOffice()
        {
            int time = _BuyEntranceService.GetTime(_ticket.Date, seconds);
            if (time < 0)
            {
                TempData["Error"] = "Limit of time exceded. Please try again";
                return RedirectToAction(nameof(Cancel));
            }

            ViewData["Time"] = time;

            _ticket = _BuyTicketService.GetAllBuy_Tickets()
               .Include(b => b.Reserved_Seats)
               .Include(b => b.Horary)
               .Include(b => b.ApplicationUser)
               .FirstOrDefault(t => t.Id == _ticket.Id);

            var horary = _ticket.Horary;
            var user = _ticket.ApplicationUser;

            if (user.Code != null && user.Puntuation >= horary.PriceInPoints * _ticket.NumberOfEntrance)
            {
                TempData["IsMember"] = true;
            }

            ViewData["Total"] = _ticket.Payment;
            ViewData["Puntos"] = _ticket.NumberOfEntrance * _ticket.Horary.PriceInPoints;

            return View();
        }

        [Authorize(Roles = "Ticket_Agent")]
        [HttpPost]
        public IActionResult PayInTicketOffice(int cash, long code)
        {
            int time = _BuyEntranceService.GetTime(_ticket.Date, seconds);
            if (time < 0)
            {
                return RedirectToAction(nameof(Cancel));
            }

            ViewData["Time"] = time;

            _ticket = _BuyTicketService.GetAllBuy_Tickets()
               .Include(b => b.Reserved_Seats)
               .Include(b => b.Horary)
               .Include(b => b.ApplicationUser)
               .FirstOrDefault(t => t.Id == _ticket.Id);

            var _ticketAgent = _ticket.ApplicationUser;

            _ticket.Date = DateTime.Now;

            _ticket.Voucher = "Date: " + _ticket.Date.ToString() + '\n' +
                      "Ticket Agent: " + _ticketAgent.FirstName + " " + _ticketAgent.LastName + '\n' +
                      "Number of entrances: " + _ticket.NumberOfEntrance.ToString() + '\n' +
                      "User Types: " + '\n' + _ticket.VoucherUserTypes;

            _BuyTicketService.UpdateBuyTicket(_ticket);

            if (cash != 0)
            {
                int change = cash - _ticket.Payment.Value;
                if (change < 0)
                {
                    ViewData["msg"] = "Cash not enough.";
                    return View();
                }

                _ticket.Voucher += "Price: " + _ticket.Payment.ToString() + '\n'
                    + "Change: " + change.ToString() + '\n' + "Seats: " + '\n' + _ticket.VoucherSeats;

                _ticket.PayWithPoints = false;
                _ticket.PayCompleted = true;

                _BuyTicketService.UpdateBuyTicket(_ticket);

                if (code != 0)
                {
                    var user = _UserService.GetAllUsers()
                                .FirstOrDefault(m => m.Code != null && m.Code.Value == code);

                    if (user == null)
                    {
                        ViewData["msg"] = "Code doesnt belong to any user.";
                        return View();
                    }

                    user.Puntuation += _ticket.NumberOfEntrance * _ticket.Horary.PointsForBuying;

                    _UserService.UpdateUser(user);
                }

                return RedirectToAction(nameof(ViewVoucher));
            }

            else if (code != 0)
            {
                var user = _UserService.GetAllUsers()
               .FirstOrDefault(m => m.Code != null && m.Code.Value == code);

                if (user == null)
                {
                    ViewData["msg"] = "Code doesnt belong to any user.";
                    return View();
                }

                else if (user.Puntuation < _ticket.NumberOfEntrance * _ticket.Horary.PriceInPoints)
                {
                    ViewData["msg"] = "User have no enough points to pay";
                    return View();
                }

                _ticket.Payment = _ticket.NumberOfEntrance * _ticket.Horary.PriceInPoints;
                user.Puntuation -= _ticket.Payment;

                _UserService.UpdateUser(user);

                _ticket.Voucher += "Points: " + _ticket.Payment.ToString() + '\n' +
                    "Your Points: " + user.Puntuation.ToString() + '\n' + "Seats: " + '\n' + _ticket.VoucherSeats;

                _ticket.PayWithPoints = true;
                _ticket.PayCompleted = true;

                _BuyTicketService.UpdateBuyTicket(_ticket);

                return RedirectToAction(nameof(ViewVoucher));
            }

            ViewData["msg"] = "Invalid Values";
            return View();
        }

        public IActionResult ViewVoucher(int id)
        {
            if (id != 0)
            {
                _ticket = _BuyTicketService.Get(id);
            }

            return View(new List<string> { _ticket.Voucher });
        }

        public IActionResult PDF()
        {
            return new ViewAsPdf("ViewVoucher", new List<string> { _ticket.Voucher })
            {
                FileName = "Voucher.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        public IActionResult Cancel()
        {
            _ticket = _BuyTicketService.GetAllBuy_Tickets()
                .Include(b => b.Reserved_Seats)
                .Include(b => b.Horary)
                .FirstOrDefault(t => t.Id == _ticket.Id);

            _ticket.Horary.ReservedTickets -= _ticket.NumberOfEntrance.Value;
            _HoraryService.UpdateHorary(_ticket.Horary);

            _ReservedSeatsService.RemoveReservedSeats(_ticket.Reserved_Seats);
            _BuyTicketService.RemoveBuyTicket(_ticket);

            TempData["msg"] = "Buy has been canceled";
            return RedirectToAction("Index", "Horaries");
        }

        
    }
}
