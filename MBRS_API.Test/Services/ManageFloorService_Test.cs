using FakeItEasy;
using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBRS_API.Tests.Services
{
    public class ManageFloorService_Test
    {
        private IConfiguration _configuration;
        private IManageFloorRepository _manageFloorRepository;
        private ManageFloorService _manageFloorService;

        public ManageFloorService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageFloorRepository = A.Fake<IManageFloorRepository>();
            _manageFloorService = new ManageFloorService(_configuration, _manageFloorRepository);
        }

        [Fact]
        public void ManageFloorService_getAllFloor_ReturnEqualCount()
        {
            var floor = A.Fake<Floor>();
            floor = new Floor
            {
                floorID = 1,
                floorCode = "T1",
                floorName = "Tầng 1",
                numberFloor = 1
            };
            var listFloor = A.Fake<List<Floor>>();
            listFloor.Add(floor);
            A.CallTo(() => _manageFloorRepository.getAllFloor()).Returns(listFloor);
            var result = _manageFloorService.getAllFloor();
            Assert.Equal(listFloor.Count, result.Count);
        }

        [Fact]
        public void ManageFloorService_getAllFloorWithFilter_ReturnEqualCount()
        {
            var floor = A.Fake<Floor>();
            floor = new Floor
            {
                floorID = 1,
                floorCode = "T1",
                floorName = "Tầng 1",
                numberFloor = 1
            };
            var listFloor = A.Fake<List<Floor>>();
            listFloor.Add(floor);
            A.CallTo(() => _manageFloorRepository.getAllFloorWithFilter(floor.floorCode,"Mã tầng")).Returns(listFloor);
            var result = _manageFloorService.getAllFloorWithFilter(floor.floorCode, "Mã tầng");
            Assert.Equal(listFloor.Count, result.Count);
        }

        [Fact]
        public void ManageFloorService_deleteFloor_ReturnSuccess()
        {
            var floor = A.Fake<Floor>();
            floor = new Floor
            {
                floorID = 1,
                floorCode = "T1",
                floorName = "Tầng 1",
                numberFloor = 1
            };
            var listFloor = A.Fake<List<Floor>>();
            listFloor.Add(floor);
            A.CallTo(() => _manageFloorRepository.deleteFloor(floor.floorID)).Returns(1);
            var result = _manageFloorService.deleteFloor(floor.floorID);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageFloorService_updateTheFloor_ReturnSuccess()
        {
            var floor = A.Fake<Floor>();
            floor = new Floor
            {
                floorID = 1,
                floorCode = "T1",
                floorName = "Tầng 1",
                numberFloor = 1
            };
            var listFloor = A.Fake<List<Floor>>();
            listFloor.Add(floor);
            A.CallTo(() => _manageFloorRepository.updateTheFloor(floor)).Returns(1);
            var result = _manageFloorService.updateTheFloor(floor);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageFloorService_getFloorInformation_ReturnEqualCount()
        {
            var floor = A.Fake<Floor>();
            floor = new Floor
            {
                floorID = 1,
                floorCode = "T1",
                floorName = "Tầng 1",
                numberFloor = 1
            };
            var listFloor = A.Fake<List<Floor>>();
            listFloor.Add(floor);
            A.CallTo(() => _manageFloorRepository.getFloorInformation(floor.floorID)).Returns(listFloor);
            var result = _manageFloorService.getFloorInformation(floor.floorID);
            Assert.Equal(listFloor.Count, result.Count);
        }

        [Fact]
        public void ManageFloorService_createFloor_ReturnSuccess()
        {
            var floor = A.Fake<Floor>();
            floor = new Floor
            {
                floorCode = "T1",
                floorName = "Tầng 1",
                numberFloor = 1
            };
            var listFloor = A.Fake<List<Floor>>();
            listFloor.Add(floor);
            A.CallTo(() => _manageFloorRepository.createFloor(floor)).Returns(1);
            var result = _manageFloorService.createFloor(floor);
            Assert.Equal(1, result);
        }
    }
}
