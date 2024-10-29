namespace FamilyCookbook.Repository
{
    public class BannerRespository : AbstractRepository<Banner, BannerFilter>, IBannerRepository
    {
        private readonly DapperDBContext _dbContext;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
        public BannerRespository(DapperDBContext context, 
            IErrorMessages errorMessages, 
            ISuccessResponses successResponses) : base(context, errorMessages, successResponses)
        {
            _dbContext = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

    }
}
