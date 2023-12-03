using System.ComponentModel.DataAnnotations;

namespace BookStore.Books.Entity.CommandEntity
{
    public class AddUpdateBook
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$")]
        public string BookName { get; set; }

        [Required]
        [StringLength(50)]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        public double Price { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
    }
}
