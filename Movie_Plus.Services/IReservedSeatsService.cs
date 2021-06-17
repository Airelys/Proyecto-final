using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IReservedSeatsService
    {
        void RemoveReservedSeats(ICollection<Reserved_Seats> reserved_seats);
        void InsertReservedSeats(Reserved_Seats reserved_seats);
        ICollection<Reserved_Seats> GetReservedSeats(int buyTicketId);
    }
}
