using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Application
{
    public class FileUtils
    {
        private readonly FileUtilsConfiguration Configuration;

        public FileUtils(IOptions<FileUtilsConfiguration> configuration)
        {
            Configuration = configuration.Value;
        }

        public bool IsFileStreamAllowed(string filename, long filesize)
        {
            var extension = Path.GetExtension(filename.ToLower()).Replace(".", "");

            return Configuration.AcceptedTypes.Contains(extension) && filesize <= Configuration.AcceptedMaxSize;
        }
    }
}
