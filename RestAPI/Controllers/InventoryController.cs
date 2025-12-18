using AppBLL.InventoryServices;
using AppBLL.Services;
using AppBO.Models;
using AppBO.ModelsApi;
using AppBO.ModelsInventory;
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
        private readonly BrandService brandService;
        private readonly CategoryService categoryService;
        private readonly ColorService colorService;
        private readonly SizeService sizeService;
        private readonly UnitService unitService;

        public InventoryController(
            BrandService brandService,
            CategoryService categoryService,
            ColorService colorService,
            SizeService sizeService,
            UnitService unitService
            )
        {
            this.brandService = brandService;
            this.categoryService = categoryService;
            this.colorService = colorService;
            this.sizeService = sizeService;
            this.unitService = unitService;
        }

        [Authorize, HttpPost, Route("brand-save")]
        public ActionResult BrandSave([FromBody] BrandSaveModelApiReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                BrandModel brandModel = Extention.MapObject<BrandModel>(model);
                brandModel.EntryBy = logedUser;
                brandModel.UpdatedBy = logedUser;
                ProccessResult proccessResult = brandService.Save(brandModel);

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


        [Authorize, HttpGet, Route("brand-all")]
        public ActionResult BrandAll()
        {
            try
            {
                var data = brandService.GetBy();

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

        [Authorize, HttpPost, Route("category-save")]
        public ActionResult CategorySave([FromBody] CategorySaveModelApiReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                CategoryModel takeModel = Extention.MapObject<CategoryModel>(model);
                takeModel.EntryBy = logedUser;
                takeModel.UpdatedBy = logedUser;
                ProccessResult proccessResult = categoryService.Save(takeModel);

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

        [Authorize, HttpGet, Route("category-all")]
        public ActionResult CategoryAll()
        {
            try
            {
                var data = categoryService.GetBy();

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

        [Authorize, HttpPost, Route("color-save")]
        public ActionResult ColorSave([FromBody] ColorSaveModelApiReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                ColorModel takeModel = Extention.MapObject<ColorModel>(model);
                takeModel.EntryBy = logedUser;
                takeModel.UpdatedBy = logedUser;
                ProccessResult proccessResult = colorService.Save(takeModel);

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

        [Authorize, HttpGet, Route("color-all")]
        public ActionResult ColorAll()
        {
            try
            {
                var data = colorService.GetBy();

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

        [Authorize, HttpPost, Route("size-save")]
        public ActionResult SizeSave([FromBody] SizeSaveModelApiReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                SizeModel takeModel = Extention.MapObject<SizeModel>(model);
                takeModel.EntryBy = logedUser;
                takeModel.UpdatedBy = logedUser;
                ProccessResult proccessResult = sizeService.Save(takeModel);

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

        [Authorize, HttpGet, Route("size-all")]
        public ActionResult SizeAll()
        {
            try
            {
                var data = sizeService.GetBy();

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

        [Authorize, HttpPost, Route("unit-save")]
        public ActionResult UnitSave([FromBody] UnitSaveModelApiReq model)
        {
            try
            {
                long logedUser = GetLoggedUserId();
                UnitModel takeModel = Extention.MapObject<UnitModel>(model);
                takeModel.EntryBy = logedUser;
                takeModel.UpdatedBy = logedUser;
                ProccessResult proccessResult = unitService.Save(takeModel);

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

        [Authorize, HttpGet, Route("unit-all")]
        public ActionResult UnitAll()
        {
            try
            {
                var data = unitService.GetBy();

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

        // Helper method to extract logged user ID
        private long GetLoggedUserId()
        {
            var loggedUserClaim = User.FindFirst(m => m.Type == "ApplicationUserId");
            return loggedUserClaim != null ? Convert.ToInt64(loggedUserClaim.Value) : 0; // return 0 or handle as needed
        }
    }
}
