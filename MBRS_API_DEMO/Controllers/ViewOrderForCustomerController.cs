using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.VIEW_ORDER_CUSTOMER)]
    [ApiController]
    [Authorize(Roles = "CS")]
    public class ViewOrderForCustomerController : Controller
    {
        public readonly IViewOrderForCustomerService _viewOrderForCustomerService;
        public ViewOrderForCustomerController(IViewOrderForCustomerService viewOrderForCustomerService)
        {
            _viewOrderForCustomerService = viewOrderForCustomerService;
        }
        [HttpGet]
        [Route("GetAllOrderRoom")]
        public IActionResult getAllOrderRoom(int customerID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _viewOrderForCustomerService.getAllOrderRoom(customerID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<CustomerOrderRoom>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ORDER_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ORDER_ROOM_FAILED));
            }
        }
        [HttpGet]
        [Route("GetAllOrderBike")]
        public IActionResult getAllOrderBike(int customerID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _viewOrderForCustomerService.getAllOrderBike(customerID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<CustomerOrderBike>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_BIKE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ORDER_BIKE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ORDER_BIKE_FAILED));
            }
        }
        [HttpGet]
        [Route("GetAllOrderFood")]
        public IActionResult getAllOrderFood(int customerID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _viewOrderForCustomerService.getAllOrderFood(customerID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<CustomerOrderFood>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ORDER_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ORDER_FOOD_FAILED));
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
                var result = _viewOrderForCustomerService.getCustomerInformationByID(accountID);
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
        [Route("CancelOrderRoomCustomer")]
        public IActionResult cancelOrderRoomCustomer([FromBody] CancelOrderRoomCustomer cancelOrderRoomCustomer)
        {
            try
            {
                var result = _viewOrderForCustomerService.cancelOrderRoom(cancelOrderRoomCustomer);
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
