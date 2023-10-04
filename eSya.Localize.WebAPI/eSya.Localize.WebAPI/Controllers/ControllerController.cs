using eSya.Localize.DO;
using eSya.Localize.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Localize.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ControllerController : ControllerBase
    {
        private readonly IControllerRepository _controllerRepository;

        public ControllerController(IControllerRepository controllerRepository)
        {
            _controllerRepository = controllerRepository;

        }
        #region Language Controller
        /// <summary>
        /// Getting  All Controllers.
        /// UI Reffered - Language Controller
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllControllers()
        {
            var ctrls = await _controllerRepository.GetAllControllers();
            return Ok(ctrls);
        }

        /// <summary>
        /// Getting Controller by Resource.
        /// UI Reffered - Language Controller
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLanguageControllersbyResource(string Resource)
        {
            var ctrls = await _controllerRepository.GetLanguageControllersbyResource(Resource);
            return Ok(ctrls);
        }

        /// <summary>
        /// Insert  Language Controller.
        /// UI Reffered -Language Controller
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLanguageController(DO_LanguageController lobj)
        {
            var msg = await _controllerRepository.InsertOrUpdateLanguageController(lobj);
            return Ok(msg);

        }

        /// <summary>
        /// Active/De Language Controller.
        /// UI Reffered -Language Controller
        /// Params status-ResourceId
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveLanguageController(bool status, int ResourceId)
        {
            var msg = await _controllerRepository.ActiveOrDeActiveLanguageController(status, ResourceId);
            return Ok(msg);

        }
        #endregion
    }
}
