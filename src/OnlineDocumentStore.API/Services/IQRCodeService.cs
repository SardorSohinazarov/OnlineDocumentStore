using System.Drawing;

namespace OnlineDocumentStore.API.Services
{
    public interface IQRCodeService
    {
        ValueTask<Bitmap> GenerateQRCodeAsync(string qrCode);
    }
}
