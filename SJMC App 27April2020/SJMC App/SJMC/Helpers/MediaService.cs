using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SJMC.Helpers
{
    public class MediaService
    {
        public async Task<Tuple<ImageSource, string, Byte[], Stream, string>> SelectSource()
        {
            try
            {
                var selectedItem = await UserDialogs.Instance.ActionSheetAsync("Choose Media", "Cancel",
                    null, null, "Take Photo", "Browse From Album");

                Tuple<ImageSource, string, Byte[], Stream, string> src = null;
                switch (selectedItem)
                {
                    case "Take Photo":
                        src = await TakePhoto();
                        break;
                    case "Browse From Album":
                        src = await PickPhoto();
                        break;
                }

                return src;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<Tuple<ImageSource, string, Byte[], Stream, string>> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Photos Not Supported", ":( Permission not granted to take photo", "OK");
                return new Tuple<ImageSource, string, Byte[], Stream, string>(null, ":( Permission not granted to take photo", null, null, null);
            }

            // string imageName = "Ld" + new Guid();
            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                CustomPhotoSize = 30,
                PhotoSize = PhotoSize.Medium,
            });

            var words = file.Path.Split('/');
            var path = words[words.Length - 1];
            return new Tuple<ImageSource, string, Byte[], Stream, string>(ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            }), path, ConvertStreamToByteArray(file.GetStream()), file.GetStream(), null);
        }
        public async Task<Tuple<ImageSource, string, Byte[], Stream, string>> TakePhoto()
        {
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Message", "Camera not Available!", "Okay");

                return new Tuple<ImageSource, string, Byte[], Stream, string>(null, ":( Camera not Available!", null, null, null);
            }


            string imageName = "ProfilePic" + Guid.NewGuid() + ".png";
            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                // Directory = "IncidentTrackerStorage",
                SaveToAlbum = true,
                Name = imageName,
                CustomPhotoSize = 20,
                PhotoSize = PhotoSize.Small
            });

            //  var path = file.Path;
            return new Tuple<ImageSource, string, Byte[], Stream, string>(ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            }), imageName, ConvertStreamToByteArray(file.GetStream()), file.GetStream(), null);
        }
        public async Task<string> PickVideo()
        {
            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Videos Not Supported", ":( Permission not granted to videos.", "OK");
                return null;
            }
            var file = await CrossMedia.Current.PickVideoAsync();
            if (file == null)
                return null;

            var filePath = file.Path;
            await UserDialogs.Instance.AlertAsync("Video Selected", "Location: " + file.Path, "OK");
            file.Dispose();

            return filePath;
        }
        public async Task<string> TakeVideo()
        {
            if (!CrossMedia.Current.IsTakeVideoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Message", "Camera not Available !", "Okay");
                return null;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            {
                Name = "video.mp4",
                Directory = "DefaultVideos",
            });

            if (file == null)
                return null;

            var filePath = file.Path;
            await UserDialogs.Instance.AlertAsync("Video Recorded", "Location: " + file.Path, "OK");

            file.Dispose();

            return filePath;
        }
        private byte[] ConvertStreamToByteArray(Stream stream)
        {
            byte[] imgBytes;
            using (var streamReader = new MemoryStream())
            {
                stream.CopyTo(streamReader);
                imgBytes = streamReader.ToArray();
            }
            return imgBytes;
        }
    }

}
