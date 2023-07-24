namespace Vmp.Services.Extensions
{
    using System;
    public static class HelperClass
    {

        public static class PaginationService
        {
            public static PaginatedList<T> GetPaginatedList<T>(ICollection<T> items, int currentPage, int rowsPerPage)
            {
                int totalItems = items.Count;
                int totalPages = (int)Math.Ceiling(totalItems / (double)rowsPerPage);
                int startIndex = (currentPage - 1) * rowsPerPage;

                var paginatedItems = items.Skip(startIndex).Take(rowsPerPage).ToList();

                return new PaginatedList<T>(paginatedItems, currentPage, totalPages);
            }
        }

        public class PaginatedList<T>
        {
            public List<T> Items { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }

            public PaginatedList(List<T> items, int currentPage, int totalPages)
            {
                Items = items;
                CurrentPage = currentPage;
                TotalPages = totalPages;
            }
        }
    }
}
