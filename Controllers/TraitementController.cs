using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockWebApi.Core.Dtos.Traitement;
using StockWebApi.Interfaces;
using StockWebApi.Services;
using System.IO.Pipelines;

namespace StockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraitementController : ControllerBase
    {
        private ITraitement _traitementService;

        public TraitementController(ITraitement traitementService)
        {
            _traitementService = traitementService;
        }

        [HttpGet]
        [Route("GetStockTotals")]
        public async Task<ActionResult<TraitementServiceResponse>> GetStockTotals()
        {
            var stockTotals = await _traitementService.ReadStockTotalAsync();
            return Ok(stockTotals);
        }

        [HttpGet]
        [Route("GetStockEntries/{CategoryId}")]
        public async Task<ActionResult<TraitementServiceResponse>> GetStockEntries([FromRoute] long CategoryId)
        {
            var EntrieTotals = await _traitementService.ReadStockTotalEntriesAsync(CategoryId);
            if (EntrieTotals.isSucceed == false)
            {
                return NotFound(EntrieTotals);
            }
            return Ok(EntrieTotals);
        }

        [HttpGet]
        [Route("GetStockOutput/{CategoryId}")]
        public async Task<ActionResult<TraitementServiceResponse>> GetStockOutput([FromRoute] long CategoryId)
        {
            var outputTotals = await _traitementService.ReadStockTotalOutputAsync(CategoryId);
            if (outputTotals.isSucceed == false)
            {
                return NotFound(outputTotals);
            }
            return Ok(outputTotals);
        }

        [HttpGet]
        [Route("GetRepartitionStock")]
        public async Task<ActionResult<RepartitionStock>> GetRepartitionStock()
        {
            var repartition = await _traitementService.ReadRepartitionStockAsync();
            return Ok(repartition);
        }
    }
}
