using Autofac;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;

namespace FamilyCookbook.Repository
{
    public class AutofacRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DapperDBContext>().AsSelf().SingleInstance();

            builder.RegisterType<CategoryRepository>()
                .As<IRepository<Category>>().InstancePerLifetimeScope();
        }
    }
}
