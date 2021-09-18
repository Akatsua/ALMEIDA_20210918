using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AzuriteRepository
    {
        private readonly AzuriteConfiguration Configuration;

        public AzuriteRepository(IOptions<AzuriteConfiguration> configuration)
        {
            Configuration = configuration.Value;
        }

        public string GetFullBlobUrl(string file)
        {
            return Path.Combine(Configuration.Endpoint, "filedata", file);
        }

        public async Task<string> UploadStreamAsync(Stream stream, string blobname)
        {
            BlobContainerClient container = new BlobContainerClient(Configuration.ConnectionString, "filedata");

            if(!await container.ExistsAsync())
            {
                await container.CreateAsync(PublicAccessType.Blob);
            }

            // We can specify blob access here if we need more granularity
            BlobClient blob = container.GetBlobClient(blobname);

            await blob.UploadAsync(stream);

            return Path.Combine(Configuration.Endpoint, "filedata", blobname);
        }

        public async Task DownloadFileAsync(string blobname)
        {
            BlobContainerClient container = new BlobContainerClient(Configuration.ConnectionString, "filedata");

            if (!await container.ExistsAsync())
            {
                await container.CreateAsync(PublicAccessType.Blob);
            }

            BlobClient blob = container.GetBlobClient(blobname);
            
            await blob.DownloadToAsync(blobname);
        }
    }
}
