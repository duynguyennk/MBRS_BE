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
using static log4net.Appender.RollingFileAppender;

namespace MBRS_API.Tests.Services
{
    public class ManageOrderBikeService_Test
    {
        private IConfiguration _configuration;
        private IManageOrderBikeRepository _manageOrderBikeRepository;
        private ManageOrderBikeService _manageOrderBikeService;

        public ManageOrderBikeService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageOrderBikeRepository = A.Fake<IManageOrderBikeRepository>();
            _manageOrderBikeService = new ManageOrderBikeService(_configuration, _manageOrderBikeRepository);
        }

        [Fact]
        public void ManageOrderBikeService_getAllOrderBike_ReturnEqualCount()
        {
            var orderBike = A.Fake<OrderBike>();
            orderBike = new OrderBike
            {
                bikeID = 1,
                customerID = 3,
                fullName = "Dam Nhat",
                identifyNumber = "45367275832",
                phoneNumber = "0982736453",
                statusPayment = "Đã Thanh Toán",
                statusBike = 3,
                contentPayment = "Trả tiền thuê 1 Xe Thể Thao trong 1 tiếng",
                price = 15000,
                dateTimeGetBike = DateTime.ParseExact("22/12/2022","dd/MM/yyyy", CultureInfo.InvariantCulture),
                dateTimeBackBike = DateTime.ParseExact("22/12/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                vnpTransactionNumber = "13898980",
                bankTransactionNumber = "BBJ2WPW84TB"
            };
            var listOrderBike = A.Fake<List<OrderBike>>();
            listOrderBike.Add(orderBike);
            A.CallTo(() => _manageOrderBikeRepository.getAllOrderBike()).Returns(listOrderBike);
            var result = _manageOrderBikeService.getAllOrderBike();
            Assert.Equal(listOrderBike.Count, result.Count);
        }

        [Fact]
        public void ManageOrderBikeService_getAllOrderBikeFilter_ReturnEqualCount()
        {
            var orderBike = A.Fake<OrderBike>();
            orderBike = new OrderBike
            {
                bikeID = 1,
                customerID = 3,
                fullName = "Dam Nhat",
                identifyNumber = "45367275832",
                phoneNumber = "0982736453",
                statusPayment = "Đã Thanh Toán",
                statusBike = 3,
                contentPayment = "Trả tiền thuê 1 Xe Thể Thao trong 1 tiếng",
                price = 15000,
                dateTimeGetBike = DateTime.ParseExact("22/12/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                dateTimeBackBike = DateTime.ParseExact("22/12/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                vnpTransactionNumber = "13898980",
                bankTransactionNumber = "BBJ2WPW84TB"
            };
            var listOrderBike = A.Fake<List<OrderBike>>();
            listOrderBike.Add(orderBike);
            A.CallTo(() => _manageOrderBikeRepository.getAllOrderBike()).Returns(listOrderBike);
            var result = _manageOrderBikeService.getAllOrderBike();
            Assert.Equal(listOrderBike.Count, result.Count);
        }

        [Fact]
        public void ManageOrderBikeService_deleteOrderBike_ReturnSuccess()
        {
            var orderBike = A.Fake<OrderBike>();
            orderBike = new OrderBike
            {
                bikeID = 1,
                customerID = 3,
                fullName = "Dam Nhat",
                identifyNumber = "45367275832",
                phoneNumber = "0982736453",
                statusPayment = "Đã Thanh Toán",
                statusBike = 3,
                contentPayment = "Trả tiền thuê 1 Xe Thể Thao trong 1 tiếng",
                price = 15000,
                dateTimeGetBike = DateTime.ParseExact("22/12/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                dateTimeBackBike = DateTime.ParseExact("22/12/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                vnpTransactionNumber = "13898980",
                bankTransactionNumber = "BBJ2WPW84TB",
                orderID = 1,
            };
            var listOrderBike = A.Fake<List<OrderBike>>();
            listOrderBike.Add(orderBike);
            A.CallTo(() => _manageOrderBikeRepository.deleteOrderBike(orderBike.orderID)).Returns(1);
            var result = _manageOrderBikeService.deleteOrderBike(orderBike.orderID);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageOrderBikeService_updateStatusBike_ReturnSuccess()
        {
            var statusBike = A.Fake<StatusBike>();
            statusBike = new StatusBike
            {
                orderID = 1,
                status = 2
            };
            var listOrderBike = A.Fake<List<StatusBike>>();
            listOrderBike.Add(statusBike);
            A.CallTo(() => _manageOrderBikeRepository.updateStatusBike(statusBike)).Returns(1);
            var result = _manageOrderBikeService.updateStatusBike(statusBike);
            Assert.Equal(1, result);
        }
    }
}
