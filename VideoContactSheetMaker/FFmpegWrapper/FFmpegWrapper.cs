using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace VideoContactSheetMaker.FFmpegWrapper
{
    sealed class FFmpegWrapper
    {
        private Process FFMpegProcess;
        private readonly TimeSpan ExecutionTimeout = new TimeSpan(0, 0, 15);
        private string InputFile;
        public byte[] GetVideoThumbnail(string inputFile, float frameTime, int width = -1, int height = -1)
        {
            InputFile = inputFile;
            var settings = new FFmpegSettings
            {
                Seek = frameTime,
                OutputFormat = "mjpeg",
                VideoFrameSize = $"-vf scale={width}:{height}",
            };

            return RunFFmpeg(inputFile, settings);
        }

        void WaitFFMpegProcessForExit()
        {
            if (FFMpegProcess.HasExited)
            {
                return;
            }
            var milliseconds = (int)ExecutionTimeout.TotalMilliseconds;
            if (FFMpegProcess.WaitForExit(milliseconds)) return;
            EnsureFFMpegProcessStopped();
            throw new FFMpegException(-2,
                string.Format(Properties.Resources.FFMpegTimeoutFile, InputFile));
        }
        void EnsureFFMpegProcessStopped()
        {
            if (FFMpegProcess == null || FFMpegProcess.HasExited) return;
            try
            {
                FFMpegProcess.Kill();
                FFMpegProcess = null;
            }
            catch (Exception)
            {
            }
        }

        internal byte[] RunFFmpeg(string input, FFmpegSettings settings)
        {
            byte[] data;
            try
            {
                var arguments = $" -hide_banner -loglevel panic -y -ss {settings.Seek.ToString(CultureInfo.InvariantCulture)} -i \"{input}\" -t 1 -f {settings.OutputFormat} -vframes 1 {settings.VideoFrameSize} \"-\"";
                var processStartInfo =
                    new ProcessStartInfo(Utils.FfmpegPath, arguments)
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = Path.GetDirectoryName(Utils.FfmpegPath) ?? string.Empty,
                        RedirectStandardInput = true, //required to avoid ffmpeg printing to console
                        RedirectStandardOutput = true,
                    };

                if (FFMpegProcess != null)
                {
                    throw new InvalidOperationException();
                }
                FFMpegProcess = Process.Start(processStartInfo);
                if (FFMpegProcess == null)
                {
                    throw new FFMpegException(-1, "FFMpeg process was aborted");
                }


                var ms = new MemoryStream();
                //start reading here, otherwise the streams fill up and ffmpeg will block forever
                var imgDataTask = FFMpegProcess.StandardOutput.BaseStream.CopyToAsync(ms);

                WaitFFMpegProcessForExit();

                imgDataTask.Wait(1000);
                data = ms.ToArray();

                FFMpegProcess?.Close();
                FFMpegProcess = null;

            }
            catch (Exception)
            {
                EnsureFFMpegProcessStopped();
                throw;
            }
            return data;
        }

    }
}
