using EasyMicroservices.Configuration.Interfaces;
using EasyMicroservices.Configuration.Tests.Models;
using EasyMicroservices.FileManager.Interfaces;
using EasyMicroservices.FileManager.Providers.DirectoryProviders;
using EasyMicroservices.FileManager.Providers.FileProviders;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyMicroservices.Configuration.Tests
{
    public abstract class BaseConfigurationProviderTest
    {
        IConfigProvider _configProvider;
        public BaseConfigurationProviderTest(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
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
        /// <summary>
        /// content config json file
        /// </summary>
        public string AppSettingConfigFile = "appsettings.json";

        public IFileManagerProvider GetFileProvider()
        {
            return new DiskFileProvider(new DiskDirectoryProvider(AppDomain.CurrentDomain.BaseDirectory));
        }

        async Task<(string Json, string FileAddress)> GetJsonFile(IFileManagerProvider fileProvider, string fileName)
        {
            var assembly = typeof(BaseConfigurationProviderTest).Assembly;
            var resourceNames = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(fileName));
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceNames));
            string json = await reader.ReadToEndAsync();
            string fileAddress = fileProvider.PathProvider.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            return (json, fileAddress);
        }

        protected async Task<T> GenerateConfigFile<T>(string fileName)
        {
            var fileProvider = GetFileProvider();
            (string Json, string FileAddress) = await GetJsonFile(fileProvider, fileName);
            await fileProvider.WriteAllTextAsync(FileAddress, Json);
            return JsonConvert.DeserializeObject<T>(Json);
        }

        protected async Task GenerateInvalidConfigFile(string fileName)
        {
            var fileProvider = GetFileProvider();
            (string Json, string FileAddress) = await GetJsonFile(fileProvider, fileName);
            await fileProvider.WriteAllTextAsync(FileAddress, Json);
        }

        protected async Task RemoveFile(string filePath)
        {
            var fileProvider = GetFileProvider();
            if (await fileProvider.IsExistFileAsync(filePath))
                await fileProvider.DeleteFileAsync(filePath);
        }

        [Theory]
        [InlineData("Port", "8080")]
        [InlineData("ConnectionString", "Server=127.0.0.1;Database=DbName;User ID=test;Password=test")]
        [InlineData("LogType", "4")]
        public async Task RootPropertyTest(string key, string value)
        {
            var result = await _configProvider.GetValue(key);
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task InlinePropertyTest()
        {
            var result = await _configProvider.GetValue<ConfigBase>();
            Assert.NotNull(result);
            Assert.True(result.Persons.Count > 0);
            Assert.Equal("mahdi", result.Persons[0].Name);
            Assert.Equal("Ali", result.Persons[1].Name);
            Assert.Equal("Iran-mashhad-beheshti st", result.Persons[0].Address);
            Assert.Equal("Canada-Vancouver", result.Persons[1].Address);
        }
    }
}
