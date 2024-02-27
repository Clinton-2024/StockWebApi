using StockWebApi.Core.Dtos.Marchandise;

namespace StockWebApi.Interfaces
{
    public interface IMarchandise
    {
        Task<MarchandiseServiceResponse> CreateAsync(MarchandiseCreateDto createDto);
        Task<IEnumerable<MarchandiseGetAllDto>> ReadAsync();
        Task<MarchandiseServiceResponse> ReadByIdAsync(string reference);
        Task<MarchandiseServiceResponse> UpdateAsync(string reference, MarchandiseEditDto UpdateDto);
        Task<MarchandiseServiceResponse> UpdateStatusAsync(string reference);
        Task<MarchandiseServiceResponse> DeleteAsync(string reference);
        Task<MarchandiseServiceResponse> UpdatePhotoAsync(string reference, string photo);
    }
}
