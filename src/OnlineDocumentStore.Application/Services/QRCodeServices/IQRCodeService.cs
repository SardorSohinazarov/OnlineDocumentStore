using System.Drawing;

namespace OnlineDocumentStore.Application.Services.QRCodeServices
{
    public interface IQRCodeService
    {
        ValueTask<Bitmap> GenerateQRCodeAsync(string qrCode);
    }
}
