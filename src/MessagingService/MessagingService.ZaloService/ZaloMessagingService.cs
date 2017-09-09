using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingService.Service.Abstraction;

namespace MessagingService.ZaloService
{
    public class ZaloMessagingService : IService
    {
        public async Task Start()
        {
            Console.Write("Running");
        }

        public async Task Stop()
        {
            Console.WriteLine("Stopping");
        }
    }
}
