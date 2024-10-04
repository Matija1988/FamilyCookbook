using FamilyCookbook.Model;
using static FamilyCookbook.REST_Models.Comment.CommentModels;

namespace FamilyCookbook.Mapping.MapperWrappers
{
    public class CommentMapperWrapper : IMapper<Comment, CommentRead, CommentCreate>
    {
        private readonly CommentMapper _mapper = new();

        public CommentRead MapReadToDto(Comment entity)
        {
            return _mapper.CommentRead(entity);
        }

        public Comment MapToEntity(CommentCreate dto)
        {
            return _mapper.CommentCreate(dto);
        }

        public List<CommentRead> MapToReadList(List<Comment> entities)
        {
            return _mapper.CommentsReadList(entities);
        }
    }
}
