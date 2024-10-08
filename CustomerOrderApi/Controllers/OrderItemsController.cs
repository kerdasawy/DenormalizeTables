using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
[ApiController]
[Route("api/[controller]")]
public class OrderItemsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetOrderItems()
    {
        var orderItems = _context.OrderItems.Include(oi => oi.Order).ToList();
        return Ok(orderItems);
    }

    [HttpPost]
    public IActionResult CreateOrderItem([FromBody] OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetOrderItems), new { id = orderItem.OrderItemId }, orderItem);
    }
}
