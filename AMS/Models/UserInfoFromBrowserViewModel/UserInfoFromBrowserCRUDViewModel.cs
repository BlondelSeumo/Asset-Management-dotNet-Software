using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.UserInfoFromBrowserViewModel
{
    public class UserInfoFromBrowserCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public string BrowserUniqueID { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string TimeZone { get; set; }
        public string BrowserMajor { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string CPUArchitecture { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceType { get; set; }
        public string DeviceVendor { get; set; }
        public string EngineName { get; set; }
        public string EngineVersion { get; set; }
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public string UA { get; set; }


        public static implicit operator UserInfoFromBrowserCRUDViewModel(UserInfoFromBrowser _UserInfoFromBrowser)
        {
            return new UserInfoFromBrowserCRUDViewModel
            {
                Id = _UserInfoFromBrowser.Id,
                BrowserUniqueID = _UserInfoFromBrowser.BrowserUniqueID,
                Lat = _UserInfoFromBrowser.Lat,
                Long = _UserInfoFromBrowser.Long,
                TimeZone = _UserInfoFromBrowser.TimeZone,
                BrowserMajor = _UserInfoFromBrowser.BrowserMajor,
                BrowserName = _UserInfoFromBrowser.BrowserName,
                BrowserVersion = _UserInfoFromBrowser.BrowserVersion,
                CPUArchitecture = _UserInfoFromBrowser.CPUArchitecture,
                DeviceModel = _UserInfoFromBrowser.DeviceModel,
                DeviceType = _UserInfoFromBrowser.DeviceType,
                DeviceVendor = _UserInfoFromBrowser.DeviceVendor,
                EngineName = _UserInfoFromBrowser.EngineName,
                EngineVersion = _UserInfoFromBrowser.EngineVersion,
                OSName = _UserInfoFromBrowser.OSName,
                OSVersion = _UserInfoFromBrowser.OSVersion,
                UA = _UserInfoFromBrowser.UA,
                CreatedDate = _UserInfoFromBrowser.CreatedDate,
                ModifiedDate = _UserInfoFromBrowser.ModifiedDate,
                CreatedBy = _UserInfoFromBrowser.CreatedBy,
                ModifiedBy = _UserInfoFromBrowser.ModifiedBy,
                Cancelled = _UserInfoFromBrowser.Cancelled,
            };
        }

        public static implicit operator UserInfoFromBrowser(UserInfoFromBrowserCRUDViewModel vm)
        {
            return new UserInfoFromBrowser
            {
                Id = vm.Id,
                BrowserUniqueID = vm.BrowserUniqueID,
                Lat = vm.Lat,
                Long = vm.Long,
                TimeZone = vm.TimeZone,
                BrowserMajor = vm.BrowserMajor,
                BrowserName = vm.BrowserName,
                BrowserVersion = vm.BrowserVersion,
                CPUArchitecture = vm.CPUArchitecture,
                DeviceModel = vm.DeviceModel,
                DeviceType = vm.DeviceType,
                DeviceVendor = vm.DeviceVendor,
                EngineName = vm.EngineName,
                EngineVersion = vm.EngineVersion,
                OSName = vm.OSName,
                OSVersion = vm.OSVersion,
                UA = vm.UA,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
