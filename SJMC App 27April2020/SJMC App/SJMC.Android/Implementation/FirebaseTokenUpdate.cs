using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Firebase.Messaging;
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

[assembly: Dependency(typeof(FirebaseTokenUpdate))]
namespace SJMC.Droid
{
    public class FirebaseTokenUpdate : IFirebaseTokenUpdate
    {
        public async Task<string> GetNewFirebaseToken()
        {
            MyFirebaseIIDService service = new MyFirebaseIIDService();
            return await service.GenerateNewToken();


        }
    }
}




