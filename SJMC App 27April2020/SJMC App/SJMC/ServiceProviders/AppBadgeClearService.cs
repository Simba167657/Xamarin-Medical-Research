using System;
using System.Threading.Tasks;
using SJMC.Models;

namespace SJMC.ServiceProviders
{
    public class AppBadgeClearService : ApiBase
    {

        public async Task ClearBadgeForApp(string id, string type)
        {

            try
            {
                var endPoint = String.Format(AppBadgeClear, id, type);
                var result = await HttpClientBase.Get<AppBadgeClearResponse>(endPoint);
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message.ToString());
            }
        }

    }
}
