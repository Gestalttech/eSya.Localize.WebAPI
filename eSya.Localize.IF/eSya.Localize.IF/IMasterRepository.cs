using eSya.Localize.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Localize.IF
{
    public interface IMasterRepository
    {
        #region Localization Table Mapping
        Task<List<DO_LocalizationMaster>> GetLocalizationTableMaster();

        Task<DO_ReturnParameter> InsertOrUpdateLocalizationTableMaster(DO_LocalizationMaster obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveLocalizationTableMaster(bool status, int Tablecode);
        #endregion

        #region Language Mapping
        Task<List<DO_LocalizationMaster>> GetLocalizationMaster();

        Task<DO_ReturnParameter> InsertOrUpdateLocalizationLanguageMapping(List<DO_LocalizationLanguageMapping> obj);

        List<DO_LocalizationLanguageMapping> GetLocalizationLanguageMapping(string languageCode, int tableCode);

        #endregion
    }
}
