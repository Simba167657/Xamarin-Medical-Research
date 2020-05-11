using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using SJMC.CustomControl;
using SJMC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomRatingBar), typeof(RatingBarRenderer))]
namespace SJMC.iOS.Renderer
{
    class RatingBarRenderer : ViewRenderer<CustomRatingBar, UIView>
    {
        const string IconStarBlank = "ic_star_blank.png";
        const string IconStarYellow = "ic_star_yellow.png";
        const string IconStarGreen = "ic_star_green.png";
        const string IconStarOrange = "ic_star_orange.png";
        const string IconStarRed = "ic_star_red.png";

        UIView rateView;
        UIButton oneStar, twoStar, threeStar, fourStar, fiveStar;
        float starSize;
        CustomRatingBar element;


        protected override void OnElementChanged(ElementChangedEventArgs<CustomRatingBar> e)
        {
            base.OnElementChanged(e);
            element = Element as CustomRatingBar;
            if (element == null)
                return;
            InitializeButton();
            SetTouchEvents(element.IsReadonly);

            rateView = new UIView()
            {

            };
            if (element.IsSmallStyle)
            {
                starSize = 15;
                rateView.Frame = new CGRect(0, 0, 95, 15);
                SetRatingBarSmall();
                AddStarInView();
                if (element.Rating >= 0)
                    ShowRatingBar();
            }
            else
            {
                starSize = 30;
                rateView.Frame = new CGRect(0, 0, 170, 30);
                SetRatingBarDefault();
                AddStarInView();
                ShowRatingBar();
            }
            SetNativeControl(rateView);
            var tapGesture = new UITapGestureRecognizer(OnRateViewTapped);
            rateView.AddGestureRecognizer(tapGesture);
        }

        private void OnRateViewTapped()
        {
            element.OnTapped();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals(CustomRatingBar.RatingProperty.PropertyName))
            {
                ShowRatingBar();
            }
            else if (e.PropertyName.Equals(CustomRatingBar.IsReadonlyProperty.PropertyName))
            {
                SetTouchEvents(element.IsReadonly);
            }
        }

        // rating bar bind with value;
        private void ShowRatingBar()
        {
            if (Element.Rating <= 0)
                SetBlankStarRating();
            else if (Element.Rating <= 1)
                SetOneStarRating();
            else if (Element.Rating <= 2)
                SetTwoStarRating();
            else if (Element.Rating <= 3)
                SetThreeStarRating();
            else if (Element.Rating <= 4)
                SetFourStarRating();
            else if (Element.Rating <= 5)
                SetFiveStarRating();
        }

        // button initialize
        private void InitializeButton()
        {
            oneStar = UIButton.FromType(UIButtonType.Custom);
            oneStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);

            twoStar = UIButton.FromType(UIButtonType.Custom);
            twoStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);

            threeStar = UIButton.FromType(UIButtonType.Custom);
            threeStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);

            fourStar = UIButton.FromType(UIButtonType.Custom);
            fourStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);

            fiveStar = UIButton.FromType(UIButtonType.Custom);
            fiveStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
        }

        void OneStar_TouchUpInside(object sender, EventArgs e)
        {
            element.OnRatingChanged(1);
        }
        void TwoStar_TouchUpInside(object sender, EventArgs e)
        {
            element.OnRatingChanged(2);
        }
        void ThreeStar_TouchUpInside(object sender, EventArgs e)
        {
            element.OnRatingChanged(3);
        }
        void FourStar_TouchUpInside(object sender, EventArgs e)
        {
            element.OnRatingChanged(4);
        }
        void FiveStar_TouchUpInside(object sender, EventArgs e)
        {
            element.OnRatingChanged(5);
        }

        private void SetTouchEvents(bool isReadOnly)
        {
            oneStar.TouchUpInside -= OneStar_TouchUpInside;
            twoStar.TouchUpInside -= TwoStar_TouchUpInside;
            threeStar.TouchUpInside -= ThreeStar_TouchUpInside;
            fourStar.TouchUpInside -= FourStar_TouchUpInside;
            fiveStar.TouchUpInside -= FiveStar_TouchUpInside;
            if (isReadOnly == false)
            {
                oneStar.TouchUpInside += OneStar_TouchUpInside;
                twoStar.TouchUpInside += TwoStar_TouchUpInside;
                threeStar.TouchUpInside += ThreeStar_TouchUpInside;
                fourStar.TouchUpInside += FourStar_TouchUpInside;
                fiveStar.TouchUpInside += FiveStar_TouchUpInside;
            }
        }


        // rating bar size
        private void SetRatingBarSmall()
        {
            //int x = (int)(App.ScreenSize.Width - ratingBarWidth) / 2;
            int x = 0;
            oneStar.Frame = new CGRect(x, 0, starSize, starSize);
            twoStar.Frame = new CGRect(x = x + 20, 0, starSize, starSize);
            threeStar.Frame = new CGRect(x = x + 20, 0, starSize, starSize);
            fourStar.Frame = new CGRect(x = x + 20, 0, starSize, starSize);
            fiveStar.Frame = new CGRect(x = x + 20, 0, starSize, starSize);
        }

        private void SetRatingBarDefault()
        {
            //int x = (int)(App.ScreenSize.Width - ratingBarWidth) / 2;
            int x = 0;
            oneStar.Frame = new CGRect(x, 0, starSize, starSize);
            twoStar.Frame = new CGRect(x = x + 35, 0, starSize, starSize);
            threeStar.Frame = new CGRect(x = x + 35, 0, starSize, starSize);
            fourStar.Frame = new CGRect(x = x + 35, 0, starSize, starSize);
            fiveStar.Frame = new CGRect(x = x + 35, 0, starSize, starSize);
        }

        private void AddStarInView()
        {
            rateView.AddSubview(oneStar);
            rateView.AddSubview(twoStar);
            rateView.AddSubview(threeStar);
            rateView.AddSubview(fourStar);
            rateView.AddSubview(fiveStar);
        }

        private void SetBlankStarRating()
        {
            oneStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            twoStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            threeStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            fourStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            fiveStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
        }


        private void SetOneStarRating()
        {
            oneStar.SetImage(UIImage.FromFile(IconStarRed), UIControlState.Normal);
            twoStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            threeStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            fourStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            fiveStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
        }

        private void SetTwoStarRating()
        {
            oneStar.SetImage(UIImage.FromFile(IconStarRed), UIControlState.Normal);
            twoStar.SetImage(UIImage.FromFile(IconStarRed), UIControlState.Normal);
            threeStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            fourStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            fiveStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
        }

        private void SetThreeStarRating()
        {
            oneStar.SetImage(UIImage.FromFile(IconStarOrange), UIControlState.Normal);
            twoStar.SetImage(UIImage.FromFile(IconStarOrange), UIControlState.Normal);
            threeStar.SetImage(UIImage.FromFile(IconStarOrange), UIControlState.Normal);
            fourStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
            fiveStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
        }

        private void SetFourStarRating()
        {
            oneStar.SetImage(UIImage.FromFile(IconStarYellow), UIControlState.Normal);
            twoStar.SetImage(UIImage.FromFile(IconStarYellow), UIControlState.Normal);
            threeStar.SetImage(UIImage.FromFile(IconStarYellow), UIControlState.Normal);
            fourStar.SetImage(UIImage.FromFile(IconStarYellow), UIControlState.Normal);
            fiveStar.SetImage(UIImage.FromFile(IconStarBlank), UIControlState.Normal);
        }

        private void SetFiveStarRating()
        {
            oneStar.SetImage(UIImage.FromFile(IconStarGreen), UIControlState.Normal);
            twoStar.SetImage(UIImage.FromFile(IconStarGreen), UIControlState.Normal);
            threeStar.SetImage(UIImage.FromFile(IconStarGreen), UIControlState.Normal);
            fourStar.SetImage(UIImage.FromFile(IconStarGreen), UIControlState.Normal);
            fiveStar.SetImage(UIImage.FromFile(IconStarGreen), UIControlState.Normal);
        }
    }
}