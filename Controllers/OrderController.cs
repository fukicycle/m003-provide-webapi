using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace m003_provide_api.Controllers
{
    [ApiController]
    [Route("/api/v1/orders")]
    public sealed class OrderController : ControllerBase
    {
        private readonly Data _data;
        public OrderController(Data data)
        {
            _data = data;
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Guid>(201)]
        [ProducesResponseType<ErrorResponseDTO>(400)]
        [ProducesResponseType<ErrorResponseDTO>(500)]
        public IActionResult Order([FromBody] OrderDTO order)
        {
            if (order.OrderTrackingId == Guid.Empty)
            {
                return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), "Please provide a order id!"));
            }
            if (!order.Details.Any())
            {
                return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), "Order has no item."));
            }
            foreach (var item in order.Details)
            {
                if (!_data.Suppliers.Any(a => a.Id == item.SupplierId))
                {
                    return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), $"No such supplier id:{item.SupplierId}"));
                }
                if (!_data.SupplierIngredients.Any(a => a.IngredientId == item.IngredientId && a.SupplierId == item.SupplierId))
                {
                    return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), $"No such ingredient id:{item.IngredientId}"));
                }
                if (item.Amount <= 0)
                {
                    return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), "Can not order with Zero."));
                }
            }
            try
            {
                if (!Directory.Exists("order"))
                {
                    Directory.CreateDirectory("order");
                }
                System.IO.File.WriteAllText(System.IO.Path.Combine("order", order.OrderTrackingId.ToString() + ".txt"), System.Text.Json.JsonSerializer.Serialize(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO(Guid.NewGuid(), ex.Message));
            }
            return StatusCode(201, order.OrderTrackingId);
        }

        [HttpDelete]
        [Route("{orderTrackingId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType<ErrorResponseDTO>(400)]
        [ProducesResponseType<ErrorResponseDTO>(500)]
        public IActionResult Cancel(Guid orderTrackingId)
        {
            try
            {
                //string fileName = "order/" + orderTrackingId.ToString() + ".txt";
                string fileName = System.IO.Path.Combine("order", orderTrackingId.ToString() + ".txt");
                if (!Directory.GetFiles("order").Any(a => a == fileName))
                {
                    return BadRequest(new ErrorResponseDTO(Guid.NewGuid(), $"No such order:{orderTrackingId}"));
                }
                System.IO.File.Delete(fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDTO(Guid.NewGuid(), ex.Message));
            }
            return Ok();
        }
    }

    public record OrderDTO(Guid OrderTrackingId, List<OrderDetailDTO> Details);
    public record OrderDetailDTO(int SupplierId, int IngredientId, int Amount);
}
