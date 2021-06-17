using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Data
{
    public class CreditCard: BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public long Number { get; set; }
        public double Money { get; set; }
        public int Code { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Buy_Ticket> Buys_Tickets { get; set; }
    }
}
