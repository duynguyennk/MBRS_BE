using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using MBRS_API.Models.ViewModels;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using MBRS_API.Models;
using MBRS_API.Services.Service;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_ACTIVITY_EMPLOYEE)]
    [ApiController]
    [Authorize(Roles = "MN,LT")]
    public class ManageActivityEmployeeController : Controller
    {
        public readonly IManageActivityEmployeeService _manageActivityEmployeeService;
        public ManageActivityEmployeeController(IManageActivityEmployeeService manageActivityEmployeeService)
        {
            _manageActivityEmployeeService = manageActivityEmployeeService;
        }

        [HttpGet]
        [Route("GetListActivityEmployee")]
        public IActionResult getListActivityEmployee()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageActivityEmployeeService.getAllActivityEmployee();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<ActivityEmployee>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ACTIVITY_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ACTIVITY_EMPLOYEE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ACTIVITY_EMPLOYEE_FAILED));
            }
        }

        [HttpGet]
        [Route("GetListActivityEmployeeWithFilter")]
        public IActionResult getListActivityEmployeeWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageActivityEmployeeService.getAllActivityEmployeeWithFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<ActivityEmployee>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_ACTIVITY_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_ACTIVITY_EMPLOYEE_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ACTIVITY_EMPLOYEE_FAILED));
            }
        }

        [HttpPost]
        [Route("CreateActivityEmployee")]
        public IActionResult createActivityEmployee([FromBody] ActivityEmployee activityEmployee)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageActivityEmployeeService.createActivityEmployee(activityEmployee);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_ACTIVITY_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_ACTIVITY_EMPLOYEE_FAILED));
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
