using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Services.IService;
using MBRS_API.Services.Service;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_ORDER_ROOM)]
    [ApiController]
    [Authorize(Roles = "LT")]
    public class ManageOrderRoomController : Controller
    {
        public readonly IManageOrderRoomService _manageOrderRoomService;
        public ManageOrderRoomController(IManageOrderRoomService manageOrderRoomService)
        {
            _manageOrderRoomService = manageOrderRoomService;
        }
        [HttpGet]
        [Route("GetListOrderRoom")]
        public IActionResult getAllOrderRoom()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderRoomService.getAllOrderRoom();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<OrderRoom>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ORDER_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetListOrderRoomWithFilter")]
        public IActionResult getListOrderRoomWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderRoomService.getAllOrderRoomFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<OrderRoom>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ORDER_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("UpdateStatusPayment")]
        public IActionResult updateStatusPayment(int orderID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderRoomService.updateStatusPayment(orderID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("CompletedBackPayment")]
        public IActionResult completedBackPayment(int orderID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderRoomService.completedBackPayment(orderID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteAOrderRoom")]
        public IActionResult deleteAOrderRoom(int orderID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderRoomService.deleteOrderRoom(orderID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_ORDER_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_ORDER_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
    }
}
