using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using MBRS_API.Models.ViewModels;
using MBRS_API.Models;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using MBRS_API.Services.Service;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_TYPE_BIKE)]
    [ApiController]
    [Authorize(Roles = "MN")]
    public class ManageTypeBikeController : Controller
    {

        public readonly IManageTypeBikeService _manageTypeBikeService;
        public ManageTypeBikeController(IManageTypeBikeService manageTypeBikeService)
        {
            _manageTypeBikeService = manageTypeBikeService;
        }

        [HttpGet]
        [Route("GetListTypeBike")]
        public IActionResult getAllTypeBike()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeBikeService.getAllTypeBike();
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
        [Route("GetListTypeBikeWithFilter")]
        public IActionResult getListTypeBikeWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeBikeService.getAllTypeBikeWithFilter(filterName, filterValue);
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
        [Route("GetTheTypeBikeInformation")]
        public IActionResult getTheTypeBikeInformation(int typeBikeID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeBikeService.getTypeBikeInformation(typeBikeID);
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

        [HttpPost]
        [Route("UpdateImageTypeBike")]
        public IActionResult updateImageTypeBike([FromBody] ItemImage itemImage)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeBikeService.updateImageTypeBike(itemImage);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_IMAGE_TYPE_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_IMAGE_TYPE_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_IMAGE_TYPE_BIKE_FAILED));
            }
        }

        [HttpPost]
        [Route("CreateTypeBike")]
        public IActionResult createTypeBike([FromBody] TypeBike typeBike)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeBikeService.createTypeBike(typeBike);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_TYPE_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_TYPE_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_TYPE_BIKE_FAILED));
            }
        }

        [HttpPost]
        [Route("UpdateTypeBike")]
        public IActionResult updateTypeBike([FromBody] TypeBike typeBike)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeBikeService.updateTheTypeBike(typeBike);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_TYPE_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_TYPE_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_TYPE_BIKE_FAILED));
            }
        }


        [HttpDelete]
        [Route("DeleteATypeBike")]
        public IActionResult DeleteATypeBike(int typeBikeID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeBikeService.deleteTypeBike(typeBikeID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_TYPE_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_TYPE_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.DELETE_TYPE_BIKE_FAILED));
            }
        }


    }
}
