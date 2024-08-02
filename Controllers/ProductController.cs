using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Section3Crud.Model;
using Swashbuckle.AspNetCore.Annotations;

namespace Section3Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMsSqlRepository _repo;
        public ProductController(IMsSqlRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReturnObject))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ReturnObject))]
        [Route("getAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var r = await _repo.GetAll();
            return Ok(r);
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ReturnObject))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ReturnObject))]
        [Route("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductFormModel obj)
        {
            var prod = new Product
            {
                Price = obj.Price,
                ProductName = obj.ProductName,
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
        [Route("Productbyid/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            return Ok(await _repo.Get(id));
        }
    }
}