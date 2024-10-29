namespace FamilyCookbook.Repository.Common
{
    public interface ISearchRepository
    {
        Task<RepositoryResponse<List<Recipe>>> GetAllBySearchText(string searchText);
    }
}
