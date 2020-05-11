using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using Java.IO;
using Plugin.CurrentActivity;
using SJMC.CustomControl;
using SJMC.Droid;
using SJMC.Interfaces;
using SJMC.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]

[assembly: Dependency(typeof(HtmlToPDFConvertor))]
namespace SJMC.Droid
{

    public class HtmlToPDFConvertor : IHtmlToPDFConvertor
    {
        Android.Webkit.WebView webpage;
        public byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public byte[] GetFromAssets(Context context, string assetName)
        {
            AssetManager assetManager = context.Assets;
            Stream inputStream;
            try
            {
                using (inputStream = assetManager.Open(assetName))
                {
                    // return inputStream;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        inputStream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public void SafeHTMLToPDF(string html, string filename)
        {
            var nativeFileHandler = DependencyService.Get<INativeFileHandler>();
            var bytes = GetFromAssets(CrossCurrentActivity.Current.Activity, "Reports2.pdf");
            // var bytes = ReadFully(inputStream);
            nativeFileHandler.SaveFile(filename, bytes);

            //  html = nativeFileHandler.GetText("index.html");

            //string path = nativeFileHandler.path;
            //string pdfPath = System.IO.Path.Combine(path, filename);
            //System.IO.FileStream fs = new FileStream(pdfPath, FileMode.Create);
            //Document document = new Document(PageSize.A4);
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //HTMLWorker worker = new HTMLWorker(document);
            //document.Open();
            //  TextReader reader = new System.IO.StringReader(str);
            //worker.StartDocument();
            //worker.Parse(reader);
            //worker.EndDocument();
            //worker.Close();
            //document.Close();
            //writer.Close();
            //fs.Close();

            //var dir = new Java.IO.File(nativeFileHandler.path);
            //var file = new Java.IO.File(filename);

            //if (!dir.Exists())
            //    dir.Mkdirs();

            //int x = 0;
            ////while (file.Exists())
            ////{
            ////    x++;
            ////    file = new Java.IO.File(dir + "/" + filename + "( " + x + " )" + ".pdf");
            ////}

            //if (webpage == null)
            //    webpage = new Android.Webkit.WebView(CrossCurrentActivity.Current.Activity);

            //int width = 2102;
            //int height = 2973;

            //webpage.Layout(0, 0, width, height);
            //webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);
            //webpage.SetWebViewClient(new WebViewCallBack(nativeFileHandler.path + "/" + filename));

            //return filename;
        }


        class WebViewCallBack : WebViewClient
        {

            string fileNameWithPath = null;

            public WebViewCallBack(string path)
            {
                this.fileNameWithPath = path;
            }


            public override void OnPageFinished(Android.Webkit.WebView myWebview, string url)
            {
                PdfDocument document = new PdfDocument();
                PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(2120, 3000, 1).Create());

                myWebview.Draw(page.Canvas);
                document.FinishPage(page);
                Stream filestream = new MemoryStream();
                FileOutputStream fos = new Java.IO.FileOutputStream(fileNameWithPath, false); ;
                try
                {
                    document.WriteTo(filestream);
                    fos.Write(((MemoryStream)filestream).ToArray(), 0, (int)filestream.Length);
                    fos.Close();
                    PdfPage.IsHtmlSet = true;

                }
                catch
                {
                    PdfPage.IsHtmlSet = false;
                }
            }
        }
    }

}




