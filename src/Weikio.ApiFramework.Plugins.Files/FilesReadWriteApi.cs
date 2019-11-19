using System;
using System.IO;
using System.Threading.Tasks;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FilesReadWrite : FilesRead
    {
        public async Task Create(string filePath, FileContent content)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path must be supplied.", nameof(filePath));
            }

            //TODO: Find a way to get byte array as an argument directly. FileContent class is only to make the API to read the contents from body instead
            //of query string, and FileContent.Content expects Base64 encoded string instead of a byte array.
            var decoded = Convert.FromBase64String(content.Content);

            var fullPath = Path.Combine(Configuration?.RootPath ?? "", filePath);

            using (var file = File.OpenWrite(fullPath))
            {
                await file.WriteAsync(decoded, 0, decoded.Length);
            }
        }

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
