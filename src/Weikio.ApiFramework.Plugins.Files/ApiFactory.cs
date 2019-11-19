using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weikio.ApiFramework.Plugins.Files
{
    public static class ApiFactory
    {
        public static Task<IEnumerable<Type>> Create(FileApiConfiguration configuration)
        {
            var result = new List<Type>();

            if (configuration.Mode == FileMode.ReadWrite)
            {
                result.Add(typeof(FilesReadWrite));
            }
            else
            {
                result.Add(typeof(FilesRead));
            }

            return Task.FromResult<IEnumerable<Type>>(result);
        }        
    }
}
