using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FilesRead
    {
        public FileApiConfiguration Configuration { get; set; }

        public FileInfo GetAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path must be supplied.", nameof(filePath));
            }

            var path = Path.Combine(Configuration?.RootPath ?? "", filePath);

            return new FileInfo(path);
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
