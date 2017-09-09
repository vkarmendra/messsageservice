using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using MessagingService.KeyProvider.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessagingService.KeyProvider.Json.Implementation
{
    public class JsonKeyProvider : IKeyProvider
    {
        private readonly string keyStorePath;

        public JsonKeyProvider(string keyStorePath)
        {
            this.keyStorePath = keyStorePath;
        }

        public async Task<string> GetValue(string keyName)
        {
            using(var streamReader = new StreamReader(keyStorePath))
            {
                var json = await streamReader.ReadToEndAsync();
                var jobject = JsonConvert.DeserializeObject<JObject>(json);
                return jobject[keyName]?.ToString();
            }
        }
    }
}
