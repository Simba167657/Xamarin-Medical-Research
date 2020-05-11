using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.Models
{

    public class ImageUploadApiModel : BaseModel
    {
        public string data { get; set; }
    }

    public class ProfilePicModel
    {
        public string LabId { get; set; }
        public byte[] ImageData { get; set; }
    }

    public class ProfilePicResponseModel : ResponseModel
    {
        public byte[] ImageData { get; set; }
    }

    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
