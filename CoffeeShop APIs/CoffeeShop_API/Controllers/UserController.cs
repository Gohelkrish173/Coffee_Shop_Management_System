using CoffeeShop_API.DATA;
using CoffeeShop_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;

        #region Constructor
        public UserController(UserRepository _userRepository)
        {
            userRepository = _userRepository;
        }
        #endregion

        #region GetAllUser
        [HttpGet]
        public IActionResult GetAllUser()
        {
            List<UserModel> user = userRepository.GetAllUser().ToList();
            return Ok(user);
        }
        #endregion

        #region DeleteUser
        [HttpDelete]
        public IActionResult DeleteUser(int UserID)
        {
            var responce = userRepository.DeleteUser(UserID);
            return Ok(responce + UserID.ToString());
        }
        #endregion

        #region InsertUser
        [HttpPost]
        public IActionResult InsertUser(UserModel user) 
        {
            bool responce = userRepository.InsertUser(user);
            return Ok(responce);
        }
        #endregion

        #region UpdateUser
        [HttpPut]
        public IActionResult UpdateUser(UserModel user)
        {
            bool responce = userRepository.UpdateUser(user);
            return Ok(responce);
        }
        #endregion

        #region GetUserByPK
        [HttpGet("{UserID:int}")]
        public IActionResult GetUserByPK(int UserID)
        {
            UserModel userModel = userRepository.GetUserByPK(UserID);
            return Ok(userModel);
        }
        #endregion

    }
}
