using System.ComponentModel.DataAnnotations;

namespace AMS.Models.SubscriptionRequestViewModel
{
    public class SubscriptionRequestCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Email"), Required]
        public string Email { get; set; }
        [Display(Name = "Time Zone")]
        public string TimeZone { get; set; }

        public static implicit operator SubscriptionRequestCRUDViewModel(SubscriptionRequest _SubscriptionRequest)
        {
            return new SubscriptionRequestCRUDViewModel
            {
                Id = _SubscriptionRequest.Id,
                Email = _SubscriptionRequest.Email,
                TimeZone = _SubscriptionRequest.TimeZone,
                CreatedDate = _SubscriptionRequest.CreatedDate,
                ModifiedDate = _SubscriptionRequest.ModifiedDate,
                CreatedBy = _SubscriptionRequest.CreatedBy,
                ModifiedBy = _SubscriptionRequest.ModifiedBy,
                Cancelled = _SubscriptionRequest.Cancelled,
            };
        }

        public static implicit operator SubscriptionRequest(SubscriptionRequestCRUDViewModel vm)
        {
            return new SubscriptionRequest
            {
                Id = vm.Id,
                Email = vm.Email,
                TimeZone = vm.TimeZone,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
