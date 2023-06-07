using AutoMapper;
using Police.Business.Abstractions.Identity;
using Police.Business.Identity.Roles;

namespace Police.Business.Identity.Users {

    public class UserIdentityInfoMapping : Profile {

        public UserIdentityInfoMapping() {
            CreateMap<User, IdentityInfo>();
            CreateMap<UserRole, IdentityInfo.IdentityRoleInfo>();
            CreateMap<RolePermission, IdentityInfo.IdentityRoleInfo.IdentityRolePermissionInfo>();
        }

    }

}