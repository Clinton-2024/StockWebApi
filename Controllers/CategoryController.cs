using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockWebApi.Core.Dtos.Category;
using StockWebApi.Core.Dtos.Marchandise;
using StockWebApi.Interfaces;
using StockWebApi.Services;

namespace StockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _CategoryService;

        public CategoryController(ICategory CategoryService)
        {
            _CategoryService = CategoryService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto CategoryDto)
        {
            var createResult = await _CategoryService.CreateAsync(CategoryDto);

            if (createResult.IsSucceed)
                return Ok(createResult);

            return BadRequest(createResult);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<CategoryGetAllDto>>> Get()
        {
            var categories = await _CategoryService.ReadAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CategoryGetAllDto>> Get([FromRoute] long id)
        {
            var readResultat = await _CategoryService.ReadByIdAsync(id);
            if (readResultat.IsSucceed == false)
            {
                return NotFound(readResultat);
            }

            return Ok(readResultat);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] long id,
            [FromBody] CategoryCreateDto updateMarchandiseDto
        )
        {
            var resultUpdate = await _CategoryService.UpdateAsync(id, updateMarchandiseDto);

            if (resultUpdate.IsSucceed == false)
            {
                return NotFound(resultUpdate);
            }

            return Ok(resultUpdate);
        }

        [HttpPut]
        [Route("Status/{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] long id)
        {
            var resultUpdate = await _CategoryService.UpdateStatusAsync(id);

            if (resultUpdate.IsSucceed == false)
            {
                return NotFound(resultUpdate);
            }

            return Ok(resultUpdate);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var result = await _CategoryService.DeleteAsync(id);
            if (result.IsSucceed == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
