using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ViewStatusRoomService : IViewStatusRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IViewStatusRoomRepository _viewStatusRoomRepository;

        public ViewStatusRoomService(IConfiguration configuration, IViewStatusRoomRepository viewStatusRoomRepository)
        {
            this._configuration = configuration;
            _viewStatusRoomRepository = viewStatusRoomRepository;
        }
        public List<StatusRoomViewModel> getAllRoom(DateTime selectDate)
        {
            return _viewStatusRoomRepository.getAllRoom(selectDate);
        }
        public List<Floor> getAllFloor()
        {
            return _viewStatusRoomRepository.getAllFloor();
        }

        public NumberRoomStatusViewModel getNumberOfRoomStatus(DateTime selectDate)
        {
           int numberRoomCheckIn = _viewStatusRoomRepository.getCountRoomCheckIn(selectDate);
           int numberRoomEmpty = _viewStatusRoomRepository.getCountRoomEmpty(selectDate);
           int numberRoomHaveOrder = _viewStatusRoomRepository.getCountRoomHaveOrder(selectDate);
            return new NumberRoomStatusViewModel(numberRoomCheckIn, numberRoomHaveOrder, numberRoomEmpty);
        }

        public int updateStatusRoom(int valueStatus, int orderID)
        {
            return _viewStatusRoomRepository.updateStatusRoom(valueStatus, orderID);
        }
    }
}
