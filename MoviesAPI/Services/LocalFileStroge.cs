
namespace MoviesAPI.Services
{
    public class LocalFileStroge : IFileStorge
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalFileStroge(IWebHostEnvironment env , IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task Delete(string container, string? route)
        {
            if (string.IsNullOrEmpty(route)) {
                return Task.CompletedTask;
            }
            var fileName = Path.GetFileName(route);
            var fileDirectory = Path.Combine(env.WebRootPath, container, fileName);

            if (File.Exists(fileDirectory)) {
                File.Delete(fileDirectory);
            }
            return Task.CompletedTask;
        }

        public async Task<string> Store(string container, IFormFile file)
        {
            var extenion = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extenion}";

            string folder = Path.Combine(env.WebRootPath, container);

            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            var root = Path.Combine(folder, fileName);

            using (var ms = new MemoryStream()) {
                await file.CopyToAsync(ms);
                var content = ms.ToArray();
                await File.WriteAllBytesAsync(root, content);
            }

            var request = httpContextAccessor.HttpContext!.Request;
            var url = $"{request.Scheme}://{request.Host}";
            var fileURL = Path.Combine(url, container , fileName).Replace("\\", "/");
            return fileURL;


        }
    }
}
