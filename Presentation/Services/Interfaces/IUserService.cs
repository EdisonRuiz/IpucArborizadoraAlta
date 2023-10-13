using IPUC.AA.Back.Models;

namespace Presentation.Services.Interfaces
{
    public interface IUserService
    {
        Task<AddUserResponseModel> AddUser(AddUserModel model);
        Task<AddUserResponseModel> ValidateUser(int documentNumber);
        Task<AddUserModel> GetUser(int documentNumber);
        Task<AddUserResponseModel> UpdateUser(AddUserModel model);
    }
}