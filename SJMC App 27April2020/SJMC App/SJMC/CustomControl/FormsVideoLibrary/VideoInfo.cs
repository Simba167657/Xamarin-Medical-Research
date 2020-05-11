using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.CustomControl.FormsVideoLibrary
{
    public class VideoInfo
    {
        public string DisplayName { set; get; }

        public VideoSource VideoSource { set; get; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
