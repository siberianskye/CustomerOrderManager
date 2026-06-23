using CustomerOrderManager.Business.Entities;

namespace CustomerOrderManager.Business.Repositories
{
    public interface ICustomerOrderRepository
    {
        public Task<List<CustomerOrder>> GetAll();
        public Task<CustomerOrder> GetByID(int id);
        public Task<CustomerOrder> GetByUID(Guid uid);
        public Task<CreateCustomerOrderResponse> SaveAsync(CustomerOrder customerOrder);
    }
}
