using SJMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Views
{
    public class BrowserDetailPageVM : BaseViewModel
    {
        public BrowserDetailPageVM(INavigation _Nav)
        {
            Navigation = _Nav;

            BackCommand = new DelegateCommand(BackCommandAsync);
        }

        public DelegateCommand BackCommand { get; }

        /// <summary>
        /// TODO : To Define Send Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void BackCommandAsync(object obj)
        {
            await this.Navigation.PopAsync();
        }
    }
}
