using IPUC.AA.Back.Models;

namespace IPUC.AA.Back.BL.Interfaces
{
    public interface IUserBL
    {
        Task<AddUserResponseModel> AddUserAsync(AddUserModel model);
        Task<AddUserModel> GetUser(int documentNumber);
        Task<AddUserResponseModel> UpdateUser(AddUserModel model);
        Task<AddUserResponseModel> ValidateUserAsync(int documentNumber);
    }
}
