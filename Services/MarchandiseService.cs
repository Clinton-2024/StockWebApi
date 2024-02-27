using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockWebApi.Core.Context;
using StockWebApi.Core.Dtos.Marchandise;
using StockWebApi.Core.Entities;
using StockWebApi.Interfaces;

namespace StockWebApi.Services
{
    public class MarchandiseService : IMarchandise
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MarchandiseService(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<MarchandiseServiceResponse> CreateAsync(MarchandiseCreateDto createDto)
        {
            createDto.Reference =  GenerateId(createDto.Designation);
            Marchandise marchandise = _mapper.Map<Marchandise>(createDto);
            var m = await _context.Marchandises.Include(a => a.Category).FirstOrDefaultAsync(a => a.Reference == marchandise.Reference);

            if (m is not null)
            {
                return new MarchandiseServiceResponse() { IsSucceed = false, Message = "The product already exists" };
            }
            
            await _context.Marchandises.AddAsync(marchandise);
            await _context.SaveChangesAsync();

            return new MarchandiseServiceResponse()
            {
                IsSucceed = true,
                Message = $"Marchandise {marchandise.Reference} Created Successfully"
            };
        }

        public async Task<MarchandiseServiceResponse> DeleteAsync(string reference)
        {
            var marchandise = await _context.Marchandises.FirstOrDefaultAsync(a => a.Reference == reference);

            if (marchandise is null)
            {
                return new MarchandiseServiceResponse()
                {
                    IsSucceed = false,
                    Message = "Marchandise Not Found"
                };
            }

            _context.Marchandises.Remove(marchandise);
            await _context.SaveChangesAsync();

            return new MarchandiseServiceResponse()
            {
                IsSucceed = true,
                Message = "Marchandise Delete successfully"
            };
        }

        public async Task<IEnumerable<MarchandiseGetAllDto>> ReadAsync()
        {
            var marchandises = await _context.Marchandises.Include(a => a.Category).ToListAsync();
            var convertedMarchandises = _mapper.Map<IEnumerable<MarchandiseGetAllDto>>(marchandises);

            return convertedMarchandises;
        }

        public async Task<MarchandiseServiceResponse> ReadByIdAsync(string reference)
        {
            var marchandise = await _context.Marchandises.Include(a => a.Category).FirstOrDefaultAsync(a => a.Reference == reference);

            if (marchandise is null)
            {
                return new MarchandiseServiceResponse() { IsSucceed = false, Message = "Marchandise Not Found" };
            }
            else
            {
                var convertedMarchandise = _mapper.Map<MarchandiseGetDto>(marchandise);
                return new MarchandiseServiceResponse() { IsSucceed = true, Data = convertedMarchandise };
            }
        }

        public async Task<MarchandiseServiceResponse> UpdateAsync(string reference, MarchandiseEditDto UpdateDto)
        {
            var marchandise = await _context.Marchandises.FirstOrDefaultAsync(a => a.Reference == reference);

            if (marchandise is null)
            {
                return new MarchandiseServiceResponse { IsSucceed = false, Message = "Marchandise not found" };
            }

            marchandise.Unite = UpdateDto.Unite;
            marchandise.Currency = UpdateDto.Currency;
            marchandise.SeuilAlerte = UpdateDto.SeuilAlerte;
            marchandise.StockInitial = UpdateDto.StockInitial;
            marchandise.PrixUnitaire = UpdateDto.PrixUnitaire;
            marchandise.Designation= UpdateDto.Designation;
            marchandise.CategoryId = UpdateDto.CategoryId;
           
            var convertedMarchandise = _mapper.Map<MarchandiseGetDto>(marchandise);
            await _context.SaveChangesAsync();

            return new MarchandiseServiceResponse()
            {
                IsSucceed = true,
                Message = $"Marchandise {marchandise.Designation}  Update successfully.",
                Data = convertedMarchandise
            };
            

            
        }

        public async Task<MarchandiseServiceResponse> UpdateStatusAsync(string reference)
        {
            var marchandise = await _context.Marchandises.FirstOrDefaultAsync(a => a.Reference == reference);

            if (marchandise is null)
            {
                return new MarchandiseServiceResponse { IsSucceed = false, Message = "Marchandise not found" };
            }

            marchandise.IsActive = !marchandise.IsActive;

            var convertedMarchandise = _mapper.Map<MarchandiseGetDto>(marchandise);
            await _context.SaveChangesAsync();

            return new MarchandiseServiceResponse()
            {
                IsSucceed = true,
                Message = $"Status {marchandise.Designation}  Update successfully.",
                Data = convertedMarchandise
            };
        }

        public async Task<MarchandiseServiceResponse> UpdatePhotoAsync(string reference, string photo)
        {
            var marchandise = await _context.Marchandises.FirstOrDefaultAsync(a => a.Reference == reference);

            if (marchandise is null)
            {
                return new MarchandiseServiceResponse { IsSucceed = false, Message = "Marchandise not found" };
            }

            marchandise.Photo = photo;

            var convertedMarchandise = _mapper.Map<MarchandiseGetDto>(marchandise);
            await _context.SaveChangesAsync();

            return new MarchandiseServiceResponse()
            {
                IsSucceed = true,
                Message = $"Photo {marchandise.Designation}  Update successfully.",
                Data = convertedMarchandise
            };
        }

        private string GenerateId(string designation )
        {
          string reference = "";
          foreach(var item in designation.Split(" "))
            {
                if(item.Length >= 2) { 
                    reference = reference + item.Substring(0,2);
                }
            }
          reference = reference + DateTime.Now.Millisecond.ToString();
          return reference;
        }

    }
}
