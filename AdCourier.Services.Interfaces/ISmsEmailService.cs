using AdCourier.Domain.Entities.BodyModel.Permission;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface ISmsEmailService
    {
        Task<bool> SmsSend(dynamic listOfData);
        Task<dynamic> SendSMSInfobip(InfobipSMSBodyModel request);
    }
}
