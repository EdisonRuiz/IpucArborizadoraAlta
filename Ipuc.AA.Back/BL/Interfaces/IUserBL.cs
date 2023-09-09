using IPUC.AA.Back.Models;

namespace IPUC.AA.Back.BL.Interfaces
{
    public interface IUserBL
    {
        Task<AddUserResponseModel> AddUserAsync(AddUserModel model);
        Task<AddUserResponseModel> ValidateUserAsync(int documentNumber);
    }
}
