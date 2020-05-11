using SJMC.Helpers;
using SJMC.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
        Position HospitalPosition;

        public LocationPage()
        {
            InitializeComponent();
            HospitalPosition = new Position(33.881437, 35.520532);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await PermissionUtils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Location);
            await PermissionUtils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.LocationAlways);
            await PermissionUtils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.LocationWhenInUse);

            var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
            if (location == null) return;

            Device.BeginInvokeOnMainThread(() =>
            {

                map.Pins.Add(new Pin()
                {
                    Position = HospitalPosition,
                    IsDraggable = false,
                    Type = PinType.SavedPin,
                    Label = "St. Joseph Medical Center - Achrafieh",
                    Address= "St. Joseph Medical Center - Achrafieh",
                    

                });

                map.Pins.Add(new Pin()
                {
                    Position = new Position(location.Latitude, location.Longitude),
                    Address = "Your Location",
                    Label = "Your Location",
                });
                map.MoveCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(HospitalPosition.Latitude, HospitalPosition.Longitude), 18d, 0d, 0d)));
                map.UiSettings.RotateGesturesEnabled = false;
            });

            await LoadRoute(location.Latitude.ToString(), location.Longitude.ToString(), HospitalPosition.Latitude.ToString(), HospitalPosition.Longitude.ToString());


        }

        public async Task LoadRoute(string originLat, string originLng, string endLat, string endLng)
        {
            //endLat = "28.644800";
            //endLng = "77.216721";
            try
            {
                var googleDirection = await new GoogleMapsApiService().GetDirections(originLat, originLng, endLat, endLng);
                if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
                {
                    var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                    var polyline = new Polyline();
                    polyline.StrokeColor = Color.Blue;
                    polyline.StrokeWidth = 5f;

                    foreach (var item in positions)
                    {
                        polyline.Positions.Add(new Position(item.Latitude, item.Longitude));
                    }
                    map.Polylines.Add(polyline);
                    return;
                }
            }
            catch (Exception ex)
            {
                //
            }
            await MaterialDialog.Instance.AlertAsync("No Suitable path exist !");
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}