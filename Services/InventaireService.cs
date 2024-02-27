using AutoMapper;
using IStockWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using StockWebApi.Core.Context;
using StockWebApi.Core.Dtos.Inventaire;

namespace StockWebApi.Services
{
    public class InventaireService : IInventaire
    {

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public InventaireService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<InventaireGetAllDto>> ReadAsync()
        {
            var inventaires = await _context.Marchandises.Include(a => a.Operations).Include(b => b.Category).ToListAsync();
            var convertedInventaire = _mapper.Map<IEnumerable<InventaireGetAllDto>>(inventaires);

            return convertedInventaire;
        }

        public async Task<InventaireServiceResponse> ReadByRefAsync(string reference)
        {
            var inventaires = await _context.Marchandises.Include(a => a.Operations).Include(b => b.Category).ToListAsync();
            var convertedInventaire = _mapper.Map<IEnumerable<InventaireGetAllDto>>(inventaires);
            var result = convertedInventaire.FirstOrDefault(m => m.Reference == reference);
            if(result == null)
            {
                return new InventaireServiceResponse()
                {
                    IsSucceed = false,
                    Message = "The reference product does not exist"
                };
            }
            return new InventaireServiceResponse()
            {
                IsSucceed = true,
                Data = result
            };
        }

    }
}
