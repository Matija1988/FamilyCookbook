﻿using Autofac;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Respository.Common;

namespace FamilyCookbook.Repository
{
    public class AutofacRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DapperDBContext>().AsSelf().SingleInstance();

            builder.RegisterType<CategoryRepository>()
                .As<ICategoryRepository>().InstancePerLifetimeScope();

            builder.RegisterType<RoleRepository>()
              .As<IRoleRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<PictureRepository>()
                .As<IPictureRespository>().InstancePerLifetimeScope();


            builder.RegisterType<MemberRepository>()
                .As<IMemberRepository>().InstancePerLifetimeScope();

            builder.RegisterType<RecipeRepository>()
                .As<IRecipeRepository>().InstancePerLifetimeScope();

            builder.RegisterType<CommentRepository>().As<ICommentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TagsRepository>().As<ITagRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SearchRepository>().As<ISearchRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BannerRespository>().As<IBannerRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BannerPositionRepository>().As<IBannerPositionRepository>()
                .InstancePerLifetimeScope();

        }
    }
}
