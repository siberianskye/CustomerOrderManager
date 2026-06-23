using CustomerOrderManager.Business.Context;
using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CustomerOrderManager.Business.Repositories
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private ICustomerOrderManagerDbContext __CustomerOrderManagerDbContext;
        private ICustomerOrderParameterRepository __CustomerOrderParameterRepository;

        public CustomerOrderRepository(ICustomerOrderManagerDbContext customerOrderManagerDbContext, ICustomerOrderParameterRepository customerOrderParameterRepository)
        {
            __CustomerOrderManagerDbContext = customerOrderManagerDbContext ?? throw new ArgumentNullException(nameof(customerOrderManagerDbContext));
            __CustomerOrderParameterRepository = customerOrderParameterRepository ?? throw new ArgumentNullException(nameof(customerOrderParameterRepository));
        }

        public async Task<List<CustomerOrder>> GetAll()
        {
            List<CustomerOrder> _CustomerOrders = [];

            if (!__CustomerOrderManagerDbContext.CustomerOrders.IsNullOrEmpty())
            {
                _CustomerOrders = __CustomerOrderManagerDbContext.CustomerOrders.ToList();
            }

            return _CustomerOrders;   
        }

        public async Task<CustomerOrder> GetByID(int id)
        {
            return await __CustomerOrderManagerDbContext.CustomerOrders.FirstOrDefaultAsync(customerOrder => customerOrder.ID == id);
        }

        public async Task<CustomerOrder> GetByUID(Guid uid)
        {
            return await __CustomerOrderManagerDbContext.CustomerOrders.FirstOrDefaultAsync(customerOrder => customerOrder.CustomerOrder_UID == uid);
        }

        private async Task<bool> IsExistingCustomerOrder(CustomerOrder customerOrder)
        {
            return __CustomerOrderManagerDbContext.CustomerOrders.Any(order =>
                order.CustomerName == customerOrder.CustomerName &&
                order.OrderDate == customerOrder.OrderDate &&
                order.OrderValue == customerOrder.OrderValue
                );
        }

        public async Task<CreateCustomerOrderResponse> SaveAsync(CustomerOrder customerOrder)
        {
            CustomerOrderValidationResult _ValidationResult = await Validate(customerOrder);

            if(_ValidationResult == CustomerOrderValidationResult.Validated)
            {
                await __CustomerOrderManagerDbContext.CustomerOrders.AddAsync(customerOrder);

                await __CustomerOrderManagerDbContext.SaveChangesAsync();
            } 

            return new CreateCustomerOrderResponse()
            {
                IsSuccess = customerOrder.ID > 0,
                CustomerOrder = customerOrder,
                ValidationResult = _ValidationResult
            };
        }

        private async Task<CustomerOrderValidationResult> Validate(CustomerOrder customerOrder)
        {
            CustomerOrderParameter _MaximumOrderValue = await __CustomerOrderParameterRepository.GetByParameterType(ParameterType.MaximumOrderValue);

            if (_MaximumOrderValue != null && _MaximumOrderValue.Value > 0 && customerOrder.OrderValue > _MaximumOrderValue.Value)
            {
                return CustomerOrderValidationResult.MaxValueExceeded;
            }
            else if (await IsExistingCustomerOrder(customerOrder))
            {
                return CustomerOrderValidationResult.Duplicate;
            }
            else
            {
                return CustomerOrderValidationResult.Validated;
            }
        }
    }
}
