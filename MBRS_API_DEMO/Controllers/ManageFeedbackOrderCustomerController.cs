using MBRS_API.Models;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_FEEDBACK_ORDER_ROOM_CUSTOMER)]
    [ApiController]
    [Authorize(Roles = "CS")]
    public class ManageFeedbackOrderCustomerController : Controller
    {
        public readonly IManageFeedbackOrderCustomerService _manageFeedbackCustomerService;
        public ManageFeedbackOrderCustomerController(IManageFeedbackOrderCustomerService manageFeedbackCustomerService)
        {
            _manageFeedbackCustomerService = manageFeedbackCustomerService;
        }

        [HttpGet]
        [Route("GetRatingRoomByRatingID")]
        public IActionResult getRatingRoomByRatingID(int ratingID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFeedbackCustomerService.getRatingRoomByRatingID(ratingID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<FeedbackRoom>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_FEEDBACK_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.GET_FEEDBACK_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetRatingServiceByRatingID")]
        public IActionResult getRatingServiceByRatingID(int ratingID, int selectedOption)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFeedbackCustomerService.getRatingServiceByRatingID(ratingID, selectedOption);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<FeedbackService>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_FEEDBACK_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.GET_FEEDBACK_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("CreateRoomFeedback")]
        public IActionResult createRoomFeedback(FeedbackRoom feedbackRoom)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFeedbackCustomerService.createRoomFeedback(feedbackRoom);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_FEEDBACK_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_FEEDBACK_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("CreateServiceFeedback")]
        public IActionResult createServiceFeedback(FeedbackService feedbackService)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageFeedbackCustomerService.createServiceFeedback(feedbackService);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_FEEDBACK_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_FEEDBACK_FAILED));
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
