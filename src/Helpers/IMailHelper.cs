using dropped_kerb_service.Models;
using StockportGovUK.NetStandard.Models.Enums;
using StockportGovUK.NetStandard.Models.Addresses;

namespace dropped_kerb_service.Helpers
{
    public interface IMailHelper
    {
        void SendEmail(Person person, EMailTemplate template, string caseReference, Address street);
    }
}
