using System;

namespace Contracts
{
    public class VideoContract
    {
        public VideoContract()
        {

        }

        public VideoContract(Guid id, string filename, string title, string category)
        {
            Id = id;
            Filename = filename;
            Title = title;
            Category = category;
        }

        public VideoContract(
            Guid id, 
            string filename, 
            string title, 
            string category, 
            byte[] thumbnailSmall, 
            byte[] thumbnailMedium, 
            byte[] thumbnailLarge) 
            : this(id, filename, title, category)
        {
            ThumbnailSmall = thumbnailSmall;
            ThumbnailMedium = thumbnailMedium;
            ThumbnailLarge = thumbnailLarge;
        }

        public Guid Id { get; set; }

        public string Filename { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public byte[] ThumbnailSmall { get; set; }

        public byte[] ThumbnailMedium { get; set; }

        public byte[] ThumbnailLarge { get; set; }
    }
}
