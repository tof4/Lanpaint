using System.Diagnostics;
using Lanpaint.Elements;

namespace Lanpaint
{
    public class TraceListener : TextWriterTraceListener
    {
        private readonly DebugLog _debugLog;

        public TraceListener(DebugLog debugLog)
        {
            _debugLog = debugLog;
        }
        
        public override void WriteLine(string message)
        {
            _debugLog.AddToLog(message);
        }
    }
}