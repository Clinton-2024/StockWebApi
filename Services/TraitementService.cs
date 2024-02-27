using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockWebApi.Core.Context;
using StockWebApi.Core.Dtos.Traitement;
using StockWebApi.Interfaces;
using System.Numerics;

namespace StockWebApi.Services
{
    public class TraitementService : ITraitement
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public TraitementService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<TraitementServiceResponse> ReadStockTotalAsync()
        {
            var stockTotals = await _context.Marchandises.Include(m => m.Operations).Include(m => m.Category).ToListAsync();
            var converterStockTotals = _mapper.Map< IEnumerable<TraitementStockTotal>>(stockTotals);
            var total = converterStockTotals.Sum(el => el.StockTotal);
            var valeur = converterStockTotals.Sum(el => el.ValeurStock);
            return new TraitementServiceResponse()
            {
                total = total,
                valeur = valeur,
                currency = converterStockTotals.FirstOrDefault().Currency
            };
        }

        public async Task<TraitementServiceResponse> ReadStockTotalEntriesAsync(long CategoryId)
        {
            var entries = await _context.Marchandises.Include(m => m.Operations).Include(m => m.Category).ToListAsync();
            var converterEntries = _mapper.Map<IEnumerable<TraitementEntries>>(entries);

            var filterCategory = converterEntries;
            if (CategoryId != 0)
            {
                filterCategory = converterEntries.Where(w => w.CategoryId == CategoryId);
                if(filterCategory.Count() != 0)
                {
                    return new TraitementServiceResponse()
                    {
                        isSucceed = true,
                        total = filterCategory.Sum(el => el.TotalEntries),
                        valeur = filterCategory.Sum(el => el.ValeurEntries),
                        currency = filterCategory.FirstOrDefault().Currency
                    };
                }
                return new TraitementServiceResponse()
                {
                    isSucceed = false,
                    message = "No operations in this category",
                    total = converterEntries.Sum(el => el.TotalEntries),
                    valeur = converterEntries.Sum(el => el.ValeurEntries),
                    currency = converterEntries.FirstOrDefault().Currency
                };
            }

            return new TraitementServiceResponse()
            {
                isSucceed = true,
                total = converterEntries.Sum(el => el.TotalEntries),
                valeur = converterEntries.Sum(el => el.ValeurEntries),
                currency = filterCategory.FirstOrDefault().Currency
            };
        }

        public async Task<TraitementServiceResponse> ReadStockTotalOutputAsync(long CategoryId)
        {
            var output = await _context.Marchandises.Include(m => m.Operations).Include(m => m.Category).ToListAsync();
            var converterOutput = _mapper.Map<IEnumerable<TraitementOutput>>(output);
            var filterCategory = converterOutput;

            if (CategoryId != 0) {
                 filterCategory = converterOutput.Where(w => w.CategoryId == CategoryId);

                if(filterCategory.Count() != 0) 
                {
                    return new TraitementServiceResponse()
                    {
                        isSucceed = true,
                        total = filterCategory.Sum(el => el.TotalOutput),
                        valeur = filterCategory.Sum(el => el.ValeurOutput),
                        currency = filterCategory.FirstOrDefault().Currency

                    };
                }

                return new TraitementServiceResponse()
                {
                    isSucceed = false,
                    message = "No operations in this category",
                    total = converterOutput.Sum(el => el.TotalOutput),
                    valeur = converterOutput.Sum(el => el.ValeurOutput),
                    currency = converterOutput.FirstOrDefault().Currency

                };
            }
            
            return new TraitementServiceResponse()
            {
                isSucceed = true,
                total = converterOutput.Sum(el => el.TotalOutput),
                valeur = converterOutput.Sum(el => el.ValeurOutput),
                currency = converterOutput.FirstOrDefault().Currency

            };
        }

        Task<IEnumerable<RepartitionServiceResponse>> ITraitement.ReadRepartitionEntrieAsync()
        {
            throw new NotImplementedException();

        }

        Task<IEnumerable<RepartitionServiceResponse>> ITraitement.ReadRepartitionOutputAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RepartitionStock>> ReadRepartitionStockAsync()
        {
            var repartition = await _context.Categories.Include(m => m.Marchandises).ToListAsync();
            var converter = _mapper.Map<IEnumerable<RepartitionStock>>(repartition);
            return converter;
        }
    }
}
