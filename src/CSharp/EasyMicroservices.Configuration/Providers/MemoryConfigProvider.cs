using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace EasyMicroservices.Configuration.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryConfigProvider : BaseConfigProvider
    {
        ConcurrentDictionary<string, string> Values { get; set; } = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Task<string> GetValue(string key)
        {
            Values.TryGetValue(key, out var value);
            return Task.FromResult(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<T> GetValue<T>(string key)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<T> GetValue<T>()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task SetValue(string key, string value)
        {
            Values[key] = value;
            return TaskHelper.GetCompletedTask();
        }
    }
}
