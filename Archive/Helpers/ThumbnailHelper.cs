using Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Archive.Helpers
{
    public class ThumbnailHelper
    {
        private readonly AzuriteRepository azuriteRepository;

        // Default thumbnail cache
        private byte[] ThumbnailS = null;
        private byte[] ThumbnailM = null;
        private byte[] ThumbnailL = null;

        public ThumbnailHelper(AzuriteRepository azuriteRepository)
        {
            this.azuriteRepository = azuriteRepository ??
                throw new ArgumentNullException(nameof(azuriteRepository));
        }

        public byte[] GetDefaultThumbnail(ThumbnailSize size)
        {
            if (ThumbnailS == null ||
                ThumbnailM == null ||
                ThumbnailL == null)
            {
                ThumbnailS = File.ReadAllBytes("Dependencies/64.png");
                ThumbnailM = File.ReadAllBytes("Dependencies/128.png");
                ThumbnailL = File.ReadAllBytes("Dependencies/256.png");
            }

            if (size == ThumbnailSize.Small)
            {
                return ThumbnailS;
            }
            else if (size == ThumbnailSize.Medium)
            {
                return ThumbnailM;
            }

            return ThumbnailL;
        }

        public async Task<(byte[], byte[], byte[])> GenerateThumbnailsAsync(string blobname)
        {
            try
            {
                await azuriteRepository.DownloadFileAsync(blobname);

                var thumbnail = Guid.NewGuid() + ".png";

                var tasks = new Task[]
                {
                    GenerateThumbnailAsync(blobname, "64x64"  , "s_" + thumbnail),
                    GenerateThumbnailAsync(blobname, "128x128", "m_" + thumbnail),
                    GenerateThumbnailAsync(blobname, "256x256", "l_" + thumbnail),
                };

                Task.WaitAll(tasks);

                var bytess = File.ReadAllBytes("s_" + thumbnail);
                var bytesm = File.ReadAllBytes("m_" + thumbnail);
                var bytesl = File.ReadAllBytes("l_" + thumbnail);

                File.Delete(blobname);
                File.Delete("s_" + thumbnail);
                File.Delete("m_" + thumbnail);
                File.Delete("l_" + thumbnail);

                return new(bytess, bytesm, bytesl);
            }
            catch (Exception)
            {
                return new
                (
                    GetDefaultThumbnail(ThumbnailSize.Small),
                    GetDefaultThumbnail(ThumbnailSize.Medium),
                    GetDefaultThumbnail(ThumbnailSize.Large)
                );   
            }

        }

        private static async Task GenerateThumbnailAsync(string blobname, string sizeStr, string thumbnail)
        {
            var process = new Process
            {
                StartInfo =
                    {
                        FileName = "Dependencies/ffmpeg",
                        Arguments = $"-i /app/{blobname} -vframes 1 -s {sizeStr} -y {thumbnail}\""
                    },
                EnableRaisingEvents = true
            };

            process.Start();
            await process.WaitForExitAsync();
        }
    }

    public enum ThumbnailSize
    {
        Small,
        Medium,
        Large
    }
}
