using Police.Security.Authorization.Policies;
using Police.Security.Authorization.Requirements;

namespace Police.Business.ResistanceResponse {

    public class ResistanceResponseAuthorizationPolicies {

        public const string IsAllowedToSubmit = "RestRep.IsAllowedToSubmit";
        public const string IsAllowedToApproveAsSupervisor = "RestRep.IsAllowedToApproveAsSupervisor";
        public const string IsAllowedToApproveAsTraining = "RestRep.IsAllowedToApproveAsTraining";
        public const string IsAllowedToViewAllReports = "RestRep.IsAllowedToViewAllReports";
        public const string IsAllowedToDeleteIncidents = "RestRep.IsAllowedToDeleteIncidents";
        public const string IsAllowedToUpdateIncidents = "RestRep.IsAllowedToUpdateIncidents";

        public class Generator : AuthorizationPolicyGenerator {

            public Generator() {

                AddPolicy(IsAllowedToSubmit, policy =>
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(ResistanceResponsePermissions.AllowedToSubmit)));

                AddPolicy(IsAllowedToApproveAsSupervisor, policy =>
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(ResistanceResponsePermissions.AllowedToApproveAsSupervisor)));

                AddPolicy(IsAllowedToApproveAsTraining, policy =>
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(ResistanceResponsePermissions.AllowedToApproveAsTraining)));

                AddPolicy(IsAllowedToViewAllReports, policy =>
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(ResistanceResponsePermissions.AllowedToViewAllReports)));

                AddPolicy(IsAllowedToDeleteIncidents, policy =>
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(ResistanceResponsePermissions.AllowedToDeleteIncidents)));

                AddPolicy(IsAllowedToUpdateIncidents, policy =>
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(ResistanceResponsePermissions.AllowedToUpdateIncidents)));

            }

        }

    }

}
