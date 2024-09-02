using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopapp.Domain.Interface;
using shopapp.Infrastructure;
using shopapp.Application;
using shopapp.Domain.DTOs.Stock;
using shopapp.Resources;

namespace shopapp.API.Controllers
{
    // API'yi "api/stock" rotası ile ilişkilendiriyoruz
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        // Bağımlılıkları (ApplicationDBContext ve IStockRepository) constructor üzerinden ekliyoruz.
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        // Constructor, StockController sınıfının bir instance'ı oluşturulduğunda çalışır
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo; // Business Logic için oluşturuyoruz. 
            _context = context; // Veritabanı işlemleri için kullanılan veritabanı contextini atıyoruz.
        }

        // Tüm stokları almak için kullanılan GET metodu.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Tüm stokları veri tabanından alıyoruz.
            var stocks = await _stockRepo.GetAllAsync();

            // Stokları DTO formatına dönüştürüyoruz. (Burda örnek olarak "Comments"'leri istemediğimiz senaryo için).
            var stockDto = stocks.Select(s => s.ToStockDto());

            // Alınan stokları döndürüyoruz.
            return Ok(stocks);
        }

        // Belirli bir stok ID'si ile stok almak için kullanılan GET metodu.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            // İlgili ID'ye sahip stok veritabanında aranıyor.
            var stock = await _stockRepo.GetByIdAsync(id);

            // Eğer stok bulunamazsa Resource'a eklediğimiz ErrorMessage sınıfından Stock Bulunmadığı senaryoyu çekiyorz.
            if (stock == null)
            {
                throw new Exception(ErrorMessages.StockNotFound);
            }

            // Bulunan stok verisini DTO formatında döndürüyoruz
            return Ok(stock.ToStockDto());
        }

        // Yeni bir stok oluşturmak için kullanılan POST metodu.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            // DTO'dan model oluşturuyoruz. (Yeniden Örnek olması amacıyla, Id otomatik oluştuğu için Id'siz bir DTO tanımlıyoruz.)
            var stockModel = stockDto.ToStockFromCreateDto();

            // Veritabanına yeni stok ekliyoruz
            await _stockRepo.CreateAsync(stockModel); 

            // Yeni oluşturulan stok bilgisini ve lokasyonunu geriye döndürüyoruz
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }

        // Var olan bir stoğu güncellemek için kullanılan PUT metodu
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            // Stok modelini güncelliyoruz
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);

            // Eğer stok bulunamazsa Resource'a eklediğimiz ErrorMessage sınıfından Stock Bulunmadığı senaryoyu çekiyorz.
            if (stockModel == null)
            {
                throw new Exception(ErrorMessages.StockNotFound);
            }

            // Güncellenmiş stok bilgisini DTO formatında döndürüyoruz
            return Ok(stockModel.ToStockDto());
        }

        // Var olan bir stoğu silmek için kullanılan DELETE metodu
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // İlgili ID'ye sahip stok veritabanında aranıyor ve siliniyor
            var stockModel = await _stockRepo.DeleteAsync(id);

            // Eğer stok bulunamazsa Resource'a eklediğimiz ErrorMessage sınıfından Stock Bulunmadığı senaryoyu çekiyoruz.
            if(stockModel == null)
            {
                throw new Exception(ErrorMessages.StockNotFound);
            }

            return NoContent();
        }
    }
}
