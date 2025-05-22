using MoviesAPI.DTOs;

namespace MoviesAPI.Utilites
{
    public static class IQueryableExterions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> querybale,
            PaginationDTO pagination) {
            return querybale
                .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
                .Take(pagination.RecordsPerPage);
        }
    }
}
