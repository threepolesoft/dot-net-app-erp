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
    public class InventoryController : ControllerBase
    {
        public ApiResponse res = new ApiResponse();



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
    }
}
