using Microsoft.AspNetCore.Mvc;
using StockWebApi.Core.Dtos.Marchandise;
using StockWebApi.Core.Dtos.Operation;
using StockWebApi.Core.Entities;
using StockWebApi.Interfaces;
using StockWebApi.Services;

namespace StockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        public readonly IOperation _operationService;

        public OperationController(IOperation operation)
        {
            _operationService = operation;
        }

        [HttpPost]
        [Route("Create_Entrie")]
        public async Task<IActionResult> CreateEntrie([FromBody] OperationEntrieCreateDto operationDto)
        {
            var createResult = await _operationService.CreateEntrieAsync(operationDto);

            if (createResult.IsSucceed)
                return Ok(createResult);

            return BadRequest(createResult);
        }

        [HttpPost]
        [Route("Create_Output")]
        public async Task<IActionResult> CreateOutput([FromBody] OperationOutPutCreateDto operationDto)
        {
            var createResult = await _operationService.CreateOutPutAsync(operationDto);

            if (createResult.IsSucceed)
                return Ok(createResult);

            return BadRequest(createResult);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<OperationGetAllDto>>> Get()
        {
            var operations = await _operationService.ReadAsync();
            return Ok(operations);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] long id,
            [FromBody] OperationUpdateDto updateDto
        )
        {
            var resultUpdate = await _operationService.UpdateAsync(id, updateDto);

            if (resultUpdate.IsSucceed == false)
            {
                return NotFound(resultUpdate);
            }

            return Ok(resultUpdate);
        }
    }
}
