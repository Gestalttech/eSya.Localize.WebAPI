using eSya.Localize.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Localize.IF
{
   public interface IUserCreationRepository
    {
        Task<List<DO_UserCreation>> GetAllUsers();

        Task<DO_ReturnParameter> InsertOrUpdateUserCreation(DO_UserCreation obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveUser(bool status, int UserId);
    }
}
