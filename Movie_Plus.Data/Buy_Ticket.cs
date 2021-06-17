using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Movie_Plus.Data
{
    public class Buy_Ticket : BaseEntity, IEquatable<Buy_Ticket>
    {
        public string ApplicationUserId { get; set; }
        public int? HoraryId { get; set; }
        public int? CreditCardId { get; set; }
        public int? NumberOfEntrance { get; set; }
        public int? Payment { get; set; }
        public string Voucher { get; set; }
        public string VoucherSeats { get; set; }
        public string VoucherUserTypes { get; set; }
        public DateTime Date { get; set; }
        public bool? PayWithPoints { get; set; }
        public bool? PayCompleted { get; set; }

        public virtual CreditCard CreditCard { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Horary Horary { get; set; }
        public virtual ICollection<Reserved_Seats> Reserved_Seats { get; set; }

        public bool Equals([AllowNull] Buy_Ticket other)
        {
            if (other == null) return false;
            return Id == other.Id && HoraryId == other.HoraryId && Date == other.Date;
        }
    }


}
