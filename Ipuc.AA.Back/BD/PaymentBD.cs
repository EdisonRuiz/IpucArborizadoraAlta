using IPUC.AA.Back.DataBase;
using IPUC.AA.Back.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPUC.AA.Back.BD
{
    public class PaymentBD
    {
        private readonly DBContext _dbContext;

        public PaymentBD(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<bool> AddPaymentAsync(Payment entity)
        {
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Payment>> GetPaymentsAsync(int documentNumber)
        {
            return await _dbContext.Payments.Where(x => x.UserId == documentNumber).Include(x => x.User).ToListAsync();
        }

        public async Task<Payment> GetPaymentAsync(Guid idPayment)
        {
            List<Payment> entities = await _dbContext.Payments.Where(x => x.Id == idPayment).Include(x => x.User).ToListAsync();
            return entities.FirstOrDefault()?? new Payment();
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _dbContext.Payments.Include(x => x.User).Take(30).ToListAsync();
        }
    }
}
