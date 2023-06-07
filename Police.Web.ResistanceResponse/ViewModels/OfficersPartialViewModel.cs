using System;
using System.Collections.Generic;

namespace Police.Web.ResistanceResponse.ViewModels {

    public record OfficersPartialViewModel(
        IEnumerable<Police.Business.ResistanceResponse.Incidents.IncidentOfficers.IncidentOfficerInfo>
            IncidentOfficerInfo,
        Guid IncidentId,
        Guid OfficerId);

}
