using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class BuyEntranceService : IBuyEntranceService
    {
        IBuyTicketService _BuyTicketService;
        IReservedSeatsService _ReserverdSatsService;

        public BuyEntranceService(IBuyTicketService BuyTicketService, IReservedSeatsService ReservedSeatsService)
        {
            _BuyTicketService = BuyTicketService;
            _ReserverdSatsService = ReservedSeatsService;
        }

        public int GetTime(DateTime date, int seconds)
        {
           return (int)date.AddSeconds(seconds).Subtract(DateTime.Now).TotalSeconds;
        }

        public Buy_Ticket GetUserPendingTicket(string userId, int seconds)
        {
            return _BuyTicketService.GetAllBuy_Tickets().Include(p => p.ApplicationUser).
                ToList().FirstOrDefault(b => b.ApplicationUserId == userId &&
                                                            b.PayCompleted == false &&
                                                            DateTime.Now.Subtract(b.Date).TotalSeconds < seconds);
        }

        public Buy_Ticket InsertReservedSeats(Buy_Ticket _ticket, IFormCollection form)
        {
            foreach (var item in form)
            {
                try
                {
                    int row = int.Parse(item.Key.Split(",")[0]);
                    int column = int.Parse(item.Key.Split(",")[1]);

                    Reserved_Seats _reserved_seats = new Reserved_Seats()
                    {
                        Buy_TicketId = _ticket.Id,
                        Row = row,
                        Column = column
                    };

                    _ReserverdSatsService.InsertReservedSeats(_reserved_seats);

                    _ticket.VoucherSeats += "Row : " + (row+1).ToString() + ", " +
                                            "Seat : " + (column+1).ToString() + '\n';

                    _BuyTicketService.UpdateBuyTicket(_ticket);

                }
                catch { break; }
            }
            return _ticket;
        }

        public bool[,] ShowMovieLocalSeats(bool[,] AllSeats, int horaryId)
        {
            var _ticketsWithHorary = _BuyTicketService.GetAllBuy_Tickets()
                        .Where(x => x.HoraryId == horaryId).Include(x => x.Reserved_Seats);

            foreach (var ticket in _ticketsWithHorary)
            {
                foreach (var _seat in ticket.Reserved_Seats)
                {
                    AllSeats[_seat.Row, _seat.Column] = true;
                }
            }

            return AllSeats;
        }

        public void TenMinutesCheck(int seconds)
        {
            var _tickets = _BuyTicketService.GetAllBuy_Tickets()
               .Include(b => b.Reserved_Seats)
               .Include(b => b.Horary)
               .Where(b => b.PayCompleted == false).ToList();

            foreach (var item in _tickets)
            {
                if (DateTime.Now.Subtract(item.Date).TotalSeconds > seconds)
                {
                    item.Horary.ReservedTickets -= item.Reserved_Seats.Count();

                    _ReserverdSatsService.RemoveReservedSeats(item.Reserved_Seats);
                    _BuyTicketService.RemoveBuyTicket(item);
                }
            }
        }
    }
}
