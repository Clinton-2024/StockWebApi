using AutoMapper;
using IStockWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using StockWebApi.Core.Context;
using StockWebApi.Core.Dtos.Inventaire;
using StockWebApi.Core.Dtos.Marchandise;
using StockWebApi.Core.Dtos.Operation;
using StockWebApi.Core.Dtos.OtherObjects;
using StockWebApi.Core.Entities;
using StockWebApi.Interfaces;

namespace StockWebApi.Services
{
    public class OperationService : IOperation
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IInventaire _inventaireService;

        public OperationService(ApplicationDbContext context, IMapper mapper, IInventaire inventaireService)   
        {
            _context = context;
            _mapper = mapper;
            _inventaireService = inventaireService;
        }

        public async Task<OperationServiceResponse> CreateEntrieAsync(OperationEntrieCreateDto createDto)
        {
            Operation operation = _mapper.Map<Operation>(createDto);

            var m = await _context.Operations.Include(a => a.Marchandise).FirstOrDefaultAsync(a => a.num == operation.num);

            if (m is not null)
            {
                return new OperationServiceResponse() { IsSucceed = false, Message = "The Operation already exists" };
            }

            operation.Type = StaticTypeOperation.Entrie;

            await _context.Operations.AddAsync(operation);
            await _context.SaveChangesAsync();

            return new OperationServiceResponse()
            {
                IsSucceed = true,
                Message = $"Operation {operation.num} save successfully"
            };
        }

        public async Task<OperationServiceResponse> CreateOutPutAsync(OperationOutPutCreateDto createDto)
        {
            Operation operation = _mapper.Map<Operation>(createDto);

            var op = await _context.Operations.Include(a => a.Marchandise).FirstOrDefaultAsync(a => a.num == operation.num);

            if (op is not null)
            {
                return new OperationServiceResponse() { IsSucceed = false, Message = "The Operation already exists" };
            }

            var quantityOutput = await _inventaireService.ReadByRefAsync(operation.Reference);

            if (quantityOutput.IsSucceed == false)
            {
                return new OperationServiceResponse() { IsSucceed = false, Message = $"The {operation.Reference} reference product does not exist" };
            }

            int Stockfinal = 0;

            if (quantityOutput.IsSucceed == true)
            {
                Stockfinal = quantityOutput.Data.StockFinal;
            }

            if(operation.Quantity > Stockfinal)
            {
                return new OperationServiceResponse()
                {
                    IsSucceed = false,
                    Message = "Insufficient stock"
                };
            }
            operation.Type = StaticTypeOperation.OutPut;

            await _context.Operations.AddAsync(operation);
            await _context.SaveChangesAsync();

            return new OperationServiceResponse()
            {
                IsSucceed = true,
                Message = $"Operation {operation.num} save successfully"
            };
        }

        public async Task<IEnumerable<OperationGetAllDto>> ReadAsync()
        {
            var operations = await _context.Operations.Include(a => a.Marchandise).Include(b => b.Marchandise.Category).ToListAsync();
            var convertedOperations = _mapper.Map<IEnumerable<OperationGetAllDto>>(operations);

            return convertedOperations;
        }

       public async Task<OperationServiceResponse> UpdateAsync(long id, OperationUpdateDto UpdateDto)
        {
            var operation = await _context.Operations.FirstOrDefaultAsync(a => a.num == id);

            if (operation is null)
            {
                return new OperationServiceResponse { IsSucceed = false, Message = "Operation not found" };
            }

            operation.Reference = UpdateDto.Reference;
            operation.Price = UpdateDto.Price;
            operation.Quantity = UpdateDto.Quantity;
            operation.Type = UpdateDto.Type;
            operation.UpdatedAt = DateTime.Now;

            var convertedOperation = _mapper.Map<OperationGetAllDto>(operation);
            await _context.SaveChangesAsync();

            return new OperationServiceResponse()
            {
                IsSucceed = true,
                Message = " Operation Update successfully.",
                Data = convertedOperation
            };
        }
    }
}
