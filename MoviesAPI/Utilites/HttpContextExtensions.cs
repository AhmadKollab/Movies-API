using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Utilites
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPahintionParameterInHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable) {
            if (httpContext is null) {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Append("total-records-count", count.ToString());
        }
    }
}
