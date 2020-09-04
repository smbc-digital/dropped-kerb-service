using StockportGovUK.NetStandard.Models.Addresses;
using System.ComponentModel.DataAnnotations;

namespace dropped_kerb_service.Models
{
    public class DroppedKerbRequest
    {
        [Required]
        public string PlanningPermission { get; set; }
        [Required]
        public string PlanningReference { get; set; }
        public string DischargeReference { get; set; }
        [Required]
        public Address StreetAddressDroppedKerb { get; set; }
        [Required]
        public string FurtherLocationDetails { get; set; }
        [Required]
        public string KerbLocationOther { get; set; }
        [Required]
        public string PropertyOwner { get; set; }
        [Required]
        public string kerbLocation { get; set; }
        [Required]
        public string AccessFor { get; set; }
        [Required]
        public string RedundantAccessDetails { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string ContactPreference { get; set; }
        public string Phone { get; set; }
        public string EmailOptional { get; set; }
        public string Email { get; set; }
        public string PhoneOptional { get; set; }
        public Address CustomersAddress { get; set; }
    }
}
