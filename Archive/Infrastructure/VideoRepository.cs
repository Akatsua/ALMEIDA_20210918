using Archive.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive.Infrastructure
{
    public class VideoRepository
    {
        private readonly VideoContext VideoContext;

        public VideoRepository(VideoContext videoContext)
        {
            VideoContext = videoContext ?? 
                throw new ArgumentNullException(nameof(videoContext));
        }

        public async Task<IEnumerable<Dto.Video>> ListVideosAsync()
        {
            return await VideoContext
                .Videos
                .Select(v => new Dto.Video
                {
                    VideoId = v.VideoId,
                    VideoCategory = v.VideoCategory,
                    VideoFilename = v.VideoFilename,
                    VideoTitle = v.VideoTitle,
                    VideoThumbnailSmall = v.VideoThumbnailSmall,
                    VideoThumbnailMedium = v.VideoThumbnailMedium,
                    VideoThumbnailLarge = v.VideoThumbnailLarge
                })
                .ToListAsync();
        }

        // I would probably not store the thumbnails in the database, but in 
        // the storage.
        public async Task InsertVideoAsync(Dto.Video video)
        {
            VideoContext.Videos.Add(
                new Video(
                    video.VideoId, 
                    video.VideoTitle, 
                    video.VideoCategory, 
                    video.VideoFilename,
                    video.VideoThumbnailSmall,
                    video.VideoThumbnailMedium,
                    video.VideoThumbnailLarge));

            await VideoContext.SaveChangesAsync();
        }
    }
}
