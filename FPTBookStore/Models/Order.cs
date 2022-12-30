namespace FPTBookStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? UserName { get; set; }
        public string? OrderAddress { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderTotal { get; set; }
        public string? OrderPhone { get; set; }
        public string? OrderName { get; set; }
        public int? OrderStatus { get; set; }
    }
}
