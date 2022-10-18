using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace FlowerShopManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        ICustomerServices _customerServices;
        public TestController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        // GET: api/<AccountController>
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            return await _customerServices.GetAllCustomers();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            return await _customerServices.GetCustomerById(id);
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Customer value)
        {
            await _customerServices.AddNewCustomer(value);

            return NoContent();
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            var systemAccount = await _customerServices.GetCustomerById(id);
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
            var todoItem = await _customerServices.GetCustomerById(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _customerServices.RemoveCustomerById(id);
             

            return NoContent();
        }
    }
}