using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Movie_Plus.Data
{
    public class UserType: BaseEntity
    {
        [Required]
        public string Type { get; set; }
        public int Discount { get; set; }
    }
}
