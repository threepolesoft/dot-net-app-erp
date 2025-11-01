using AppBLL.Services;
using AppBO.Models;
using AppBO.ModelsApi;
using AppBO.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AppBO.Utility.Utility;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public ApiResponse res = new ApiResponse();
        public MenuService menuService;
        public MenuDetailService menuDetailService;
        public RoleMenuService roleMenuService;
        public MenuController(
            MenuService menuService,
            MenuDetailService menuDetailService,
            RoleMenuService roleMenuService
            )
        {
            this.menuService = menuService;
            this.menuDetailService = menuDetailService;
            this.roleMenuService = roleMenuService;
        }

        [Authorize, HttpPost, Route("save-menu")]
        public ActionResult SaveMenu([FromBody] MenuSaveModelApiReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                MenuModel settingModel = Extention.MapObject<MenuModel>(model);
                settingModel.EntryBy = logedUser;
                settingModel.UpdatedBy = logedUser;
                ProccessResult proccessResult = menuService.Save(settingModel);

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

        [Authorize, HttpPost, Route("save-role-menu")]
        public ActionResult SaveRoleMenu([FromBody] List<RoleMenuApiReq> model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                List<RoleMenuModel> bindModel = Extention.MapObject<List<RoleMenuModel>>(model);
                ProccessResult proccessResult = roleMenuService.Save(bindModel, logedUser);

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

        [Authorize, HttpPost, Route("menu-detail-save")]
        public ActionResult SaveMenuDetail([FromBody] MenuDetailSaveModelApiReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                MenuDetailModel addModel = Extention.MapObject<MenuDetailModel>(model);
                ProccessResult proccessResult = menuDetailService.Save(addModel, logedUser);

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

        [Authorize, HttpPost, Route("menu-detail-delete")]
        public ActionResult MenuDetailDelete([FromBody] List<long> model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                ProccessResult proccessResult = menuDetailService.Delete(model, logedUser);

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
                var data = menuService.GetBy(Scope);

                res.Status = true;
                res.Message = ProccessStatus.Success;
                res.Data = data.Data;
                return StatusCode((int)StatusCodes.Status200OK, res);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return StatusCode((int)StatusCodes.Status500InternalServerError, res);
            }
        }

        [Authorize, HttpGet, Route("menu-detail-all")]
        public ActionResult MenuDetailAll()
        {
            try
            {
                ProccessResult proccessResult = menuDetailService.GetAll();

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

        [Authorize, HttpGet, Route("menu-detail-by-role")]
        public ActionResult MenuDetailByRole()
        {
            try
            {
                long logedUser = GetLoggedUserId();
                ProccessResult proccessResult = menuDetailService.GetAll(logedUser);

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

        [Authorize, HttpGet, Route("role-menu")]
        public ActionResult RoleMenu()
        {
            try
            {
                ProccessResult proccessResult = menuDetailService.GetByRole();

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

        // Helper method to extract logged user ID
        private long GetLoggedUserId()
        {
            var loggedUserClaim = User.FindFirst(m => m.Type == "ApplicationUserId");
            return loggedUserClaim != null ? Convert.ToInt64(loggedUserClaim.Value) : 0; // return 0 or handle as needed
        }
    }
}
