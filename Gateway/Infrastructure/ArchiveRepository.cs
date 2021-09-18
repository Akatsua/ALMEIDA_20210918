using Contracts;
using Gateway.Dto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Infrastructure
{
    public class ArchiveRepository
    {
        private readonly ArchiveRepositoryConfiguration Configuration;

        public ArchiveRepository(IOptions<ArchiveRepositoryConfiguration> configuration)
        {
            Configuration = configuration.Value ?? 
                throw new ArgumentNullException(nameof(configuration));
        }

        public async Task NotifyNewFile(Video video)
        {
            var client = new HttpClient();

            var data = JsonConvert.SerializeObject(
                new VideoContract(
                    video.Id, 
                    video.Filename, 
                    video.Title, 
                    video.Category));

            await client.PostAsync(Configuration.Host + "/api/archive", new StringContent(data, Encoding.UTF8, "application/json"));
        }

        public async Task<IEnumerable<Video>> ListVideosAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(Configuration.Host + "/api/archive");

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject<IEnumerable<VideoContract>>(json)
                .Select(contract => new Video(
                    contract.Id, 
                    contract.Filename, 
                    contract.Title, 
                    contract.Category, 
                    contract.ThumbnailSmall, 
                    contract.ThumbnailMedium, 
                    contract.ThumbnailLarge))
                .ToList();
        }
    }
}
