using Inventory_Management_System.Data;
using Inventory_Management_System.Models.StoreModels;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly InventoryDbContext _context;

    public OrderRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddOrder(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<Order?> GetOrderWithItems(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
