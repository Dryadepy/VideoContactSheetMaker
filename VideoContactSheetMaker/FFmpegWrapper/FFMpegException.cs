using System;

namespace VideoContactSheetMaker.FFmpegWrapper
{
    sealed class FFMpegException : Exception
    {
        public int ErrorCode { get; }

        public FFMpegException(int errCode, string message) : base(string.Format(Properties.Resources.FFProbeProcessExceededExecutionTimeout,message, errCode)) => ErrorCode = errCode;
    }
}
