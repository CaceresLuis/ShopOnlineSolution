using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProducRepository _producRepository;

        public ProductController(IProducRepository producRepository, IMapper mapper)
        {
            _mapper = mapper;
            _producRepository = producRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                IEnumerable<Product> product = await _producRepository.GetItems();
                IEnumerable<ProductCategory> productCategories = await _producRepository.GetCategories();

                if (product == null || productCategories == null)
                    return NotFound();

                IEnumerable<ProductDto> productDto = _mapper.Map<IEnumerable<ProductDto>>(product);

                return Ok(productDto);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from the database");
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
