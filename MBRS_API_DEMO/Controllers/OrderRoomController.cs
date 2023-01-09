using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Services.IService;
using MBRS_API.Utils;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.ORDER_ROOM)]
    [ApiController]
    public class OrderRoomController : Controller
    {
        private readonly IConfiguration _configuration;
        public readonly IOrderRoomService _orderRoomService;
        public OrderRoomController(IOrderRoomService orderRoomService, IConfiguration configuration)
        {
            _orderRoomService = orderRoomService;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetListTypeRoom")]
        public IActionResult getAllTypeRoom(DateTime checkInt, DateTime checkOut, int numberOfRoom, int numberOfChild, int numberOfAdult)
        {
            try
            {
                var result = _orderRoomService.getAllTypeRoom(checkInt, checkOut, numberOfRoom, numberOfChild, numberOfAdult);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeRoomViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_TYPE_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_TYPE_ROOM_FAILED));
            }
        }
        [HttpGet]
        [Route("GetTheTypeRoomInformation")]
        public IActionResult getTheTypeRoomInformation(int typeRoomID)
        {
            try
            {
                var result = _orderRoomService.getTypeRoomInformation(typeRoomID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeRoom>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_TYPE_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_TYPE_ROOM_FAILED));
            }
        }
        [HttpGet]
        [Route("GetRatingList")]
        public IActionResult getRatingList(int typeRoomID)
        {
            try
            {
                var result = _orderRoomService.getAllListRating(typeRoomID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<RatingViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_RATING_SUCCESS));
                }

                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_RATING_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_RATING_FAILED));
            }
        }
        [HttpGet]
        [Route("GetCustomerInformationByID")]
        public IActionResult getCustomerInformationByID(int accountID)
        {
            try
            {
                var result = _orderRoomService.getCustomerInformationByID(accountID);
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
                var result = _orderRoomService.getCustomerInformationByIdentityNumber(identifyNumber);
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
        [Route("GetRatingPercent")]
        public IActionResult getRatingPercent(int typeRoomID)
        {
            try
            {
                var result = _orderRoomService.getRatingPercent(typeRoomID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<RatingPercentViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_RATING_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_RATING_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_RATING_FAILED));
            }
        }

        [HttpGet]
        [Route("Payment")]
        public IActionResult paymentRoom(string price, string nameTypeRoom, int numberOfRooms, int numberOfDays)
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
                vnpay.AddRequestData("vnp_OrderInfo", "Trả tiền " + numberOfRooms + " phòng " + nameTypeRoom + " " + numberOfDays + " đêm");
                vnpay.AddRequestData("vnp_OrderType", "100000");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", "RPB" + randomCodeOrder);
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
        [Route("PaymentDeposit")]
        public IActionResult paymentDeposit(string price, string nameTypeRoom, int numberOfRooms, int numberOfDays)
        {
            try
            {
                string vnp_Returnurl = _configuration.GetSection("VNPayInfo").GetSection("vnp_Returnurl").Value;
                string vnp_Url = _configuration.GetSection("VNPayInfo").GetSection("vnp_Url").Value;
                string vnp_TmnCode = _configuration.GetSection("VNPayInfo").GetSection("vnp_TmnCode").Value;
                string vnp_HashSecret = _configuration.GetSection("VNPayInfo").GetSection("vnp_HashSecret").Value;
                string randomCodeOrder = Common.RandomString();
                double priceDeposit = (Convert.ToDouble(price) * 30) / 100;
                if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                {
                    return Json("Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file appsetting.json");
                }
                string locale = "vn";

                VnPayLibrary vnpay = new VnPayLibrary();
                vnpay.AddRequestData("vnp_Version", "2.1.0");
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", priceDeposit + "00");
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress());
                vnpay.AddRequestData("vnp_Locale", locale);
                vnpay.AddRequestData("vnp_OrderInfo", "Trả trước 30 phần trăm số tiền " + numberOfRooms + " phòng " + nameTypeRoom + " " + numberOfDays + " đêm");
                vnpay.AddRequestData("vnp_OrderType", "100000");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", "RPD" + randomCodeOrder);
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
        [Route("CreateOrderRoom")]
        public IActionResult createOrderRoom([FromBody] OrderRoomInformationViewModel orderInformationViewModel)
        {
            try
            {
                var result = _orderRoomService.createOrderRoom(orderInformationViewModel);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
            }
        }

        [HttpPost]
        [Route("CreateOrderRoomReceptionist")]
        public IActionResult createOrderRoomReceptionist([FromBody] OrderRoomInformationViewModel orderInformationViewModel)
        {
            try
            {
                var result = _orderRoomService.createOrderRoomReceptionist(orderInformationViewModel);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_TYPE_ROOM_FAILED));
            }
        }

        [HttpPost]
        [Route("CreateOrderRoomUnpayment")]
        public IActionResult createOrderRoomUnpayment([FromBody] OrderRoomUnpayment orderRoomUnpayment)
        {
            try
            {
                var result = _orderRoomService.createOrderRoomUnpayment(orderRoomUnpayment);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
            }
        }
        [HttpPost]
        [Route("CreateOrderRoomUnpaymentForCustomer")]
        public IActionResult createOrderRoomUnpaymentForCustomer([FromBody] OrderRoomUnpayment orderRoomUnpayment)
        {
            try
            {
                var result = _orderRoomService.createOrderRoomUnpaymentForCustomer(orderRoomUnpayment);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
            }
        }
        [HttpPost]
        [Route("CreateOrderRoomCash")]
        public IActionResult createOrderRoomCash([FromBody] OrderRoomUnpayment orderRoomUnpayment)
        {
            try
            {
                var result = _orderRoomService.createOrderRoomCash(orderRoomUnpayment);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
            }
        }
        [HttpPost]
        [Route("CreateOrderRoomForCustomer")]
        public IActionResult createOrderRoomForCustomer([FromBody] OrderRoomInformationViewModel orderInformationViewModel)
        {
            try
            {
                var result = _orderRoomService.createOrderRoomForCustomer(orderInformationViewModel);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_ORDER_ROOM_FAILED));
            }
        }

    }
}
