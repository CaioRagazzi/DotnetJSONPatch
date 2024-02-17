using JSONPatch.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JSONPatch.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private static List<Customer> Customers { get; set; } = new List<Customer>();

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Customers);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Customer customer)
    {
        Customers.Add(customer);

        return NoContent();
    }

    [HttpPatch("{customerId:int}")]
    public IActionResult Patch([FromRoute] Guid customerId, [FromBody] JsonPatchDocument<Customer> patchDoc)
    {
        if (patchDoc != null)
        {
            var customer = Customers.FirstOrDefault(customer => customer.Id == customerId);

            if (customer is null)
            {
                return NotFound("Customer was not found!");
            }

            patchDoc.ApplyTo(customer, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return new ObjectResult(customer);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}
