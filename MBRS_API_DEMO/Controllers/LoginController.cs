using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Services.IService;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Mvc;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API_DEMO.Controllers
{
    [Route(IRoutes.LOGIN)]
    [ApiController]
    public class LoginController : Controller
    {
        public readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] User user)
        {
            int checkNotActive = _loginService.CheckNotActive(user.UserName);
            if (checkNotActive == 1)
            {
                return NotFound(new BaseResponse<string>(IErrorCodeApi.NOT_FOUND, IConstants.Data.DataNull, ErrorCodeResponse.CHECK_ACCOUNT_STATUS));
            }
            var token = _loginService.Login(user);
            return Ok(token);
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout([FromBody] User user)
        {
            var result = _loginService.Logout(user);
            return Ok(result);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                int checkNotActive = _loginService.CheckPasswordCorrect(changePassword);
                if (checkNotActive == 0)
                {
                    return Ok(new BaseResponse<string>(IErrorCodeApi.NOT_FOUND, IConstants.Data.DataNull, ErrorCodeResponse.CHECK_ACCOUNT_STATUS));
                }
                var result = _loginService.ChangePassword(changePassword);
                if (result > 0)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.CHANGE_PASSWORD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.CHANGE_PASSWORD_FAILED));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ex.ToString()));
            }
        }
        [HttpGet]
        [Route("GetCustomerInformationByID")]
        public IActionResult getCustomerInformationByID(int accountID)
        {
            try
            {
                var result = _loginService.getCustomerInformationByID(accountID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new BaseResponse<List<CustomerViewModel>>(IConstants.IErrorCodeApi.OK, result, ErrorCodeResponse.GET_LIST_CUSTOMER_SUCCESS));
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
        [HttpPost]
        [Route("ForgetPassword")]
        public IActionResult ChangePassword([FromBody] User userForgetPassword)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var user = Common.GetUserByToken(token.ToString().Replace("Bearer ", ""));
                var result = _loginService.ForgetPassword(userForgetPassword);
                if (result == 1)
                {
                    return Ok(new BaseResponse<int>(IErrorCodeApi.OK, result, ErrorCodeResponse.SEND_NEW_PASSWORD_SUCCESS));
                }
                else
                {
                    return BadRequest(new BaseResponse<int>(IErrorCodeApi.BAD_REQUEST, result, ErrorCodeResponse.SEND_NEW_PASSWORD_FAILED));
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
