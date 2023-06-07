using Microsoft.AspNetCore.Authorization;
using Police.Security.Authorization.Requirements;

namespace Police.Security.Authorization {

    public static class AuthorizationOptionsExtensions {

        public static void RegisterAuthorizationPolicies(this AuthorizationOptions authorizationOptions) {
            authorizationOptions.AddPolicy(AuthorizationPolicies.MustBeActiveUser, policy =>
                policy.Requirements.Add(new MustBeActiveUserRequirement()));
        }

    }

}