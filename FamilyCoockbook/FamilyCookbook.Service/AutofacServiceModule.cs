﻿using Autofac;
using FamilyCookbook.Model;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public class AutofacServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerDependency();

            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerDependency();

            builder.RegisterType<PictureService>().As<IPictureService>().InstancePerDependency();

            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerDependency();

            builder.RegisterType<RecipeService>().As<IRecipeService>().InstancePerDependency();

            builder.RegisterType<CommentService>().As<ICommentService>().InstancePerDependency();

            builder.RegisterType<TagService>().As<ITagService>().InstancePerDependency();   

            builder.RegisterType<SearchService>().As<ISearchService>().InstancePerDependency(); 

            builder.RegisterType<BannerService>().As<IBannerService>().InstancePerDependency();

            builder.RegisterType<BannerPositionService>().As<IBannerPositionService>().InstancePerDependency();
        }
    }
}
