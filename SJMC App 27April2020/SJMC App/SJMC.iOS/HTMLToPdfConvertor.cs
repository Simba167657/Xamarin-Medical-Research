using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using SJMC.Interfaces;
using SJMC.iOS;
using UIKit;
using WebKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(HTMLToPdfConvertor))]
namespace SJMC.iOS
{
    public class HTMLToPdfConvertor: IHtmlToPDFConvertor
    {
        public string SafeHTMLToPDF(string html, string filename)
        {
            //WKWebView webView = new WKWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72));
            var config = new WKWebViewConfiguration();
            WKWebView webView = new WKWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72), config);

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = Path.Combine(documents, "Invoice" + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString() + ".pdf");

            webView.NavigationDelegate = new WebViewCallBack(file);
            
            //webView.Delegate = new WebViewCallBack(file);
            //webView.ScalesPageToFit = true;
            webView.UserInteractionEnabled = false;
            webView.BackgroundColor = UIColor.White;
            webView.LoadHtmlString(html, null);



            return file;
        }

        void IHtmlToPDFConvertor.SafeHTMLToPDF(string html, string filename)
        {
        }

        class WebViewCallBack : WKNavigationDelegate
        {

            string filename = null;
            public WebViewCallBack(string path)
            {
                this.filename = path;

            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                double height, width;
                int header, sidespace;

                width = 595.2;
                height = 841.8;
                header = 10;
                sidespace = 10;


                UIEdgeInsets pageMargins = new UIEdgeInsets(header, sidespace, header, sidespace);
                webView.ViewPrintFormatter.ContentInsets = pageMargins;

                UIPrintPageRenderer renderer = new UIPrintPageRenderer();
                renderer.AddPrintFormatter(webView.ViewPrintFormatter, 0);

                CGSize pageSize = new CGSize(width, height);
                CGRect printableRect = new CGRect(sidespace,
                                  header,
                                  pageSize.Width - (sidespace * 2),
                                  pageSize.Height - (header * 2));
                CGRect paperRect = new CGRect(0, 0, width, height);
                renderer.SetValueForKey(NSValue.FromObject(paperRect), (NSString)"paperRect");
                renderer.SetValueForKey(NSValue.FromObject(printableRect), (NSString)"printableRect");
                NSData file = PrintToPDFWithRenderer(renderer, paperRect);
                File.WriteAllBytes(filename, file.ToArray());
            }

            private NSData PrintToPDFWithRenderer(UIPrintPageRenderer renderer, CGRect paperRect)
            {
                NSMutableData pdfData = new NSMutableData();
                UIGraphics.BeginPDFContext(pdfData, paperRect, null);

                renderer.PrepareForDrawingPages(new NSRange(0, renderer.NumberOfPages));

                CGRect bounds = UIGraphics.PDFContextBounds;

                for (int i = 0; i < renderer.NumberOfPages; i++)
                {
                    UIGraphics.BeginPDFPage();
                    renderer.DrawPage(i, paperRect);
                }
                UIGraphics.EndPDFContent();

                return pdfData;
            }

        }
    }
}