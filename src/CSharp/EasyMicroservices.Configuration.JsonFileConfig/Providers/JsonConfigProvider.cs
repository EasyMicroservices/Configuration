using EasyMicroservices.Configuration.Models;
using EasyMicroservices.Configuration.Providers;
using EasyMicroservices.FileManager.Interfaces;
using EasyMicroservices.FileManager.Providers.PathProviders;
using EasyMicroservices.Serialization.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.Configuration.JsonConfig.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonConfigProvider : BaseConfigProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="fileManagerProvider"></param>
        /// <param name="textSerialization"></param>
        /// <returns></returns>
        public JsonConfigProvider(Option option, IFileManagerProvider fileManagerProvider, ITextSerializationProvider textSerialization)
        {
            Option = option ?? new Option(new SystemPathProvider());
            _fileManagerProvider = fileManagerProvider;
            _textSerialization = textSerialization;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileManagerProvider"></param>
        /// <param name="textSerialization"></param>
        public JsonConfigProvider(IFileManagerProvider fileManagerProvider, ITextSerializationProvider textSerialization)
            : this(null, fileManagerProvider, textSerialization)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public Option Option { get; private set; }
        IFileManagerProvider _fileManagerProvider;
        ITextSerializationProvider _textSerialization;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        async Task<T> Load<T>()
        {
            if (!await _fileManagerProvider.IsExistFileAsync(Option.LoadedFilePath))
                throw new Exception($"{Option.DefaultConfigFileName} file not found in {Option.LoadedFilePath} of type {typeof(T).FullName} {Environment.StackTrace}");
            string json = await _fileManagerProvider.ReadAllTextAsync(Option.LoadedFilePath);
            if (Option.ThrowExceptionWhenHasError)
                ValidateConfigFile<T>(json);
            return _textSerialization.Deserialize<T>(json);
        }

        async Task<JObject> Load()
        {
            if (!await _fileManagerProvider.IsExistFileAsync(Option.LoadedFilePath))
                throw new Exception($"{Option.DefaultConfigFileName} file not found in {Option.LoadedFilePath} {Environment.StackTrace}");
            string json = await _fileManagerProvider.ReadAllTextAsync(Option.LoadedFilePath);
            return (JObject)JsonConvert.DeserializeObject(json);
        }

        async Task<string> GetProperty(string name)
        {
            return (await Load()).Property(name).Value.ToString();
        }
        /// <summary>
        /// validate your config to clean depricate properties and add new properties
        /// </summary>
        void ValidateConfigFile<T>(string json)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Task<string> GetValue(string key)
        {
            return GetProperty(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task SetValue(string key, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<T> GetValue<T>(string key)
        {
            return (T)GetPropertyValue(await GetValue<T>(), key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override Task<T> GetValue<T>()
        {
            return Load<T>();
        }
    }
}