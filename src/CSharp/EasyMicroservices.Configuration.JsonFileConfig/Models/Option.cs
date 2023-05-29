using EasyMicroservices.FileManager.Interfaces;
using System;

namespace EasyMicroservices.Configuration.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Option
    {
        /// <summary>
        /// set completed option
        /// </summary>
        /// <param name="pathProvider"></param>
        /// <param name="defaultConfigFileName"> default file name that choose for your microservises config</param>
        /// <param name="loadedFilePath">path where json file is located</param>
        /// <param name="throwExceptionWhenHasError">throw Exception when validate config file</param>
        public Option(IPathProvider pathProvider, string defaultConfigFileName, string loadedFilePath, bool throwExceptionWhenHasError)
        {
            SetOption(pathProvider, defaultConfigFileName, loadedFilePath, throwExceptionWhenHasError);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathProvider"></param>
        /// <param name="defaultConfigFileName"></param>
        /// <param name="loadedFilePath"></param>
        public Option(IPathProvider pathProvider, string defaultConfigFileName, string loadedFilePath)
        {
            SetOption(pathProvider, defaultConfigFileName, loadedFilePath, true);
        }
        /// <summary>
        /// change only config file name
        /// </summary>
        /// <param name="pathProvider"></param>
        /// <param name="defaultConfigFileName"></param>
        public Option(IPathProvider pathProvider, string defaultConfigFileName)
        {
            SetOption(pathProvider, defaultConfigFileName, pathProvider.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), true);
        }
        /// <summary>
        /// set default value for option
        /// </summary>
        public Option(IPathProvider pathProvider)
        {
            string defaultConfigFileName = "Config.json";
            SetOption(pathProvider, defaultConfigFileName, pathProvider.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathProvider"></param>
        /// <param name="defaultConfigFileName"></param>
        /// <param name="throwExceptionWhenHasError"></param>
        public Option(IPathProvider pathProvider, string defaultConfigFileName, bool throwExceptionWhenHasError)
        {
            SetOption(pathProvider, defaultConfigFileName, pathProvider.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), throwExceptionWhenHasError);
        }
        /// <summary>
        /// default file name that choose for your microservises config
        ///default value is : Config.json
        ///use this property to find config file in product mode
        /// </summary>
        public string DefaultConfigFileName { get; private set; }
        /// <summary>
        /// path where json file is located
        /// </summary>
        public string LoadedFilePath { get; private set; }
        /// <summary>
        /// throw Exception when validate config file
        /// </summary>
        public bool ThrowExceptionWhenHasError { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IPathProvider PathProvider { get; private set; }
        /// <summary>
        /// set option value
        /// </summary>
        /// <param name="pathProvider"></param>
        /// <param name="defaultConfigFileName"></param>
        /// <param name="loadedFilePath"></param>
        /// <param name="throwExceptionWhenHasError"></param>
        void SetOption(IPathProvider pathProvider, string defaultConfigFileName, string loadedFilePath, bool throwExceptionWhenHasError)
        {
            if (string.IsNullOrEmpty(defaultConfigFileName))
                throw new Exception("DefaultConfigFileName could not be null.");
            if (string.IsNullOrEmpty(loadedFilePath))
                throw new Exception("LoadedFilePath could not be null.");
            DefaultConfigFileName = defaultConfigFileName;
            LoadedFilePath = loadedFilePath;
            ThrowExceptionWhenHasError = throwExceptionWhenHasError;
            PathProvider = pathProvider;
        }
    }
}
