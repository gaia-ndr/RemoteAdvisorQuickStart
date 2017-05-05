using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SfB.PlatformService.SDK.Common;

namespace MTC.UCAP
{
    public class StringLogger:IPlatformServiceLogger
    {
        public string InformationMessages { get; set; }
        public string WarningMessages { get; set; }
        public string ErrorMessages { get; set; }

        public StringLogger()
        {
            InformationMessages = string.Empty;
            WarningMessages = string.Empty;
            ErrorMessages = string.Empty;
        }

        public void Information(string message)
        {
            InformationMessages += message + Environment.NewLine;
        }

        public void Information(string fmt, params object[] vars)
        {
            InformationMessages += string.Format(fmt, vars) + Environment.NewLine;
        }

        public void Information(Exception exception, string fmt, params object[] vars)
        {
            InformationMessages += exception.FullMessage() + string.Format(fmt, vars) + Environment.NewLine;
        }

        public void Warning(string message)
        {
            WarningMessages += message + Environment.NewLine;
        }

        public void Warning(string fmt, params object[] vars)
        {
            WarningMessages += string.Format(fmt, vars) + Environment.NewLine;
        }

        public void Warning(Exception exception, string fmt, params object[] vars)
        {
            WarningMessages += exception.FullMessage() + string.Format(fmt, vars) + Environment.NewLine;
        }

        public void Error(string message)
        {
            ErrorMessages += message + Environment.NewLine;
        }

        public void Error(string fmt, params object[] vars)
        {
            ErrorMessages += string.Format(fmt, vars) + Environment.NewLine;
        }

        public void Error(Exception exception, string fmt, params object[] vars)
        {
            ErrorMessages += exception.FullMessage() + string.Format(fmt, vars) + Environment.NewLine;
        }

        public bool HttpRequestResponseNeedsToBeLogged { get; set; }
    }
}
