namespace Inventory_Management_System.Services.ServiceInterface
{
    public interface IWalletService
    {
        Task<decimal> GetWalletBalance(int userId);

        Task<int> CreateWalletAsync(int userId);


        Task CreateTransactionByUserAsync(
            int fromUserId,
            string fromWalletType,
            int toUserId,
            string toWalletType,
            decimal amount);

        Task SeedAdminBalanceAsync(int adminUserId, decimal amount);

        Task<decimal> GetUserWalletBalanceAsync(int userId, string walletType);



    }

}
