using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageOrderBikeService : IManageOrderBikeService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageOrderBikeRepository _manageOrderBikeRepository;

        public ManageOrderBikeService(IConfiguration configuration, IManageOrderBikeRepository manageOrderBikeRepository)
        {
            this._configuration = configuration;
            _manageOrderBikeRepository = manageOrderBikeRepository;
        }
        public int deleteOrderBike(int orderBikeID)
        {
            return _manageOrderBikeRepository.deleteOrderBike(orderBikeID);
        }

        public List<OrderBike> getAllOrderBike()
        {
            return _manageOrderBikeRepository.getAllOrderBike();
        }

        public List<OrderBike> getAllOrderBikeFilter(string filterName, string filterValue)
        {
            return _manageOrderBikeRepository.getAllOrderBikeFilter(filterName, filterValue);
        }

        public int updateStatusBike(StatusBike statusBike)
        {
            return _manageOrderBikeRepository.updateStatusBike(statusBike);
        }
    }
}
