using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Orders.Entity
{
    public class OrderEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Quantity {  get; set; }
        public DateTime OrderDate { get; set; }
        [NotMapped]
        public BookEntity BookDetails { get; set; }

        [NotMapped]
        public UserEntity UserDetails { get; set; }
        
        public double OrderAmount { get; set; }
        public string url { get; set; }
        public bool IsSuccess { get; set; }
    }
}
