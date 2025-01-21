using System.Collections.Generic;

namespace AMS.Models.DashboardViewModel
{
    public class DashboardDataViewModel
    {
        public DashboardSummaryViewModel DashboardSummaryViewModel { get; set; }
        public List<UserProfile> UserProfileList { get; set; }
    }
}
