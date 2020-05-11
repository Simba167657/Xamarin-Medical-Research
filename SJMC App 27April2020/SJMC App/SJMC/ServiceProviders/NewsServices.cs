using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class NewsServices : ApiBase
    {
        public async Task<NewsApiModel> GetAllNews()
        {
            try
            {
                var endPoint = String.Format(GetAllNewsKey);
                var result = await HttpClientBase.Get<NewsApiModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new NewsApiModel() { Message = ex.Message };
            }

        }
    }
}
