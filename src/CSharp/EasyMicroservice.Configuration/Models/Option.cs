namespace EasyMicroservice.Configuration.Models
{
    public class Option
    {
        /// <summary>
        /// set completed option
        /// </summary>
        /// <param name="defaultConfigFileName"> default file name that choose for your microservises config</param>
        /// <param name="loadedFilePath">path where json file is located</param>
        /// <param name="throwExceptionWhenHasError">throw Exception when validate config file</param>
        public Option(string defaultConfigFileName, string loadedFilePath, bool throwExceptionWhenHasError)
        {
            SetOption(defaultConfigFileName, loadedFilePath, throwExceptionWhenHasError);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultConfigFileName"></param>
        /// <param name="loadedFilePath"></param>
        public Option(string defaultConfigFileName, string loadedFilePath)
        {
            SetOption(defaultConfigFileName, loadedFilePath, true);
        }
        /// <summary>
        /// change only config file name
        /// </summary>
        /// <param name="defaultConfigFileName"></param>
        public Option(string defaultConfigFileName)
        {
            SetOption(defaultConfigFileName, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), true);
        }
        /// <summary>
        /// set default value for option
        /// </summary>
        public Option()
        {
            string defaultConfigFileName = "Config.json";
            SetOption(defaultConfigFileName, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), true);
        }
        public Option(string defaultConfigFileName, bool throwExceptionWhenHasError)
        {
            SetOption(defaultConfigFileName, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), throwExceptionWhenHasError);
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
        /// set option value
        /// </summary>
        /// <param name="defaultConfigFileName"></param>
        /// <param name="loadedFilePath"></param>
        /// <param name="throwExceptionWhenHasError"></param>
        void SetOption(string defaultConfigFileName, string loadedFilePath, bool throwExceptionWhenHasError)
        {
            if (string.IsNullOrEmpty(defaultConfigFileName))
                throw new Exception("DefaultConfigFileName could not be null.");
            if (string.IsNullOrEmpty(loadedFilePath))
                throw new Exception("LoadedFilePath could not be null.");
            DefaultConfigFileName = defaultConfigFileName;
            LoadedFilePath = loadedFilePath;
            ThrowExceptionWhenHasError = throwExceptionWhenHasError;
        }
    }
}
