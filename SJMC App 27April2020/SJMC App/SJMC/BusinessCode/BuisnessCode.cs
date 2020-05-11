using SJMC.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.BusinessCode
{
    public class BuisnessCode : IBusinessCode
    {
        IApiProvider _apiProvider;
        string Language;
        public BuisnessCode(IApiProvider apiProvider)
        {
            //To initialize service providers...
            _apiProvider = apiProvider;
        }



    }
}
