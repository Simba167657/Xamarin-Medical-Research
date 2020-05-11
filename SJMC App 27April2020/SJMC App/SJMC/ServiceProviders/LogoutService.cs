using System;
using System.Threading.Tasks;
using SJMC.Interfaces;
using SJMC.Models;
using Xamarin.Forms;
//using UIKit; (TBD)
namespace SJMC.ServiceProviders
{
    public class LogoutService : ApiBase
    {

        public async Task LogoutTokenFromApp(string id, string type)
        {

            try
            {
                var endPoint = String.Format(AppLogout, id, type);
                var result = await HttpClientBase.Get<LogoutResponse>(endPoint);
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message.ToString());
            }
        }

    }
}
