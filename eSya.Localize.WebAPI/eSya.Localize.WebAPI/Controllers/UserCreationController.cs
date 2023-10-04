using eSya.Localize.DO;
using eSya.Localize.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Localize.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserCreationController : ControllerBase
    {
        private readonly IUserCreationRepository _userCreationRepository;

        public UserCreationController(IUserCreationRepository userCreationRepository)
        {
            _userCreationRepository = userCreationRepository;

        }
        #region User Creation
        /// <summary>
        /// Getting  All Users for Grid.
        /// UI Reffered -User Creation
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userCreationRepository.GetAllUsers();
            return Ok(users);
        }


        /// <summary>
        /// InsertOr Update  User
        /// UI Reffered -User Creation
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateUserCreation(DO_UserCreation obj)
        {
            var msg = await _userCreationRepository.InsertOrUpdateUserCreation(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active/De User Creation.
        /// UI Reffered -User Creation
        /// Params status-UserId
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveUser(bool status, int UserId)
        {
            var msg = await _userCreationRepository.ActiveOrDeActiveUser(status, UserId);
            return Ok(msg);

        }
        #endregion
    }
}
