using EasyMicroservices.Configuration.Providers;
using EasyMicroservices.Serialization.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace EasyMicroservices.Configuration.MicrosoftConfiguration.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class MicrosoftConfigurationProvider : BaseConfigProvider
    {
        IConfigurationRoot _configurationRoot;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationRoot"></param>
        public MicrosoftConfigurationProvider(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Task<string> GetValue(string key)
        {
            var split = key.Split(':');
            if (split.Length > 1)
                return Task.FromResult(_configurationRoot.GetSection(split[0])[split[1]]);
            else
                return Task.FromResult(_configurationRoot.GetSection(key).Value);
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
            return Task.FromResult(_configurationRoot.GetSection(key).Get<T>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<T> GetValue<T>()
        {
            return Task.FromResult(_configurationRoot.Get<T>());
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
    }
}