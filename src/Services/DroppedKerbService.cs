using System;
using System.Threading.Tasks;
using dropped_kerb_service.Helpers;
using dropped_kerb_service.Mappers;
using dropped_kerb_service.Models;
using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Extensions.VerintExtensions.VerintOnlineFormsExtensions.ConfirmIntegrationFromExtensions;
using StockportGovUK.NetStandard.Gateways.VerintService;
using StockportGovUK.NetStandard.Models.Enums;

namespace dropped_kerb_service.Services
{
    public class DroppedKerbService : IDroppedKerbService
    {
        private readonly IVerintServiceGateway _verintServiceGateway;
        private readonly IMailHelper _mailHelper;
        private readonly VerintOptions _verintOptions;
        private readonly ConfirmIntegrationFormOptions _VOFConfiguration;

        public DroppedKerbService(IVerintServiceGateway verintServiceGateway,
                                       IMailHelper mailHelper,
                                       IOptions<VerintOptions> verintOptions,
                                       IOptions<ConfirmIntegrationFormOptions> VOFConfiguration)
        {
            _verintServiceGateway = verintServiceGateway;
            _mailHelper = mailHelper;
            _verintOptions = verintOptions.Value;
            _VOFConfiguration = VOFConfiguration.Value;
        }

        public async Task<string> CreateCase(DroppedKerbRequest kerbRequest)
        {
            var crmCase = kerbRequest
                .ToCase(_VOFConfiguration, _verintOptions);

            var streetResult = await _verintServiceGateway.GetStreet(kerbRequest.StreetAddressDroppedKerb.PlaceRef);

            if (!streetResult.IsSuccessStatusCode)
                throw new Exception("DroppedKerbService.CreateCase: GetStreet status code not successful");

            // confrim uses the USRN for the street,
            // however Verint uses the verint-address-id (Reference) (kerbRequest.StreetAddress.PlaceRef) for streets
            crmCase.Street.USRN = streetResult.ResponseContent.USRN;

            try
            {
                var response = await _verintServiceGateway.CreateVerintOnlineFormCase(crmCase.ToConfirmIntegrationFormCase(_VOFConfiguration));
                if (!response.IsSuccessStatusCode)
                    throw new Exception("DroppedKerbService.CreateCase: CreateVerintOnlineFormCase status code not successful");

                var person = new Person
                {
                    FirstName = kerbRequest.FirstName,
                    LastName = kerbRequest.LastName,
                    Email = kerbRequest.Email,
                    Phone = kerbRequest.Phone
                };

                //_mailHelper.SendEmail(
                //    person, kerbRequest,
                //    EMailTemplate.kerbRequest,
                //    response.ResponseContent.VerintCaseReference,
                //    kerbRequest.StreetAddress);

                return response.ResponseContent.VerintCaseReference;
            }
            catch (Exception ex)
            {
                throw new Exception($"DroppedKerbService.CreateCase: CRMService CreateAbandonedVehicleService an exception has occured while creating the case in verint service", ex);
            }
        }
    }
}
