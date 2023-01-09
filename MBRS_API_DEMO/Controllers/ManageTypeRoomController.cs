using MBRS_API.Models;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;
using System;
using System.IO;
using System.Net.Http.Headers;
using MBRS_API.Services.Service;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_TYPE_ROOM)]
    [ApiController]
    [Authorize(Roles = "MN")]
    public class ManageTypeRoomController : Controller
    {
        public readonly IManageTypeRoomService _manageTypeRoomService;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ManageTypeRoomController(IManageTypeRoomService manageTypeRoomService,IWebHostEnvironment webHostEnvironment)
        {
            _manageTypeRoomService = manageTypeRoomService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("GetListTypeRoom")]
        public IActionResult getAllTypeRoom()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.getAllTypeRoom();
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
        [Route("GetListTypeRoomWithFilter")]
        public IActionResult getListTypeRoomWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.getAllTypeRoomWithFilter(filterName, filterValue);
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
        [Route("GetTheTypeRoomInformation")]
        public IActionResult getTheTypeRoomInformation(int typeRoomID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.getTypeRoomInformation(typeRoomID);
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



        [HttpPost]
        [Route("CreateTypeRoom")]
        public IActionResult createTypeRoom([FromBody] TypeRoom typeRoom)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.createTypeRoom(typeRoom);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_TYPE_ROOM_SUCCESS));
                }
                else if(result == -2)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_TYPE_ROOM_CODE));
                }
                else if (result == -3)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_TYPE_ROOM_NAME));
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

        [HttpPost]
        [Route("UpdateTypeRoom")]
        public IActionResult updateTypeRoom([FromBody] TypeRoom typeRoom)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.updateTheTypeRoom(typeRoom);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_TYPE_ROOM_SUCCESS));
                }
                else if (result == -2)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_TYPE_ROOM_CODE));
                }
                else if (result == -3)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_TYPE_ROOM_NAME));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_TYPE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_TYPE_ROOM_FAILED));
            }
        }

        [HttpPost]
        [Route("UpdateImageTypeRoom")]
        public IActionResult updateImageTypeRoom([FromBody] List<ItemImage> itemImage)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.updateImageTypeRoom(itemImage);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_IMAGE_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_IMAGE_TYPE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_IMAGE_TYPE_ROOM_FAILED));
            }
        }
        [HttpPost]
        [Route("UpdateAImageTypeRoom")]
        public IActionResult updateAImageTypeRoom([FromBody] ImageTypeRoom imageTypeRoom)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.updateAImageTypeRoom(imageTypeRoom);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_IMAGE_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_IMAGE_TYPE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_IMAGE_TYPE_ROOM_FAILED));
            }
        }
        [HttpDelete]
        [Route("DeleteImageTypeRoom")]
        public IActionResult deleteImageTypeRoom(int position, int typeRoomID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.deleteImageTypeRoom(position, typeRoomID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_IMAGE_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_IMAGE_TYPE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.DELETE_IMAGE_TYPE_ROOM_FAILED));
            }
        }
        [HttpDelete]
        [Route("DeleteATypeRoom")]
        public IActionResult DeleteATypeRoom(int typeRoomID, int listUtilitiesID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeRoomService.deleteTypeRoom(typeRoomID, listUtilitiesID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_TYPE_ROOM_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_TYPE_ROOM_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.DELETE_TYPE_ROOM_FAILED));
            }
        }
    }
}
