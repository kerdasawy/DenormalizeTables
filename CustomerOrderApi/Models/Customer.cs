using System.Text.Json.Serialization;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public ICollection<Order>? Orders { get; set; }
}
