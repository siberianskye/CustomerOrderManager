using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;

namespace CustomerOrderManager.Business.Repositories
{
    public class CreateCustomerOrderResponse
    {
        public bool IsSuccess { get; set; }
        public CustomerOrderValidationResult ValidationResult { get; set; }
        public CustomerOrder? CustomerOrder { get; set; }
    }
}
