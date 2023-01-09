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
    [Route(IRoutes.MANAGE_FOOD)]
    [ApiController]
    [Authorize(Roles = "MN")]
    public class ManageFoodController : Controller
    {
        public readonly IManageFoodService _manageFoodService;
        public ManageFoodController(IManageFoodService manageFoodService)
        {
            _manageFoodService = manageFoodService;
        }

        [HttpGet]
        [Route("GetListFood")]
        public IActionResult getAllFood()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.getAllFood();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<FoodViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllTypeFood")]
        public IActionResult getAllTypeRoom()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.getAllTypeFood();
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
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetListFoodWithFilter")]
        public IActionResult getListFoodWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.getAllFoodWithFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<FoodViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_FOOD_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_FOOD_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetTheFoodInformation")]
        public IActionResult getTheFoodInformation(int foodID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.getFoodInformation(foodID);
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
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }



        [HttpPost]
        [Route("CreateFood")]
        public IActionResult createFood([FromBody] Food food)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.createFood(food);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_FOOD_SUCCESS));
                }
                else if (result == -2)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_FOOD_CODE));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_FOOD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateImageFood")]
        public IActionResult updateImageFood([FromBody] ItemImage itemImage)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.updateImageFood(itemImage);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_FOOD_SUCCESS));
                }
                else if (result == -2)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DUPLICATE_FOOD_CODE));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_FOOD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateFood")]
        public IActionResult updateFood([FromBody] Food food)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.updateTheFood(food);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_FOOD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_FOOD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }


        [HttpDelete]
        [Route("DeleteAFood")]
        public IActionResult DeleteAFood(int foodID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFoodService.deleteFood(foodID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_FOOD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_FOOD_FAILED));
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
