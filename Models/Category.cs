using System.ComponentModel.DataAnnotations;

namespace pfm.Models
{
    public class Category
    {
        [Required]
        public string Code;
        [Required]
        public string Name;
        public string ParentCode;
    }
}