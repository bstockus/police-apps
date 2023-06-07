using Lax.Business.Bus.Logging;
using MediatR;

namespace Police.Business.Abstractions.Identity {

    [LogRequest]
    public class FetchIdentityByWindowsSidQuery : IRequest<IdentityInfo> {

        public string WindowsSid { get; }

        public FetchIdentityByWindowsSidQuery(string windowsSid) {
            WindowsSid = windowsSid;
        }

    }

}