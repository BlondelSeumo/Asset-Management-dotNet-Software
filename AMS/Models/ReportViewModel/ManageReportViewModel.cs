using AMS.Models.CompanyInfoViewModel;
using System.Collections.Generic;

namespace AMS.Models.ReportViewModel
{
    public class ManageReportViewModel
    {
        public List<AllocationViewModel> AllocationViewModel { get; set; }
        public AssetStatusViewModel AssetStatusViewModel { get; set; }
        public CompanyInfoCRUDViewModel CompanyInfoCRUDViewModel { get; set; }
    }
}
