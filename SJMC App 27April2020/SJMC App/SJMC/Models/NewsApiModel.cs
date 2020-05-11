using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Models
{


    public class NewsApiModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<NewsModel> data { get; set; }
    }

    public class NewsModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IssueDate { get; set; }
        public bool IsApproved { get; set; }
        public string ImageURL { get; set; }
        public ImageSource ImageData { get; set; } = null;
        public string BackgroundColor { get; set; } = "#6CAAD3";
    }


}
