using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MessagingService.Service.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessagingService.TextLocal
{
    public class TextLocalMessageTask : IMessageTask
    {
        public async Task<JObject> GetInboxes(string apiKey)
        {
            return await Task.Run(() =>
            {
                using (var wb = new WebClient())
                {
                    var response = wb.UploadValues(new Uri("https://api.textlocal.in/get_inboxes/"), new NameValueCollection
                {
                    {"apiKey",apiKey }
                });
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    return JsonConvert.DeserializeObject<JObject>(result);
                }
            });
            
        }

        public async Task<JObject> GetMessages(NameValueCollection configurations)
        {
            return await Task.Run(() =>
            {
                using (var wb = new WebClient())
                {
                    var response = wb.UploadValues(new Uri("http://api.textlocal.in/get_messages/"), configurations);
                    var result = System.Text.Encoding.UTF8.GetString(response);
                    return JsonConvert.DeserializeObject<JObject>(result);
                }
            });
            
        }
    }
}
