using CustomerOrderManager.Business.Context;
using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerOrderManager.Business.Repositories
{
    public class CustomerOrderParameterRepository : ICustomerOrderParameterRepository
    {
        private ICustomerOrderManagerDbContext __CustomerOrderManagerDbContext;
        public CustomerOrderParameterRepository(ICustomerOrderManagerDbContext customerOrderManagerDbContext)
        {
            __CustomerOrderManagerDbContext = customerOrderManagerDbContext ?? throw new ArgumentNullException(nameof(customerOrderManagerDbContext));
        }

        public async Task<List<CustomerOrderParameter>> GetAll()
        {
            List<CustomerOrderParameter> _CustomerOrderParameters = [];

            if (!__CustomerOrderManagerDbContext.CustomerOrderParameters.IsNullOrEmpty())
            {
                _CustomerOrderParameters = __CustomerOrderManagerDbContext.CustomerOrderParameters.ToList();
            }

            return _CustomerOrderParameters;
        }

        public async Task<CustomerOrderParameter> GetByID(int id)
        {
            return await __CustomerOrderManagerDbContext.CustomerOrderParameters.FirstOrDefaultAsync(customerOrderParameter => customerOrderParameter.ID == id);
        }

        public async Task<CustomerOrderParameter> GetByParameterType(ParameterType parameterType)
        {
            return await __CustomerOrderManagerDbContext.CustomerOrderParameters.FirstOrDefaultAsync(customerOrderParameter => customerOrderParameter.ParameterType == parameterType);
        }

        public async Task<CustomerOrderParameter> GetByUID(Guid uid)
        {
            return await __CustomerOrderManagerDbContext.CustomerOrderParameters.FirstAsync(customerOrderParameter => customerOrderParameter.CustomerOrderParameter_UID == uid);
        }

        public async Task<CustomerOrderParameter?> SaveAsync(CustomerOrderParameter customerOrderParameter)
        {
            CustomerOrderParameter _Parameter = customerOrderParameter;

            if (customerOrderParameter == null)
            {
                return null;
            }

            if (__CustomerOrderManagerDbContext.CustomerOrderParameters.Any(parameter => parameter.ParameterType == customerOrderParameter.ParameterType))
            {
                _Parameter = await GetByParameterType(customerOrderParameter.ParameterType);

                _Parameter.Value = customerOrderParameter.Value;
            }
            else
            {
                await __CustomerOrderManagerDbContext.CustomerOrderParameters.AddAsync(_Parameter);
            }


            await __CustomerOrderManagerDbContext.SaveChangesAsync();

            return _Parameter;
        }
    }
}
