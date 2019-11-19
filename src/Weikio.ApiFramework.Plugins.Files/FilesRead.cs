using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FilesRead
    {
        public FileApiConfiguration Configuration { get; set; }

        public async Task<FileResult> GetAsync(string filePath)
        {
            var result = new FileResult();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path must be supplied.", nameof(filePath));
            }

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

        public FileListResult List(string directory = "")
        {
            string path;
            var result = new FileListResult
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
    }
}
