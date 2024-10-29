namespace FamilyCookbook.Service
{
    public class BannerService : AbstractService<Banner, BannerFilter>, IBannerService
    {
        private readonly IBannerRepository _repository;

        public BannerService(IBannerRepository repository) : base(repository) 
        {
            _repository = repository;
        }

        public async Task<MessageResponse> CreateAsync(Banner banner)
        {
            banner.DateCreated = DateTime.Now;
            banner.DateUpdated = DateTime.Now;
            banner.IsActive = true;
            
            var response = await _repository.CreateAsync(banner);

            return response;
        }

        public async Task<RepositoryResponse<Lazy<List<Banner>>>> PaginateAsync(Paging paging, BannerFilter filter)
        {
            var response = await _repository.PaginateAsync(paging, filter);

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);

            return response;

        }
    }
}
