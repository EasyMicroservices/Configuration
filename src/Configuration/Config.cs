using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configuration
{

    /// <summary>
    /// manage your config ,
    /// add your config type  :T,
    /// load your config from json file,
    /// validate your json file for your microservices
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Config<T>
        where T : class
    {
        /// <summary>
        /// default file name that choose for your microservises config
        ///default value is : Config.json
        ///use this property to find config file in product mode
        /// </summary>
        public static string DefaultConfigFileName { get; private set; }= "Config.json";
        /// <summary>
        /// path where json file is located
        /// </summary>
        public static string LoadedFilePath { get; private set; }
        /// <summary>
        /// check configuration is load or not
        /// </summary>       
        public static bool IsLoaded
        {
            get
            {
                return _Current != default;
            }
        }
        /// <summary>
        /// current instance of configuration field
        /// </summary>
        static T _Current;
        /// <summary>
        /// current instance of configuration property
        /// </summary>
        public static T Current
        {
            get
            {
                return _Current;
            }
            set
            {
                _Current = value;
            }
        }


        /// <summary>
        /// initialize and load current config json
        /// </summary>
        /// <param name="defaultPath">default path for test and migrations</param>
        /// <param name="throwExceptionWhenHasError"> when your csharp class and json file are differrent throw exception</param>
        /// <exception cref="Exception"></exception>

        public static void Initialize(string defaultPath, bool throwExceptionWhenHasError = true)
        {
            //find config file in current directory
            string fileAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigFileName);
            if (!File.Exists(fileAddress))
            {
                if (!File.Exists(defaultPath))
                    throw new Exception($"{DefaultConfigFileName} file not found in {defaultPath}");
                Console.WriteLine($"Loaded Config From {defaultPath}");
                Debug.WriteLine($"Loaded Config From {defaultPath}");
                LoadedFilePath = defaultPath;
                var json = File.ReadAllText(defaultPath);
                if (throwExceptionWhenHasError)
                    ValidateConfigFile(json);
                Current = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
        }
        /// <summary>
        /// method for load config json
        /// this method check current directory for find config file first
        /// </summary>
        /// <exception cref="Exception"></exception>
        static async Task Load()
        {
            string fileAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigFileName);
            if (!File.Exists(fileAddress))
                throw new Exception($"{DefaultConfigFileName} file not found in {fileAddress} of type {typeof(T).FullName} {Environment.StackTrace}");
            Console.WriteLine($"Loaded Config From {fileAddress}");
            Debug.WriteLine($"Loaded Config From {fileAddress}");
            LoadedFilePath = fileAddress;
            string json = await File.ReadAllTextAsync(fileAddress);
            ValidateConfigFile(json);
            Current = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// reload config file
        /// </summary>
        static async Task Reload()
        {
            Console.WriteLine($"Reload Config From {LoadedFilePath}");
            var json = await File.ReadAllTextAsync(LoadedFilePath);
            ValidateConfigFile(json);
            Current = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            Console.WriteLine($"Reload Config success!");
        }

        /// <summary>
        /// validate your config to clean depricate properties and add new properties
        /// </summary>
        public static void ValidateConfigFile(string json)
        {
            var typeProperties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var jsonObject = (JObject)JsonConvert.DeserializeObject(json);
            if (jsonObject == null)
                throw new Exception("Your Config json file is not a valid json, please fix it or add a valid json file.");
            List<string> exceptionProperties = new List<string>();
            foreach (var jProperty in jsonObject.Properties())
            {
                if (!typeProperties.Any(x => x.Name.Equals(jProperty.Name, StringComparison.OrdinalIgnoreCase)))
                    exceptionProperties.Add(jProperty.Name);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            if (exceptionProperties.Count > 0)
                stringBuilder.AppendLine($"{DefaultConfigFileName} found properties: {string.Join(",", exceptionProperties)} in your config json file but its not found in your type {typeof(T).Name} please clean and remove this property from your json file!");
            exceptionProperties.Clear();
            foreach (var property in typeProperties)
            {
                if (!jsonObject.Properties().Any(x => x.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                    exceptionProperties.Add(property.Name);
            }
            if (exceptionProperties.Count > 0)
                stringBuilder.AppendLine($"type {typeof(T).Name} found properties: {string.Join(",", exceptionProperties)} but its not found in your Config json file, please add this properties in your json file!");
            var exception = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(exception.Trim()))
                throw new Exception(exception);
        }
    }
}
