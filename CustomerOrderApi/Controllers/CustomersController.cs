using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetCustomers()
    {
        var customers = _context.Customers.Include(c => c.Orders).ToList();
        return Ok(customers);
    }

    [HttpPost]
    public IActionResult CreateCustomer([FromBody] Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetCustomers), new { id = customer.CustomerId }, customer);
    }
}
