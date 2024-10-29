namespace FamilyCookbook.Model
{
    public class PaginatedList<T> where T : class
    {
        public  T Items { get; set; }
        public int TotalCount { get; set; }

        public int PageCount { get; set; }
    }
}
