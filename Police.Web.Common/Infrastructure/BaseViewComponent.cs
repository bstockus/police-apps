using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Police.Security.User;

namespace Police.Web.Common.Infrastructure {

    public abstract class BaseViewComponent : ViewComponent {

        protected readonly IMediator Mediator;
        protected readonly IUserService UserService;

        protected BaseViewComponent(
            IMediator mediator,
            IUserService userService) {
            Mediator = mediator;
            UserService = userService;
        }

        protected async Task<Guid> FetchCurrentUserId() =>
            (await FetchCurrentUser())?.UserId ?? Guid.Empty;

        protected async Task<UserInformation> FetchCurrentUser() =>
            await UserService.GetUserInformationForClaimsPrincipal(HttpContext.User);

    }

}