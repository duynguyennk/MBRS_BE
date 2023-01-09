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
    [Route(IRoutes.MANAGE_ROOM)]
    [ApiController]
    [Authorize(Roles = "MN")]
    public class ManageRoomController : Controller
    {
        public readonly IManageRoomService _manageRoomService;

        public ManageRoomController(IManageRoomService manageRoomService)
        {
            _manageRoomService = manageRoomService;
        }

        [HttpGet]
        [Route("GetListRoom")]
        public IActionResult getAllRoom()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.getAllRoom();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<RoomViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllTypeRoom")]
        public IActionResult getAllTypeRoom()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.getAllTypeRoom();
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
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllFloor")]
        public IActionResult getAllFloor()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.getAllFloor();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Floor>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_FLOOR_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_FLOOR_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetListRoomWithFilter")]
        public IActionResult getListRoomWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.getAllRoomWithFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<RoomViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetTheRoomInformation")]
        public IActionResult getTheRoomInformation(int roomID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.getTheRoomInformation(roomID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<RoomViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetDetailInformationRoom")]
        public IActionResult getDetailInformationRoom(int typeRoomID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.getDetailInformationRoom(typeRoomID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeRoom>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ROOM_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ROOM_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("CreateRoom")]
        public IActionResult createRoom(RoomInformation roomInformation)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.createRoom(roomInformation);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_ROOM_SUCCESS));
                }
                else if(result == -2)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_ROOM_CODE));
                }
                else if( result == -3)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_ROOM_NAME));
                }    
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateRoom")]
        public IActionResult updateRoom(RoomInformation roomInformation)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.updateTheRoom(roomInformation);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_ROOM_SUCCESS));
                }
                else if (result == -2)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_ROOM_CODE));
                }
                else if (result == -3)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_ROOM_NAME));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteARoom")]
        public IActionResult DeleteARoom(int roomID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageRoomService.deleteRoom(roomID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_ROOM_FAILED));
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



