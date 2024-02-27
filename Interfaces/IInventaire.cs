using StockWebApi.Core.Dtos.Inventaire;
using StockWebApi.Core.Dtos.Marchandise;

namespace IStockWebApi.Interfaces
{
    public interface IInventaire
    {
        Task<IEnumerable<InventaireGetAllDto>> ReadAsync();
        Task<InventaireServiceResponse> ReadByRefAsync(string reference);
    }
}
