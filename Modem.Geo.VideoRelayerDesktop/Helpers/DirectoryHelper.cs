using Modem.Geo.VideoRelayerDesktop.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Helpers
{
    internal class DirectoryHelper
    {
        private static readonly string _basePath = Path.Combine(Settings.Default.batPath, "Modem.Geo.VideoRelayerDesktop");

        public static readonly string FFmpegPath = Path.Combine(Settings.Default.batPath, "ffmpeg.exe");

        public static string BuildCameraDirectoryPath(long wellboreId, long cameraId)
        {
            return Path.Combine(
                BuildWellboreDirectoryPath(wellboreId),
                cameraId.ToString());
        }

        public static string BuildWellboreDirectoryPath(long wellboreId)
        {
            return Path.Combine(_basePath, wellboreId.ToString());
        }

        public static void ThrowIfNoFFmpeg()
        {
            if (!File.Exists(FFmpegPath))
                throw new ArgumentException($"Executable inside {FFmpegPath} doesn't exist!", nameof(FFmpegPath));
        }

        public static string[] CreateOutputFiles(long id)
        {
            string[] res = new string[2];

            FileInfo StdOut = new FileInfo(Path.Combine(_basePath, $"std{id}.txt"));
            if (!StdOut.Exists)
            {
                StdOut.Create();
            }
            
            FileInfo ErrOut = new FileInfo(Path.Combine(_basePath, $"err{id}.txt"));
            if (!ErrOut.Exists)
            {
                ErrOut.Create();
            }

            res[0] = StdOut.FullName;
            res[1] = ErrOut.FullName;
            return res;
        }
    }
}
