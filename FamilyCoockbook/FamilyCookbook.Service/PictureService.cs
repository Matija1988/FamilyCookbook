using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public class PictureService : IService<Picture>
    {
        private readonly IPictureRespository _repository;

        public PictureService(IPictureRespository repository)
        {
            _repository = repository;
        }

        public async Task<RepositoryResponse<Picture>> CreateAsync(Picture entity)
        {
            var response = await _repository.CreateAsync(entity);

            return response;
        }

        public async Task<RepositoryResponse<Picture>> DeleteAsync(int id)
        {
            var response = await _repository.DeleteAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<List<Picture>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            return response;
        }

        public async Task<RepositoryResponse<Picture>> GetByIdAsync(int id)
        {
            var response = await _repository.GetByIdAsync(id);

            return response;
        }

        public async Task<RepositoryResponse<Picture>> UpdateAsync(int id, Picture entity)
        {
            var response = await _repository.UpdateAsync(id, entity);

            return response;
        }
    }
}
