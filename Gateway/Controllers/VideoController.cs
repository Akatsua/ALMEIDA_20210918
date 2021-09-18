using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Gateway.Application;
using Infrastructure;
using Gateway.Infrastructure;
using Gateway.Model;
using Gateway.Dto;
using System.IO;
using System.Linq;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly AzuriteRepository Repository;
        private readonly FileUtils FileUtils;
        private readonly ArchiveRepository Archive;
        private readonly CategoryRepository Categories;

        public VideoController(AzuriteRepository repository, FileUtils fileUtils, ArchiveRepository archive, CategoryRepository categories)
        {
            Repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));
            FileUtils = fileUtils ?? 
                throw new ArgumentNullException(nameof(fileUtils));
            Archive = archive ?? 
                throw new ArgumentNullException(nameof(archive));
            Categories = categories ?? 
                throw new ArgumentNullException(nameof(categories));
        }

        [HttpGet("category")]
        public async Task<IActionResult> ListCategoriesAsync()
        {
            var categories = await Categories.GetCategoriesAsync();

            return Ok(categories);
        }

        // Probably would add pagination in the endpoint
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var videos = await Archive.ListVideosAsync();

            var vmodel = videos.Select(v => new Model.VideoModel()
            {
                VideoId = v.Id,
                VideoCategory = v.Category,
                VideoFilename = Repository.GetFullBlobUrl(v.Filename),
                VideoTitle = v.Title,
                VideoThumbnailSmall = v.ThumbnailSmall,
                VideoThumbnailMedium = v.ThumbnailMedium,
                VideoThumbnailLarge = v.ThumbnailLarge
            })
            .ToList();

            var model = new VideoListModel(vmodel);

            return Ok(model);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadModel uploadData)
        {
            if (uploadData == null)
            {
                return BadRequest();
            }

            // This can be cached / and can be on redis
            var categories = await Categories.GetCategoriesAsync();

            // User text validation
            if (string.IsNullOrWhiteSpace(uploadData.Title) ||
                string.IsNullOrWhiteSpace(uploadData.Category) ||
                !categories.Contains(uploadData.Category.ToString()))
            {
                return BadRequest("Metadata failure");
            }

            // File size & type validation
            if (uploadData.File == null ||
                !FileUtils.IsFileStreamAllowed(uploadData.File.FileName, uploadData.File.Length))
            {
                return BadRequest("File failure");
            }

            var video = Video
                .NewVideo(
                    uploadData.Title, 
                    uploadData.Category, 
                    Path.GetExtension(uploadData.File.FileName));

            await Repository
                .UploadStreamAsync(
                    uploadData.File.OpenReadStream(), 
                    video.Filename);

            // To be changed for an event (via message broker)
            _ = Archive.NotifyNewFile(video);

            return Ok();

        }
    }
}
