using IPUC.AA.Back.BD;
using IPUC.AA.Back.BL.Interfaces;
using IPUC.AA.Back.DataBase.Entities;
using IPUC.AA.Back.Models;
using QRCoder;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

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
            string image = GenerateCodeQR(model.Value, guidPayment.ToString());
            return new DebitResponseModel
            {
                IsOK = true,
                Phone = phone,
                Image = image
            };
        }

        private string GenerateCodeQR(int value, string guidPayment)
        {
            string filePath = GenerateImageTemp();
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode($"Comprobante de pago, valido para el campamento por valor de: $ {value}\r\nLlave de seguridad: {guidPayment}\r\n", QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap logoImage = new Bitmap(filePath);
            //Bitmap qrCodeAsBitmap = QrCode.GetGraphic(60);
            Bitmap qrCodeAsBitmap = QrCode.GetGraphic(60, Color.Black, Color.White, logoImage);
            byte[] BitmapArray = BitmapToByteArray(qrCodeAsBitmap);
            return Convert.ToBase64String(BitmapArray);
        }


        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static string GenerateImageTemp()
        {
            string filePath = "Temp";
            bool exists = Directory.Exists(filePath);

            if (!exists)
                Directory.CreateDirectory(filePath);

            filePath = "Temp/logo.png";

            if (!Directory.Exists(filePath))
            {
                using (var stream = File.Create("logo.png"))
                {
                    File.WriteAllBytes(filePath, Convert.FromBase64String(GetImageBase64()));
                }
            }
            return filePath;
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
                CampSpace = entity.User.CampSpace,
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
                CampSpace = entities.FirstOrDefault().User.CampSpace,
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
                CampSpace = item.First().User.CampSpace,
                Name = item.First().User.Name,
                TotalDebit = item.Count(),
                Value = item.Sum(x => x.Value)
            }).ToList();
            return response.Take(10).ToList();

        }

        public static string GetImageBase64()
        {
            return "iVBORw0KGgoAAAANSUhEUgAAASwAAADTCAMAAAAMEN4/AAAC9FBMVEVHcEz9/f39/f39/f39/f39/f39/f39/f39/f39/f39/f39/f39/f39/f39/v39/f39/f39/f3///////7+/v6mnHr8/Pz5+/zw9fnn7/Xi6/P9/Pz9wDLO3uu3zuOau9d+p8xnlsJMhLhAfLMucKwjaKcdZKUSXKAMV547eLGxyuD9/f3r8vfJ2umhvtlxoMgobKoAUJkARpQAQ5IARZMAR5UASpYATJgATplGgLbY5fClwtptm8UXYKMAUZqrxd0HUpr9/f2Frc+NsdIAQZEAUpo1dK/d6PH+/v5Zjb1Sibr9/f2VttUEVJyqxt5gksB4osn1+PvT4u6fvtnD1uf9/f280uUAOIwAPo/8/PwCVZ4FW6IAT5wEWaEDV58HX6UYaqoufbcGXaQJYqcJZKlPksNgnsoLZ6sMaawPbK8RcLINaq0QbrCOvt0+icAAV6YSc7QFcLQCabATdbUWermButslg72ayOP9//8UdrYVeLgui8Q/mMoXfLthqtVys9gLd7sYfrwZgL6r0um33O8agb8cg8HF4vFQoM8dhcIOgcIfisYeiMQhjMgkkswViMcijskhkMsllM4nl9AomtIpnNQrn9YsotgtpNr9/v4wp9wxqt4xrOAyreE0ruI1sOM2s+V4xeg4tucfltI5uOlaueM7u+s9v+0/wvFmp6C8pTv/vxGOlVrosRz9sgL/ugf+8Mz24afNqC79txD7y1T8wzVPl6P/ug/62IT9+u799uMDbb0AW68rd55KgIk1aIBRdHJ6iF6omUQASqQZWoz/vgT8uBD/whD/xRH8uA78uA/9uA/9uA/9uQ/9uQ79uQ/9uQ/9uQ/9uA79uQ/9uQ39uQ/9uQ/9uQ/9uA/9uA/8uA/9uQ79uA/9uQ39uA/8uQ/9uA79uQ39uQv8uQ78uA79uQ/8uA//zRL/1BL/xxHUqiYKU5YAUZwAUJoAUZoAUJsAUJoAUJoAUpwAUZoAUZoAUp0AU58AUZoAUZoAUZsAUZwAUJv+f0OjAAAA/HRSTlMAESA3UGZ7kqnF1OLv+P9eibL///0DLP////+dB///////////////////Q////P////3////////////////8/nb///////9s//9y////////////uf///3X///////////////////////////////////////////////////////////////////////////////////+B//////////////////////7//v7///////////////////////37/v7//f/6///z38ytmX9nVkdBOjMshqPCj9VeHCO36VAOE3Zzbnr///+hQxAngQlVNsehk+b2brXZ8BpdvKhoAAA/DUlEQVR4AeyQgxUDURAA79t2nPTfY6wOwlnv80zvAYAIE8q4kEobe8JoJQVnlGAEwfTnogk5H6S2McUjKZ/HhXybWgbv0G8rK9UxoZ/l5NT6GLMTY3ST8vF3R4u5q4ufNFUJV3dLeSxX6812dyDXzP/TNgIl/mnu2K7Sjkg58rgFEgRq5F3tLgJUigSqKAKEieH1+v//jKcVGGNe759wOokvbr7MjPZQVavVdWMnvV7TGpVyqdjMZ1MHZpdvXn7zH4vex68uUkiCl2q1v72udHTzhhBCLcY450Iq/smYRSkhN0LvdK+/bbf2xFIXrz9+8V8h9eLD3lIpu9dvDHgMiXHh/LEEZzEzrjf6PXvvscsPL/4DvD6+3pFK5b4ra8MbYnHT+XsyuUVuhlq5l7vn9fGzJvXs+4sdqfyoatwQJpx/KsHIjVEd5Xe8Lr5/9tma6s3VjlRJE8QSzr8Vt4jQSjteV28+R3u9f/d2lz63IbzfJWWKJJBjixA+dg5iv5dT0/LExPUTXm/fvf/MSv2HiwRVITD+v6dEfIFJKTeSK/i0VOow56Ca/vutJixiBIUE18UPX3xGqH5MSj07024oP+0gSnjoiHFp3s5VaXwJLQALcrg+9FvtOk/YWOPTxqc3mptNyv7HzwTX+5cSVcpfDoh1WtZE1KZuu0RZHbEKCaweUD7AYvU0Ip07bNIsdXmcUfrYlzdEXyZpvHz5OYTxxYVEtSoPvVNThZ1FMR8B8AWtIh1B0WKa9PYYFu0Ca2Y6pAw0qcOCypCe1L03LK8krosXTx3V80/J4HMxJEeoTEYIt7q+AgBKpu2GXhlZFyjGkEgR6B9gkf7+UhdYEhG24Je4JH2MiwzLtsT16fmTLqvXCoBcf0iO3xxlWn9UIxqASEFT59QkM2TrOUR15pDZMSy6Q2eyAtCltAsVkSErjLDxI1z9HIDU66dbXe8uJY+R/jiAtHqnAIEXzpZa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";
        }
    }
}
