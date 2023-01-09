using MBRS_API.Models;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_ORDER_FOOD)]
    [ApiController]
    [Authorize(Roles = "LT")]
    public class ManageOrderFoodController : Controller
    {
        public readonly IManageOrderFoodService _manageOrderFoodService;
        public ManageOrderFoodController(IManageOrderFoodService manageOrderFoodService)
        {
            _manageOrderFoodService = manageOrderFoodService;
        }
        [HttpGet]
        [Route("GetListOrderFood")]
        public IActionResult getListOrderFood()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderFoodService.getAllOrderFood();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<OrderFood>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ORDER_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetListOrderFoodWithFilter")]
        public IActionResult getListOrderFoodWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderFoodService.getAllOrderFoodFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<OrderFood>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_FOOD_SUCCESS));
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
        [HttpPost]
        [Route("UpdateStatusFood")]
        public IActionResult updateStatusFood([FromBody] StatusFood statusFood)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderFoodService.updateStatusFood(statusFood);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_ORDER_FOOD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_ORDER_FOOD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.UPDATE_ORDER_FOOD_FAILED));
            }
        }
    }
}
