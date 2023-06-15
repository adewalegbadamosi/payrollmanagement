using System.Threading.Tasks;

using EmployeeService.Dto;

namespace EmployeeService.SyncDataServices.Http
{
    public interface IAuthenticationDataClient
    {
        Task<string?> GetAuthFromAuthenticationService(object auth);
        Task<object?> GetTestUserFromAuthenticationService();
         
    }
}