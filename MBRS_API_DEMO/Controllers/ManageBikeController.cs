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
    [Route(IRoutes.MANAGE_BIKE)]
    [ApiController]
    [Authorize(Roles = "MN")]
    public class ManageBikeController : Controller
    {
        public readonly IManageBikeService _manageBikeService;
        public ManageBikeController(IManageBikeService manageBikeService)
        {
            _manageBikeService = manageBikeService;
        }

        [HttpGet]
        [Route("GetListBike")]
        public IActionResult getAllBike()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.getAllBike();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<BikeViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_BIKE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_BIKE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetListBikeWithFilter")]
        public IActionResult getListBikeWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.getAllBikeWithFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<BikeViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_BIKE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_BIKE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllTypeBike")]
        public IActionResult getAllTypeBike()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.getAllTypeBike();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeBike>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_TYPE_BIKE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.GET_LIST_TYPE_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetDetailInformationBike")]
        public IActionResult getDetailInformationBike(int typeBikeID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.getDetailInformationBike(typeBikeID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<TypeBike>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_BIKE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_BIKE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetTheBikeInformation")]
        public IActionResult getTheBikeInformation(int bikeID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.getTheBikeInformation(bikeID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Bike>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_BIKE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_BIKE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("CreateBike")]
        public IActionResult createBike([FromBody] Bike bike)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.createBike(bike);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateBike")]
        public IActionResult updateBike([FromBody] Bike bike)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.updateTheBike(bike);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.UPDATE_BIKE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteABike")]
        public IActionResult DeleteABike(int bikeID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageBikeService.deleteBike(bikeID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_BIKE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_BIKE_FAILED));
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
