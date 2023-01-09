using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class OrderBikeService : IOrderBikeService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderBikeRepository _orderBikeRepository;

        public OrderBikeService(IConfiguration configuration, IOrderBikeRepository orderBikeRepository)
        {
            this._configuration = configuration;
            _orderBikeRepository = orderBikeRepository;
        }

        public int createOrderBike(OrderBikeInformationViewModel orderBikeInformationViewModel)
        {
            return _orderBikeRepository.createOrderBike(orderBikeInformationViewModel);
        }

        public List<TypeBike> getAllTypeBike(DateTime dateGet, string hoursGet, int hoursRent, int quantity)
        {
            return _orderBikeRepository.getAllTypeBike(dateGet,hoursGet,hoursRent,quantity);
        }

        public List<CustomerViewModel> getCustomerInformationByID(int accountID)
        {
            return _orderBikeRepository.getCustomerInformationByID(accountID);
        }

        public List<CustomerViewModel> getCustomerInformationByRoomName(string roomName)
        {
            return _orderBikeRepository.getCustomerInformationByRoomName(roomName);
        }
    }
}
