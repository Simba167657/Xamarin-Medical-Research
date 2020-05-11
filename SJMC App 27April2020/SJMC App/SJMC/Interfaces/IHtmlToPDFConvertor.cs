using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.Interfaces
{
   public interface IHtmlToPDFConvertor
    {
        void SafeHTMLToPDF(string html, string filename);

       
    }
}
