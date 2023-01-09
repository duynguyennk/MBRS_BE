using MBRS_API.Models.ViewModels;
using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface IOrderFoodService
    {
        public List<Food> getAllFood();
        public int createOrderFood(List<OrderFoodInformationViewModel> orderFoodInformationViewModels);
        public List<CustomerViewModel> getCustomerInformationByID(int accountID);

        public List<CustomerViewModel> getCustomerInformationByIdentityNumber(string identityNumber);
        public List<CustomerViewModel> getCustomerInformationByRoomName(string roomName);
    }
}
