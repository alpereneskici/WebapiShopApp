using Microsoft.EntityFrameworkCore;
using shopapp.Domain;
using shopapp.Domain.DTOs.Stock;
using shopapp.Domain.Interface;
using shopapp.Resources;

namespace shopapp.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        // Context yapısının değiştirilmemesi için.
        private readonly ApplicationDBContext _context;

        // Context yapısını ApplicationDBContext sınıfnddan alır.
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // Yeni bir hisse senedi oluşturur ve veritabanına kaydeder.
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel); // Veritabanı bağlamına yeni hisse senedini ekler.
            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydeder.
            return stockModel; // Oluşturulan hisse senedini döner.
        }

        // Belirli bir ID'ye sahip hisse senedini siler.
        public async Task<Stock> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id); // ID'ye göre hisse senedini bulur.
            if (stockModel == null)
            {
                return null; // Hisse senedi bulunamazsa null döner.
            }
            _context.Stocks.Remove(stockModel); // Hisse senedini veritabanı bağlamından çıkarır.
            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydeder.
            return stockModel; // Silinen hisse senedini döner.
        }

        // Tüm hisse senetlerini alır.
        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync(); // Veritabanındaki tüm hisse senetlerini liste olarak döner.
        }

        // Belirli bir ID'ye sahip hisse senedini alır.
        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id); // ID'ye göre hisse senedini bulur ve döner. Hisse senedi bulunamazsa null döner.
        }

        // Var olan bir hisse senedini günceller.
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id); // ID'ye göre mevcut hisse senedini bulur.
            if (existingStock == null)
            {
                throw new Exception(ErrorMessages.StockNotFound); // Hisse senedi bulunamazsa özel bir hata fırlatır.
            }

            // Mevcut hisse senedinin özelliklerini güncellenmiş değerlerle değiştirir.
            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.MarketCap = stockDto.MarketCap;
            existingStock.Industry = stockDto.Industry;

            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydeder.
            return existingStock; // Güncellenmiş hisse senedini döner.
        }
    }
}
