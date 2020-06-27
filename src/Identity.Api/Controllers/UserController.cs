using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Identity.Application.Users;
using Identity.Application.Users.Model;
using Identity.Common;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserFacade _userFacade;

        public UserController(ILogger<UserController> logger, IUserFacade UserFacade)
        {
            _logger = logger;
            _userFacade = UserFacade;
        }

        [HttpGet]
        [Route("{username}")]
        [Route("{username}/detail")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userFacade.GetUser(username);
            return user != null ? (ActionResult)Ok(user) : (ActionResult)NotFound();
        }

        [HttpPost]
        [Route("{username}/authenticate")]
        public async Task<IActionResult> Authenticate(string username, UserCredential credential)
        {
            if (username != credential.Username)
                return BadRequest();

            var user = await _userFacade.ValidateCredentials(credential);
            return (user != null && user.UserStatus == UserStatus.Enabled)
                ? (ActionResult)Ok(user) : (ActionResult)Unauthorized();
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> NewUser(NewUserDto newUser)
        {
            var exists = await _userFacade.GetUser(newUser.Username);
            if (exists != null)
                return BadRequest();

            var user = await _userFacade.NewUser(newUser);
            return user != null ? (ActionResult)CreatedAtAction(nameof(GetUser), new { username = user.Username }, user)
                : (ActionResult)BadRequest();
        }
    }
}
