using System;

namespace VideoContactSheetMaker.FFProbeWrapper
{
    sealed class FFProbeException : Exception
    {
        public int ErrorCode { get; }

        public FFProbeException(int errCode, string message) : base(string.Format(Properties.Resources.FFProbeProcessExceededExecutionTimeout, message, errCode))
        {
            ErrorCode = errCode;
        }
   }

}
