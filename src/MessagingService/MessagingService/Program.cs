using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingService.KeyProvider.Abstraction;
using MessagingService.KeyProvider.Json.Implementation;
using MessagingService.Service.Abstraction;
using MessagingService.TextLocal;
using MessagingService.ZaloService;
using SimpleInjector;
using Topshelf;

namespace MessagingService
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.Register<IService, TextLocalMessagingService>();
            container.Register<IMessageTask, TextLocalMessageTask>();
            container.Register<IKeyProvider>(() => new JsonKeyProvider(ConfigurationManager.AppSettings["KeyStorePath"].ToString()));
            container.Verify();
            HostFactory.Run(conf =>
            {
                var service = container.GetInstance<IService>();
                conf.Service<IService>(x =>
                {
                    x.ConstructUsing(tc => service);
                    x.WhenStarted(async tc => await tc.Start());
                    x.WhenStopped(async tc => await tc.Stop());
                });

                conf.RunAsLocalSystem();
                conf.SetServiceName(service.GetType().Name);
                conf.SetDisplayName(service.GetType().Name);
                conf.SetDescription(service.GetType().Name);
            });
        }
    }
}
