
using Autofac;

namespace FamilyCookbook.Common
{
    public class AutofacCommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ErrorMessages>().As<IErrorMessages>().InstancePerDependency();
            builder.RegisterType<SuccessResponses>().As<ISuccessResponses>().InstancePerDependency();
        }
    }
}
