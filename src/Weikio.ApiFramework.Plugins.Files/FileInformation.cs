using System;

namespace Weikio.ApiFramework.Plugins.Files
{
    public class FileInformation
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
