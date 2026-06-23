using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;

namespace CustomerOrderManager.Business.Repositories
{
    public interface ICustomerOrderParameterRepository
    {
        public Task<List<CustomerOrderParameter>> GetAll();
        public Task<CustomerOrderParameter> GetByID(int id);
        public Task<CustomerOrderParameter> GetByParameterType(ParameterType parameterType);
        public Task<CustomerOrderParameter> GetByUID(Guid uid);
        public Task<CustomerOrderParameter?> SaveAsync(CustomerOrderParameter customerOrder);
    }
}
