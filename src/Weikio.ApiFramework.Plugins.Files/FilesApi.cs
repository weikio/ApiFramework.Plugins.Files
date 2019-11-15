using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace Weikio.ApiFramework.Plugins.Files
{
    public partial class FilesApi
    {
        public FileApiConfiguration Configuration { get; set; }

        public async Task<ApiFileResult> GetAsync(string filePath)
        {
            var result = new ApiFileResult();
            var path = Path.Combine(Configuration?.RootPath ?? "", filePath);

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

            var rootPath = Configuration?.RootPath ?? "";

            if (string.IsNullOrWhiteSpace(directory))
            {
                path = rootPath;
            }
            else
            {
                path = Path.Combine(rootPath, directory);
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

        public async Task Create(string filePath, FileContent content) 
        {
            //TODO: Find a way to get byte array as an argument directly. FileContent class is only to make the API to read the contents from body instead
            //of query string, and FileContent.Content expects Base64 encoded string instead of a byte array.

            if (Configuration.Mode != FileMode.ReadWrite)
            {
                throw new UnauthorizedAccessException("Mode is set to Read in configuration, writing is not allowed.");
            }

            var decoded = Convert.FromBase64String(content.Content);
            
            var fullPath = Path.Combine(Configuration?.RootPath ?? "", filePath);

            using (var file = File.OpenWrite(fullPath))
            {
                await file.WriteAsync(decoded, 0, decoded.Length);
            }            
        }       
    }
    
    public class FileContent 
    {
        public string Content { get; set; }
    }
}
