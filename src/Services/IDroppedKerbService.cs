using System.Threading.Tasks;
using dropped_kerb_service.Models;

namespace dropped_kerb_service.Services
{
    public interface IDroppedKerbService
    {
        Task<string> CreateCase(DroppedKerbRequest kerbRequest);
    }
}