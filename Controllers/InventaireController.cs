using IStockWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StockWebApi.Core.Dtos.Inventaire;

namespace StockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventaireController : ControllerBase
    {
        private readonly IInventaire _inventaireService;

        public InventaireController(IInventaire inventaireService)
        {
            _inventaireService = inventaireService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<InventaireGetAllDto>>> Get()
        {
            var inventaires = await _inventaireService.ReadAsync();
            return Ok(inventaires);
        }

        [HttpGet]
        [Route("{reference}")]
        public async Task<ActionResult<InventaireServiceResponse>> Get([FromRoute] string reference)
        {
            var readResultat = await _inventaireService.ReadByRefAsync(reference);
            if (readResultat.IsSucceed == false)
            {
                return NotFound(readResultat);
            }

            return Ok(readResultat);
        }
    }
}
