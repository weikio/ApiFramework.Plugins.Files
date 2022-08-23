using Microsoft.AspNetCore.Http;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FileContent 
    {
        public string FilePath { get; set; }
        public IFormFile Data { get; set; }
    }
}
