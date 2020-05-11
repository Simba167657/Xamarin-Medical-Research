using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.Interfaces
{
    public interface IFirebaseTokenUpdate
    {
        Task<string> GetNewFirebaseToken();
    }
}
