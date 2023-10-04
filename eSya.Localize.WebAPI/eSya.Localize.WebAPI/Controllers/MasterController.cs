using eSya.Localize.DO;
using eSya.Localize.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Localize.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _masterRepository;

        public MasterController(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;

        }

        #region Localization Table Mapping
        /// <summary>
        /// Getting  All Localization Table Master.
        /// UI Reffered -Localization Table Mapping
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLocalizationTableMaster()
        {
            var tbl_masters = await _masterRepository.GetLocalizationTableMaster();
            return Ok(tbl_masters);
        }


        /// <summary>
        /// Insert  Localization Table Master.
        /// UI Reffered -Localization Table Mapping
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLocalizationTableMaster(DO_LocalizationMaster obj)
        {
            var msg = await _masterRepository.InsertOrUpdateLocalizationTableMaster(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active/De Localization Table Master.
        /// UI Reffered -Localization Table Mapping
        /// Params status-Tablecode
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveLocalizationTableMaster(bool status, int Tablecode)
        {
            var msg = await _masterRepository.ActiveOrDeActiveLocalizationTableMaster(status, Tablecode);
            return Ok(msg);

        }
        #endregion

        #region Language Mapping
        /// <summary>
        /// Getting  All Localization Master.
        /// UI Reffered -Localization Language Mapping
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLocalizationMaster()
        {
            var l_master = await _masterRepository.GetLocalizationMaster();
            return Ok(l_master);
        }


        /// <summary>
        /// Insert  Localization Language Mapping.
        /// UI Reffered -Localization Language Mapping
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLocalizationLanguageMapping(List<DO_LocalizationLanguageMapping> obj)
        {
            var msg = await _masterRepository.InsertOrUpdateLocalizationLanguageMapping(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Getting  Language Mapping by Table Code.
        /// UI Reffered -Localization Language Mapping
        /// Params status-Tablecode
        /// </summary>
        [HttpGet]
        public IActionResult GetLocalizationLanguageMapping(string languageCode, int tableCode)
        {
            var L_maps = _masterRepository.GetLocalizationLanguageMapping(languageCode, tableCode);
            return Ok(L_maps);

        }
        #endregion
    }
}
