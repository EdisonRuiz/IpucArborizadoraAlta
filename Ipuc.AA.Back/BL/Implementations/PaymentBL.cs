using IPUC.AA.Back.BD;
using IPUC.AA.Back.BL.Interfaces;
using IPUC.AA.Back.DataBase.Entities;
using IPUC.AA.Back.Models;
using IronBarCode;

namespace IPUC.AA.Back.BL.Implementations
{
    public class PaymentBL : IPaymentBL
    {
        private readonly PaymentBD _paymentBD;
        private readonly UserDB _userDB;

        public PaymentBL(PaymentBD paymentBD,
            UserDB userDB)
        {
            _paymentBD = paymentBD;
            _userDB = userDB;
        }

        public async Task<DebitResponseModel> AddDebitAsync(DebitModel model)
        {
            Guid guidPayment = Guid.NewGuid();
            bool isUserCreated = await _userDB.GetUserAsync(model.DocumentNumber);
            if (!isUserCreated)
                return BadResponse("El usuario no existe en base de datos!");
            bool isCreated = await _paymentBD.AddPaymentAsync(new Payment
            {
                DateCreated = DateTime.Now,
                Id = guidPayment,
                UserId = model.DocumentNumber,
                Value = model.Value
            });
            if (!isCreated)
                return BadResponse("Ocurrio un error al guardar el pago!");

            long phone = await _userDB.GetUserPhoneAsync(model.DocumentNumber);
            string image = GenerateCodeQR(model, guidPayment);
            return new DebitResponseModel
            {
                IsOK = true,
                Image = image,
                Phone = phone
            };
        }

        private static string GenerateCodeQR(DebitModel model, Guid guidPayment)
        {
            // You may add styling with color, logo images or branding:
            QRCodeLogo qrCodeLogo = new QRCodeLogo("wwwroot/images/logoIpuc.png");
            GeneratedBarcode myQRCodeWithLogo = QRCodeWriter.CreateQrCodeWithLogo($"Usted realizo un pago para el campamento por valor de: $ {model.Value}\r\nLlave de seguridad: {guidPayment}\r\n", qrCodeLogo);
            myQRCodeWithLogo.ResizeTo(500, 500).SetMargins(10);
            // Logo will automatically be sized appropriately and snapped to the QR grid.
            //myQRCodeWithLogo.SaveAsPng("myQRWithLogo.png");
            string image = Convert.ToBase64String(myQRCodeWithLogo.Image.ExportBytes());
            return image;
        }

        private static DebitResponseModel BadResponse(string message)
        {
            return new DebitResponseModel()
            {
                IsOK = false,
                Message = message
            };
        }
        public async Task<PaymentModel> GetDebit(Guid idPayment)
        {
            var entity = await _paymentBD.GetPaymentAsync(idPayment);
            if (entity.UserId == default(int))
                return new PaymentModel();

            return new PaymentModel()
            {
                DocumentNumber = entity.UserId,
                Name = entity.User.Name,
                TotalDebit = 1,
                Value = entity.Value
            };
        }

        public async Task<PaymentModel> GetTotalDebit(int documentNumber)
        {
            var entities = await _paymentBD.GetPaymentsAsync(documentNumber);
            if (!entities.Any())
                return new PaymentModel();

            return new PaymentModel()
            {
                DocumentNumber = entities.FirstOrDefault().UserId,
                Name = entities.FirstOrDefault().User.Name,
                TotalDebit = entities.Count,
                Value = entities.Sum(x => x.Value)
            };
        }

        public async Task<List<PaymentModel>> GetAllTotalDebits()
        {
            var entities = await _paymentBD.GetAllPaymentsAsync();
            List<PaymentModel> response = new List<PaymentModel>();
            response = entities.GroupBy(x => x.UserId).Select(item => new PaymentModel()
            {
                DocumentNumber = item.First().UserId,
                Name = item.First().User.Name,
                TotalDebit = item.Count(),
                Value = item.Sum(x => x.Value)
            }).ToList();
            return response.Take(10).ToList();

        }
    }
}
