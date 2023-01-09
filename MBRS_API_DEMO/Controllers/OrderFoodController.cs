using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using MBRS_API.Services.IService;
using MBRS_API.Models.ViewModels;
using MBRS_API.Services.Service;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using MBRS_API.Models;
using MBRS_API.Utils;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.ORDER_FOOD)]
    [ApiController]
    [Authorize(Roles = "CS,LT")]
    public class OrderFoodController : Controller
    {
        private readonly IConfiguration _configuration;
        public readonly IOrderFoodService _orderFoodService;
        public OrderFoodController(IOrderFoodService orderFoodService, IConfiguration configuration)
        {
            _orderFoodService = orderFoodService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetListFood")]
        public IActionResult getListFood()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _orderFoodService.getAllFood();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Food>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_FOOD_FAILED));
            }
        }
        [HttpGet]
        [Route("CreateLinkPaymentFoodOrder")]
        public IActionResult createLinkPaymentFoodOrder(string price)
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
                vnpay.AddRequestData("vnp_OrderInfo", "Trả tiền đồ ăn");
                vnpay.AddRequestData("vnp_OrderType", "100000");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", "F" + randomCodeOrder);
                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                return Ok(new BaseResponse<String>(paymentUrl));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest();
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
                var result = _orderFoodService.getCustomerInformationByID(accountID);
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
        [Route("GetCustomerInformationByIdentity")]
        public IActionResult getCustomerInformationByIdentity(string identifyNumber)
        {
            try
            {
                var result = _orderFoodService.getCustomerInformationByIdentityNumber(identifyNumber);
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
        [Route("GetCustomerInformationByRoomName")]
        public IActionResult getCustomerInformationByRoomName(string roomName)
        {
            try
            {
                var result = _orderFoodService.getCustomerInformationByRoomName(roomName);
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
        [HttpPost]
        [Route("CreateOrderFood")]
        public IActionResult createOrderFood([FromBody] List<OrderFoodInformationViewModel> orderFoodInformationViewModel)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _orderFoodService.createOrderFood(orderFoodInformationViewModel);
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
