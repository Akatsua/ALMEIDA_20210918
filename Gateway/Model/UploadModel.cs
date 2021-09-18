using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Model
{
    public class UploadModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public IFormFile File { get; set; }
    }
}
