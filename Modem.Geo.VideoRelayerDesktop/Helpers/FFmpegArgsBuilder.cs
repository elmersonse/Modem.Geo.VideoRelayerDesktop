using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Helpers
{
    internal class FFmpegArgsBuilder
    {
        private StringBuilder _argsStringBuilder = new StringBuilder();

        public FFmpegArgsBuilder()
        {
            Reset();
        }

        public FFmpegArgsBuilder SetProtocolForRTSP(string protocol)
        {
            if (string.IsNullOrEmpty(protocol)) throw new ArgumentException("Protocol cannot be null or empty", nameof(protocol));
            if (protocol != "tcp" && protocol != "udp") throw new ArgumentException("Protocol must be tcp or udp!", nameof(protocol));

            _argsStringBuilder.Append($"-rtsp_transport {protocol} ");
            return this;
        }

        public FFmpegArgsBuilder SetTimeout(int timeout)
        {
            if (timeout <= 0) throw new ArgumentException("Timeout must be greater than zero", nameof(timeout));
            _argsStringBuilder.Append($"-timeout {timeout} ");
            return this;
        }

        public FFmpegArgsBuilder SetInputUrl(string inputUrl)
        {
            if (string.IsNullOrEmpty(inputUrl)) throw new ArgumentException("Input URL cannot be null or empty", nameof(inputUrl));
            _argsStringBuilder.Append($"-i {inputUrl} ");
            return this;
        }

        public FFmpegArgsBuilder SetVerbosity(string verbosity)
        {
            if (string.IsNullOrEmpty(verbosity)) throw new ArgumentException("Verbosity cannot be null or empty", nameof(verbosity));
            _argsStringBuilder.Append($"-v {verbosity} ");
            return this;
        }

        public FFmpegArgsBuilder SetResolution(int width, int height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("Width and height must be greater than zero");
            _argsStringBuilder.Append($"-vf scale={width}:{height} ");
            return this;
        }

        public FFmpegArgsBuilder SetVideoCodec(string codec)
        {
            if (string.IsNullOrEmpty(codec)) throw new ArgumentException("Video codec cannot be null or empty", nameof(codec));
            _argsStringBuilder.Append($"-c:v {codec} ");
            return this;
        }

        public FFmpegArgsBuilder SetVideoBitrate(int bitrate)
        {
            if (bitrate <= 0) throw new ArgumentException("Bitrate time must be greater than zero", nameof(bitrate));
            _argsStringBuilder.Append($"-b:v {bitrate}k ");
            return this;
        }

        public FFmpegArgsBuilder SetMaxVideoBitrate(int maxBitrate)
        {
            if (maxBitrate <= 0) throw new ArgumentException("MaxBitrate time must be greater than zero", nameof(maxBitrate));
            _argsStringBuilder.Append($"-maxrate {maxBitrate}k ");
            return this;
        }

        public FFmpegArgsBuilder SetBufferSize(int bufferSize)
        {
            if (bufferSize <= 0) throw new ArgumentException("BufferSize time must be greater than zero", nameof(bufferSize));
            _argsStringBuilder.Append($"-bufsize {bufferSize}k ");
            return this;
        }

        public FFmpegArgsBuilder SetPresentForEncoding(string preset)
        {
            if (string.IsNullOrEmpty(preset)) throw new ArgumentException("Audio preset cannot be null or empty", nameof(preset));
            _argsStringBuilder.Append($"-preset {preset} ");
            return this;
        }

        public FFmpegArgsBuilder SetTuneForEncoding(string tune)
        {
            if (string.IsNullOrEmpty(tune)) throw new ArgumentException("Tune preset cannot be null or empty", nameof(tune));
            _argsStringBuilder.Append($"-tune {tune} ");
            return this;
        }

        public FFmpegArgsBuilder SetAudioCodec(string codec)
        {
            if (string.IsNullOrEmpty(codec)) throw new ArgumentException("Audio codec cannot be null or empty", nameof(codec));
            _argsStringBuilder.Append($"-c:a {codec} ");
            return this;
        }

        public FFmpegArgsBuilder SetAudioBitrate(int bitrate)
        {
            if (bitrate <= 0) throw new ArgumentException("Segment time must be greater than zero", nameof(bitrate));
            _argsStringBuilder.Append($"-b:a {bitrate}k ");
            return this;
        }

        public FFmpegArgsBuilder SetOutputFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) throw new ArgumentException("Output format cannot be null or empty", nameof(format));
            _argsStringBuilder.Append($"-f {format} ");
            return this;
        }

        public FFmpegArgsBuilder SetOutputUrl(string outputUrl)
        {
            if (string.IsNullOrEmpty(outputUrl)) throw new ArgumentException("Output URL cannot be null or empty", nameof(outputUrl));
            _argsStringBuilder.Append(outputUrl);
            return this;
        }

        public FFmpegArgsBuilder SetSegmentTime(int segmentTime)
        {
            if (segmentTime <= 0) throw new ArgumentException("Segment time must be greater than zero", nameof(segmentTime));
            _argsStringBuilder.Append($"-segment_time {segmentTime} ");
            return this;
        }

        public FFmpegArgsBuilder ResetTimestamps(int reset)
        {
            if (reset < 0) throw new ArgumentException("Reset value must be zero or positive", nameof(reset));
            _argsStringBuilder.Append($"-reset_timestamps {reset} ");
            return this;
        }

        public FFmpegArgsBuilder CopyTimestamps()
        {
            _argsStringBuilder.Append("-copyts ");
            return this;
        }

        public FFmpegArgsBuilder SetGlobalHeaderFlag()
        {
            _argsStringBuilder.Append("-flags global_header ");
            return this;
        }

        public FFmpegArgsBuilder UseStrftime()
        {
            _argsStringBuilder.Append("-strftime 1 ");
            return this;
        }

        public FFmpegArgsBuilder SetSegmentFormatOptions(string options)
        {
            if (string.IsNullOrEmpty(options)) throw new ArgumentException("Segment format options cannot be null or empty", nameof(options));
            _argsStringBuilder.Append($"-segment_format_options {options} ");
            return this;
        }

        public FFmpegArgsBuilder SetStandardOutput(string options)
        {
            if (string.IsNullOrEmpty(options)) throw new ArgumentException("Segment format options cannot be null or empty", nameof(options));
            _argsStringBuilder.Append($"1>{options} ");
            return this;
        }

        public FFmpegArgsBuilder SetErrorOutput(string options)
        {
            if (string.IsNullOrEmpty(options)) throw new ArgumentException("Segment format options cannot be null or empty", nameof(options));
            _argsStringBuilder.Append($"2>{options} ");
            return this;
        }

        public FFmpegArgsBuilder SetOutputToSingleFile(string options)
        {
            if (string.IsNullOrEmpty(options)) throw new ArgumentException("Segment format options cannot be null or empty", nameof(options));
            _argsStringBuilder.Append($"1>{options} 2>&1");
            return this;
        }

        public string Build()
        {
            return _argsStringBuilder.ToString();
        }

        public void Reset()
        {
            _argsStringBuilder = new StringBuilder();
        }

    }
}
