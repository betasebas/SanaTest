using Microsoft.AspNetCore.Mvc;
using SanaTest.Contracts.Request;
using SanaTest.Contracts.Response;
using SanaTest.Service.ShoppingCart;

namespace SanaTest.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult> AddProductsAsync([FromBody] ShoppingCartRequest shoppingCartRequest)
        {
            try
            {
                return Ok(await _shoppingCartService.AddProductShoppingCartAsync(shoppingCartRequest));

            }catch(Exception ex)
            {
                
                return BadRequest(new ShoppingCartResponse{ Code = 400, Message = ex.Message });
            }
        }

         [HttpPost("DeleteProduct")]
        public ActionResult DeleteProductsAsync([FromBody] ShoppingCartDeleteRequest shoppingCartDeleteRequest)
        {
            try
            {
                return Ok(_shoppingCartService.DeleteProductShoppingCart(shoppingCartDeleteRequest));

            }catch(Exception ex)
            {
                
                return BadRequest(new GenericResponse{ Code = 400, Message = ex.Message });
            }
        }

        [HttpGet("GetProductsShoppingCart")]
        public ActionResult GetProductsShoppingCartAsync([FromHeader] Guid CustomerId)
        {
            try
            {
                return Ok(_shoppingCartService.GetAllProductShoppingCartAsync(CustomerId));

            }catch(Exception ex)
            {
                
                return BadRequest(new ShoppingCartDataResponse{ Code = 400, Message = ex.Message });
            }
        }
    }
}