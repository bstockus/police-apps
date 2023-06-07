using System.Reflection;
using Autofac;
using Police.Security.Authorization;
using Police.Security.User;
using Module = Autofac.Module;

namespace Police.Security {

    public class SecurityModule : Module {

        private readonly Assembly[] _businessAssemblies;

        public SecurityModule(
            Assembly[] businessAssemblies) {

            _businessAssemblies = businessAssemblies;
        }

        protected override void Load(ContainerBuilder builder) {

            builder
                .RegisterAuthorizationHandlers(ThisAssembly)
                .RegisterAuthorizationPolicyGenerators(_businessAssemblies)
                .RegisterAuthorizationPolicyProvider()
                .RegisterUserService();

        }

    }

}
