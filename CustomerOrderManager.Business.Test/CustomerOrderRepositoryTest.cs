using CustomerOrderManager.Business.Context;
using CustomerOrderManager.Business.Entities;
using CustomerOrderManager.Business.Enums;
using CustomerOrderManager.Business.Repositories;
using Moq;
using Moq.EntityFrameworkCore;

namespace CustomerOrderManager.Business.Test
{
    [TestClass]
    public class CustomerOrderRepositoryTest
    {
        private ICustomerOrderRepository __CustomerOrderRepository;
        private Mock<ICustomerOrderParameterRepository> __CustomerOrderParameterRepositoryMock;
        private Mock<ICustomerOrderManagerDbContext> __CustomerOrderManagerDbContextMock;

        private List<CustomerOrder> __CustomerOrders;
        private List<CustomerOrderParameter> __CustomerOrderParameters;
        private List<CustomerOrder> GetTestCustomerOrders()
        {
            return new List<CustomerOrder>()
            {
                new CustomerOrder()
                {
                    ID = 1,
                    CustomerName = "Bob",
                    Description = "Order 1",
                    OrderValue = 150.00,
                    OrderDate = new DateTime(2026, 06, 23),
                    CustomerOrder_UID = Guid.NewGuid()
                },
                new CustomerOrder()
                {
                    ID = 2,
                    CustomerName = "Jim",
                    Description = "Order 2",
                    OrderValue = 175.00,
                    OrderDate = new DateTime(2026, 05, 20),
                    CustomerOrder_UID = Guid.NewGuid()
                },
                new CustomerOrder()
                {
                    ID = 2,
                    CustomerName = "Tim",
                    Description = "Order 3",
                    OrderValue = 200.25,
                    OrderDate = new DateTime(2026, 04, 15),
                    CustomerOrder_UID = Guid.NewGuid()
                }
            };
        }

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

        private void SetupCustomerOrderParameterMock()
        {
            __CustomerOrderParameterRepositoryMock = new Mock<ICustomerOrderParameterRepository>();
            __CustomerOrderParameterRepositoryMock.Setup(repo => repo.GetByParameterType(ParameterType.MaximumOrderValue)).ReturnsAsync(__CustomerOrderParameters[0]);
        }

        private void SetupDbContextMock()
        {
            __CustomerOrderManagerDbContextMock = new Mock<ICustomerOrderManagerDbContext>();

            __CustomerOrderManagerDbContextMock.Setup(context => context.CustomerOrders).ReturnsDbSet(__CustomerOrders);
            __CustomerOrderManagerDbContextMock.Setup(context => context.CustomerOrderParameters).ReturnsDbSet(__CustomerOrderParameters);

            __CustomerOrderManagerDbContextMock.Setup(context => context.SaveChangesAsync()).Verifiable();
        }

        private void AssertCustomerOrdersAreEqual(CustomerOrder expected, CustomerOrder actual)
        {
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.CustomerName, actual.CustomerName);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.OrderValue, actual.OrderValue);
            Assert.AreEqual(expected.OrderDate, actual.OrderDate);
            Assert.AreEqual(expected.CustomerOrder_UID, actual.CustomerOrder_UID);
        }

        [TestInitialize]
        public void TestInitilize()
        {
            __CustomerOrders = GetTestCustomerOrders();
            __CustomerOrderParameters = GetTestCustomerOrderParameters();

            SetupCustomerOrderParameterMock();
            SetupDbContextMock();

            __CustomerOrderRepository = new CustomerOrderRepository(__CustomerOrderManagerDbContextMock.Object, __CustomerOrderParameterRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CustomerOrderRepository_GetAll_ReturnsAllRecords()
        {
            List<CustomerOrder> _Result = await __CustomerOrderRepository.GetAll();

            Assert.HasCount(__CustomerOrderManagerDbContextMock.Object.CustomerOrders.Count(), _Result);

            for(int i=0; i < _Result.Count; i++)
            {
                AssertCustomerOrdersAreEqual(__CustomerOrders[i], _Result[i]);
            }
        }

        [TestMethod]
        public async Task CustomerOrderRepository_GetByID_ReturnsSingleRecord()
        {
            CustomerOrder _CustomerOrder = __CustomerOrders[0];

            CustomerOrder _Result = await __CustomerOrderRepository.GetByID(_CustomerOrder.ID);

            AssertCustomerOrdersAreEqual(_CustomerOrder, _Result);
        }

        [TestMethod]
        public async Task CustomerOrderRepository_GetByUID_ReturnsSingleRecord()
        {
            CustomerOrder _CustomerOrder = __CustomerOrders[0];

            CustomerOrder _Result = await __CustomerOrderRepository.GetByUID(_CustomerOrder.CustomerOrder_UID);

            AssertCustomerOrdersAreEqual(_CustomerOrder, _Result);
        }

        [TestMethod]
        public async Task CustomerOrderRepository_Save_ValidOrder_IsSuccessful()
        {
            CustomerOrder _ValidCustomerOrder = new CustomerOrder()
            {
                CustomerName = "Sarah",
                Description = "Order 55",
                OrderValue = 450.50,
                OrderDate = new DateTime(2026,06,22)
            };

            CreateCustomerOrderResponse _Response = await __CustomerOrderRepository.SaveAsync(_ValidCustomerOrder);

            __CustomerOrderManagerDbContextMock.Verify(context => context.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(_Response.IsSuccess);
            AssertCustomerOrdersAreEqual(_ValidCustomerOrder, _Response.CustomerOrder);
            Assert.AreEqual(CustomerOrderValidationResult.Validated, _Response.ValidationResult);
        }

        [TestMethod]
        public async Task CustomerOrderRepository_Save_DuplicateOrder_IsUnsuccessful()
        {
            CustomerOrder _ValidCustomerOrder = new CustomerOrder()
            {
                CustomerName = "Bob",
                Description = "Order 1",
                OrderValue = 150.00,
                OrderDate = new DateTime(2026, 06, 23)
            };

            CreateCustomerOrderResponse _Response = await __CustomerOrderRepository.SaveAsync(_ValidCustomerOrder);

            __CustomerOrderManagerDbContextMock.Verify(context => context.SaveChangesAsync(), Times.Never);

            Assert.IsFalse(_Response.IsSuccess);
            Assert.AreEqual(CustomerOrderValidationResult.Duplicate, _Response.ValidationResult);

        }

        [TestMethod]
        public async Task CustomerOrderRepository_Save_MaxValueExceeded_IsUnsuccessful()
        {
            CustomerOrder _ValidCustomerOrder = new CustomerOrder()
            {
                CustomerName = "Sarah",
                Description = "Order 55",
                OrderValue = 1000.00,
                OrderDate = new DateTime(2026, 06, 22)
            };

            CreateCustomerOrderResponse _Response = await __CustomerOrderRepository.SaveAsync(_ValidCustomerOrder);

            __CustomerOrderManagerDbContextMock.Verify(context => context.SaveChangesAsync(), Times.Never);

            Assert.IsFalse(_Response.IsSuccess);
            Assert.AreEqual(CustomerOrderValidationResult.MaxValueExceeded, _Response.ValidationResult);

        }
    }
}
