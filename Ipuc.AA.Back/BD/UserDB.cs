using IPUC.AA.Back.DataBase;
using IPUC.AA.Back.DataBase.Entities;
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

    }
}
