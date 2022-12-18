using System.Diagnostics;
using System.Text;
using EasyMicroservice.Configuration.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyMicroservice.Configuration
{

    /// <summary>
    /// manage your config ,
    /// add your config type  :T,
    /// load your config from json file,
    /// validate your json file for your microservices
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Config<T>
        where T : class
    {
        public Option Option { get; private set; }
        /// <summary>
        /// check configuration is load or not
        /// </summary>       
        public bool IsLoaded
        {
            get
            {
                return _Current != default;
            }
        }
        /// <summary>
        /// current instance of configuration field
        /// </summary>
        T _Current;
        /// <summary>
        /// current instance of configuration property
        /// </summary>
        public T Current
        {
            get
            {
                return _Current;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPath"> set path for your config, if not set, it searches app domain for config file</param>
        /// <param name="defaultConfigFileName"> </param>
        /// <param name="throwExceptionWhenHasError"></param>
        /// <exception cref="Exception"></exception>
        public async Task Initialize(Option option)
        {
            Option = option ?? new Option();

            await Load();
        }
        /// <summary>
        /// initialize with default option
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            await Initialize(new Option());
        }
        /// <summary>
        /// method for load config json
        /// this method check current directory for find config file first
        /// </summary>
        /// <exception cref="Exception"></exception>
        async Task Load()
        {
            if (!File.Exists(Option.LoadedFilePath))
                throw new Exception($"{Option.DefaultConfigFileName} file not found in {Option.LoadedFilePath} of type {typeof(T).FullName} {Environment.StackTrace}");
            Console.WriteLine($"Load Config From {Option.LoadedFilePath}");
            Debug.WriteLine($"Load Config From {Option.LoadedFilePath}");
            string json = await File.ReadAllTextAsync(Option.LoadedFilePath);
            if (Option.ThrowExceptionWhenHasError)
                ValidateConfigFile(json);
            _Current = JsonConvert.DeserializeObject<T>(json);
            Console.WriteLine($"Load Config success!");
        }

        /// <summary>
        /// validate your config to clean depricate properties and add new properties
        /// </summary>
        void ValidateConfigFile(string json)
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
                stringBuilder.AppendLine($"{Option.DefaultConfigFileName} found properties: {string.Join(",", exceptionProperties)} in your config json file but its not found in your type {typeof(T).Name} please clean and remove this property from your json file!");
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
