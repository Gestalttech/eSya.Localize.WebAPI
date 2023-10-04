using eSya.Localize.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Localize.IF
{
    public interface ICultureRepository
    {
        #region Language Culture
        Task<List<DO_LanguageController>> GetResources();

        Task<List<DO_LanguageCulture>> GetLanguageCulture(string Culture, string Resource);

        Task<DO_ReturnParameter> InsertOrUpdateLanguageCulture(List<DO_LanguageCulture> obj);
        #endregion

        #region Culture Keys
        List<DO_LanguageCulture> GetDistinictCultureKeys(string Culture);

        Task<DO_ReturnParameter> InsertOrUpdateCultureKeys(List<DO_LanguageCulture> obj);
        #endregion
    }
}
