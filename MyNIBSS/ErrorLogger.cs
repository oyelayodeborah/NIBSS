using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MyNIBSS
{
    public class ErrorLogger
    {
        public static void Log(string msg)
        {
            string message = "Error: " + msg + " occured at " + DateTime.Now + "\n";
            Trace.TraceInformation(message);
            Trace.Flush();
        }
    }
}