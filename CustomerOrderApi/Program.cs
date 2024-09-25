using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer("Data Source=.;Initial Catalog=CustomerOrder;Integrated Security=True"));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(); // Ensure this line is present
builder.Services.AddEndpointsApiExplorer(); // Optional for minimal APIs

 
builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated(); // Automatically create the database if it doesn't exist
    SeedData(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); // Ensure this line is present



app.Run();


  void SeedData(ApplicationDbContext dbContext)
{
    if (!dbContext.Customers.Any())
    {
        var customer1 = new Customer { Name = "Alice" };
        var customer2 = new Customer { Name = "Bob" };

        dbContext.Customers.AddRange(customer1, customer2);
        dbContext.Orders.AddRange(
            new Order { OrderDate = DateTime.Now, Customer = customer1 },
            new Order { OrderDate = DateTime.Now, Customer = customer2 });
        dbContext.OrderItems.AddRange(
            new OrderItem { ProductName = "Laptop", Quantity = 1, OrderId = 1 },
            new OrderItem { ProductName = "Phone", Quantity = 2, OrderId = 1 },
            new OrderItem { ProductName = "Tablet", Quantity = 1, OrderId = 2 });

        dbContext.SaveChanges();
    }
}
