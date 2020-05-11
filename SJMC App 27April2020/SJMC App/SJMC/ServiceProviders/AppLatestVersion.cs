using System;
using System.Threading.Tasks;
using SJMC.Models;

namespace SJMC.ServiceProviders
{
    public class AppLatestVersion : ApiBase
    {

        public async Task<AppLatestVersionResponseModel> CheckAppVersion(string platform, string version)
        {

            try
            {
                var endPoint = String.Format(AppVersion, platform, version);
                var result = await HttpClientBase.Get<AppLatestVersionResponseModel>(endPoint);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
