using FakeItEasy;
using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBRS_API.Tests.Services
{
    public class OrderFoodService_Test
    {
        private IConfiguration _configuration;
        private IOrderFoodRepository _orderFoodRepository;
        private OrderFoodService _orderFoodService;

        public OrderFoodService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _orderFoodRepository = A.Fake<IOrderFoodRepository>();
            _orderFoodService = new OrderFoodService(_configuration, _orderFoodRepository);
        }

        [Fact]
        public void ManageCustomerAccountService_getAllFood_ReturnEqualCount()
        {
            var food = A.Fake<Food>();
            food = new Food
            {
                foodID = 1,
                foodCode = "T01",
                foodName = "Coca",
                quantity = 1,
                price = 15000,
                typeFoodID = 3
            };
            var listFood = A.Fake<List<Food>>();
            listFood.Add(food);
            A.CallTo(() => _orderFoodRepository.getAllFood()).Returns(listFood);
            var result = _orderFoodService.getAllFood();
            Assert.Equal(listFood.Count, result.Count);
        }

        [Fact]
        public void ManageCustomerAccountService_getCustomerInformationByID_ReturnEqualCount()
        {
            var cus = A.Fake<CustomerViewModel>();
            cus = new CustomerViewModel
            {
                accountID = 1,
                customerID = 5,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                role = "3",
                Total = 5,
                userName = "nhatdv1",
                password = "88DFA2EA2E9EF240F45935D257FEA20A"
            };
            var listCus = A.Fake<List<CustomerViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _orderFoodRepository.getCustomerInformationByID(cus.accountID)).Returns(listCus);
            var result = _orderFoodService.getCustomerInformationByID(cus.accountID);
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageCustomerAccountService_getCustomerInformationByIdentityNumber_ReturnEqualCount()
        {
            var cus = A.Fake<CustomerViewModel>();
            cus = new CustomerViewModel
            {
                accountID = 1,
                customerID = 5,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                role = "3",
                Total = 5,
                userName = "nhatdv1",
                password = "88DFA2EA2E9EF240F45935D257FEA20A"
            };
            var listCus = A.Fake<List<CustomerViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _orderFoodRepository.getCustomerInformationByIdentityNumber(cus.identifyNumber)).Returns(listCus);
            var result = _orderFoodService.getCustomerInformationByIdentityNumber(cus.identifyNumber);
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageCustomerAccountService_getCustomerInformationByRoomName_ReturnEqualCount()
        {
            var cus = A.Fake<CustomerViewModel>();
            cus = new CustomerViewModel
            {
                accountID = 1,
                customerID = 5,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                role = "3",
                Total = 5,
                userName = "nhatdv1",
                password = "88DFA2EA2E9EF240F45935D257FEA20A"
            };
            var listCus = A.Fake<List<CustomerViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _orderFoodRepository.getCustomerInformationByRoomName("P01")).Returns(listCus);
            var result = _orderFoodService.getCustomerInformationByRoomName("P01");
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageCustomerAccountService_createOrderFood_ReturnSuccess()
        {
            var cus = A.Fake<OrderFoodInformationViewModel>();
            cus = new OrderFoodInformationViewModel
            {
                foodID = 1,
                foodName = "Coca",
                customerID = 3,
                statusPayment = "Đã thanh toán",
                orderCode = "HN1",
                contentPayment = "Trả tiền đồ ăn 3x Coca (P101,P102)",
                price = 15000,
                quanity = 3,
                vnpTransactionNumber = "13899931"               
            };
            var listCus = A.Fake<List<OrderFoodInformationViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _orderFoodRepository.createOrderFood(listCus)).Returns(1);
            var result = _orderFoodService.createOrderFood(listCus);
            Assert.Equal(1, result);
        }
    }
}
