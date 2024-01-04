namespace BookStore.Orders.Entity
{
    public class PayURequest
    {
        public string Key { get; set; }
        public string TransactionId { get; set; }
        public double Amount {  get; set; }
        public string ProductInfo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Surl { get; set; }
        public string Furl { get; set; }
        public string Hash { get; set; }

    }
}
