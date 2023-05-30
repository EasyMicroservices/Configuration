using EasyMicroservices.Configuration.Providers;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace EasyMicroservices.Configuration.SystemConfiguration.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemConfigurationProvider : BaseConfigProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Task<string> GetValue(string key)
        {
            return Task.FromResult(ConfigurationManager.AppSettings[key]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<T> GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<T> GetValue<T>()
        {
            throw new NotImplementedException();
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
            ConfigurationManager.AppSettings[key] = value;
            return Task.CompletedTask;
        }
    }
}
