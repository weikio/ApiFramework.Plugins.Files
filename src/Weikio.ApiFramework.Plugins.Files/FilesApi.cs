using System.IO;
using System.Threading.Tasks;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FilesApi
    {
        public FileApiConfiguration Configuration { get; set; }

        public async Task<ApiFileResult> GetFile(string filePath)
        {
            var result = new ApiFileResult();
            var path = Path.Combine(Configuration.RootPath, filePath);

            using (var file = File.Open(path, System.IO.FileMode.Open))
            {
                result.Bytes = new byte[file.Length];
                await file.ReadAsync(result.Bytes, 0, (int)file.Length);
            }

            return result;
        }
    }
}
