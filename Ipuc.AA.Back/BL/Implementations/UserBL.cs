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
            bool isCreated = await _userDB.AddUserAsync(new User
            {
                Id = model.DocumentNumber,
                Name = model.Name,
                Phone = model.Phone,  
                CampSpace = model.CampSpace,
                TypeTransportId = (byte)model.TypeTransportId
            });

            return new AddUserResponseModel()
            {
                IsCreated = isCreated,
                Message = "Ok"
            };
        }

        public async Task<AddUserModel> GetUser(int documentNumber)
        {
            var model  = await _userDB.GetUserInfoAsync(documentNumber);
            if (model == null)
                return new AddUserModel();
            else
            return new AddUserModel()
            {
                CampSpace = model.CampSpace,
                DocumentNumber = documentNumber,
                Name = model.Name,
                Phone = model.Phone,
                TypeTransportId = (TypeTransports)model.TypeTransportId
            };
        }

        public async Task<AddUserResponseModel> UpdateUser(AddUserModel model)
        {
            bool isUpdate = await _userDB.UpdateUserAsync(model);
            return new AddUserResponseModel
            {
                IsCreated = isUpdate,
                Message = "OK"
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
