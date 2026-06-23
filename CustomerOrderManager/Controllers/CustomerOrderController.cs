using Azure;
using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;
using CustomerOrderManager.Business.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrderManager.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerOrderController : ControllerBase
    {
        private readonly ICustomerOrderRepository __CustomerOrderRepository;

        public CustomerOrderController(ICustomerOrderRepository customerOrderRepository)
        {
            __CustomerOrderRepository = customerOrderRepository ?? throw new ArgumentNullException(nameof(customerOrderRepository));
        }

        [HttpGet]
        [Route("{customerOrder_UID}")]
        public async Task<IActionResult> Get(Guid customerOrder_UID)
        {
            if(customerOrder_UID == Guid.Empty)
            {
                return BadRequest("Invalid customerOrder_UID.");
            }

            CustomerOrder _CustomerOrder = await __CustomerOrderRepository.GetByUID(customerOrder_UID);

            if(_CustomerOrder == null)
            {
                return NotFound("Customer Order Not Found.");
            }

            return Ok(_CustomerOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<CustomerOrder> _CustomerOrders = await __CustomerOrderRepository.GetAll();

            return Ok(_CustomerOrders);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerOrder customerOrder)
        {
            if (string.IsNullOrWhiteSpace(customerOrder.CustomerName))
            {
                return BadRequest("Customer Name is required.");
            }
            else if (customerOrder.OrderValue <= 0)
            {
                return BadRequest("Order Value must be greater than zero.");
            }
            else if (customerOrder.OrderDate == DateTime.MinValue)
            {
                return BadRequest("Order Date is required.");
            }

            try
            {
                CreateCustomerOrderResponse _Response = await __CustomerOrderRepository.SaveAsync(customerOrder);

                if(_Response.ValidationResult == CustomerOrderValidationResult.MaxValueExceeded)
                {
                    return BadRequest("Maximum Order Value exceeded.");
                }
                else if (_Response.ValidationResult == CustomerOrderValidationResult.Duplicate) 
                {
                    return BadRequest("Duplicate Customer Order found.");
                }

                return Ok(_Response.CustomerOrder?.CustomerOrder_UID ?? Guid.Empty);
            }
            catch 
            {           
                return StatusCode(500);
            }           
        }
    }
}
