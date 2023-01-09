using FluentEmail.Core;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using static MBRS_API_DEMO.Utils.IConstants;
using MBRS_API.Services.IService;

namespace MBRS_API.Controllers
{
    [Route(IRoutes.NOTIFICATION)]
    [ApiController]
    public class SendEmailController : Controller
    {
        public readonly IFluentEmail _fluentEmail;
        public SendEmailController(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        [HttpGet]
        [Route("sendEmail")]
        public async Task<IActionResult> getAllOrderRoom(string emailCustomer)
        {
            try
            {
                 var i = 1;
                 var newEmail = _fluentEmail.To("binhdvse04856@fpt.edu.vn").Subject("Thank You").UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Template/EmailForm/welcome.cshtml",i);
                 await newEmail.SendAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return BadRequest(new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.GET_LIST_ORDER_ROOM_FAILED));
            }
        }
    }
}
