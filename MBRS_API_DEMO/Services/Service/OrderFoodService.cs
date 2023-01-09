using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class OrderFoodService : IOrderFoodService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderFoodRepository _orderFoodRepository;

        public OrderFoodService(IConfiguration configuration, IOrderFoodRepository orderFoodRepository)
        {
            this._configuration = configuration;
            _orderFoodRepository = orderFoodRepository;
        }
        public int createOrderFood(List<OrderFoodInformationViewModel> orderFoodInformationViewModels)
        {
            return _orderFoodRepository.createOrderFood(orderFoodInformationViewModels);
        }

        public List<Food> getAllFood()
        {
            return _orderFoodRepository.getAllFood();
        }

        public List<CustomerViewModel> getCustomerInformationByID(int accountID)
        {
            return _orderFoodRepository.getCustomerInformationByID(accountID);
        }

        public List<CustomerViewModel> getCustomerInformationByIdentityNumber(string identityNumber)
        {
            return _orderFoodRepository.getCustomerInformationByIdentityNumber(identityNumber);
        }
        public List<CustomerViewModel> getCustomerInformationByRoomName(string roomName)
        {
            return _orderFoodRepository.getCustomerInformationByRoomName(roomName);
        }    
    }
}
