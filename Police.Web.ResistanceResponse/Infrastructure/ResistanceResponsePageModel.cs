using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Police.Security.User;
using Police.Web.Common.Infrastructure;
using Police.Web.ResistanceResponse.Constants;

namespace Police.Web.ResistanceResponse.Infrastructure {

    public abstract class ResistanceResponsePageModel : BasePageModel {

        protected RedirectToPageResult RedirectToAttachOfficerPage(Guid incidentId) =>
            RedirectToPage(PageConstants.AttachOfficer, new {
                incidentId
            });

        protected RedirectToPageResult RedirectToCreatePage(string caseNumber) =>
            RedirectToPage(PageConstants.Create, new {
                caseNumber
            });

        protected RedirectToPageResult RedirectToDetailsPage(Guid incidentId) =>
            RedirectToPage(PageConstants.Details, new {
                incidentId
            });

        protected RedirectToPageResult RedirectToIndexPage() =>
            RedirectToPage(PageConstants.Index);

        protected ResistanceResponsePageModel(IMediator mediator, IUserService userService) : base(mediator, userService) { }

    }
}
