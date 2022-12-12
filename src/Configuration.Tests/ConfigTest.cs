using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Configuration.Tests
{
    public class ConfigTest : TestBase
    {
        [Fact]
        public async Task LoadConfigFile_WithDefaultPath_MustLoadCompletely()
        {

            var config = await GenerateConfigFile<ConfigBase>(ExactConfigFile);
            var loadedConfiguration = new Config<ConfigBase>();
            await loadedConfiguration.Initialize(new Option(ExactConfigFile));
            Assert.True(loadedConfiguration.Current != null);
            Assert.Equal(config.ConnectionString, loadedConfiguration.Current.ConnectionString);
            Assert.Equal(config.Port, loadedConfiguration.Current.Port);
            Assert.True(config.Ports.SequenceEqual(loadedConfiguration.Current.Ports));
            Assert.Equal(config.Persons.Count, loadedConfiguration.Current.Persons.Count);
            Assert.Equal(config.LogType, loadedConfiguration.Current.LogType);
            Assert.True(loadedConfiguration.IsLoaded);
        }
        [Fact]
        public async Task LoadConfigFile_WithDefaultinItilize_MustLoadCompletely()
        {

            var config = await GenerateConfigFile<ConfigBase>("Config.json");
            var loadedConfiguration = new Config<ConfigBase>();
            await loadedConfiguration.Initialize();
            Assert.True(loadedConfiguration.Current != null);
            Assert.Equal(config.ConnectionString, loadedConfiguration.Current.ConnectionString);
            Assert.Equal(config.Port, loadedConfiguration.Current.Port);
            Assert.True(config.Ports.SequenceEqual(loadedConfiguration.Current.Ports));
            Assert.Equal(config.Persons.Count, loadedConfiguration.Current.Persons.Count);
            Assert.Equal(config.LogType, loadedConfiguration.Current.LogType);
            Assert.True(loadedConfiguration.IsLoaded);
        }
        [Fact]
        public async Task LoadConfigFile_WithNullOption_MustLoadCompletely()
        {

            var config = await GenerateConfigFile<ConfigBase>("Config.json");
            var loadedConfiguration = new Config<ConfigBase>();
            await loadedConfiguration.Initialize(null);
            Assert.True(loadedConfiguration.Current != null);
            Assert.Equal(config.ConnectionString, loadedConfiguration.Current.ConnectionString);
            Assert.Equal(config.Port, loadedConfiguration.Current.Port);
            Assert.True(config.Ports.SequenceEqual(loadedConfiguration.Current.Ports));
            Assert.Equal(config.Persons.Count, loadedConfiguration.Current.Persons.Count);
            Assert.Equal(config.LogType, loadedConfiguration.Current.LogType);
            Assert.True(loadedConfiguration.IsLoaded);
        }
        [Fact]
        public async Task LoadConfigFile_WithMoreProperty_MustCatchException()
        {
            var config = await GenerateConfigFile<ConfigBase>(MoreConfigFile);
            var loadedConfiguration = new Config<ConfigBase>();
            var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await loadedConfiguration.Initialize(new Option(MoreConfigFile)));
        }
        [Fact]
        public async Task LoadConfigFile_WithLessProperty_MustCatchException()
        {
            var config = await GenerateConfigFile<ConfigBase>(LessConfigFile);
            var loadedConfiguration = new Config<ConfigBase>();
            var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await loadedConfiguration.Initialize(new Option(LessConfigFile)));
        }
        [Fact]
        public async Task LoadConfigFile_InvalidConfigFile_MustCatchException()
        {
            await GenerateInvalidConfigFile(InvalidConfigFile);
            var loadedConfiguration = new Config<ConfigBase>();
            var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await loadedConfiguration.Initialize(new Option(InvalidConfigFile)));
        }
        [Fact]
        public async Task LoadConfigFile_EmptyConfigFile_MustCatchException()
        {
            await GenerateInvalidConfigFile(EmptyConfigFile);
            var loadedConfiguration = new Config<ConfigBase>();
            var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await loadedConfiguration.Initialize(new Option(EmptyConfigFile)));
        }
        [Fact]
        public async Task LoadConfigFile_WithNotCurrectOption_MustCatchException()
        {
            await GenerateInvalidConfigFile(ExactConfigFile);
            var loadedConfiguration = new Config<ConfigBase>();
            var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await loadedConfiguration.Initialize(new Option(ExactConfigFile,null)));
        }
    }
}