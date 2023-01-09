using FakeItEasy;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using Microsoft.Extensions.Configuration;
using MBRS_API.Models;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using MBRS_API.Controllers;

namespace MBRS_API.Tests.Services
{

    public class ManageBikeServiceTest
    {
        private IConfiguration _configuration;
        private IManageBikeRepository _manageBikeRepository;
        private ManageBikeService _manageBikeService;
        public ManageBikeServiceTest()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageBikeRepository = A.Fake<IManageBikeRepository>();
            _manageBikeService = new ManageBikeService(_configuration, _manageBikeRepository);
        }

        [Fact]
        public void ManageBikeService_createBike_Success()
        {
            var bikes = A.Fake<Bike>();
            bikes = new Bike { bikeCode = "B01", bikeName = "Xe", typeBikeID = 4 };
            A.CallTo(() => _manageBikeRepository.createBike(bikes)).Returns(1);
            var result = _manageBikeService.createBike(bikes);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageBikeService_getAllBike_EqualCount()
        {
            var bikes = A.Fake<List<BikeViewModel>>();
            bikes.Add(new BikeViewModel { bikeCode = "B01", bikeID = 1, bikeName = "Xe", color = "Xam", numberOfSeat = 1, price = 10000, typeBikeID = 2, typeBikeName = "Type" });
            A.CallTo(() => _manageBikeRepository.getAllBike()).Returns(bikes);
            var result = _manageBikeService.getAllBike();
            Assert.Equal(bikes.Count, result.Count);
        }

        [Fact]
        public void ManageBikeService_updateTheBike_ReturnSuccess()
        {
            var bikes = A.Fake<Bike>();
            bikes = new Bike { bikeCode = "B03", bikeName = "Xe", typeBikeID = 4 };
            A.CallTo(() => _manageBikeRepository.updateTheBike(bikes)).Returns(1);
            var result = _manageBikeService.updateTheBike(bikes);
            Assert.Equal(1, result);
        }


        [Fact]
        public void ManageBikeService_DeleteBike()
        {
            var bikes = A.Fake<Bike>();
            bikes = new Bike { bikeCode = "B01", bikeName = "Xe", typeBikeID = 4 };
            A.CallTo(() => _manageBikeRepository.deleteBike(bikes.bikeID)).Returns(1);
            var result = _manageBikeService.deleteBike(bikes.bikeID);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageBikeService_getAllBikeWithFilter_ReturnSuccess()
        {
            var bikes = A.Fake<List<BikeViewModel>>();
            var bikeViewModel = new BikeViewModel { bikeCode = "B01", bikeID = 1, bikeName = "Xe", color = "Xam", numberOfSeat = 1, price = 10000, typeBikeID = 2, typeBikeName = "Type" };
            bikes.Add(bikeViewModel);
            A.CallTo(() => _manageBikeRepository.getAllBikeWithFilter(bikeViewModel.bikeCode,"B01")).Returns(bikes);
            var result = _manageBikeService.getAllBikeWithFilter(bikeViewModel.bikeCode, "B01");
            Assert.Equal(bikes.Count, result.Count);
        }

        [Fact]
        public void ManageBikeService_getAllTypeBike_EqualCount()
        {
            var bikes = A.Fake<List<TypeBike>>();
            var typeBike = new TypeBike { typeBikeID = 1,typeBikeCode = "A01",typeBikeName="Xe thể thao",price = 30000,totalBike = 10,color = "Red",listImageID = 5,numberOfSeat = 2 };
            bikes.Add(typeBike);
            A.CallTo(() => _manageBikeRepository.getAllTypeBike()).Returns(bikes);
            var result = _manageBikeService.getAllTypeBike();
            Assert.Equal(bikes.Count, result.Count);
        }

        [Fact]
        public void ManageBikeService_getDetailInformationBike_EqualCount()
        {
            var bikes = A.Fake<List<TypeBike>>();
            var typeBike = new TypeBike { typeBikeID = 1, typeBikeCode = "A01", typeBikeName = "Xe thể thao", price = 30000, totalBike = 10, color = "Red", listImageID = 5, numberOfSeat = 2 };
            bikes.Add(typeBike);
            A.CallTo(() => _manageBikeRepository.getDetailInformationBike(typeBike.typeBikeID)).Returns(bikes);
            var result = _manageBikeService.getDetailInformationBike(typeBike.typeBikeID);
            Assert.Equal(bikes.Count, result.Count);
        }

        [Fact]
        public void ManageBikeService_getTheBikeInformation_EqualCount()
        {
            var bikes = A.Fake<Bike>();
            bikes = new Bike { bikeCode = "B01", bikeName = "Xe", typeBikeID = 4 };
            var listBike = A.Fake<List<Bike>>();
            listBike.Add(bikes);
            A.CallTo(() => _manageBikeRepository.getTheBikeInformation(bikes.bikeID)).Returns(listBike);
            var result = _manageBikeService.getTheBikeInformation(bikes.bikeID);
            Assert.Equal(listBike.Count, result.Count);
        }

        [Fact]
        public void ManageBikeService_getTheBikeInformation1_EqualCount()
        {
            var bikes = A.Fake<Bike>();
            bikes = new Bike { bikeCode = "B01", bikeName = "Xe", typeBikeID = 4 };
            var listBike = A.Fake<List<Bike>>();
            listBike.Add(bikes);
            A.CallTo(() => _manageBikeRepository.getTheBikeInformation(bikes.bikeID)).Returns(listBike);

        }
    }
}
