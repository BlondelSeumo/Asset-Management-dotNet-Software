using System;

namespace AMS.Models
{
    public class SubscriptionRequest: EntityBase
    {
        public Int64 Id { get; set; }
        public string Email { get; set; }
        public string TimeZone { get; set; }
    }
}
