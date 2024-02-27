using StockWebApi.Core.Dtos.Traitement;

namespace StockWebApi.Interfaces
{
    public interface ITraitement
    {
        Task<TraitementServiceResponse> ReadStockTotalAsync();
        Task<TraitementServiceResponse> ReadStockTotalEntriesAsync(long CategoryId);
        Task<TraitementServiceResponse> ReadStockTotalOutputAsync(long CategoryId);
        Task<IEnumerable<RepartitionStock>> ReadRepartitionStockAsync();
        Task<IEnumerable<RepartitionServiceResponse>> ReadRepartitionEntrieAsync();
        Task<IEnumerable< RepartitionServiceResponse>> ReadRepartitionOutputAsync();
    }
}
