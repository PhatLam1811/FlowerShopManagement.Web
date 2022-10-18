using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace FlowerShopManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test2Controller : ControllerBase
    {
        ICartCRUD _cartServices;
        public Test2Controller(ICartCRUD customerServices)
        {
            _cartServices = customerServices;
        }

       

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> Get(string id)
        {
            return await _cartServices.GetCartOfCustomerIdAsync(id);
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string value)
        {
            bool r = await _cartServices.AddNewCartByCustomerIdAsync(value);

            return NoContent();
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id, Cart cart)
        {
            var result = await _cartServices.UpdateCartByCustomerIdAsync(id, cart);
            if (result == false)
            {
                return NotFound();
            }
            return NoContent();
        }

      
    }
}