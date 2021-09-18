using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive.Dto
{
    public class Video
    {
        public Video()
        {
        }

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

        public string VideoTitle { get; set; }
        public string VideoCategory { get; set; }
        public string VideoFilename { get; set; }

        public byte[] VideoThumbnailSmall { get; set; }
        public byte[] VideoThumbnailMedium { get; set; }
        public byte[] VideoThumbnailLarge { get; set; }
    }
}
