public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(\"DemoDb\"));
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedData(dbContext);
        }
    }

    private void SeedData(ApplicationDbContext dbContext)
    {
        if (!dbContext.Customers.Any())
        {
            var customer1 = new Customer { Name = \"Alice\" };
            var customer2 = new Customer { Name = \"Bob\" };

            dbContext.Customers.AddRange(customer1, customer2);
            dbContext.Orders.AddRange(
                new Order { OrderDate = DateTime.Now, Customer = customer1 },
                new Order { OrderDate = DateTime.Now, Customer = customer2 });
            dbContext.OrderItems.AddRange(
                new OrderItem { ProductName = \"Laptop\", Quantity = 1, OrderId = 1 },
                new OrderItem { ProductName = \"Phone\", Quantity = 2, OrderId = 1 },
                new OrderItem { ProductName = \"Tablet\", Quantity = 1, OrderId = 2 });

            dbContext.SaveChanges();
        }
    }
}
