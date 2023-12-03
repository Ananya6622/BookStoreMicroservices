namespace BookStore.Admin.Entity.QueryEntity
{
    public class ViewBook
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
