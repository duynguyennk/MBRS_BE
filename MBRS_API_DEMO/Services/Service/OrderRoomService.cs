using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MBRS_API.Services.Service
{
    public class OrderRoomService : IOrderRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRoomRepository _orderRoomRepository;

        public OrderRoomService(IConfiguration configuration, IOrderRoomRepository orderRoomRepository)
        {
            this._configuration = configuration;
            _orderRoomRepository = orderRoomRepository;
        }
        public List<TypeRoom> getTypeRoomInformation(int typeRoomID)
        {
            return _orderRoomRepository.getTypeRoomInformation(typeRoomID);
        }    
        public List<TypeRoomViewModel> getAllTypeRoom(DateTime checkInt, DateTime checkOut, int numberOfRoom, int numberOfChild ,int numberOfAdult)
        {
            return _orderRoomRepository.getAllTypeRoom(checkInt, checkOut, numberOfRoom, numberOfChild, numberOfAdult);
        }
        public List<RatingPercentViewModel> getRatingPercent(int typeRoomID)
        {
            return _orderRoomRepository.getRatingPercent(typeRoomID);
        }
        public List<RatingViewModel> getAllListRating(int typeRoomID)
        {
            return _orderRoomRepository.getAllListRating(typeRoomID);
        }
        public int createOrderRoom(OrderRoomInformationViewModel orderInformationViewModel)
        {
            return _orderRoomRepository.createOrderRoom(orderInformationViewModel);
        }
        public List<CustomerViewModel> getCustomerInformationByID(int accountID)
        {
            return _orderRoomRepository.getCustomerInformationByID(accountID);
        }

        public int createOrderRoomForCustomer(OrderRoomInformationViewModel orderInformationViewModel)
        {
            return _orderRoomRepository.createOrderRoomForCustomer(orderInformationViewModel);
        }
        public int createOrderRoomUnpayment(OrderRoomUnpayment orderRoomUnpayment)
        {
            return _orderRoomRepository.createOrderRoomUnpayment(orderRoomUnpayment);
        }
        public int createOrderRoomUnpaymentForCustomer(OrderRoomUnpayment orderRoomUnpayment)
        {
            return _orderRoomRepository.createOrderRoomUnpaymentForCustomer(orderRoomUnpayment);
        }

        public List<CustomerViewModel> getCustomerInformationByIdentityNumber(string identityNumber)
        {
            return _orderRoomRepository.getCustomerInformationByIdentityNumber(identityNumber);
        }

        public int createOrderRoomReceptionist(OrderRoomInformationViewModel orderInformationViewModel)
        {
            return _orderRoomRepository.createOrderRoomReceptionist(orderInformationViewModel);
        }

        public int createOrderRoomCash(OrderRoomUnpayment orderRoomUnpayment)
        {
            return _orderRoomRepository.createOrderRoomCash(orderRoomUnpayment);
        }
    }
}
