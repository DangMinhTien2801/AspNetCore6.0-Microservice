using Infrastructure.Common.Models;
using Inventory.Api.Services;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryServices;
        public InventoryController(IInventoryService inventoryServices)
        {
            _inventoryServices = inventoryServices;
        }

        [HttpGet]
        [Route("items/{itemNo}", Name = "GetAllByItemNo")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByItemNo([Required] string itemNo)
        {
            var result = await _inventoryServices.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }

        [HttpGet]
        [Route("items/{itemNo}/paging", Name = "GetAllByItemNoPaging")]
        [ProducesResponseType(typeof(PagedList<InventoryEntryDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByItemNoPaging([Required] string itemNo, [FromQuery] GetInventoryPagingQuery query)
        {
            query.SetItemNo(itemNo);
            var result = await _inventoryServices.GetAllByItemNoPagingAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetInventoryById")]
        [ProducesResponseType(typeof(InventoryEntryDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetInventoryById([Required] string id)
        {
            var result = await _inventoryServices.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("purchase/{itemNo}", Name = "PurchaseOrder")]
        [ProducesResponseType(typeof(InventoryEntryDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PurchaseOrder(
            [Required] string itemNo,
            [FromBody] PurchaseProductDto model
            )
        {
            var result = await _inventoryServices.PurchaseItemAsync(itemNo, model);
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteById")]
        [ProducesResponseType(typeof(InventoryEntryDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteById(
            [Required] string id
            )
        {
            var entity = await _inventoryServices.GetByIdAsync(id);
            if(entity == null) return NotFound();

            await _inventoryServices.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("sales/{itemNo}", Name = "SalesOrder")]
        [ProducesResponseType(typeof(InventoryEntryDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SalesOrder(
            [Required] string itemNo,
            [FromBody] SalesProductDto model
            )
        {
            model.SetItemNo(itemNo);
            var result = await _inventoryServices.SalesItemAsync(itemNo, model);
            return Ok(result);
        }
    }
}
