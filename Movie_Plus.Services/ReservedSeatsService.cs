using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class ReservedSeatsService : IReservedSeatsService
    {
        private IRepository<Reserved_Seats> _ReservedSeatsRepository;

        public ReservedSeatsService(IRepository<Reserved_Seats> ReservedSeatsRepository)
        {
            _ReservedSeatsRepository = ReservedSeatsRepository;
        }

        public ICollection<Reserved_Seats> GetReservedSeats(int buyTicketId)
        {
            return _ReservedSeatsRepository.GetAll().ToList().Where(x => x.Buy_TicketId == buyTicketId).ToList();
        }

        public void InsertReservedSeats(Reserved_Seats reserved_seats)
        {
            _ReservedSeatsRepository.Insert(reserved_seats);
        }

        public void RemoveReservedSeats(ICollection<Reserved_Seats> reserved_seats)
        {
            _ReservedSeatsRepository.RemoveRange(reserved_seats);
        }

        

    }
}
