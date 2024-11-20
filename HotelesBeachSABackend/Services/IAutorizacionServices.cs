using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Models.Custom;

namespace HotelesBeachSABackend.Services
{
    public interface IAutorizacionServices
    {
        Task<AutorizacionResponse> DevolverToken(Usuario autorizacion);
    }
}
