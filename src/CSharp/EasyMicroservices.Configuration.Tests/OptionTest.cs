using EasyMicroservices.Configuration.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace EasyMicroservices.Configuration.Tests
{
    public class OptionTest : TestBase
    {
        [InlineData("my.json", @"C:\Users\admin", false)]
        [InlineData("ConfigurationFile.json", @"C:\Users\admin2", true)]
        [Theory]
        public async Task GenerateOption_WithCompletedAccess(string defaultConfigFileName, string loadedFilePath, bool throwExceptionWhenHasError)
        {
            var option = new Option(defaultConfigFileName, loadedFilePath, throwExceptionWhenHasError);
            Assert.Equal(defaultConfigFileName, option.DefaultConfigFileName);
            Assert.Equal(loadedFilePath, option.LoadedFilePath);
            Assert.Equal(throwExceptionWhenHasError, option.ThrowExceptionWhenHasError);
            Assert.NotNull(option);
        }
        [InlineData("my.json", false)]
        [InlineData("Config.json", true)]
        [Theory]
        public async Task GenerateOption_With_default_LoadedFilePath(string defaultConfigFileName, bool throwExceptionWhenHasError)
        {
            var option = new Option(defaultConfigFileName, throwExceptionWhenHasError);
            Assert.Equal(defaultConfigFileName, option.DefaultConfigFileName);
            Assert.Equal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), option.LoadedFilePath);
            Assert.Equal(throwExceptionWhenHasError, option.ThrowExceptionWhenHasError);
            Assert.NotNull(option);
        }
        [InlineData("my.json", @"C:\Users\admin")]
        [Theory]
        public async Task GenerateOption_With_default_ThrowExceptionWhenHasError_True(string defaultConfigFileName, string loadedFilePath)
        {
            var option = new Option(defaultConfigFileName, loadedFilePath);
            Assert.Equal(defaultConfigFileName, option.DefaultConfigFileName);
            Assert.Equal(loadedFilePath, option.LoadedFilePath);
            Assert.True(option.ThrowExceptionWhenHasError);
            Assert.NotNull(option);
        }
        [InlineData("configuration.json")]
        [Theory]
        public async Task GenerateOption_With_Only_DefaultConfigFileName(string defaultConfigFileName)
        {
            var option = new Option(defaultConfigFileName);
            Assert.Equal(defaultConfigFileName, option.DefaultConfigFileName);
            Assert.Equal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), option.LoadedFilePath);
            Assert.True(option.ThrowExceptionWhenHasError);
            Assert.NotNull(option);
        }

        [Fact]
        public async Task GenerateOption_Default_Value()
        {
            var option = new Option();
            var defaultConfigFileName = "Config.json";
            Assert.Equal(defaultConfigFileName, option.DefaultConfigFileName);
            Assert.Equal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultConfigFileName), option.LoadedFilePath);
            Assert.True(option.ThrowExceptionWhenHasError);
            Assert.NotNull(option);
        }
        [InlineData(null, null)]
        [InlineData("config.json", null)]
        [InlineData(null, @"C:\Users\admin")]
        [Theory]
        public void GenerateOption_InvaliDdata_MustCatchException(string defaultConfigFileName, string loadedFilePath)
        {
            Assert.ThrowsAny<Exception>(() => new Option(defaultConfigFileName, loadedFilePath));
        }
    }
}
