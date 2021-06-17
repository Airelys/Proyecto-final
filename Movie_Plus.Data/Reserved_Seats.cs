using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Data
{
    public class Reserved_Seats : BaseEntity
    {
        public int Buy_TicketId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public virtual Buy_Ticket Buy_Ticket { get; set; }
    }
}
