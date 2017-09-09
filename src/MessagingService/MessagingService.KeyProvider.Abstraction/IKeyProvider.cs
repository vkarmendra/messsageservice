using System;
using System.Threading.Tasks;

namespace MessagingService.KeyProvider.Abstraction
{
    public interface IKeyProvider
    {
        Task<string> GetValue(string keyName);
    }
}
