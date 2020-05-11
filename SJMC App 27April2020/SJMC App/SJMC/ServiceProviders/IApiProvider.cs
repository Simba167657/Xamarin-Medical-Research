using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public interface IApiProvider
    {
        ApiResult<T> Get<T>(string url, Dictionary<string, string> headers = null);
        /// <summary>
        /// Post to the specified url the body.
        /// </summary>
        /// <param name="url">URL to post to.</param>
        /// <param name="body">Body of post.</param>
        /// <typeparam name="T">The Response type (Jsonable object).</typeparam>
        /// <typeparam name="TR">The RequestType (Jsonable object).</typeparam>
        Task<ApiResult<T>> Post<T, TR>(string url, TR body, Dictionary<string, string> headers = null);
        Task<ApiResult<T>> PostPDF<T, TR>(string url, TR body, Dictionary<string, string> headers = null);
        ApiResult<T> Delete<T>(string url, Dictionary<string, string> headers = null);
        ApiResult<T> Put<T, TR>(string url, TR body, Dictionary<string, string> headers = null);
        string GetString(string url, Dictionary<string, string> headers = null);
        void SaveCookies(string path);
        void LoadCookies(string path);
        void DeleteCookies(string path);

        void ExpireCookies();
    }
}