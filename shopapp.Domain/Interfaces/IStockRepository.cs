
using shopapp.Domain;
using shopapp.Domain.DTOs.Stock;


namespace shopapp.Domain.Interface
{
    // Veri erişimin hangi metotları sağlaması gerektiğini belirtir.
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();

        Task<Stock?> GetByIdAsync(int id );
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock> DeleteAsync(int id);
    }

}