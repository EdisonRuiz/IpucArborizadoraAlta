using IPUC.AA.Back.Models;

namespace Presentation.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<DebitResponseModel> AddDebit(DebitModel model);
        public Task<PaymentModel> GetDebit(Guid idPayment);
        public Task<PaymentModel> GetTotalDebits(int documentNumber);
        public Task<List<PaymentModel>> GetAllTotalDebits();
        public Task<PaymentTotalModel> GetAllTotals();
    }
}
