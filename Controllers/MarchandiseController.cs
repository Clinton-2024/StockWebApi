using Microsoft.AspNetCore.Mvc;
using StockWebApi.Core.Dtos.Marchandise;
using StockWebApi.Interfaces;

namespace StockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarchandiseController : ControllerBase
    {
        private readonly IMarchandise _marchandiseService;

        public MarchandiseController(IMarchandise marchandiseService)
        {
            _marchandiseService = marchandiseService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] MarchandiseCreateDto MarchandiseDto)
        {
            var createResult = await _marchandiseService.CreateAsync(MarchandiseDto);

            if (createResult.IsSucceed)
                return Ok(createResult);

            return BadRequest(createResult);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<MarchandiseGetAllDto>>> Get()
        {
            var Marchandises = await _marchandiseService.ReadAsync();
            return Ok(Marchandises);
        }

        [HttpGet]
        [Route("{reference}")]
        public async Task<ActionResult<MarchandiseGetDto>> Get([FromRoute] string reference)
        {
            var readResultat = await _marchandiseService.ReadByIdAsync(reference);
            if (readResultat.IsSucceed == false)
            {
                return NotFound(readResultat);
            }

            return Ok(readResultat);
        }

        [HttpPut]
        [Route("{reference}")]
        public async Task<IActionResult> Update(
            [FromRoute] string reference,
            [FromBody] MarchandiseEditDto updateMarchandiseDto
        )
        {
            var resultUpdate = await _marchandiseService.UpdateAsync(reference, updateMarchandiseDto);

            if (resultUpdate.IsSucceed == false)
            {
                return NotFound(resultUpdate);
            }

            return Ok(resultUpdate);
        }

        [HttpPut]
        [Route("Status/{reference}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] string reference)
        {
            var resultUpdate = await _marchandiseService.UpdateStatusAsync(reference);

            if (resultUpdate.IsSucceed == false)
            {
                return NotFound(resultUpdate);
            }

            return Ok(resultUpdate);
        }

        [HttpPut]
        [Route("Photo/{reference}/{photo}")]
        public async Task<IActionResult> UpdatePhoto([FromRoute] string reference, [FromRoute] string photo)
        {
            var resultUpdate = await _marchandiseService.UpdatePhotoAsync(reference,photo);

            if (resultUpdate.IsSucceed == false)
            {
                return NotFound(resultUpdate);
            }

            return Ok(resultUpdate);
        }

        [HttpDelete]
        [Route("{reference}")]
        public async Task<IActionResult> Delete([FromRoute] string reference)
        {
            var result = await _marchandiseService.DeleteAsync(reference);
            if (result.IsSucceed == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
