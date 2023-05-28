using EasyMicroservices.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.Configuration.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseConfigProvider : IConfigProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Task<string> GetValue(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract Task SetValue(string key, string value);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<T> GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}
