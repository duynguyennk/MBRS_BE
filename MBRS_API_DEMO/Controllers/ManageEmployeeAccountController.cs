using FluentEmail.Core;
using FluentEmail.Core.Models;
using MBRS_API.Models;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Models.ViewModels;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Services.IService;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mail;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API_DEMO.Controllers
{
    [Route(IRoutes.MANAGE_ACCOUNT)]
    [ApiController]
    [Authorize(Roles = "AM")]
    public class ManageEmployeeAccountController : Controller
    {
        public readonly IManageEmployeeAccountService _manageAccountService;

        public ManageEmployeeAccountController(IManageEmployeeAccountService manageAccountService)
        {
            _manageAccountService = manageAccountService;
        }

        [HttpGet]
        [Route("GetListEmployeeAccount")]
        public IActionResult getAllEmployeeAccount()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageAccountService.getAllEmployeeAccount();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Employee>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_EMPLOYEE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetListEmployeeAccountWithFilter")]
        public IActionResult getAllEmployeeAccountWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageAccountService.getAllEmployeeAccountWithFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Employee>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_EMPLOYEE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetListDeparment")]
        public IActionResult getListDepartment()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageAccountService.getListDepartment();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Department>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_DEPARTMENT_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_DEPARTMENT_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAEmployeeAccountToUpdateByID")]
        public IActionResult getAEmployeeAccountToUpdateByID(int employeeID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageAccountService.getEmployeeInformationToUpdateByID(employeeID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<EmployeeViewModel>>(IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return NotFound(new BaseResponse<String>(IErrorCodeApi.NOT_FOUND, IConstants.Data.DataNull, ErrorCodeResponse.LIST_EMPLOYEE_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("CreateEmployeeAccount")]
        public async Task<IActionResult> createAccountEmployee([FromBody] EmployeeViewModel employee)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageAccountService.createEmployee(employee);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_EMPLOYEE_SUCCESS));
                }
                else if (result == -2)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.USER_NAME_DUPLICATE));
                }
                else if (result == -3)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.EMAIL_DUPLICATE));
                }
                else if (result == -4)
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.IDENTITY_NUMBER_DUPLICATE));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_CUSTOMER_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpPut]
        [Route("UpdateEmployeeAccount")]
        public IActionResult updateAccountEmployeeByID([FromBody] EmployeeViewModel employee, int employeeID, int accountID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageAccountService.updateEmployeeAccount(employee, employeeID, accountID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CREATE_CUSTOMER_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteEmployeeAccount")]
        public IActionResult DeleteEmployeeByID(int employeeID, int accountID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageAccountService.deleteEmloyeeByID(employeeID, accountID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_EMPLOYEE_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_EMPLOYEE_FAILED));
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


