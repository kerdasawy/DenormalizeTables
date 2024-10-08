using System.Text.Json.Serialization;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public int OrderId { get; set; }
    [JsonIgnore]
    public Order? Order { get; set; }
}
