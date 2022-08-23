using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FilesReadWrite : FilesRead
    {
        [HttpPost]
        public async Task Create([FromForm] FileContent content)
        {
            if (string.IsNullOrWhiteSpace(content?.FilePath))
            {
                throw new ArgumentException("File name must be supplied.", nameof(content.FilePath));
            }

            var fullPath = Path.Combine(Configuration?.RootPath ?? "", content.FilePath);

            using (var stream = System.IO.File.OpenWrite(fullPath))
            {
                await content.Data.CopyToAsync(stream);
            }
        }

        [HttpDelete]
        public void Delete(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path must be supplied.", nameof(filePath));
            }

            var fullPath = Path.Combine(Configuration?.RootPath ?? "", filePath);
            File.Delete(fullPath);
        }
    }
}
