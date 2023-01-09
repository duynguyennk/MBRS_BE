using MBRS_API.Models;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_TYPE_FOOD)]
    [ApiController]
    [Authorize(Roles = "MN")]
    public class ManageTypeFoodController : Controller
    {
        public readonly IManageTypeFoodService _manageTypeFoodService;
        public ManageTypeFoodController(IManageTypeFoodService manageTypeFoodService)
        {
            _manageTypeFoodService = manageTypeFoodService;
        }

        [HttpGet]
        [Route("GetListTypeFood")]
        public IActionResult getAllTypeFood()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeFoodService.getAllTypeFood();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeFood>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_TYPE_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_TYPE_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_TYPE_FOOD_FAILED));
            }
        }

        [HttpGet]
        [Route("GetListTypeFoodWithFilter")]
        public IActionResult getListTypeFoodWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeFoodService.getAllTypeFoodWithFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeFood>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_TYPE_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_TYPE_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_TYPE_FOOD_FAILED));
            }
        }

        [HttpGet]
        [Route("GetTheTypeFoodInformation")]
        public IActionResult getTheTypeFoodInformation(int typeFoodID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeFoodService.getTypeFoodInformation(typeFoodID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeFood>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_TYPE_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_TYPE_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_TYPE_FOOD_FAILED));
            }
        }



        [HttpPost]
        [Route("CreateTypeFood")]
        public IActionResult createTypeFood([FromBody] TypeFood typeFood)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeFoodService.createTypeFood(typeFood);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_TYPE_FOOD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_TYPE_FOOD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.CREATE_TYPE_FOOD_FAILED));
            }
        }

        [HttpPost]
        [Route("UpdateTypeFood")]
        public IActionResult updateTypeFood([FromBody] TypeFood typeFood)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeFoodService.updateTheTypeFood(typeFood);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_TYPE_FOOD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_TYPE_FOOD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_TYPE_FOOD_FAILED));
            }
        }


        [HttpDelete]
        [Route("DeleteATypeFood")]
        public IActionResult DeleteATypeFood(int typeFoodID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageTypeFoodService.deleteTypeFood(typeFoodID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_TYPE_FOOD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_TYPE_FOOD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.DELETE_TYPE_FOOD_FAILED));
            }
        }

    }
}
