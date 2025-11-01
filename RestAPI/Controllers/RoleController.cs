using AppBLL.Services;
using AppBO.Models;
using AppBO.ModelsApi;
using AppBO.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using static AppBO.Utility.Utility;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public ApiResponse res = new ApiResponse();
        public RoleService roleService;
        public RoleUserService roleUserService;

        public RoleController(
            RoleService roleService,
            RoleUserService roleUserService
            )
        {
            this.roleService = roleService;
            this.roleUserService = roleUserService;
        }

        [Authorize, HttpPost, Route("save-role")]
        public async Task<ActionResult> RoleSave([FromBody] SaveRoleModelReq model)
        {
            try
            {
                RoleModel roleModel = Extention.MapObject<RoleModel>(model);
                long LogedUser = GetLoggedUserId();
                roleModel.EntryBy = LogedUser;
                roleModel.UpdatedBy = LogedUser;
                ProccessResult proccessResult=roleService.Save(roleModel);

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

        [Authorize, HttpGet, Route("all")]
        public ActionResult RoleByAll()
        {
            try
            {
                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = roleService.GetByAll();
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpPost, Route("save-user-role")]
        public ActionResult SaveUsererRole([FromBody] List<UserRollModelApiReq> modelApi)
        {
            try
            {
                List<RoleUserModel> roleUserModels = Extention.MapObject<List<RoleUserModel>>(modelApi);

                long LogedUser = GetLoggedUserId();

                ProccessResult proccessResult = roleUserService.Save(roleUserModels, LogedUser);

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


        [Authorize, HttpGet, Route("get-user-role")]
        public ActionResult GetByUserId()
        {
            try
            {
                long LogedUser = GetLoggedUserId();
                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = roleService.GetByUserId(LogedUser).Data;
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpGet, Route("get-user-role-all")]
        public ActionResult GetUserRoleAll(long ApplicationUserId)
        {
            try
            {
                long LogedUser = GetLoggedUserId();
                var deviceSetting = roleService.GetUserRole(ApplicationUserId, LogedUser);

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
