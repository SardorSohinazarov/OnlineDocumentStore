using OnlineDocumentStore.API.Middlewares;
using OnlineDocumentStore.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<IFileService, FileService>()
    .AddScoped<IPDFFileService, PDFFileService>()
    .AddScoped<IQRCodeService, QRCodeService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{*/
app.UseSwagger();
app.UseSwaggerUI();
/*}*/

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
