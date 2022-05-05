using AutoMapper;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;

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
                IEnumerable<ProductDto> product = await _producRepository.GetItems();

                if (product == null)
                    return NotFound();

                return Ok(product);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from the database");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            try
            {
                ProductDto product = await _producRepository.GetItem(id);
                if (product == null)
                    return NotFound();

                return Ok (product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from the database");
            }
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
