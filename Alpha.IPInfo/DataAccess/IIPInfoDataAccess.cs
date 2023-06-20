using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.IPInfo.Models;

namespace Alpha.IPInfo.DataAccess
{
    public interface IIPInfoDataAccess
    {
        Task<IEnumerable<IPV4Information>> GetIPV4InfoByTopCountries(string countries);

        Task<IEnumerable<IPV4Information>> GetIPV4Info(string ipList);

        Task<IEnumerable<IPV6Information>> GetIPV6Info(string ipList);
    }
}