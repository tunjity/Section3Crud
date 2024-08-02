using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Section3Crud.Model;
using Swashbuckle.AspNetCore.Annotations;

namespace Section3Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMongoRepository _repo;
        public CustomerController(IMongoRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReturnObject))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ReturnObject))]
        [Route("getAllCustomer")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var r = await _repo.GetAll();
            return Ok(r);
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReturnObject))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ReturnObject))]
        [Route("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerFormModel obj)
        {
            var prod = new MongoCustomerModel
            {
                Name = obj.Name,
                Email = obj.Email,
                //if authenticated the createdBy will be the userId
                CreatedBy = 1,
                DateCreated = DateTime.UtcNow
            };
            var r = await _repo.Insert(prod);
            return Ok(r);
        }
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReturnObject))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ReturnObject))]
        [Route("Customerbyid/{id}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] string id)
        {
            return Ok(_repo.Get(id));
        }
    }
}
