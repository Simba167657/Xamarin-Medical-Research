using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class HttpClientBase
    {
        public string BaseUrl = "https://sjmc.stjosephlab-results.com/"; //  "http://binigurvinder1-001-site1.etempurl.com/"; //    "https://sjmc.stjosephlab-results.com/";// "http://192.168.1.2:8080/";//; //"http://binigurvinder1-001-site1.etempurl.com/"; //
        //public string NewServer = "https://stjosephlab-results.com/";
        public async Task<T> Post<T>(string endpoint, string jsonobject, string accessToken = "")
        {
            try
            {

                using (var httpClient = PreparedClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(BaseUrl);

                    if (accessToken != "")
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                    }
                    var response = await httpClient.PostAsync(endpoint, new StringContent(jsonobject, Encoding.UTF8, "application/json"));
                    var resultStr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultStr);
                }


            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<byte[]> PostPdfAttachment(string fullEndpoint, HttpContent jsonobject)
        {
            try
            {
                using (var httpClient = PreparedClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    var response = await httpClient.PostAsync(fullEndpoint, jsonobject );
                    var resultByte = await response.Content.ReadAsByteArrayAsync();
                    return (resultByte);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<T> Get<T>(string endpoint, string accessToken = "")
        {
            try
            {

                using (var httpClient = PreparedClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(BaseUrl);
                    if (accessToken != "")
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                    }
                    var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<T>(result);
                    return json;
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }



        public async Task<byte[]> GetStream(string endpoint)
        {
            try
            {

                using (var httpClient = PreparedClient())
                {
                    //httpClient.DefaultRequestHeaders.Accept.Clear();
                    // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(BaseUrl);

                    var response = await httpClient.GetAsync(endpoint);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return new byte[1];
                    }
                    var result = await response.Content.ReadAsByteArrayAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new byte[1];
            }


        }

        //public async Task<byte[]> GetStreamFromNewServer(string endpoint)
        //{
        //    try
        //    {

        //        using (var httpClient = PreparedClient())
        //        {
        //            //httpClient.DefaultRequestHeaders.Accept.Clear();
        //            // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            httpClient.BaseAddress = new Uri(NewServer);

        //            var response = await httpClient.GetAsync(endpoint);
        //            if (response.StatusCode != HttpStatusCode.OK)
        //            {
        //                return new byte[1];
        //            }
        //            var result = await response.Content.ReadAsByteArrayAsync();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new byte[1];
        //    }


        //}
        

        //public async Task<byte[]> GetStream(string endpoint, string html)
        //{
        //    try
        //    {
        //        var form = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> {
        //            new KeyValuePair<string, string>("html", html)
        //        });

        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Accept.Clear();
        //            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            httpClient.BaseAddress = new Uri("https://api.html2pdf.app/");

        //            //var encodedItems = form.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));
        //            //var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

        //            var response = await httpClient.PostAsync(endpoint, form);
        //            if (response.StatusCode != HttpStatusCode.OK)
        //            {
        //                return null;
        //            }
        //            var result =await response.Content.ReadAsByteArrayAsync();
        //       //     var mybytearray = Convert.FromBase64String(result);
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }


        //}


        public async Task<T> Upload<T>(string endpoint, byte[] file, string name, string accessToken = "")
        {
            using (var httpClient = PreparedClient())
            {
                httpClient.BaseAddress = new Uri(BaseUrl);
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(new MemoryStream(file)), "file", name);
                    var response = await httpClient.PostAsync(endpoint, content);
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<T>(result);
                    return json;

                }
            }
        }

        public byte[] GetBytes(string url)
        {
            var bytes = new WebClient().DownloadData(url);
            return bytes;
        }

        private HttpClient PreparedClient()
        {
            HttpClientHandler handler = new HttpClientHandler();

            //not sure about this one, but I think it should work to allow all certificates:
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssPolicyError) =>
            {
                return true;
            };
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };


            HttpClient client = new HttpClient(handler);

            return client;
        }

    }

}
