using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SJMC.ServiceProviders
{
    public class PdfAttachmentService
    {
        public PdfAttachmentService()
        {

        }

        public async Task<byte[]> PostFileUploadAndGetPDF(string html)
        {
            try
            {
                var endPoint = String.Format("https://online-dimensions.com/pdf-to-html/pdf-to-html.php");

                RequestModel htmlClass = new RequestModel
                {
                    html = html,
                    height = 350,
                    width = 247
                };

                var serialisedString = JsonConvert.SerializeObject(htmlClass);
                var httpContent = new StringContent(serialisedString);

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var reshttpFirst = await httpClient.PostAsync(endPoint, httpContent);
                var resStringFirst = await reshttpFirst.Content.ReadAsStringAsync();

                if (resStringFirst != String.Empty)
                {
                    ResponseModel resFirst = JsonConvert.DeserializeObject<ResponseModel>(resStringFirst);

                    using (var httpClient1 = new HttpClient())
                    {
                        var response = await httpClient1.GetAsync(resFirst.path);

                        var resultByte = await response.Content.ReadAsByteArrayAsync();
                        return resultByte;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }

    public class RequestModel
    {
        public string html { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class ResponseModel
    {
        public string path { get; set; }
    }

}

