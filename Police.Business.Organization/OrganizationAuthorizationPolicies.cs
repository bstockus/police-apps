using Police.Security.Authorization.Policies;
using Police.Security.Authorization.Requirements;

namespace Police.Business.Organization {

    public class OrganizationAuthorizationPolicies {

        public const string IsAllowedToViewUsers = "Org.IsAllowedToViewUsers";
        public const string IsAllowedToAddAndUpdateUsers = "Org.IsAllowedToAddAndUpdateUsers";

        public class Generator : AuthorizationPolicyGenerator {

            public Generator() {

                AddPolicy(IsAllowedToViewUsers, policy => 
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(OrganizationPermissions.AllowedToViewUsers)));

                AddPolicy(IsAllowedToAddAndUpdateUsers, policy =>
                    policy.AddRequirements(
                        new MustBeActiveUserRequirement(),
                        new MustHavePermission(OrganizationPermissions.AllowedToAddAndUpdateUsers)));

            }

        }

    }

}