using System;
using System.ComponentModel.DataAnnotations;

namespace ParrotIncSquawk.Entities
{
    public class Squawk
    {
        [Key]
        public Guid SquawkId { get; set; }

        public Guid UserId { get; set; }

        public string Text { get; set; }

        public DateTime SquawkDate { get; set; }
    }
}
