using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasyMicroservice.Configuration.Tests
{
    public class TestBase
    {
        static TestBase()
        {
        }
        public async Task<T> GenerateConfigFile<T>(string fileName)
        {
            var assembly = typeof(TestBase).Assembly;
            var resourceNames = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(fileName));
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceNames));
            string json = await reader.ReadToEndAsync();
            string fileAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            await File.WriteAllTextAsync(fileAddress, json);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public async Task GenerateInvalidConfigFile(string fileName)
        {
            var assembly = typeof(TestBase).Assembly;
            var resourceNames = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(fileName));
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceNames));
            string json = await reader.ReadToEndAsync();
            string fileAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            await File.WriteAllTextAsync(fileAddress, json);
        }
        public void RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// json config file which match the config C# class
        /// </summary>
        public string ExactConfigFile = "ExactConfigFile.json";
        /// <summary>
        /// json config file which have less property in compare the config C# class
        /// </summary>
        public string LessConfigFile = "LessConfigFile.json";
        /// <summary>
        /// json config file which have more property in compare the config C# class
        /// </summary>
        public string MoreConfigFile = "MoreConfigFile.json";
        /// <summary>
        /// Invalid json config file
        /// </summary>
        public string InvalidConfigFile = "InvalidConfigFile.json";
        /// <summary>
        /// empty json config file
        /// </summary>
        public string EmptyConfigFile = "EmptyConfigFile.json";
    }

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
    /// <summary>
    /// person class
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
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
