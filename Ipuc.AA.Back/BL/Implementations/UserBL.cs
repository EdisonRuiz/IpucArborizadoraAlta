using IPUC.AA.Back.BD;
using IPUC.AA.Back.BL.Interfaces;
using IPUC.AA.Back.DataBase.Entities;
using IPUC.AA.Back.Models;

namespace IPUC.AA.Back.BL.Implementations
{
    public class UserBL : IUserBL
    {
        private readonly UserDB _userDB;

        public UserBL(UserDB userDB) 
        {
            _userDB = userDB;
        }

        public async Task<AddUserResponseModel> AddUserAsync(AddUserModel model)
        {
            if(model?.DocumentNumber == default(int))
                return new AddUserResponseModel()
                {
                    IsCreated = false,
                    Message = "Data vacia!"
                };
            bool isUserCreated = await _userDB.GetUserAsync(model.DocumentNumber);
            if (isUserCreated)
                return new AddUserResponseModel()
                {
                    IsCreated = false,
                    Message = "El usuario ya existe en base de datos!"
                };
            bool esCreated = await _userDB.AddUserAsync(new User
            {
                Id = model.DocumentNumber,
                Name = model.Name,
                Phone = model.Phone,    
            });

            return new AddUserResponseModel()
            {
                IsCreated = esCreated,
                Message = "Ok"
            };
        }

        public async Task<AddUserResponseModel> ValidateUserAsync(int documentNumber)
        {
            bool isUserCreated = await _userDB.GetUserAsync(documentNumber);
            return new AddUserResponseModel()
            {
                IsCreated = isUserCreated,
                Message = isUserCreated ? "El usuario ya existe en base de datos!" : "El usuario no existe en base de datos!"
            };
        }
    }
}
