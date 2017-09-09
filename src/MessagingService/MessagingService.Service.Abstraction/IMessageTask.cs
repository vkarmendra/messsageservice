using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MessagingService.Service.Abstraction
{
    public interface IMessageTask
    {
        Task<JObject> GetInboxes(string apiKey);

        Task<JObject> GetMessages(NameValueCollection configurations);
    }
}
