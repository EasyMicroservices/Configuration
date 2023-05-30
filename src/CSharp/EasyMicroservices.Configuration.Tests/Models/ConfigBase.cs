using System;
using System.Collections.Generic;
using System.Text;

namespace EasyMicroservices.Configuration.Tests.Models
{
    /// <summary>
    /// base class for config in C#
    /// </summary>
    public class ConfigBase
    {
        /// <summary>
        ///int  port 
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// list int prop
        /// </summary>
        public int[] Ports { get; set; }
        /// <summary>
        /// string prop
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// list object prop
        /// </summary>
        public List<Person> Persons { get; set; }
        /// <summary>
        /// enum prop
        /// </summary>
        public LogType LogType { get; set; }
    }
}
