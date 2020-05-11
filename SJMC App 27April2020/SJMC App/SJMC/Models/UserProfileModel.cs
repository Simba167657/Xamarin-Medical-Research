using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.Models
{

    public class UserProfileModel : BaseModel
    {
        public string Role { get; set; }
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserPass { get; set; }
        public string Active { get; set; }
        public string LabID { get; set; }
        public string Phone { get; set; }
        public bool? Notify { get; set; }
    }
}
