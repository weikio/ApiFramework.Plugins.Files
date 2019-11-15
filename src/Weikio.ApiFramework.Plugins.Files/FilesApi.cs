using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace Weikio.ApiFramework.Plugins.Files
{
    public partial class FilesApi
    {
        public FileApiConfiguration Configuration { get; set; }

        public async Task<ApiFileResult> GetAsync(string filePath)
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

        public ApiFileListResult List(string directory = "")
        {
            string path;
            var result = new ApiFileListResult
            {
                Directory = directory,
                Files = new List<FileInformation>(),
                SubDirectories = new List<string>()
            };

            if (string.IsNullOrWhiteSpace(directory))
            {
                path = Configuration.RootPath;
            }
            else
            {
                path = Path.Combine(Configuration.RootPath, directory);
            }

            var files = Directory.GetFiles(path);
            var fileInfos = files.Select(f => new FileInfo(f));

            foreach (var fileInfo in fileInfos)
            {                
                result.Files.Add(new FileInformation
                {
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length,
                    UpdatedDate = fileInfo.LastWriteTimeUtc,
                    CreatedDate = fileInfo.CreationTimeUtc
                });
            }

            result.SubDirectories.AddRange(Directory.GetDirectories(path));

            return result;
        }
    }
}
