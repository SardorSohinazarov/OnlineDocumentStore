using System.Drawing;

namespace OnlineDocumentStore.Application.Services
{
    public interface IQRCodeService
    {
        ValueTask<Bitmap> GenerateQRCodeAsync(string qrCode);
    }
}
