using eSya.Localize.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Localize.IF
{
   public interface IControllerRepository
    {
        #region Language Controller
        Task<List<string>> GetAllControllers();

        Task<List<DO_LanguageController>> GetLanguageControllersbyResource(string Resource);

        Task<DO_ReturnParameter> InsertOrUpdateLanguageController(DO_LanguageController lobj);

        Task<DO_ReturnParameter> ActiveOrDeActiveLanguageController(bool status, int ResourceId);
        #endregion
    }
}
