using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public class BuyTicketService: IBuyTicketService
    {
        private IRepository<Buy_Ticket> _BuyTicketRepository;

        public BuyTicketService(IRepository<Buy_Ticket> BuyTicketRepository)
        {
            _BuyTicketRepository = BuyTicketRepository;
        }

        public Buy_Ticket Get(int id)
        {
            return _BuyTicketRepository.Get(id);
        }

        public DbSet<Buy_Ticket> GetAllBuy_Tickets()
        {
            return _BuyTicketRepository.GetAll();
        }

        public void InsertBuyTicket(Buy_Ticket buyTicket)
        {
            _BuyTicketRepository.Insert(buyTicket);
        }

        public void RemoveBuyTicket(Buy_Ticket buyTicket)
        {
            _BuyTicketRepository.Remove(buyTicket);
        }

        public void UpdateBuyTicket(Buy_Ticket buy_Ticket)
        {
            _BuyTicketRepository.Update(buy_Ticket);
        }
    }
}
