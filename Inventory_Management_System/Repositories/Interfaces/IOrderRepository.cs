using Inventory_Management_System.Models.StoreModels;

public interface IOrderRepository
{
    Task AddOrder(Order order);
    Task<Order?> GetOrderWithItems(int orderId);
    Task Save();
}
