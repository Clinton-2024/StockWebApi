using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockWebApi.Core.Context;
using StockWebApi.Core.Dtos.Category;
using StockWebApi.Core.Entities;
using StockWebApi.Interfaces;

namespace StockWebApi.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<CategoryServiceResponse> CreateAsync(CategoryCreateDto createDto)
        {
            Category category = _mapper.Map<Category>(createDto);
            try {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new CategoryServiceResponse()
                {
                    IsSucceed = true,
                    Message = $"Category {category.Name} Created Successfully"
                };
            }
            catch( DbUpdateException){
                return new CategoryServiceResponse()
                {
                    IsSucceed = false,
                    Message = $"The {category.Name} category already exists"
                };
            }
           

            
        }

        public async Task<CategoryServiceResponse> DeleteAsync(long id)
        {
            var categorie = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (categorie is null)
            {
                return new CategoryServiceResponse()
                {
                    IsSucceed = false,
                    Message = "This category does not exist"
                };
            }

            try
            {
                _context.Categories.Remove(categorie);
                await _context.SaveChangesAsync();
                return new CategoryServiceResponse()
                {
                    IsSucceed = true,
                    Message = "Category successfully deleted",

                };
            }
            catch (DbUpdateException)
            {
                return new CategoryServiceResponse()
                {
                    IsSucceed = false,
                    Message = "The category cannot be deleted because it contains at least one product.",

                };

            }

           


        }

        public async Task<IEnumerable<CategoryGetAllDto>> ReadAsync()
        {
            var category = await _context.Categories.Include(c => c.Marchandises).ToListAsync();
            var convertedCategory = _mapper.Map<IEnumerable<CategoryGetAllDto>>(category);

            return  convertedCategory;
        }

        public async Task<CategoryServiceResponse> UpdateAsync(long id, CategoryCreateDto UpdateDto)
        {
            var categorie = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(categorie is null){
                return new CategoryServiceResponse()
                {
                    IsSucceed = false,
                    Message = "This category does not exist"
                };
            }
            try
            {
                categorie.Name = UpdateDto.Name;

                var convertion = _mapper.Map<CategoryGetAllDto>(categorie);
                await _context.SaveChangesAsync();

                return new CategoryServiceResponse()
                {
                    IsSucceed = true,
                    Message = "Successful category update"
                };
            }
            catch(Exception ex)
            {
                return new CategoryServiceResponse()
                {
                    IsSucceed = false,
                    Message = "This category exist",

                };

            }
            
        }

        public async Task<CategoryServiceResponse> UpdateStatusAsync(long id)
        {
            var categorie = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (categorie is null)
            {
                return new CategoryServiceResponse()
                {
                    IsSucceed = false,
                    Message = "This category does not exist"
                };
            }
            categorie.IsActive = !categorie.IsActive;

            var convertion = _mapper.Map<CategoryGetAllDto>(categorie);
            await _context.SaveChangesAsync();

            return new CategoryServiceResponse()
            {
                IsSucceed = true,
                Message = "The category status has been successfully updated"
            };
        }

        public async  Task<CategoryServiceResponse> ReadByIdAsync(long id)
        {
            var categorie = await _context.Categories.Include(c => c.Marchandises).FirstOrDefaultAsync(c => c.Id == id);
            if (categorie is null)
            {
                return new CategoryServiceResponse()
                {
                    IsSucceed = false,
                    Message = "This category does not exist"
                };
            }
            else
            {
                var convertion = _mapper.Map<CategoryGetAllDto>(categorie);
                return new CategoryServiceResponse()
                {
                    IsSucceed = true,
                    Data = convertion
                };
            }

        }
    }
}
