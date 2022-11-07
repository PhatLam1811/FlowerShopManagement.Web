using DnsClient;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Xml.Linq;

namespace FlowerShopManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test1Controller : ControllerBase
    {
        IProduct _productServices;
        public Test1Controller(IProduct customerServices)
        {
            _productServices = customerServices;
        }


        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            return await _productServices.GetAllProducts();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            return await _productServices.GetProductById(id);
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult> Post()//parameter should be Product value
        {
            var p = new Product("name", "picture",new List<Categories>(),123, 123);

            await _productServices.AddNewProduct(p);

            return NoContent();
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            var systemAccount = await _productServices.GetProductById(id);
            if (systemAccount == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(string id)
        {
            var todoItem = await _productServices.GetProductById(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _productServices.RemoveProductById(id);


            return NoContent();
        }
    }
}