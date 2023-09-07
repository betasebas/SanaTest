using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanaTest.Contracts.Request;
using SanaTest.Contracts.Response;
using SanaTest.Service.Orders;

namespace SanaTest.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

         [HttpPost("AddOrder")]
        public async Task<ActionResult> AddOrderAsync([FromBody] OrderRequest orderRequest)
        {
            try
            {
                return Ok(await _orderService.AddOrderAsync(orderRequest));

            }catch(Exception ex)
            {
                
                return BadRequest(new GenericResponse{ Code = 400, Message = ex.Message });
            }
        }

    }
}