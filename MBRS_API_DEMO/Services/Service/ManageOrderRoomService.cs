using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Repositories.Repository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageOrderRoomService : IManageOrderRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageOrderRoomRepository _manageOrderRoomRepository;

        public ManageOrderRoomService(IConfiguration configuration, IManageOrderRoomRepository manageOrderRoomRepository)
        {
            this._configuration = configuration;
            _manageOrderRoomRepository = manageOrderRoomRepository;
        }
        public List<OrderRoom> getAllOrderRoom()
        {
            return _manageOrderRoomRepository.getAllOrderRoom();
        }
        public int deleteOrderRoom(int orderID)
        {
            return _manageOrderRoomRepository.deleteOrderRoom(orderID);
        }
        public List<OrderRoom> getAllOrderRoomFilter(string filterName, string filterValue)
        {
            return _manageOrderRoomRepository.getAllOrderRoomFilter(filterName, filterValue);
        }
        public int updateStatusPayment(int orderID)
        {
            return _manageOrderRoomRepository.updateStatusPayment(orderID);
        }

        public int completedBackPayment(int orderID)
        {
            return _manageOrderRoomRepository.completedBackPayment(orderID);
        }
    }
}
