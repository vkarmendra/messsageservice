using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using MessagingService.Common;
using MessagingService.KeyProvider.Abstraction;
using MessagingService.Service.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessagingService.TextLocal
{
    public class TextLocalMessagingService : IService
    {
        private readonly IMessageTask messageTask;
        private readonly IKeyProvider keyProvider;
        private IList<Inbox> inboxes;
        private Timer timer;
        private long lastRetrivedUnixTimeStamp = 0;

        string timestampPath = Path.Combine(Environment.CurrentDirectory, "timestamp.txt");
        public TextLocalMessagingService(IMessageTask messageTask,IKeyProvider keyProvider)
        {
            this.messageTask = messageTask;
            this.keyProvider = keyProvider;
        }
        public async Task Start()
        {
            var data = await messageTask.GetInboxes(await keyProvider.GetValue("apiKey"));
            inboxes = data["inboxes"].ToObject<List<Inbox>>();
            if (File.Exists(timestampPath))
            {
                lastRetrivedUnixTimeStamp = long.Parse(File.ReadAllText(timestampPath));
            }
            timer = new Timer();
            timer.Elapsed += GetMessagesAsync;
            timer.Interval = 20000;
            timer.Start();
            
            
        }

        private async void GetMessagesAsync(object sender, ElapsedEventArgs e)
        {
            var timeStampToSave = DateTime.Now.ToUnixTimestamp();
            foreach (var inbox in inboxes)
            {
                var id = inbox.id;
                var nameValueCollection = new NameValueCollection
                {
                    {"apiKey",(await keyProvider.GetValue("apiKey")) },
                    {"inbox_id",id.ToString() }
                };
                if (lastRetrivedUnixTimeStamp != 0)
                {
                    nameValueCollection.Add("min_time", lastRetrivedUnixTimeStamp.ToString());
                }
                var data = await messageTask.GetMessages(nameValueCollection);
                try
                {
                    if (data["status"].ToString() == "success")
                    {
                        var messages = data["messages"].ToObject<List<Message>>();
                        foreach (var message in messages)
                        {
                            SaveToDisk(message);
                        }
                    }
                    Console.WriteLine("Run at " + DateTime.Now);
                    Console.WriteLine(data["status"]);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            File.WriteAllText(timestampPath, DateTime.Now.ToUnixTimestamp().ToString());
        }

        public async Task Stop()
        {
            timer.Stop();
        }
        
        private void SaveToDisk(Message message)
        {
            try
            {
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, message.Date.ToOADate() + "_" + message.Number + ".json"), JsonConvert.SerializeObject(message));
            }
            catch { }
        }
    }

    public class Inbox
    {
        public int id { get; set; }
    }

    public class Message
    {
        [JsonProperty("number")]
        public decimal Number { get; set; }

        [JsonProperty("message")]
        public string Text { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("isNew")]
        public bool? IsNew { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
