using Newtonsoft.Json;
using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SJMC.ServiceProviders
{
    public class AuthServices : ApiBase
    {
        public async Task<UserProfileModel> Login(string userInfo, string password)
        {
            try
            {
                var endPoint = String.Format(LoginKey, userInfo, password);
                var result = await HttpClientBase.Get<UserProfileModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new UserProfileModel() { Message = ex.Message };

            }

        }

        public async Task<BaseModel> UpdateProfile(string role, string userId, string name, string email, string phoneNumber, string password, bool receiveNotification)
        {
            try
            {
                var endPoint = String.Format(UpdateProfileKey, role, userId, name, email, phoneNumber, password, receiveNotification);
                var result = await HttpClientBase.Get<BaseModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new UserProfileModel() { Message = ex.Message };

            }

        }

        public async Task<BaseModel> UpdateRating(string role, string userId, string rating)
        {
            try
            {
                var endPoint = String.Format(UpdateAppRatingKey, role, userId, rating);
                var result = await HttpClientBase.Get<BaseModel>(endPoint);
                return result;
            }
            catch (Exception ex)
            {
                return new UserProfileModel() { Message = ex.Message };

            }

        }

        public async Task<ProfilePicResponseModel> PostFileUpload(string name, byte[] byteArr)
        {
            try
            {
                var endPoint = String.Format(PostFileUploadKey);
                ProfilePicModel obj = new ProfilePicModel
                {
                    LabId = $"{name}",
                    ImageData = (byteArr)
                };
                var result = await HttpClientBase.Post<ProfilePicResponseModel>(endPoint, JsonConvert.SerializeObject(obj));
                return result;
            }
            catch (Exception ex)
            {
                return new ProfilePicResponseModel() { Message = ex.Message };

            }

        }

        public async Task<ProfilePicResponseModel> Download(string endPoint)
        {
            try
            {
                ProfilePicResponseModel result = await HttpClientBase.Get<ProfilePicResponseModel>(endPoint);

                return result;
            }
            catch (Exception ex)
            {
                return null;

            }

        }

        public async Task<ProfilePicResponseModel> CheckAppVesion(string endPoint)
        {
            try
            {
                ProfilePicResponseModel result = await HttpClientBase.Get<ProfilePicResponseModel>(endPoint);

                return result;
            }
            catch (Exception ex)
            {
                return null;

            }

        }


    }
}
