using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Police.Web {

    public class WebModule : Module {

        protected override void Load(ContainerBuilder builder) {

            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().SingleInstance();
            builder.Register(context => {
                var actionContext = context.Resolve<IActionContextAccessor>().ActionContext;
                var factory = context.Resolve<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            }).As<IUrlHelper>().InstancePerLifetimeScope();

        }

    }

}