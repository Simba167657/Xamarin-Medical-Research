using Octane.Xamarin.Forms.VideoPlayer;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SJMC.Helpers
{
    [ContentProperty("Resource")]
    public class VideoResourceExtension : IMarkupExtension
    {
        public string Resource { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Resource == null)
                return null;
            var videoSource = VideoSource.FromResource(Resource);
            return videoSource;
        }
    }
}
