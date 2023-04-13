using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;


namespace AppiumSpecflowCSharp.Utilities
{
    public class LoadJsonData
    {
        private readonly double cacheTime = 1; // in Hours
        private IMemoryCache memoryCache;
       
        public LoadJsonData()
        {
            memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        /// <summary>
        /// Load json into C# object
        /// </summary>
        /// <example>LoadDriverConfig<DriverConfiguration>("Pixel-XL-API28-Android-v9_YouTube.json");</example>
        /// <returns>Desired capabilities - Appium Options</returns>
        public T GetObjectFromFile<T>(string fileName)
        {
            string cacheJson = getMemoryCache(fileName);
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
            var result = JsonConvert.DeserializeObject<T>(cacheJson, serializerSettings);
            if (result is null)
            {
                throw new ArgumentNullException($"Deserialized object is NULL. Json is { cacheJson }."); 
            }
            else
            {
                return result;
            }
        }


        #region Privates

        private string getMemoryCache(string fileName)
        {
            string cacheJson = string.Empty;
            if (!memoryCache.TryGetValue(fileName, out cacheJson))
            {
                cacheJson = readJson(Path.Combine("DeviceConfigs", fileName));
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(cacheTime));
                memoryCache.Set(fileName, cacheJson, cacheEntryOptions);
            }
            return cacheJson;
        }

        private string readJson(string path)
        {
            // these are all I know of
            const int ERROR_SHARING_VIOLATION = 32;
            const int ERROR_LOCK_VIOLATION = 33;

            string json = string.Empty;
            try
            {
                json = File.ReadAllText(Path.Combine(Util.AppLocation(), path));
            }
            catch (IOException ioError)
            {
                if (ioError.HResult == ERROR_SHARING_VIOLATION || ioError.HResult == ERROR_LOCK_VIOLATION)
                {
                    // trying again, if file in use/locked
                    Thread.Sleep(500);
                    json = File.ReadAllText(Path.Combine(Util.AppLocation(), path));
                }
                else
                {
                    throw ioError;
                }
            }
            catch (Exception generalError)
            {
                throw generalError;
            }
            return json;
        }
        #endregion
    }
}
