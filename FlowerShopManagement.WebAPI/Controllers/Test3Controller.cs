using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace FlowerShopManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test3Controller : ControllerBase
    {
        ISupplierCRUD _productServices;
        public Test3Controller(ISupplierCRUD customerServices)
        {
            _productServices = customerServices;
        }

      
        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> Get()
        {
            return await _productServices.GetAllSuppliers();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> Get(string id)
        {
            return await _productServices.GetSupplierById(id);
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Supplier value)
        {
            await _productServices.AddNewSupplier(value);

            return NoContent();
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            var systemAccount = await _productServices.GetSupplierById(id);
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
            var todoItem = await _productServices.GetSupplierById(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _productServices.RemoveSupplierById(id);
             

            return NoContent();
        }
    }
}