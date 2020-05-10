using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Identity.Api.Core.Facades;
using Identity.Api.Dto;
using Identity.Common;
using Identity.Domain.UserAggregate;

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
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("{username}/authenticate")]
        public async Task<IActionResult> Authenticate(string username, UserCredential credential)
        {
            if (username != credential.Username)
            {
                return BadRequest();
            }

            var user = await _userFacade.ValidateCredentials(credential);
            if (user != null && user.UserStatus == UserStatus.Enabled)
            {
                return Ok(user);
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> NewUser(NewUserDto newUser)
        {
            var exists = await _userFacade.GetUser(newUser.Username);
            if (exists != null){
                return BadRequest();
            }
            var user = await _userFacade.NewUser(newUser);
            if (user != null)
                return CreatedAtAction(nameof(GetUser), new { username = user.Username }, user);
            
            return BadRequest();
        }

    }
}
