using IPUC.AA.Back.BL.Interfaces;
using IPUC.AA.Back.Models;
using Presentation.Services.Interfaces;

namespace Presentation.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentBL _paymentBL;

        public PaymentService(IPaymentBL paymentBL)
        {
            _paymentBL = paymentBL;
        }

        public async Task<DebitResponseModel> AddDebit(DebitModel model) => await _paymentBL.AddDebitAsync(model);

        public async Task<List<PaymentModel>> GetAllTotalDebits() => await _paymentBL.GetAllTotalDebits();

        public async Task<PaymentModel> GetDebit(Guid idPayment) => await _paymentBL.GetDebit(idPayment);

        public async Task<PaymentModel> GetTotalDebits(int documentNumber) => await _paymentBL.GetTotalDebit(documentNumber);
    }
}
