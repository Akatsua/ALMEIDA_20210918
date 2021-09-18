using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Model
{
    public class VideoListModel
    {
        public VideoListModel(IEnumerable<VideoModel> videos)
        {
            Videos = videos;
        }

        public IEnumerable<VideoModel> Videos { get; set; }
    }

    public class VideoModel
    {
        public Guid VideoId { get; set; }

        public string VideoTitle { get; set; }
        public string VideoCategory { get; set; }
        public string VideoFilename { get; set; }

        public byte[] VideoThumbnailSmall { get; set; }
        public byte[] VideoThumbnailMedium { get; set; }
        public byte[] VideoThumbnailLarge { get; set; }
    }
}
