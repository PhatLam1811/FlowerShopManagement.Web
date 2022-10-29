using FlowerShopManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlowerShopManagement.Web.Controllers
{
    //This is just a sample controller
    public class CustomerController : ControllerBase
    {
        // GET: api/<CustomerController>

        private readonly ICustomerManagementServices customerServices;

        public CustomerController(ICustomerManagementServices customerServices)
        {
            this.customerServices = customerServices;
        }

        //[HttpGet]
        /*public ActionResult<List<Customer>> Get()
        {
            return customerServices.Get();
        }*/

        //[HttpGet("{id}")]
        /*public ActionResult<Customer?> Get(string id)
        {
            var customer = customerServices.Get(id);

            if (customer == null)
            {
                return NotFound($"Customer with id = {id} isn't exist");
            }
            return customer;
        }*/

        // POST api/<CustomerController>
        //[HttpPost]
        /*public ActionResult<Customer?> Post([FromBody] string value)
        {
            Customer customer = new Customer();
            customer._fullName = value;
            //customerServices.Create(customer);
            return CreatedAtAction(nameof(Get), new { id = customer._id }, customer);
        }*/

        // PUT api/<CustomerController>/5
        //[HttpPut("{id}")]
        /*public ActionResult Put(string id, [FromBody] Customer customer)
        {
            var existingCustomer = customerSes.Get(id);

            if (existingCustomer == null)
            {
                return NotFound($"Customer with id = {id} isn't exist");
            }
            else
                customerServices.Update(id, customer);
            return NoContent();
        }*/

        // DELETE api/<CustomerController>/5
        //[HttpDelete("{id}")]
        /*public ActionResult Delete(string id)
        {
            var existingCustomer = customerServices.Get(id);

            if (existingCustomer == null)
            {
                return NotFound($"Customer with id = {id} isn't exist");
            }
            else
                customerServices.Remove(id);
            return Ok($"Customer with id = {id} deleted");
        }*/
    }
}
