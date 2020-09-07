using dropped_kerb_service.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockportGovUK.NetStandard.Gateways.MailingService;
using StockportGovUK.NetStandard.Models.Addresses;
using StockportGovUK.NetStandard.Models.Enums;
using StockportGovUK.NetStandard.Models.Mail;
using StockportGovUK.NetStandard.Models.Models.StreetReport;

namespace dropped_kerb_service.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IMailingServiceGateway _mailingServiceGateway;
        private readonly ILogger<MailHelper> _logger;
        public MailHelper(IMailingServiceGateway mailingServiceGateway,
                          ILogger<MailHelper> logger)
        {
            _mailingServiceGateway = mailingServiceGateway;
            _logger = logger;
        }

        public void SendEmail(Person person, EMailTemplate template, string caseReference, Address street)
        {
            StreetReportMailModel submissionDetails = new StreetReportMailModel();
            _logger.LogInformation(caseReference, street, person);
            submissionDetails.Subject = "Dropped kerb request - submission";
            submissionDetails.Reference = caseReference;
            submissionDetails.FormType = FormType.request;
            submissionDetails.ConcerningDescription = "a dropped kerb";
            submissionDetails.StreetInput = street.SelectedAddress;
            submissionDetails.RecipientAddress = person.Email;

            _mailingServiceGateway.Send(new Mail
            {
                Payload = JsonConvert.SerializeObject(submissionDetails),
                Template = template
            });
        }
    }
}