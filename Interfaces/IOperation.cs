

using StockWebApi.Core.Dtos.Operation;

namespace StockWebApi.Interfaces
{
    public interface IOperation
    {
        Task<OperationServiceResponse> CreateEntrieAsync(OperationEntrieCreateDto createDto);
        Task<OperationServiceResponse> CreateOutPutAsync(OperationOutPutCreateDto createDto);
        Task<IEnumerable<OperationGetAllDto>> ReadAsync();
        Task<OperationServiceResponse> UpdateAsync(long id, OperationUpdateDto UpdateDto);

    }
}
