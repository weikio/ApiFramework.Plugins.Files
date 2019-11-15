using System;
using System.Collections.Generic;
using System.Text;

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
