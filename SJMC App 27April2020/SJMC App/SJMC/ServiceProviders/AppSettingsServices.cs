using Newtonsoft.Json;
using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class AppSettingsServices : ApiBase
    {
        public async Task<AppSettings> GetAppSettingsKey(string key)
        {
            try
            {
                var endPoint = String.Format(AppSettingsKey, key);
                var result = await HttpClientBase.Get<AppSettings>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
