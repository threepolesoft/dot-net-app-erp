using AppBLL.Services;
using AppBO.Models;
using AppBO.ModelsApi;
using AppBO.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using static AppBO.Utility.Utility;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        public ApiResponse res = new ApiResponse();

        public SettingService settingService;
        public SettingUserService settingUserService;
        public SettingDeviceService settingDeviceService;
        public IConfiguration configuration;
        public SettingController(
            SettingService settingService,
            SettingUserService settingUserService,
            SettingDeviceService settingDeviceService,
            IConfiguration configuration
            )
        {
            this.settingService = settingService;
            this.settingUserService = settingUserService;
            this.settingDeviceService = settingDeviceService;
            this.configuration= configuration;
        }

        [Authorize, HttpPost, Route("save-setting")]
        public ActionResult SaveSetting([FromBody] SaveSettingModelReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                SettingModel settingModel = Extention.MapObject<SettingModel>(model);
                settingModel.EntryBy = logedUser;
                settingModel.UpdatedBy = logedUser;
                ProccessResult proccessResult = settingService.Save(settingModel);

                if (proccessResult.Status == true)
                {
                    res.Status = true;
                    res.Message = proccessResult.Message;
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

        [Authorize, HttpGet, Route("all")]
        public ActionResult ByAll(string? Scope)
        {
            try
            {
                var data = string.IsNullOrEmpty(Scope) == true ? settingService.GetByAll() : settingService.GetByScope(Scope.ToLower());

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

        [Authorize, HttpPost, Route("save-user-setting")]
        public ActionResult SaveUserSetting([FromBody] List<SaveUserSettingReq> modelApi)
        {
            try
            {
                List<SettingUserModel> rolelawyerModels = Extention.MapObject<List<SettingUserModel>>(modelApi);

                long LogedUser = Convert.ToInt64(User.Claims.FirstOrDefault(m => m.Type == "LId").Value);

                ProccessResult proccessResult = settingUserService.Save(rolelawyerModels, LogedUser);

                if (proccessResult.Status == true)
                {
                    res.Status = true;
                    res.Message = ProccessStatus.Success;
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

        [Authorize, HttpPost, Route("save-device-setting")]
        public ActionResult SaveDeviceSetting([FromBody] List<SaveDeviceSettingReq> modelApi)
        {
            try
            {
                List<SettingDeviceModel> model = Extention.MapObject<List<SettingDeviceModel>>(modelApi);

                long LogedUser = Convert.ToInt64(User.Claims.FirstOrDefault(m => m.Type == "LId").Value);

                ProccessResult proccessResult = settingDeviceService.Save(model, LogedUser);

                if (proccessResult.Status == true)
                {
                    res.Status = true;
                    res.Message = ProccessStatus.Success;
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

        [Authorize, HttpPost, Route("update-device-setting")]
        public ActionResult UpdateDeviceSetting([FromBody] UpdateDeviceSettingReq modelApi)
        {
            try
            {
                SettingDeviceModel model = Extention.MapObject<SettingDeviceModel>(modelApi);

                long LogedUser = GetLoggedUserId();
                model.UserId= LogedUser;

                ProccessResult proccessResult = settingDeviceService.Save(model);

                if (proccessResult.Status == true)
                {
                    res.Status = true;
                    res.Message = ProccessStatus.Success;
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

        [Authorize, HttpGet, Route("get-device-setting")]
        public async Task<ActionResult> GetDeviceSettingAsync(string DeviceId, string Scope)
        {
            try
            {
                // Using a helper method or service to get the logged-in user ID (for better readability)
                long LogedUser = GetLoggedUserId();

                // Assuming GetBy is an async method, we call it asynchronously
                var deviceSetting = await settingService.GetByAsync(LogedUser, DeviceId, Scope, LogedUser);

                // Responding with success
                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = deviceSetting.Data;
                return StatusCode((int)StatusCodes.Status200OK, res);
            }
            catch (Exception ex)
            {
                // Log the exception (make sure to log it properly, not just here)
                // Log.Error(ex, "Error retrieving device setting");

                res.Status = false;
                res.Message = "An error occurred while processing your request.";
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpGet, Route("get-user-setting-all")]
        public ActionResult AllUserSetting()
        {
            try
            {
                long LogedUser = GetLoggedUserId();
                var deviceSetting = settingService.GetAllUserSetting(LogedUser);

                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = deviceSetting.Data;
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpGet, Route("get-device-setting-all")]
        public ActionResult AllDeviceSetting(int UserId)
        {
            try
            {
                long LogedUser = Convert.ToInt64(User.Claims.FirstOrDefault(m => m.Type == "LId").Value);
                var deviceSetting = settingService.GetBy(UserId, LogedUser);

                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = deviceSetting.Data;
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpGet, Route("setting-junk-count")]
        public ActionResult JunkCount()
        {
            try
            {
                DateTime JunkDate = DateTime.UtcNow.AddDays(int.Parse(configuration["JunkSettingBefor"]));
                var deviceSetting = settingDeviceService.GetJunkSettings(JunkDate);

                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = deviceSetting.Data;
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpGet, Route("setting-junk-delete")]
        public ActionResult JunkDelete()
        {
            try
            {
                long LogedUser = GetLoggedUserId();
                DateTime JunkDate = DateTime.UtcNow.AddDays(int.Parse(configuration["JunkSettingBefor"]));
                var deviceSetting = settingDeviceService.JunkSettingsDelete(LogedUser, JunkDate);

                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = deviceSetting.Data;
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
