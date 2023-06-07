using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Lax.Business.Bus.Logging;
using Lax.Business.Bus.UnitOfWork;
using Lax.Business.Bus.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Police.Business.Common;
using Police.Data;

namespace Police.Business.ResistanceResponse.Incidents.Subjects {

    [LogRequest, ValidateRequest, RequestUnitOfWork(typeof(PoliceDbContext))]
    public class RemoveSubjectCommand : IRequest {

        public Guid IncidentId { get; set; }
        public Guid SubjectId { get; set; }

        public class Validator : AbstractValidator<RemoveSubjectCommand> {

            public Validator() {
                RuleFor(_ => _.IncidentId).AsEntityIdentity();
                RuleFor(_ => _.SubjectId).AsEntityIdentity();
            }

        }

        public class Handler : IRequestHandler<RemoveSubjectCommand> {

            private readonly DbSet<Subject> _subjects;

            public Handler(
                DbSet<Subject> subjects) {

                _subjects = subjects;
            }

            public async Task<Unit> Handle(RemoveSubjectCommand request, CancellationToken cancellationToken) {

                await _subjects.ThrowIfSubjectDoesNotExist(request.IncidentId, request.SubjectId, cancellationToken);

                var subject = await _subjects.FirstOrDefaultAsync(_ =>
                        _.IncidentId.Equals(request.IncidentId) &&
                        _.SubjectId.Equals(request.SubjectId),
                    cancellationToken);

                _subjects.Remove(subject);

                return Unit.Value;

            }

        }

    }

}