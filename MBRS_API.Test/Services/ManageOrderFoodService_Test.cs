using FakeItEasy;
using MBRS_API.Models;
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
    public class ManageOrderFoodService_Test
    {
        private IConfiguration _configuration;
        private IManageOrderFoodRepository _manageOrderFoodRepository;
        private ManageOrderFoodService _manageOrderFoodService;

        public ManageOrderFoodService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageOrderFoodRepository = A.Fake<IManageOrderFoodRepository>();
            _manageOrderFoodService = new ManageOrderFoodService(_configuration, _manageOrderFoodRepository);
        }

        [Fact]
        public void ManageOrderFoodService_updateStatusFood_ReturnSucess()
        {
            var statusFood = A.Fake<StatusFood>();
            statusFood = new StatusFood
            {
                orderID = 1,
                status = 3
            };
            var listOrderFood = A.Fake<List<StatusFood>>();
            listOrderFood.Add(statusFood);
            A.CallTo(() => _manageOrderFoodRepository.updateStatusFood(statusFood)).Returns(1);
            var result = _manageOrderFoodService.updateStatusFood(statusFood);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageOrderFoodService_getAllOrderFood_ReturnEqualCount()
        {
            var orderFood = A.Fake<OrderFood>();
            orderFood = new OrderFood
            {
                orderID = 1,
                fullName = "Dam Nhat",
                phoneNumber = "12312312111",
                contentPayment = "Trả tiền đồ ăn 4x Coca, 3x Phở (P101,P102)",
                price = 15000,
                statusPayment = "Đã thanh toán",
                statusFood = "Đã giao đồ ăn",
                vnpTransactionNumber = "13899931",
                orderCode = "F4K6CFA1WBC"
            };
            var listOrderFood = A.Fake<List<OrderFood>>();
            listOrderFood.Add(orderFood);
            A.CallTo(() => _manageOrderFoodRepository.getAllOrderFood()).Returns(listOrderFood);
            var result = _manageOrderFoodService.getAllOrderFood();
            Assert.Equal(listOrderFood.Count, result.Count);
        }

        [Fact]
        public void ManageOrderFoodService_getAllOrderFoodFilter_ReturnEqualCount()
        {
            var orderFood = A.Fake<OrderFood>();
            orderFood = new OrderFood
            {
                orderID = 1,
                fullName = "Dam Nhat",
                phoneNumber = "12312312111",
                contentPayment = "Trả tiền đồ ăn 4x Coca, 3x Phở (P101,P102)",
                price = 15000,
                statusPayment = "Đã thanh toán",
                statusFood = "Đã giao đồ ăn",
                vnpTransactionNumber = "13899931",
                orderCode = "F4K6CFA1WBC"
            };
            var listOrderFood = A.Fake<List<OrderFood>>();
            listOrderFood.Add(orderFood);
            A.CallTo(() => _manageOrderFoodRepository.getAllOrderFoodFilter(orderFood.fullName,"Tên khách")).Returns(listOrderFood);
            var result = _manageOrderFoodService.getAllOrderFoodFilter(orderFood.fullName, "Tên khách");
            Assert.Equal(listOrderFood.Count, result.Count);
        }

        [Fact]
        public void ManageOrderFoodService_deleteOrderFood_ReturnSuccess()
        {
            var orderFood = A.Fake<OrderFood>();
            orderFood = new OrderFood
            {
                orderID = 1,
                fullName = "Dam Nhat",
                phoneNumber = "12312312111",
                contentPayment = "Trả tiền đồ ăn 4x Coca, 3x Phở (P101,P102)",
                price = 15000,
                statusPayment = "Đã thanh toán",
                statusFood = "Đã giao đồ ăn",
                vnpTransactionNumber = "13899931",
                orderCode = "F4K6CFA1WBC"
            };
            var listOrderFood = A.Fake<List<OrderFood>>();
            listOrderFood.Add(orderFood);
            A.CallTo(() => _manageOrderFoodRepository.deleteOrderFood(orderFood.orderID)).Returns(1);
            var result = _manageOrderFoodService.deleteOrderFood(orderFood.orderID);
            Assert.Equal(1, result);
        }
    }
}
