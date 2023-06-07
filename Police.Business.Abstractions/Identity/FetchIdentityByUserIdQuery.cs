using System;
using Lax.Business.Bus.Logging;
using MediatR;

namespace Police.Business.Abstractions.Identity {

    [LogRequest]
    public class FetchIdentityByUserIdQuery : IRequest<IdentityInfo> {

        public Guid UserId { get; }

        public FetchIdentityByUserIdQuery(Guid userId) {
            UserId = userId;
        }

    }

}