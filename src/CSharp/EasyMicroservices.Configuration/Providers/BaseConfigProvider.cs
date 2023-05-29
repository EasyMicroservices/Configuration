using EasyMicroservices.Configuration.Interfaces;
using System;
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
        public abstract Task<T> GetValue<T>(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract Task<T> GetValue<T>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected object GetPropertyValue(object data, string propertyName)
        {
            if (data == null)
                return null;
            return data.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }
    }
}
