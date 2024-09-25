namespace CustomerOrderApi.Models
{
    public class CutomerOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        
        public string Customer { get; set; }
        public string OrderItems { get; set; }
    }
}
