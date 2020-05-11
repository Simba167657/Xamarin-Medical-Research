using System;
namespace SJMC.Models
{
    public class AppLatestVersionResponseModel
    {
        public bool IsLatestAvailable { get; set; }
        public string VersionDescription { get; set; }
        public string NewVersion { get; set; }
    }
}
