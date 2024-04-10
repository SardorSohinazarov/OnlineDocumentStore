using QRCoder;
using System.Drawing;

namespace OnlineDocumentStore.Application.Services.QRCodeServices
{
    public class QRCodeService : IQRCodeService
    {
        public async ValueTask<Bitmap> GenerateQRCodeAsync(string qrCodeText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }
    }
}
