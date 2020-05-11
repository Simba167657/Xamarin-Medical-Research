using Newtonsoft.Json;
using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class GoogleMapsApiService
    {
         string _googleMapsKey= "AIzaSyBzdtok4lk__zTykbElAp4fNgvgoYMyILg";

        //public static void Initialize(string googleMapsKey)
        //{
        //    _googleMapsKey = googleMapsKey;
        //}

        public async Task<GoogleDirection> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude)
        {
            GoogleDirection googleDirection = new GoogleDirection();

            using (var httpClient = CreateClient())
            {
                var endpoint = $"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={originLatitude},{originLongitude}&destination={destinationLatitude},{destinationLongitude}&key={_googleMapsKey}";
              //  endpoint = "api/directions/json?mode=driving&transit_routing_preference=less_driving&origin=28.644800,77.216721&destination=19.076090,72.877426&key=AIzaSyBzdtok4lk__zTykbElAp4fNgvgoYMyILg";


                var response = await httpClient.GetAsync(endpoint).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        googleDirection = await Task.Run(() =>
                           JsonConvert.DeserializeObject<GoogleDirection>(json)
                        ).ConfigureAwait(false);

                    }
                }
            }

            return googleDirection;
        }


        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
