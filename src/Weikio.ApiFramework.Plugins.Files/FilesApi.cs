using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace Weikio.ApiFramework.Plugins.Files
{
    public partial class FilesApi
    {
        public FileApiConfiguration Configuration { get; set; }

        public async Task<ApiFileResult> GetFileAsync(string filePath)
        {
            var result = new ApiFileResult();
            var path = Path.Combine(Configuration.RootPath, filePath);

            using (var file = File.Open(path, System.IO.FileMode.Open))
            {
                result.Bytes = new byte[file.Length];
                await file.ReadAsync(result.Bytes, 0, (int)file.Length);
            }

            var mimeConverter = new FileExtensionContentTypeProvider();
            
            if (mimeConverter.TryGetContentType(filePath, out var mimeType))
            {
                result.MimeType = mimeType;
            }
            
            return result;
        }

        public ApiFileListResult ListFiles(string directory = "")
        {
            string path;

            if (string.IsNullOrWhiteSpace(directory))
            {
                path = Configuration.RootPath;
            }
            else
            {
                path = Path.Combine(Configuration.RootPath, directory);
            }

            var files = Directory.GetFiles(path);
            var subDirectories = Directory.GetDirectories(path);

            return new ApiFileListResult { Directory = directory, Files = new List<string>(files), SubDirectories = new List<string>(subDirectories) };
        }
    }
}
