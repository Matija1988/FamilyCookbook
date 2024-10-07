using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;

namespace FamilyCookbook.Service
{
    public sealed class PictureService : AbstractService<Picture>, IPictureService
    {
        private readonly IPictureRespository _repository;

        public PictureService(IPictureRespository repository) : base(repository) 
        {
            _repository = repository;
        }

        public async Task<MessageResponse> CreateAsync(Picture entity)
        {
            entity.IsActive = true;

            var response = await _repository.CreateAsync(entity);

            return response;
        }


        public async Task<MessageResponse> UpdateAsync(int id, Picture entity)
        {
            var response = await _repository.UpdateAsync(id, entity);

            return response;
        }
    }
}
