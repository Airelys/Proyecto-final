using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Movie_Plus.Data
{
    public class Horary : BaseEntity, IEquatable<Horary>
    {
        public int MovieId { get; set; }
        public int Movie_LocalId { get; set; }

        [Required, Display(Name = "Fecha", Description = "Fecha", Prompt = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, Display(Name = "Hora", Description = "Hora", Prompt = "Hora")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int ReservedTickets { get; set; }
        public int Price { get; set; }
        public int PriceInPoints { get; set; }
        public int PointsForBuying { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Movie_Local Movie_Local { get; set; }
        public virtual ICollection<Buy_Ticket> Buys_Tickets { get; set; }

        public bool Equals([AllowNull] Horary other)
        {
            if (other == null) return false;
            return Id == other.Id && MovieId == other.MovieId && Movie_LocalId == other.Movie_LocalId &&
                Date == other.Date && Time == other.Time && ReservedTickets == other.ReservedTickets &&
                Price == other.Price && PriceInPoints == other.PriceInPoints &&
                PointsForBuying == other.PointsForBuying;
                   
        }
    }
}
