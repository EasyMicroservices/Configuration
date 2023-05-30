using System;
using System.Collections.Generic;
using System.Text;

namespace EasyMicroservices.Configuration.Tests.Models
{
    /// <summary>
    /// Type Of log
    /// Like Error, Warning, Info, Debug Or None
    /// </summary>
    public enum LogType : byte
    {
        /// <summary>
        ///  none of this types
        /// </summary>
        None = 0,
        /// <summary>
        /// debug log type
        /// </summary>
        Debug = 1,
        /// <summary>
        /// info log type
        /// </summary>
        Info = 2,
        /// <summary>
        /// warning log type
        /// </summary>
        Warning = 3,
        /// <summary>
        /// error log type
        /// </summary>
        Error = 4
    }
}
