using System.Linq;
using Police.Security.User;

namespace Police.Security.Authorization.Requirements {

    public class MustHavePermission : UserBasedRequirement {

        public string PermissionName { get; }

        public MustHavePermission(string permissionName) {
            PermissionName = permissionName;
        }

        public class Handler : Handler<MustHavePermission> {

            public Handler(IUserService userService) : base(userService) { }

            protected override bool HandleRequirementForUser(UserInformation userInformation,
                MustHavePermission requirement) =>
                userInformation.EffectivePermissions.Contains(requirement.PermissionName);

        }

    }

}