using eSya.Localize.DO;
using eSya.Localize.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Localize.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CultureController : ControllerBase
    {
        private readonly ICultureRepository _cultureRepository;

        public CultureController(ICultureRepository cultureRepository)
        {
            _cultureRepository = cultureRepository;

        }
        #region Language Culture
        /// <summary>
        /// Getting  Language Culture.
        /// UI Reffered -Language Culture
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetResources()
        {
            var res = await _cultureRepository.GetResources();
            return Ok(res);
        }

        /// <summary>
        /// Getting Language Culture
        /// UI Reffered - Language Culture
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLanguageCulture(string Culture, string Resource)
        {
            var l_cultures = await _cultureRepository.GetLanguageCulture(Culture, Resource);
            return Ok(l_cultures);
        }

        /// <summary>
        /// Insert  Language Culture
        /// UI Reffered -Language Culture
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLanguageCulture(List<DO_LanguageCulture> obj)
        {
            var msg = await _cultureRepository.InsertOrUpdateLanguageCulture(obj);
            return Ok(msg);

        }

        #endregion

        #region Culture Keys
        /// <summary>
        /// Getting Language Culture
        /// UI Reffered - Culture Keys
        /// </summary>
        [HttpGet]
        public IActionResult GetDistinictCultureKeys(string Culture)
        {
            var l_cultures = _cultureRepository.GetDistinictCultureKeys(Culture);
            return Ok(l_cultures);
        }

        /// <summary>
        /// Insert  Culture Keys
        /// UI Reffered -Culture Keys
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateCultureKeys(List<DO_LanguageCulture> obj)
        {
            var msg = await _cultureRepository.InsertOrUpdateCultureKeys(obj);
            return Ok(msg);

        }
        #endregion
    }
}
