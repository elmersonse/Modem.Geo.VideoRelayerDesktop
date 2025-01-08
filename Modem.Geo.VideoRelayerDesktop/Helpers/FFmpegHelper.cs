using Modem.Geo.VideoRelayerDesktop.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Helpers
{
    internal class FFmpegHelper
    {
        private static Process CreateFFmpegProcess(string args)
        {
            return Create(DirectoryHelper.FFmpegPath, args);
        }

        private static Process CreateBatProcess(long id, string args)
        {
            var batFilePath = Path.Combine(Settings.Default.batPath, $"{id}.bat");

            if (!File.Exists(batFilePath))
            {
                using (FileStream fs = File.Create(batFilePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes($"ffmpeg.exe {args}");
                    fs.Write(info, 0, info.Length);
                }
            }

            return Create(batFilePath, args);
        }

        private static Process Create(string filePath, string args)
        {
            var proccessStartInfo = new ProcessStartInfo
            {
                FileName = filePath,
                Arguments = args,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = false,//false - will write all logs from process to console
            };

            Process process = new Process();
            process.StartInfo = proccessStartInfo;

            return process;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputUrl">Stream url from camera</param>
        /// <param name="rtmpOutputUrl">Stream url to send video to RTMP server.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws If can't find ffmpeg.exe</exception>
        public static Process CreateRelayProcessForRTSPInputCopy(
            long id,
            string inputUrl,
            string rtmpOutputUrl)
        {
            DirectoryHelper.ThrowIfNoFFmpeg();

            var argsBuilder = new FFmpegArgsBuilder();
            var args = argsBuilder
                .SetProtocolForRTSP("tcp")
                .SetTimeout(10_000)
                .SetInputUrl(inputUrl)
                .SetVerbosity("verbose")
                .SetVideoCodec("copy")
                .SetAudioCodec("aac")
                .SetOutputFormat("flv")
                .SetOutputUrl(rtmpOutputUrl)
            .Build();

            return CreateBatProcess(id, args);
        }

        public static Process CreateRelayProcessForRTSPInputRecode(
            long id,
            string inputUrl,
            string rtmpOutputUrl)
        {
            DirectoryHelper.ThrowIfNoFFmpeg();

            string[] outNames = DirectoryHelper.CreateOutputFiles(id);

            var argsBuilder = new FFmpegArgsBuilder();
            var args = argsBuilder
                .SetProtocolForRTSP("tcp")
                .SetTimeout(10_000)
                .SetInputUrl(inputUrl)
                .SetVerbosity("verbose")
                .SetResolution(426, 240)
                .SetVideoCodec("libx264")
                .SetVideoBitrate(300)
                .SetMaxVideoBitrate(300)
                .SetBufferSize(600)
                .SetPresentForEncoding("veryfast")
                .SetTuneForEncoding("zerolatency")
                .SetAudioCodec("aac")
                .SetAudioBitrate(64)
                .SetOutputFormat("flv")
                .SetOutputUrl(rtmpOutputUrl)
                .SetStandardOutput(outNames[0])
                .SetErrorOutput(outNames[1])
                .Build();

            return CreateBatProcess(id, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputUrl">Stream url from camera</param>
        /// <param name="rtmpOutputUrl">Stream url to send video to RTMP server.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws If can't find ffmpeg.exe</exception>
        public static Process CreateRelayProcessForRTMPInputCopy(
            long id,
            string inputUrl,
            string rtmpOutputUrl)
        {
            DirectoryHelper.ThrowIfNoFFmpeg();

            var argsBuilder = new FFmpegArgsBuilder();
            var args = argsBuilder
                .SetInputUrl(inputUrl)
                .SetVerbosity("verbose")
                .SetVideoCodec("copy")
                .SetAudioCodec("aac")
                .SetOutputFormat("flv")
                .SetOutputUrl(rtmpOutputUrl)
                .Build();

            return CreateBatProcess(id, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputUrl">Stream url from camera</param>
        /// <param name="rtmpOutputUrl">Stream url to send video to RTMP server.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws If can't find ffmpeg.exe</exception>
        public static Process CreateRelayProcessForRTMPInputRecode(
            long id,
            string inputUrl,
            string rtmpOutputUrl)
        {
            DirectoryHelper.ThrowIfNoFFmpeg();

            var argsBuilder = new FFmpegArgsBuilder();
            var args = argsBuilder
                .SetInputUrl(inputUrl)
                .SetVerbosity("verbose")
                .SetResolution(426, 240)
                .SetVideoCodec("libx264")
                .SetVideoBitrate(300)
                .SetMaxVideoBitrate(300)
                .SetBufferSize(600)
                .SetPresentForEncoding("veryfast")
                .SetTuneForEncoding("zerolatency")
                .SetAudioCodec("aac")
                .SetAudioBitrate(64)
                .SetOutputFormat("flv")
                .SetOutputUrl(rtmpOutputUrl)
                .Build();

            return CreateBatProcess(id, args);
        }

    }
}
