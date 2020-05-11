using SJMC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class ReportsService : ApiBase
    {

        public async Task<ReportsApiModel> GetLabReports(string role, string id, string labType, int pageNumber)
        {
            try
            {
                var endPoint = String.Format(GetReportsKey, role, id, labType, pageNumber);
                var result = await HttpClientBase.Get<ReportsApiModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new ReportsApiModel() { Message = ex.Message };

            }

        }

        public async Task<ReportsApiModel> GetAllLabReports(string role, string id, string labType, DateTime startDate, DateTime endDate)
        {
            try
            {

                var endPoint = String.Format(GetAllReportsKey,role,
                                                              id,
                                                              labType,
                                                              startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                                                              endDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                var result = await HttpClientBase.Get<ReportsApiModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new ReportsApiModel() { Message = ex.Message };

            }

        }

        public async Task<ReportCountApiModel> GetLabReportsCount(string role, string id)
        {
            try
            {
                var endPoint = String.Format(GetLabReportsCountKey, role, id);
                var result = await HttpClientBase.Get<ReportCountApiModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new ReportCountApiModel() { Message = ex.Message };

            }

        }


        public async Task<ReportsApiModel> CheckReports(string role, string branch, string year, string refrence, string id, string date)
        {
            try
            {
                var endPoint = String.Format(CheckReportsKey, role, branch, year, refrence, id, date);
                var result = await HttpClientBase.Get<ReportsApiModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new ReportsApiModel() { Message = ex.Message };

            }

        }

        public async Task<ReportsApiModel> SetSeenReports(string role, string branch, string year, string refrence, string isViewed)
        {
            try
            {
                var endPoint = String.Format(SetSeenReportsKey, role, branch, year, refrence, isViewed);
                var result = await HttpClientBase.Get<ReportsApiModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new ReportsApiModel() { Message = ex.Message };

            }

        }

        public async Task<AttachmentApiModel> GetAttachmentsList(string name)
        {
            try
            {
                var endPoint = String.Format(GetAttachmentsListKey, name);
                var result = await HttpClientBase.Get<AttachmentApiModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new AttachmentApiModel() { Message = ex.Message };

            }

        }



        public async Task<byte[]> Download(string endPoint)
        {
            try
            {
                var result = await HttpClientBase.GetStream(endPoint);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public byte[] DownloadFileBytes(string endPoint)
        //{
        //    try
        //    {
        //        var result = HttpClientBase.GetBytes(endPoint);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public async Task<PdfModel> DownloadBytes(string endPoint)
        {
            try
            {
                PdfModel result = await HttpClientBase.Get<PdfModel>(endPoint);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<FileModel> DownloadFileBytes(string endPoint)
        {
            try
            {
                FileModel result = await HttpClientBase.Get<FileModel>(endPoint);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


    public class PdfModel
    {
        public byte[] FileContents { get; set; }
        public string ContentType { get; set; }
        public string FileDownloadName { get; set; }
    }

    public class FileModel
    {
        public byte[] FileContents { get; set; }
        public string FileDownloadName { get; set; }
    }

}
