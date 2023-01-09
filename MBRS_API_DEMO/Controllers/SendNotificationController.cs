using MBRS_API.Models;
using MBRS_API.Services.IService;
using MBRS_API.SignalR;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.NOTIFICATION)]
    [ApiController]
    public class SendNotificationController : Controller
    {
        private readonly IHubContext<ChatHub> _hub;
        private readonly TimerManager _timer;
        public readonly ISendNotificationService _sendNotificationService;
        public readonly IManageActivityEmployeeService _manageActivityEmployeeService;

        public SendNotificationController(IHubContext<ChatHub> hub, TimerManager timer, ISendNotificationService sendNotificationService, IManageActivityEmployeeService manageActivityEmployeeService)
        {
            _hub = hub;
            _timer = timer;
            _sendNotificationService = sendNotificationService;
            _manageActivityEmployeeService = manageActivityEmployeeService;
        }
        [HttpGet]
        [Route("GetNotification")]
        public IActionResult getNotification()
        {
            try
            {
                var result = _sendNotificationService.getAllOrderNotificationReceptionist();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<NotificationBell>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_NOTIFICATION_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_NOTIFICATION_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_NOTIFICATION_FAILED));
            }
        }

        [HttpGet]
        [Route("SendNotification")]
        public IActionResult sendNotification()
        {
            if (!_timer.IsTimerStarted)
                _timer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferChartData", _sendNotificationService.getAllOrderNotificationReceptionist()));
            return Ok(new { Message = "Request Completed" });
        }

        [HttpGet]
        [Route("SendActivityEmployee")]
        public IActionResult sendActivityEmployee()
        {
            if (!_timer.IsTimerStarted)
                _timer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferActivityData", _manageActivityEmployeeService.getAllActivityEmployee()));
            return Ok(new { Message = "Request Completed" });
        }

        [HttpGet]
        [Route("UpdateOrderNotificationReceptionist")]
        public IActionResult updateOrderNotificationReceptionist(int notificationID)
        {
            try
            {
                var result = _sendNotificationService.UpdateOrderNotificationReceptionist(notificationID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_NOTIFICATION_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_NOTIFICATION_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_NOTIFICATION_FAILED));
            }
        }

    }
}
