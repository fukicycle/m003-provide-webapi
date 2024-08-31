using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace m003_provide_api.Controllers
{
    [ApiController]
    [Route("/api/v1/suppliers")]
    public sealed class SupplierController : ControllerBase
    {
        private readonly Data _data;
        public SupplierController(Data data)
        {
            _data = data;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<SupplierDTO[]>(200)]
        public IActionResult Get()
        {
            return Ok(_data.Suppliers);
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<SupplierDTO>(200)]
        [ProducesResponseType<ErrorResponseDTO>(400)]
        public IActionResult GetItem(int id)
        {
            if (!_data.Suppliers.Any(a => a.Id == id))
            {
                return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), $"No such id:{id}"));
            }
            return Ok(_data.Suppliers.First(a => a.Id == id));
        }

        //price
        [HttpGet]
        [Route("{supplierId}/ingredients")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<List<SupplierIngredientDTO>>(200)]
        [ProducesResponseType<ErrorResponseDTO>(400)]
        public IActionResult GetIngredients(int supplierId)
        {
            if (!_data.Suppliers.Any(a => a.Id == supplierId))
            {
                return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), $"No such id:{supplierId}"));
            }
            return Ok(_data.SupplierIngredients.Where(a => a.SupplierId == supplierId));
        }

        [HttpGet]
        [Route("{supplierId}/ingredients/{ingredientId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<SupplierIngredientDTO>(200)]
        [ProducesResponseType<ErrorResponseDTO>(400)]
        public IActionResult GetIngredient(int supplierId, int ingredientId)
        {
            if (!_data.Suppliers.Any(a => a.Id == supplierId))
            {
                return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), $"No such supplier id:{supplierId}"));
            }
            var data = _data.SupplierIngredients.Where(a => a.SupplierId == supplierId);
            if (!data.Any(a => a.IngredientId == ingredientId))
            {
                return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), $"No such ingredient id:{ingredientId}"));
            }
            return Ok(data.First(a => a.IngredientId == ingredientId));
        }
    }

    public record SupplierDTO(int Id, string Name);
    public record IngredientDTO(int Id, string Name);
    public record SupplierIngredientDTO(int IngredientId, decimal Price, int SupplierId, string SupplierName, int LeadTime);
}
