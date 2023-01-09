using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Services.IService;
using MBRS_API.Services.Service;
using MBRS_API.Utils;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.ORDER_BIKE)]
    [ApiController]
    [Authorize(Roles = "CS,LT")]
    public class OrderBikeController : Controller
    {
        private readonly IConfiguration _configuration;
        public readonly IOrderBikeService _orderBikeService;
        public OrderBikeController(IOrderBikeService orderBikeService, IConfiguration configuration)
        {
            _orderBikeService = orderBikeService;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetListTypeBike")]
        public IActionResult getAllTypeBike(DateTime dateGet, string hoursGet, int hoursRent, int quantity)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _orderBikeService.getAllTypeBike(dateGet,hoursGet,hoursRent,quantity);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeBike>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_TYPE_BIKE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_TYPE_BIKE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_TYPE_BIKE_FAILED));
            }
        }
        [HttpGet]
        [Route("GetCustomerInformationByRoomName")]
        public IActionResult getCustomerInformationByRoomName(string roomName)
        {
            try
            {
                var result = _orderBikeService.getCustomerInformationByRoomName(roomName);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<CustomerViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_CUSTOMER_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_CUSTOMER_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_CUSTOMER_FAILED));
            }
        }
        [HttpGet]
        [Route("GetCustomerInformationByID")]
        public IActionResult getCustomerInformationByID(int accountID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _orderBikeService.getCustomerInformationByID(accountID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<CustomerViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_CUSTOMER_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_CUSTOMER_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_CUSTOMER_FAILED));
            }
        }
        [HttpGet]
        [Route("CreateLinkPaymentBikeOrder")]
        public IActionResult createLinkPaymentBikeOrder(string price, int numberHoursRent,string typeBikeName,int numberOfBike)
        {
            try
            {
                string vnp_Returnurl = _configuration.GetSection("VNPayInfo").GetSection("vnp_Returnurl").Value;
                string vnp_Url = _configuration.GetSection("VNPayInfo").GetSection("vnp_Url").Value;
                string vnp_TmnCode = _configuration.GetSection("VNPayInfo").GetSection("vnp_TmnCode").Value;
                string vnp_HashSecret = _configuration.GetSection("VNPayInfo").GetSection("vnp_HashSecret").Value;
                string randomCodeOrder = Common.RandomString();
                if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                {
                    return Json("Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file appsetting.json");
                }
                string locale = "vn";

                VnPayLibrary vnpay = new VnPayLibrary();
                vnpay.AddRequestData("vnp_Version", "2.1.0");
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", price + "00");
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress());
                vnpay.AddRequestData("vnp_Locale", locale);
                vnpay.AddRequestData("vnp_OrderInfo", "Trả tiền thuê "+numberOfBike+" "+typeBikeName+" trong "+numberHoursRent+" tiếng");
                vnpay.AddRequestData("vnp_OrderType", "100000");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", "B"+randomCodeOrder);
                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                return Ok(new BaseResponse<String>(paymentUrl));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("CreateOrderBike")]
        public IActionResult createOrderBike([FromBody] OrderBikeInformationViewModel orderBikeInformationViewModel)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _orderBikeService.createOrderBike(orderBikeInformationViewModel);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_TYPE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_TYPE_ROOM_FAILED));
            }
        }
    }
}
