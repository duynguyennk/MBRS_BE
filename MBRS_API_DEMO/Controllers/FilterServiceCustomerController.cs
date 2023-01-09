using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using MBRS_API.Models;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using MBRS_API.Services.IService;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.FILTER_USING_SERVICE_CUSTOMER)]
    [ApiController]
    [Authorize(Roles = "CS")]
    public class FilterServiceCustomerController : Controller
    {
        public readonly IFilterServiceCustomerService _filterServiceCustomerService;
        public FilterServiceCustomerController(IFilterServiceCustomerService filterServiceCustomerService)
        {
            _filterServiceCustomerService = filterServiceCustomerService;
        }

        [HttpGet]
        [Route("FilterUsingCustomerService")]
        public IActionResult filterUsingCustomerService(int accountID)
        {

            try
            {   
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _filterServiceCustomerService.checkUsingCustomerService(accountID);
                return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.FILTER_ROOM_SUCCESS));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.FILTER_ROOM_FAILED));
            }
        }
    }
}
