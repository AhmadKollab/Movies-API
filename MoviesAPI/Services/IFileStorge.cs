namespace MoviesAPI.Services
{
    public interface IFileStorge 
    {
        Task<string> Store(string container , IFormFile file);
        Task Delete (string container , string? route);

        async Task<string> edit(string container, string? route, IFormFile file) {
            Delete(container,route);
            return await Store(container,file);
        }
    }
}
