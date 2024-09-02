using Microsoft.EntityFrameworkCore;
using shopapp.Domain.Interface;
using shopapp.Infrastructure;
using shopapp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Veritabanı bağlantısını yapılandırıyoruz.
// "ApplicationDBContext" sınıfını kullanarak veritabanı işlemlerini yönetiyoruz.
// "DefaultConnection" adında, "appsettings.json" dosyasında tanımlı olan bağlantı cümlesi kullanılıyor.

builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});


// IStockRepository interface'ini StockRepository ile ilişkilendiriyoruz.
// Bu sayede "IStockRepository" talep edildiğinde "StockRepository" sınıfı enjekte edilecek.

builder.Services.AddScoped<IStockRepository, StockRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS yönlendirmeyi aktifleştiriyoruz.
// HTTP taleplerini güvenli olan HTTPS protokolüne yönlendiriyor.
// Eğer bunu yapmazsak dotnet watch run yaptığımızda hata veriyor.
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
