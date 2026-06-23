using CustomerOrderManager.Business.Context;
using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;
using CustomerOrderManager.Business.Repositories;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerOrderManager.Business.Test
{
    [TestClass]
    public class CustomerOrderParameterRepositoryTest
    {
        private ICustomerOrderParameterRepository __CustomerOrderParameterRepository;
        private Mock<ICustomerOrderManagerDbContext> __CustomerOrderManagerDbContextMock;

        private List<CustomerOrderParameter> __CustomerOrderParameters;

        private List<CustomerOrderParameter> GetTestCustomerOrderParameters()
        {
            return new List<CustomerOrderParameter>()
            {
                new CustomerOrderParameter()
                {
                    ID = 1,
                    ParameterType = ParameterType.MaximumOrderValue,
                    Value = 500.00,
                    CustomerOrderParameter_UID = Guid.NewGuid()
                }
            };
        }

        private void SetupDbContextMock()
        {
            __CustomerOrderManagerDbContextMock = new Mock<ICustomerOrderManagerDbContext>();
            __CustomerOrderManagerDbContextMock.Setup(context => context.CustomerOrderParameters).ReturnsDbSet(__CustomerOrderParameters);
            __CustomerOrderManagerDbContextMock.Setup(context => context.SaveChangesAsync()).Verifiable();
        }

        [TestInitialize]
        public void TestInitilize()
        {
            __CustomerOrderParameters = GetTestCustomerOrderParameters();
            SetupDbContextMock();

            __CustomerOrderParameterRepository = new CustomerOrderParameterRepository(__CustomerOrderManagerDbContextMock.Object);
        }

        private void AssertCustomerOrdersParametersAreEqual(CustomerOrderParameter expected, CustomerOrderParameter actual)
        {
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.ParameterType, actual.ParameterType);
            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(expected.CustomerOrderParameter_UID, actual.CustomerOrderParameter_UID);
        }

        [TestMethod]
        public async Task CustomerOrderParameterRepository_GetAll_ReturnsAllRecords()
        {
            List<CustomerOrderParameter> _Result = await __CustomerOrderParameterRepository.GetAll();

            Assert.HasCount(__CustomerOrderManagerDbContextMock.Object.CustomerOrderParameters.Count(), _Result);

            for (int i = 0; i < _Result.Count; i++)
            {
                AssertCustomerOrdersParametersAreEqual(__CustomerOrderParameters[i], _Result[i]);
            }
        }

        [TestMethod]
        public async Task CustomerOrderParameterRepository_GetByID_ReturnsSingleRecord()
        {
            CustomerOrderParameter _CustomerOrderParameter = __CustomerOrderParameters[0];

            CustomerOrderParameter _Result = await __CustomerOrderParameterRepository.GetByID(_CustomerOrderParameter.ID);

            AssertCustomerOrdersParametersAreEqual(_CustomerOrderParameter, _Result);
        }

        [TestMethod]
        public async Task CustomerOrderParameterRepository_GetByParameterType_ReturnsSingleRecord()
        {
            CustomerOrderParameter _CustomerOrderParameter = __CustomerOrderParameters[0];

            CustomerOrderParameter _Result = await __CustomerOrderParameterRepository.GetByParameterType(_CustomerOrderParameter.ParameterType);

            AssertCustomerOrdersParametersAreEqual(_CustomerOrderParameter, _Result);
        }

        [TestMethod]
        public async Task CustomerOrderParameterRepository_GetByUID_ReturnsSingleRecord()
        {
            CustomerOrderParameter _CustomerOrderParameter = __CustomerOrderParameters[0];

            CustomerOrderParameter _Result = await __CustomerOrderParameterRepository.GetByUID(_CustomerOrderParameter.CustomerOrderParameter_UID);

            AssertCustomerOrdersParametersAreEqual(_CustomerOrderParameter, _Result);
        }

        [TestMethod]
        public async Task CustomerOrderParameterRepository_Save_New_IsSuccessful()
        {
            __CustomerOrderManagerDbContextMock.Setup(context => context.CustomerOrderParameters).ReturnsDbSet([]);

            CustomerOrderParameter _ValidCustomerOrderParameter = new CustomerOrderParameter()
            {
                ParameterType = ParameterType.MaximumOrderValue,
                Value = 550
            };

            CustomerOrderParameter? _Response = await __CustomerOrderParameterRepository.SaveAsync(_ValidCustomerOrderParameter);

            __CustomerOrderManagerDbContextMock.Verify(context => context.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(_Response);
            AssertCustomerOrdersParametersAreEqual(_ValidCustomerOrderParameter, _Response);
        }

        [TestMethod]
        public async Task CustomerOrderParameterRepository_Save_Existing_IsSuccessful()
        {
            CustomerOrderParameter _ExistingCustomerOrderParameter = __CustomerOrderParameters[0];

            CustomerOrderParameter _ValidCustomerOrderParameter = new CustomerOrderParameter()
            {
                ParameterType = ParameterType.MaximumOrderValue,
                Value = 550
            };

            CustomerOrderParameter? _Response = await __CustomerOrderParameterRepository.SaveAsync(_ValidCustomerOrderParameter);

            __CustomerOrderManagerDbContextMock.Verify(context => context.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(_Response);
            Assert.AreEqual(_ExistingCustomerOrderParameter.ID, _Response.ID);
            Assert.AreEqual(_ExistingCustomerOrderParameter.ParameterType, _Response.ParameterType);
            Assert.AreEqual(_ValidCustomerOrderParameter.Value, _Response.Value);

        }
    }
}