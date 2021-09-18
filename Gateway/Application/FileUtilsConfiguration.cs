using System.Collections.Generic;

namespace Gateway.Application
{
    public class FileUtilsConfiguration
    {
        public IEnumerable<string> AcceptedTypes { get; set; }
        public long AcceptedMaxSize { get; set; }
    }
}