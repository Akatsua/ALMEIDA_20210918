using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dto
{
    public class Video
    {
        public Video(Guid id, string filename, string title, string category)
        {
            Id = id;
            Filename = filename;
            Title = title;
            Category = category;
        }

        public Video(
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

        public static Video NewVideo(string title, string category, string extension)
        {
            var id = Guid.NewGuid();

            return new Video(id, id.ToString() + "." + extension.Replace(".", ""), title, category);
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
