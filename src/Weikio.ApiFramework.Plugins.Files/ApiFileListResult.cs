using System.Collections.Generic;

namespace Weikio.ApiFramework.Plugins.Files
{
    public partial class FilesApi
    {
        public class ApiFileListResult
        {
            public string Directory { get; set; }
            public List<string> Files { get; set; }
            public List<string> SubDirectories { get; set; }
        }
    }
}
