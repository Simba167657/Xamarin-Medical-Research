using Newtonsoft.Json;
using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class ApiProvider : IApiProvider
    {
        public ApiProvider()
        {
            HttpClientHandler handler = new HttpClientHandler();

            _httpClient = new HttpClient(handler);
            TimeSpan ts = TimeSpan.FromMilliseconds(100000);
            _httpClient.Timeout = ts;
        }
        private readonly HttpClient _httpClient;
        public ApiResult<T> Get<T>(string url, Dictionary<string, string> headers = null)
        {
            HttpResponseMessage result = null;
            try
            {
                lock (_httpClient)
                {
                    if (headers != null)
                    {
                        AddHeadersToClient(headers);
                    }
                    result = _httpClient.GetAsync(url).Result;
                    if (headers != null)
                    {
                        RemoveHeadersFromClient(headers);
                    }
                }
                var rawResult = result.Content.ReadAsStringAsync().Result;

                try
                {
                    var deserialized = JsonConvert.DeserializeObject<T>(rawResult);
                    return new ApiResult<T>(rawResult, (int)result.StatusCode, deserialized);
                }
                catch (Exception ex)
                {
                    return new ApiResult<T>(rawResult, 501, Activator.CreateInstance<T>());
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Message is :-" + e.Message);
            }

            return new ApiResult<T>(null, null != result ? (int)result.StatusCode : 0, default(T));
        }

        public string GetString(string url, Dictionary<string, string> headers = null)
        {
            HttpResponseMessage result = null;
            try
            {
                lock (_httpClient)
                {
                    if (headers != null)
                    {
                        AddHeadersToClient(headers);
                    }
                    result = _httpClient.GetAsync(url).Result;
                    if (headers != null)
                    {
                        RemoveHeadersFromClient(headers);
                    }
                }

                var rawResult = result.Content.ReadAsStringAsync().Result;

                try
                {
                    return rawResult;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Message is :-" + e.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        /// Post to the specified url the body.
        /// </summary>
        /// <param name="url">URL to post to.</param>
        /// <param name="body">Body of post.</param>
        /// <typeparam name="T">The Response type (return type) (Jsonable object).</typeparam>
        /// <typeparam name="TR">The RequestType (the body) (Jsonable object).</typeparam>
        public async Task<ApiResult<T>> Post<T, TR>(string url, TR body, Dictionary<string, string> headers = null)
        {
            HttpResponseMessage result = null;
            string returnResult = string.Empty;
            try
            {
                lock (_httpClient)
                {
                    if (headers != null)
                    {
                        AddHeadersToClient(headers);
                    }
                    var x = JsonConvert.SerializeObject(body);
                    var y = JsonConvert.SerializeObject(headers);
                    if (body != null)
                    {
                        result = _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                    }
                    else
                    {
                        //var content = new FormUrlEncodedContent(headers);
                        //_httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                        _httpClient.DefaultRequestHeaders.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 1500000);
                        //Teetra.Services.FormUrlEncodedContent content = new Teetra.Services.FormUrlEncodedContent(headers);
                        //result = _httpClient.PostAsync(url, content).Result;
                    }

                    if (headers != null)
                    {
                        RemoveHeadersFromClient(headers);
                    }
                }

                var rawResult = result.Content.ReadAsStringAsync().Result;

                dynamic deserialized = "";
                try
                {
                    deserialized = JsonConvert.DeserializeObject<T>(rawResult);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return new ApiResult<T>(rawResult, 501, Activator.CreateInstance<T>());
                }
                try
                {
                    return new ApiResult<T>(rawResult, (int)result.StatusCode, deserialized);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                    return new ApiResult<T>(rawResult, 501, Activator.CreateInstance<T>());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Message is :-" + e.Message);
            }

            return new ApiResult<T>(null, null != result ? (int)result.StatusCode : 0, default(T));
        }

        public static string cleanForJSON(string s)
        {
            if (s == null || s.Length == 0)
            {
                return "";
            }

            char c = '\0';
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            String t;

            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                switch (c)
                {
                    case '\\':
                    case '"':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '/':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            t = "000" + String.Format("X", c);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }


        //PDF POST METHOD
        public async Task<ApiResult<T>> PostPDF<T, TR>(string url, TR body, Dictionary<string, string> headers = null)
        {
            _httpClient.CancelPendingRequests();
            string JoinSting = string.Join(";", headers);
            string[] AryJoinSting = JoinSting.Split(';');
            HttpResponseMessage result = null;
            string returnResult = string.Empty;
            try
            {
                lock (_httpClient)
                {

                    var x = JsonConvert.SerializeObject(body);
                    var y = JsonConvert.SerializeObject(headers);
                    var jsonString = JsonConvert.SerializeObject(headers);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TradersNetworkApp.Helpers.Settings.GeneralAccessToken);

                    result = _httpClient.PostAsync(url, content).Result;

                    if (headers != null)
                    {
                        RemoveHeadersFromClient(headers);
                    }
                }

                var rawResult = result.Content.ReadAsStringAsync().Result;
                rawResult = rawResult.Replace("base", "basee");
                dynamic deserialized = "";
                try
                {
                    deserialized = JsonConvert.DeserializeObject<T>(rawResult);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return new ApiResult<T>(rawResult, 501, Activator.CreateInstance<T>());
                }
                try
                {
                    return new ApiResult<T>(rawResult, (int)result.StatusCode, deserialized);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                    return new ApiResult<T>(rawResult, 501, Activator.CreateInstance<T>());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Message is :-" + e.Message);
            }

            return new ApiResult<T>(null, null != result ? (int)result.StatusCode : 0, default(T));
        }

        public ApiResult<T> Delete<T>(string url, Dictionary<string, string> headers = null)
        {
            HttpResponseMessage result = null;
            try
            {
                lock (_httpClient)
                {
                    if (headers != null)
                    {
                        AddHeadersToClient(headers);
                    }
                    result = _httpClient.DeleteAsync(url).Result;
                    if (headers != null)
                    {
                        RemoveHeadersFromClient(headers);
                    }
                }

                var rawResult = result.Content.ReadAsStringAsync().Result;
                try
                {
                    var deserialized = JsonConvert.DeserializeObject<T>(rawResult);
                    return new ApiResult<T>(rawResult, (int)result.StatusCode, deserialized);
                }
                catch (Exception ex)
                {
                    return new ApiResult<T>(rawResult, 501, Activator.CreateInstance<T>());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Message is :-" + e.Message);
            }

            return new ApiResult<T>(null, null != result ? (int)result.StatusCode : 0, default(T));
        }

        public ApiResult<T> Put<T, TR>(string url, TR body, Dictionary<string, string> headers = null)
        {
            HttpResponseMessage result = null;
            try
            {
                lock (_httpClient)
                {
                    if (headers != null)
                    {
                        AddHeadersToClient(headers);
                    }
                    var x = JsonConvert.SerializeObject(body);
                    result = _httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                    if (headers != null)
                    {
                        RemoveHeadersFromClient(headers);
                    }
                }

                var rawResult = result.Content.ReadAsStringAsync().Result;

                try
                {
                    var deserialized = JsonConvert.DeserializeObject<T>(rawResult);
                    return new ApiResult<T>(rawResult, (int)result.StatusCode, deserialized);
                }
                catch (Exception ex)
                {
                    return new ApiResult<T>(rawResult, 501, Activator.CreateInstance<T>());
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Message is :-" + e.Message);
            }

            return new ApiResult<T>(null, null != result ? (int)result.StatusCode : 0, default(T));
        }

        void AddHeadersToClient(Dictionary<string, string> headers)
        {
            foreach (var kv in headers)
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                }
                catch (Exception ex)
                {
                }
            }
        }

        void RemoveHeadersFromClient(Dictionary<string, string> headers)
        {
            foreach (var kv in headers)
            {
                _httpClient.DefaultRequestHeaders.Remove(kv.Key);
            }
        }

        public void SaveCookies(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the cookies.
        /// </summary>
        /// <param name="path">Path.</param>
        public void LoadCookies(string path)
        {
            throw new NotImplementedException();

        }

        public void DeleteCookies(string path)
        {
            throw new NotImplementedException();
        }

        public void ExpireCookies()
        {
            throw new NotImplementedException();
        }

        Task<ApiResult<T>> IApiProvider.Post<T, TR>(string url, TR body, Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }

        Task<ApiResult<T>> IApiProvider.PostPDF<T, TR>(string url, TR body, Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }
    }
}