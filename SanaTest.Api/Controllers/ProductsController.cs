using Microsoft.AspNetCore.Mvc;
using SanaTest.Contracts.Response;
using SanaTest.Service.Products;

namespace SanaTest.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult> GetProductsAsync()
        {
            try
            {
                return Ok(await _productService.GetProductsServiceAsync());

            }catch(Exception ex)
            {
                
                return BadRequest(new ProductResponse{ Code = 400, Message = ex.Message });
            }
        }
    }
   
    
}