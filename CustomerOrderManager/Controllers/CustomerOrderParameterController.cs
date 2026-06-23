using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;
using CustomerOrderManager.Business.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrderManager.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerOrderParameterController : ControllerBase
    {
        private ICustomerOrderParameterRepository __CustomerOrderParameterRepository;

        public CustomerOrderParameterController(ICustomerOrderParameterRepository customerOrderParameterRepository)
        {
            __CustomerOrderParameterRepository = customerOrderParameterRepository ?? throw new ArgumentNullException(nameof(customerOrderParameterRepository));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<CustomerOrderParameter> _CustomerOrderParameters = await __CustomerOrderParameterRepository.GetAll();

            return Ok(_CustomerOrderParameters);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerOrderParameter customerOrderParameter)
        {
            if(customerOrderParameter.ParameterType == ParameterType.None)
            {
                return BadRequest("Invalid Parameter Type");
            }

            try
            {
                CustomerOrderParameter? _Parameter = await __CustomerOrderParameterRepository.SaveAsync(customerOrderParameter);         

                return Ok(_Parameter?.CustomerOrderParameter_UID ?? Guid.Empty);
            }
            catch /*(Exception exception)*/
            {
                //LogException(exception);

                return StatusCode(500);
            }
        }
    }
}
