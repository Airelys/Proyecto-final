using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface IBuyTicketService
    {
        Buy_Ticket Get(int id);
        DbSet<Buy_Ticket> GetAllBuy_Tickets();
        void RemoveBuyTicket(Buy_Ticket buyTicket);
        void InsertBuyTicket(Buy_Ticket buyTicket);
        void UpdateBuyTicket(Buy_Ticket buy_Ticket);
    }
}
