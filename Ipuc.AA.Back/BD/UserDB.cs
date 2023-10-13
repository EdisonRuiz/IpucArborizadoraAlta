using IPUC.AA.Back.DataBase;
using IPUC.AA.Back.DataBase.Entities;
using IPUC.AA.Back.Models;
using Microsoft.EntityFrameworkCore;

namespace IPUC.AA.Back.BD
{
    public class UserDB
    {
        private readonly DBContext _dbContext;

        public UserDB(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<bool> AddUserAsync(User entity)
        {
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GetUserAsync(int documentNumber)
        {
            var entities = await _dbContext.Users.Where(x => x.Id == documentNumber).ToListAsync();
            return entities.Any();
        }

        public async Task<Int64> GetUserPhoneAsync(int documentNumber)
        {
            var entities = await _dbContext.Users.Where(x => x.Id == documentNumber).ToListAsync();
            User? entity = entities.FirstOrDefault();
            if(entity != null)
                return entity.Phone;

            return default;
        }

        public async Task<User> GetUserInfoAsync(int documentNumber)
        {
            var entities = await _dbContext.Users.Where(x => x.Id == documentNumber).ToListAsync();
            return entities.FirstOrDefault();
        }

        public async Task<bool> UpdateUserAsync(AddUserModel model)
        {
            var entities = await _dbContext.Users.Where(x => x.Id == model.DocumentNumber).ToListAsync();
            var entity = entities.FirstOrDefault();

            if(model.Phone != default(long))
                entity.Phone = model.Phone;
            if(model.CampSpace != default(byte))
                entity.CampSpace = model.CampSpace;
            if (model.TypeTransportId != default(byte))
                entity.TypeTransportId = (byte)model.TypeTransportId;
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
