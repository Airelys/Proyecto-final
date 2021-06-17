using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IBuyEntranceService
    {
        public void TenMinutesCheck(int seconds);
        public Buy_Ticket GetUserPendingTicket(string userId, int seconds);
        public bool[,] ShowMovieLocalSeats(bool[,] AllSeats, int horaryId);
        public Buy_Ticket InsertReservedSeats(Buy_Ticket _ticket, IFormCollection form);
        public int GetTime(DateTime dat, int seconds);
    }
}
