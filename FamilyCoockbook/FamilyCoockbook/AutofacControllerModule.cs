using Autofac;
using FamilyCookbook.Controllers;
using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;
using FamilyCookbook.REST_Models.Member;
using FamilyCookbook.REST_Models.Picture;
using FamilyCookbook.REST_Models.Recipe;
using static FamilyCookbook.REST_Models.Comment.CommentModels;

namespace FamilyCookbook
{
    public class AutofacControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MemberMapperWrapper>()
                .As<IMapper<Member, MemberRead, MemberCreate>>().InstancePerDependency();

            builder.RegisterType<CommentMapperWrapper>()
                .As<IMapper<Comment, CommentRead, CommentCreate>>().InstancePerDependency();

            builder.RegisterType<CategoryMapperWrapper>()
                .As<IMapper<Category,CategoryRead, CategoryCreate>>().InstancePerDependency(); 

            builder.RegisterType<PictureMapperWrapper>()
                .As<IMapper<Picture, PictureRead, PictureCreate>>().InstancePerDependency();

            builder.RegisterType<RecipeMapperWrapper>()
                .As<IMapperExtended<Recipe, RecipeRead, RecipeCreate, RecipeCreateDTO>>()
                .InstancePerDependency();
        }
    }
}
