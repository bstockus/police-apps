using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Police.Security.User;

namespace Police.Web.Controllers {

    public class UserController : Controller {

        private readonly IUserService _userService;

        public UserController(
            IUserService userService) {
            _userService = userService;
        }

        [HttpGet("~/__admin/current-user")]
        public async Task<ActionResult> CurrentUser() =>
            Json(await _userService.GetUserInformationForClaimsPrincipal(HttpContext.User));

    }

}