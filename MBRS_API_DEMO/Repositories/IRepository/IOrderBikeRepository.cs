using MBRS_API.Models.ViewModels;
using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IOrderBikeRepository
    {
        public List<TypeBike> getAllTypeBike(DateTime dateGet, string hoursGet, int hoursRent, int quantity);
        public int createOrderBike(OrderBikeInformationViewModel orderBikeInformationViewModel);
        public List<CustomerViewModel> getCustomerInformationByID(int accountID);

        public List<CustomerViewModel> getCustomerInformationByRoomName(string roomName);
    }
}
