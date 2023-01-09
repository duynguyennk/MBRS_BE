using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Services.IService;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Models.ViewModels;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.MANAGE_ACCOUNT)]
    [ApiController]
    [Authorize(Roles = "AM")]
    public class ManageCustomerAccountController : Controller
    {
        public readonly IManageCustomerAccountService _manageCustomerAccountService;
        public ManageCustomerAccountController(IManageCustomerAccountService manageCustomerAccountService)
        {
            _manageCustomerAccountService = manageCustomerAccountService;
        }

        [HttpGet]
        [Route("GetListCustomerAccount")]
        public IActionResult getAllCustomerAccount()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageCustomerAccountService.getAllCustomerAccount();
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Customer>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_CUSTOMER_SUCCESS));
                }
                else
                {
                    return Ok(new BaseResponse<String>(IConstants.IErrorCodeApi.OK, IConstants.Data.DataNull, ErrorCodeResponse.LIST_CUSTOMER_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetACustomerAccountToUpdateByID")]
        public IActionResult getACusotmerAccountToUpdateByID(int customerID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageCustomerAccountService.getCustomerInformationToUpdateByID(customerID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<CustomerViewModel>>(IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_CUSTOMER_SUCCESS));
                }
                else
                {
                    return NotFound(new BaseResponse<String>(IErrorCodeApi.NOT_FOUND, IConstants.Data.DataNull, ErrorCodeResponse.LIST_CUSTOMER_EMPTY));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }

        [HttpPost]
        [Route("CreateCustomerAccount")]
        public IActionResult createAccountCustomer([FromBody] CustomerViewModel customerViewModel)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageCustomerAccountService.createCustomer(customerViewModel);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CREATE_CUSTOMER_SUCCESS));
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
        [Route("UpdateCustomerAccount")]
        public IActionResult updateAccountCustomerByID([FromBody] CustomerViewModel customerViewModel, int customerID, int accountID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageCustomerAccountService.updateCustomerAccount(customerViewModel, customerID, accountID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.UPDATE_CUSTOMER_SUCCESS));
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
        [Route("DeleteCustomerAccount")]
        public IActionResult DeleteCustomerByID(int customerID, int accountID)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageCustomerAccountService.deleteCustomerByID(customerID, accountID);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.DELETE_CUSTOMER_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.DELETE_CUSTOMER_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetListCustomerAccountWithFilter")]
        public IActionResult getListCustomerAccountWithFilter(string filterName, string filterValue)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _manageCustomerAccountService.getAllCustomerAccountWithFilter(filterName, filterValue);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<Customer>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_EMPLOYEE_SUCCESS));
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
    }
}



