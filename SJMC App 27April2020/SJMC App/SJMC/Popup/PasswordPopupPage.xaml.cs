using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SJMC.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordPopupPage : PopupPage
    {
        //TODO : To Define Class Level Variables...
        public event EventHandler DialogClosed;
        public event EventHandler DialogShow;
        public event EventHandler DialogClosing;
        public event EventHandler DialogShowing;

        public PasswordPopupPage(Views.PasswordReset.PasswordResetPageVM passwordResetPageVM)
        {
            InitializeComponent();
            PopUpBgLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(HideDialog)
            });
        }
        public void HideDialog()
        {
            HideDialogAnimation(PopUpDialogLayout, PopUpBgLayout);
        }

        public void ShowDialog()
        {
            ShowDialogAnimation(PopUpDialogLayout, PopUpBgLayout);
        }

        protected virtual void OnDialogShow(EventArgs e)
        {
            DialogShow?.Invoke(this, e);
        }

        protected virtual void OnDialogClosing(EventArgs e)
        {
            DialogClosing?.Invoke(this, e);
        }

        protected virtual void OnDialogShowing(EventArgs e)
        {
            DialogShowing?.Invoke(this, e);
        }

        private void ShowDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            dialog.TranslationY = bg.Height;
            bg.IsVisible = true;
            dialog.IsVisible = true;

            ////ANIMATIONS 
            var showBgAnimation = OpacityAnimation(bg, 0, 0.5);
            var showDialogAnimation = TransLateYAnimation(dialog, bg.Height, 0);

            ////EXECUTE ANIMATIONS
            this.Animate("showBg", showBgAnimation, 16, 200, Easing.Linear, (d, f) => { });
            this.Animate("showMenu", showDialogAnimation, 16, 200, Easing.Linear, (d, f) =>
            {
                OnDialogShow(new EventArgs());
            });

            OnDialogShowing(new EventArgs());
        }

        private async void HideDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            //ANIMATIONS     
            var hideBgAnimation = OpacityAnimation(bg, 0.5, 0);
            var showDialogAnimation = TransLateYAnimation(dialog, 0, bg.Height);

            ////EXECUTE ANIMATIONS
            this.Animate("hideBg", hideBgAnimation, 16, 200, Easing.Linear, (d, f) => { });
            this.Animate("hideMenu", showDialogAnimation, 16, 200, Easing.Linear, (d, f) =>
            {
                bg.IsVisible = false;
                dialog.IsVisible = false;
                dialog.TranslationY = PopUpBgLayout.Height;

                OnDialogClosing(new EventArgs());
            });
            await Navigation.PopAllPopupAsync(true);

            OnDialogClosing(new EventArgs());
        }

        private static Animation TransLateYAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationY = d; }, from, to);
        }

        private static Animation TransLateXAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationX = d; }, from, to);
        }

        private static Animation OpacityAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.Opacity = d; }, from, to);
        }

        private void BtnNotAllowed_Tapped(object sender, EventArgs e)
        {
            HideDialog();
        }

    }

}
    
