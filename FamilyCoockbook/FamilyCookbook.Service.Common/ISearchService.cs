namespace FamilyCookbook.Service.Common
{
    public interface ISearchService
    {
        Task<RepositoryResponse<List<Recipe>>> GetAllBySearchText(string searchText);
    }
}
