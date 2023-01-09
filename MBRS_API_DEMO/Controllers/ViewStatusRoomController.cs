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
    [Route(IRoutes.VIEW_STATUS_ROOM)]
    [ApiController]
    [Authorize(Roles = "LT")]
    public class ViewStatusRoomController : Controller
    {
        public readonly IViewStatusRoomService _viewStatusRoomService;
        public ViewStatusRoomController(IViewStatusRoomService viewStatusRoomService)
        {
            _viewStatusRoomService = viewStatusRoomService;
        }

        [HttpGet]
        [Route("GetListRoom")]
        public IActionResult getListRoom(DateTime selectDate)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _viewStatusRoomService.getAllRoom(selectDate);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<StatusRoomViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ROOM_FAILED));
            }
        }

        [HttpGet]
        [Route("GetNumberRoomStatus")]
        public IActionResult getNumberRoomStatus(DateTime selectDate)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _viewStatusRoomService.getNumberOfRoomStatus(selectDate);
                if (result != null)
                {
                    return Ok(new BaseResponse<NumberRoomStatusViewModel>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ROOM_FAILED));
            }
        }

        [HttpGet]
        [Route("GetListFloor")]
        public IActionResult getListFloor()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _viewStatusRoomService.getAllFloor();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Floor>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ROOM_FAILED));
            }
        }

        [HttpGet]
        [Route("UpdateStatusRoom")]
        public IActionResult updateStatusRoom(int valueStatus, int orderID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _viewStatusRoomService.updateStatusRoom(valueStatus, orderID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ROOM_FAILED));
            }
        }
    }
}
