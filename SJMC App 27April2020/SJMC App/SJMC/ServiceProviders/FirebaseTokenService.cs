using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class FirebaseTokenService : ApiBase
    {
        public async Task<BaseModel> InsertFirebaseToken(string labId, string type, string token, bool isAndroid, string dateUpdated)
        {
            try
            {
                var endPoint = String.Format(InsertFirebaseTokenKey, labId, type, token, isAndroid, dateUpdated);
                var result = await HttpClientBase.Get<BaseModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new BaseModel() { Message = ex.Message };

            }

        }
    }
}
