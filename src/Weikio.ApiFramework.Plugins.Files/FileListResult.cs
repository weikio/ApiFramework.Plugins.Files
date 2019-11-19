using System.Collections.Generic;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FileListResult
    {
        public string Directory { get; set; }
        public List<FileInformation> Files { get; set; }
        public List<string> SubDirectories { get; set; }
    }
}
