using IPUC.AA.Back.BL.Interfaces;
using IPUC.AA.Back.Models;
using Presentation.Services.Interfaces;

namespace Presentation.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserBL _userBL;

        public UserService(
            IUserBL userBL)
        {
            _userBL = userBL;
        }

        public async Task<AddUserResponseModel> AddUser(AddUserModel model) => await _userBL.AddUserAsync(model);

        public async Task<AddUserResponseModel> ValidateUser(int documentNumber) => await _userBL.ValidateUserAsync(documentNumber);
    }
}
