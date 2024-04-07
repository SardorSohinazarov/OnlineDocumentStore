using System.Drawing;

namespace OnlineDocumentStore.API.Services
{
    public interface IQRCodeService
    {
        ValueTask<Bitmap> GenerateQRCode(string qrCode);
    }
}
