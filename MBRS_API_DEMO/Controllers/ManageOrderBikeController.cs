using MBRS_API.Models;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_ORDER_ROOM)]
    [ApiController]
    [Authorize(Roles = "LT")]
    public class ManageOrderBikeController : Controller
    {
        public readonly IManageOrderBikeService _manageOrderBikeService;
        public ManageOrderBikeController(IManageOrderBikeService manageOrderBikeService)
        {
            _manageOrderBikeService = manageOrderBikeService;
        }

        [HttpGet]
        [Route("GetListOrderBike")]
        public IActionResult getAllOrderBike()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderBikeService.getAllOrderBike();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<OrderBike>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_BIKE_SUCCESS));
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
        [Route("GetListOrderBikeWithFilter")]
        public IActionResult getListOrderBikeWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderBikeService.getAllOrderBikeFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<OrderBike>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_BIKE_SUCCESS));
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

        [HttpPost]
        [Route("UpdateStatusBike")]
        public IActionResult updateStatusBike([FromBody] StatusBike statusBike)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderBikeService.updateStatusBike(statusBike);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ORDER_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.LIST_ORDER_BIKE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ORDER_BIKE_FAILED));
            }
        }

        [HttpDelete]
        [Route("DeleteAOrderBike")]
        public IActionResult deleteAOrderBike(int orderBikeID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageOrderBikeService.deleteOrderBike(orderBikeID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_ORDER_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_ORDER_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.DELETE_ORDER_BIKE_FAILED));
            }
        }
    }
}
