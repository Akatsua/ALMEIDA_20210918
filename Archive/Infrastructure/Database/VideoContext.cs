using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive.Infrastructure.Database
{
    public class VideoContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }

        public VideoContext(DbContextOptions<VideoContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var videoTable = modelBuilder.Entity<Video>().ToTable("video");
        }
    }

    public class Video
    {
        public Video(Guid videoId, string videoTitle, string videoCategory, string videoFilename)
        {
            VideoId = videoId;
            VideoTitle = videoTitle;
            VideoCategory = videoCategory;
            VideoFilename = videoFilename;
        }

        public Video(
            Guid videoId, 
            string videoTitle, 
            string videoCategory, 
            string videoFilename, 
            byte[] videoThumbnailSmall, 
            byte[] videoThumbnailMedium, 
            byte[] videoThumbnailLarge) 
            : this(videoId, videoTitle, videoCategory, videoFilename)
        {
            VideoThumbnailSmall = videoThumbnailSmall;
            VideoThumbnailMedium = videoThumbnailMedium;
            VideoThumbnailLarge = videoThumbnailLarge;
        }

        public Guid VideoId { get; set; }

        // Could be replaced by a json / bjson metadata blob
        public string VideoTitle { get; set; }
        public string VideoCategory { get; set; }
        public string VideoFilename { get; set; }

        // These can be separated in a different table if dynamic sizes
        // start to be relevant 
        public byte[] VideoThumbnailSmall { get; set; }
        public byte[] VideoThumbnailMedium { get; set; }
        public byte[] VideoThumbnailLarge { get; set; }
    }
}
