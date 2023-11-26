using System.ComponentModel.DataAnnotations;

namespace ParrotIncSquawk.Models
{
    public class SquawkRequest
    {
        [Required]
        public string Text { get; set; }
    }
}
