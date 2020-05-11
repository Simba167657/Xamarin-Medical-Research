using System;

using Xamarin.Forms;

namespace SJMC.Views.PlayVideoResource
{
    public class PlayVideoResourcePage : ContentPage
    {
        public PlayVideoResourcePage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

