using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Config<T>
        where T : class
    {
       
        /// <summary>
        /// path where json file is located
        /// </summary>
        public static string LoadedPath { get; private set; }
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
                if (_Current == null)
                    Load();
                return _Current;
            }
            set
            {
                if (_Current != null)              
                _Current = value;             
                OnLoaded?.Invoke(value);
            }
        }
        /// <summary>
        /// action where can define onload file
        /// </summary>
        public static Action<T> OnLoaded { get; set; }
        /// <summary>
        /// initialize and load current config json
        /// </summary>
        /// <param name="defaultPath">default path for test and migrations</param>
        /// <param name="throwExceptionWhenHasError"> when your csharp class and json file are differrent throw exception</param>
        /// <exception cref="Exception"></exception>

        public static void Initialize(string defaultPath, bool throwExceptionWhenHasError = true)
        {
            string fileAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConfigGo.json");
            if (!File.Exists(fileAddress))
            {
                if (!File.Exists(defaultPath))
                    throw new Exception($"ConfigGo.json file not found in {defaultPath}");
                Console.WriteLine($"Loaded Config From {defaultPath}");
                Debug.WriteLine($"Loaded Config From {defaultPath}");
                LoadedPath = defaultPath;
                var json = File.ReadAllText(defaultPath);
                if (throwExceptionWhenHasError)
                    ValidateConfigGo(json);
                Current = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
        }
        /// <summary>
        /// method for load config json
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void Load()
        {
            string fileAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConfigGo.json");
            if (!File.Exists(fileAddress))
                throw new Exception($"ConfigGo.json file not found in {fileAddress} of type {typeof(T).FullName} {Environment.StackTrace}");
            Console.WriteLine($"Loaded Config From {fileAddress}");
            Debug.WriteLine($"Loaded Config From {fileAddress}");
            LoadedPath = fileAddress;
            var json = File.ReadAllText(fileAddress);
            ValidateConfigGo(json);
            Current = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// reload config file
        /// </summary>
        static void Reload()
        {
            Console.WriteLine($"Reload Config From {LoadedPath}");
            var json = File.ReadAllText(LoadedPath);
            ValidateConfigGo(json);
            Current = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            Console.WriteLine($"Reload success!");
        }

        /// <summary>
        /// validate your config to clean depricate properties and add new properties
        /// </summary>
        public static void ValidateConfigGo(string json)
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
                stringBuilder.AppendLine($"ConfigGo found properties: {string.Join(",", exceptionProperties)} in your configGo json file but its not found in your type {typeof(T).Name} please clean and remove this property from your json file!");
            exceptionProperties.Clear();
            foreach (var property in typeProperties)
            {
                if (!jsonObject.Properties().Any(x => x.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                    exceptionProperties.Add(property.Name);
            }
            if (exceptionProperties.Count > 0)
                stringBuilder.AppendLine($"type {typeof(T).Name} found properties: {string.Join(",", exceptionProperties)} but its not found in your ConfigGo json file, please add this properties in your json file!");
            var exception = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(exception.Trim()))
                throw new Exception(exception);
        }
    }
}
