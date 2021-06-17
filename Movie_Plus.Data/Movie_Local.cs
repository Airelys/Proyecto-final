using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Movie_Plus.Data
{
    public class Movie_Local : BaseEntity, IEquatable<Movie_Local>
    {
        [Required]
        public string Local_Name { get; set; }
        public int Capacity { get { return Rows * Columns; } }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public virtual ICollection<Horary> Horaries { get; set; }

        public bool Equals([AllowNull] Movie_Local other)
        {
            if (other == null) return false;
            return Id == other.Id &&
                   Local_Name == other.Local_Name &&
                   Rows == other.Rows &&
                   Columns == other.Columns;
        }
    }
    

}
