using AppBLL.Services;
using AppBO.Models;
using AppBO.ModelsApi;
using AppBO.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using static AppBO.Utility.Utility;
using static System.Formats.Asn1.AsnWriter;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public UserService userService;
        public ApiResponse res = new ApiResponse();
        public IConfiguration configuration;
        public ApplicationUserService applicationUserService;
        public UsersDevicesService devicesService;

        public AuthController(
            UserService userService,
            IConfiguration configuration,
            ApplicationUserService applicationUserService,
            UsersDevicesService devicesService
            )
        {
            this.configuration = configuration;
            this.userService = userService;
            this.applicationUserService = applicationUserService;
            this.devicesService = devicesService;
        }

        // POST api/<AuthController>
        [HttpPost, Route("login")]
        public ActionResult Post([FromBody] LoginModelReq loginModelReq)
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                string Password = loginModelReq.Password;

                //Create an MD5 object
                //using (MD5 md5 = MD5.Create())
                //{
                //    // Convert the input string to a byte array and compute the hash
                //    byte[] inputBytes = Encoding.UTF8.GetBytes(loginModelReq.Password);
                //    byte[] hashBytes = md5.ComputeHash(inputBytes);


                //    for (int i = 0; i < hashBytes.Length; i++)
                //    {
                //        sb.Append(hashBytes[i].ToString("x2"));
                //    }

                //    Password=sb.ToString();
                //}

                string status = userService.AuthenticationUser(loginModelReq.UserName, Password);

                if (status == ProccessStatus.Success)
                {
                    res.Message = ProccessStatus.Success;
                    res.Data = new LoginModelRes()
                    {
                        Token = userService.Token(loginModelReq.UserName, Convert.ToInt16(configuration["Expires"])),
                    };

                    return StatusCode((int)StatusCodes.Status200OK, res);
                }
                else
                {
                    res.Status = false;
                    res.Message = status;
                    return StatusCode((int)StatusCodes.Status200OK, res);
                }

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [HttpPost, Route("log-info")]
        public ActionResult LogInfo([FromBody] SaveUsersDeviceModelReq model)
        {
            try
            {
                // Fire-and-forget (non-blocking)
                //_ = Task.Run(() =>
                //{
                    devicesService.Save(new UsersDeviceModel()
                    {
                        UserId = model.UserId,
                        DeviceId = model.DeviceId,
                        DeviceName = model.DeviceName,
                        Scope = model.Scope,
                        EntryBy = model.UserId,
                        EntryAt = DateTime.UtcNow,
                        LoginAt = DateTime.UtcNow,
                    });
                //});

                res.Status = true;
                res.Message = "Success";

                return StatusCode((int)StatusCodes.Status200OK, res);
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        // POST api/<AuthController>
        [Authorize, HttpGet, Route("user-info")]
        public ActionResult UserInfo()
        {
            try
            {
                var result = userService.GeyByApplicationUserId(GetLoggedUserId());

                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = result;
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpPost, Route("user-save")]
        public async Task<ActionResult> UserSave([FromBody] ApplicationUserModelReq model)
        {
            try
            {
                ApplicationUserModel roleModel = Extention.MapObject<ApplicationUserModel>(model);
                long LogedUser = GetLoggedUserId();
                roleModel.EntryBy = LogedUser;
                roleModel.UpdatedBy = LogedUser;
                ProccessResult proccessResult = applicationUserService.Save(roleModel);

                if (proccessResult.Status == true)
                {
                    res.Status = true;
                    res.Message = ProccessStatus.Success;
                    res.Data = proccessResult.Data;
                    return StatusCode((int)StatusCodes.Status200OK, res);
                }
                else
                {
                    res.Status = false;
                    res.Message = proccessResult.Message;
                    return StatusCode((int)StatusCodes.Status200OK, res);
                }

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpGet, Route("user-all")]
        public async Task<ActionResult> UserAll()
        {
            try
            {
                var data = applicationUserService.GetByAll();

                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = data;
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        // Helper method to extract logged user ID
        private long GetLoggedUserId()
        {
            var loggedUserClaim = User.FindFirst(m => m.Type == "ApplicationUserId");
            return loggedUserClaim != null ? Convert.ToInt64(loggedUserClaim.Value) : 0; // return 0 or handle as needed
        }
    }
}
