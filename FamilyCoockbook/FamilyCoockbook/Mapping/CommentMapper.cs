using FamilyCookbook.Model;
using Riok.Mapperly.Abstractions;
using static FamilyCookbook.REST_Models.Comment.CommentModels;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public partial class CommentMapper
    {
        public partial List<CommentRead> CommentsReadList(List<Comment> comment);

        public partial CommentRead CommentRead (Comment comment);
    }
}
