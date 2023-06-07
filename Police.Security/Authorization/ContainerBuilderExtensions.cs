﻿using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Police.Security.Authorization.Policies;
using Police.Security.Authorization.Requirements;

namespace Police.Security.Authorization {

    public static class ContainerBuilderExtensions {

        public static ContainerBuilder RegisterAuthorizationHandlers(
            this ContainerBuilder builder,
            params Assembly[] assemblies) {
            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(AuthorizationHandler<>))
                .As<IAuthorizationHandler>().AsSelf().SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterAuthorizationPolicyGenerators(this ContainerBuilder builder,
            params Assembly[] assemblies) {

            builder.RegisterAssemblyTypes(assemblies).AssignableTo<AuthorizationPolicyGenerator>()
                .As<AuthorizationPolicyGenerator>().SingleInstance();

            return builder;

        }

        public static ContainerBuilder RegisterAuthorizationPolicyProvider(this ContainerBuilder builder) {

            builder.RegisterType<GeneratedAuthorizationPolicyProvider>().As<IAuthorizationPolicyProvider>()
                .InstancePerDependency();

            return builder;

        }

    }

}