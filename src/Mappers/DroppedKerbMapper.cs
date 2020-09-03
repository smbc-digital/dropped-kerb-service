using dropped_kerb_service.Models;
using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Models.Verint;
using System;
using System.Text;

namespace dropped_kerb_service.Mappers
{
    public static class DroppedKerbMapper
    {
        public static Case ToCase(this DroppedKerbRequest model,
          ConfirmIntegrationFormOptions _VOFConfiguration,
          VerintOptions _verintOptions)
        {
            var crmCase = new Case
            {
                EventCode = _VOFConfiguration.EventId,
                EventTitle = _verintOptions.EventTitle,
                Classification = _verintOptions.Classification,
                RaisedByBehaviour = RaisedByBehaviourEnum.Individual,
                FurtherLocationInformation = model.FurtherLocationDetails,
                Description = GenerateDescription(model),
                Customer = new Customer
                {
                    Forename = model.FirstName,
                    Surname = model.LastName,
                    Email = model.ContactPreference == "Email" ? model.Email : model.EmailOptional,
                    Telephone = model.ContactPreference == "phone" ? model.Phone : model.PhoneOptional,
                    Address = new Address
                    {
                        AddressLine1 = model.CustomersAddress.AddressLine1,
                        AddressLine2 = model.CustomersAddress.AddressLine2,
                        AddressLine3 = model.CustomersAddress.Town,
                        Postcode = model.CustomersAddress.Postcode,
                        Reference = model.CustomersAddress.PlaceRef,
                        Description = model.CustomersAddress.ToString()
                    }
                }
            };

            if (!string.IsNullOrWhiteSpace(model.StreetAddressDroppedKerb?.PlaceRef))
            {
                crmCase.AssociatedWithBehaviour = AssociatedWithBehaviourEnum.Street;
                crmCase.Street = new Street
                {
                    Reference = model.StreetAddressDroppedKerb.PlaceRef,
                    Description = model.StreetAddressDroppedKerb.ToString()
                };
            }

            return crmCase;
        }

        private static string GenerateDescription(DroppedKerbRequest kerbRequest)
        {
            StringBuilder description = new StringBuilder();

            if (!string.IsNullOrEmpty(kerbRequest.PlanningPermission))
                description.Append($"No. Of Posters: {kerbRequest.PlanningPermission}{Environment.NewLine}");

            if (!string.IsNullOrEmpty(kerbRequest.PlanningReference))
                description.Append($"Poster Description: {kerbRequest.PlanningReference}{Environment.NewLine}");

            if (!string.IsNullOrEmpty(kerbRequest.DischargeReference))
                description.Append($"Image Or Photo: {kerbRequest.DischargeReference}{Environment.NewLine}");

            if (!string.IsNullOrEmpty(kerbRequest.LocationDetails))
                description.Append($"Contact Preference: {kerbRequest.LocationDetails}{Environment.NewLine}");

            if (!string.IsNullOrEmpty(kerbRequest.PropertyOwner))
                description.Append($"No. Of Posters: {kerbRequest.PlanningPermission}{Environment.NewLine}");

            if (!string.IsNullOrEmpty(kerbRequest.AccessFor))
                description.Append($"Poster Description: {kerbRequest.PlanningReference}{Environment.NewLine}");

            if (!string.IsNullOrEmpty(kerbRequest.RedundantAccessDetails))
                description.Append($"Image Or Photo: {kerbRequest.DischargeReference}{Environment.NewLine}");

            return description.ToString();
        }
    }
}
