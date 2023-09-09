using IPUC.AA.Back.Models;

namespace IPUC.AA.Back.BL.Interfaces
{
    public interface IPaymentBL
    {
        Task<DebitResponseModel> AddDebitAsync(DebitModel model);
        Task<PaymentModel> GetDebit(Guid idPayment);
        Task<PaymentModel> GetTotalDebit(int documentNumber);
        Task<List<PaymentModel>> GetAllTotalDebits();
    }
}
