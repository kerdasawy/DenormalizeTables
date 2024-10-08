using CustomerOrderApi.Models;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<CutomerOrder> CutomerOrder { get; set; }
 
    public override int SaveChanges()
    {
        // Call custom saving logic before saving
        CustomSaveLogic();

        // Proceed with the actual saving operation
        return base.SaveChanges();
    }

    // Override SaveChangesAsync to intercept asynchronous save operations
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Call custom saving logic before saving
        CustomSaveLogic();

        // Proceed with the actual saving operation
        return base.SaveChangesAsync(cancellationToken);
    }
     private void CustomSaveLogic()
    {
        //Retrieve modified or added entities
       var modifiedEntries = ChangeTracker
           .Entries()
           .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
           .ToList();

        foreach (var entry in modifiedEntries)
        {
            if (entry.Entity is Order Orderentity)
            {
                // Example: Perform some custom logic before saving
                // For instance, setting a timestamp or checking for specific values
                if (entry.State == EntityState.Added)
                {
                    CutomerOrder cutomerOrders = new CutomerOrder() {
                        Customer = JsonConvert.SerializeObject( Orderentity.Customer, new JsonSerializerSettings() {   MaxDepth=1 , ReferenceLoopHandling  = ReferenceLoopHandling.Ignore}),
                        OrderId = Orderentity.OrderId,
                        OrderItems = JsonConvert.SerializeObject(Orderentity.OrderItems, new JsonSerializerSettings() { MaxDepth = 1, ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        OrderDate = Orderentity.OrderDate

                    };
                    CutomerOrder.Add(cutomerOrders);
                }
                else if (entry.State == EntityState.Modified)
                {
                   var exist =  CutomerOrder.Where(x => x.OrderId == Orderentity.OrderId).SingleOrDefault();
                        exist.Customer = JsonConvert.SerializeObject(Orderentity.Customer);
                        exist.OrderId = Orderentity.OrderId;
                        exist.OrderItems = JsonConvert.SerializeObject(Orderentity.OrderItems);
                        exist.OrderDate = Orderentity.OrderDate;
                }
                // More custom logic here...
            }
            if (entry.Entity is Customer CustomerEntity )
            {
                entry.State = EntityState.Detached;
            }
            if (entry.Entity is OrderItem OrderItemEntity)
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
