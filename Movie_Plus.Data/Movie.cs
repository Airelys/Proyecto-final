using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Movie_Plus.Data
{
    public class Movie : BaseEntity , IEquatable<Movie>
    {
        [Required]
        public string Title { get; set; }
        public int Duration { get; set; }
        [Required]
        public string KindOfMovie { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Actors { get; set; }
        public double Ranking { get; set; }
        public bool PropagandisticAndEconomics { get; set; }

        public virtual ICollection<Horary> Horaries { get; set; }

        public bool Equals([AllowNull] Movie other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title && Duration == other.Duration && 
                   KindOfMovie == other.KindOfMovie && Country == other.Country;
        }
    }
}
