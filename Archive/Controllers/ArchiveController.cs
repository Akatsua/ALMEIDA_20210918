using Archive.Helpers;
using Archive.Infrastructure;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive.Controllers
{
    [ApiController]
    [Route("api/archive")]
    public class ArchiveController : ControllerBase
    {
        private readonly VideoRepository VideoRepository;
        private readonly ThumbnailHelper ThumbnailHelper;

        public ArchiveController(VideoRepository videoRepository, ThumbnailHelper thumbnailHelper)
        {
            VideoRepository = videoRepository ?? 
                throw new ArgumentNullException(nameof(videoRepository));
            ThumbnailHelper = thumbnailHelper ?? 
                throw new ArgumentNullException(nameof(thumbnailHelper));
        }

        // I would swap this API call for a service that receives messages via
        // message broker (RabbitMQ, ServiceBus, ...)
        [HttpPost("")]
        public async Task<IActionResult> Register(VideoContract video)
        {
            var thumbnails = await ThumbnailHelper.GenerateThumbnailsAsync(video.Filename);

            // A better possibility in terms of scaling is inserting the video in the DB with
            // default thumbnails and start another async task that generates the thumbnails 
            // and updates the database. For simplicity, i decided to insert directly with 
            // generated thumbnails
            await VideoRepository
                .InsertVideoAsync(
                    new Dto.Video(
                        video.Id, 
                        video.Title, 
                        video.Category, 
                        video.Filename,
                        thumbnails.Item1,
                        thumbnails.Item2,
                        thumbnails.Item3
                    ));

            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            var videos = await VideoRepository.ListVideosAsync();

            var contract = videos
                .Select(vid => new VideoContract(
                    vid.VideoId, 
                    vid.VideoFilename, 
                    vid.VideoTitle, 
                    vid.VideoCategory,
                    vid.VideoThumbnailSmall,
                    vid.VideoThumbnailMedium,
                    vid.VideoThumbnailLarge))
                .ToList();

            return Ok(contract);
        }
    }
}
