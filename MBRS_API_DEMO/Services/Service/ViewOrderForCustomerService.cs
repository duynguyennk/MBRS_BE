using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ViewOrderForCustomerService : IViewOrderForCustomerService
    {
        private readonly IConfiguration _configuration;
        private readonly IViewOrderForCustomerRepository _viewOrderForCustomerRepository;

        public ViewOrderForCustomerService(IConfiguration configuration, IViewOrderForCustomerRepository viewOrderForCustomerRepository)
        {
            this._configuration = configuration;
            _viewOrderForCustomerRepository = viewOrderForCustomerRepository;
        }

        public List<CustomerOrderFood> getAllOrderFood(int customerID)
        {
            return _viewOrderForCustomerRepository.getAllOrderFood(customerID);
        }

        public List<CustomerOrderRoom> getAllOrderRoom(int customerID)
        {
            return _viewOrderForCustomerRepository.getAllOrderRoom(customerID);
        }

        public List<CustomerViewModel> getCustomerInformationByID(int accountID)
        {
            return _viewOrderForCustomerRepository.getCustomerInformationByID(accountID);
        }
        public List<CustomerOrderBike> getAllOrderBike(int customerID)
        {
            return _viewOrderForCustomerRepository.getAllOrderBike(customerID);
        }

        public int cancelOrderRoom(CancelOrderRoomCustomer cancelOrderRoomCustomer)
        {
            return _viewOrderForCustomerRepository.cancelOrderRoom(cancelOrderRoomCustomer);
        }
    }
}
