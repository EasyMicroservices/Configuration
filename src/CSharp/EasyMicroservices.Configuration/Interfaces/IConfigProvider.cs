using System.Threading.Tasks;

namespace EasyMicroservices.Configuration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetValue(string key, string value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetValue(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetValue<T>(string key);
    }
}
