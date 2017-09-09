using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService.Service.Abstraction
{
    public interface IService
    {
        Task Start();

        Task Stop();
    }
}
