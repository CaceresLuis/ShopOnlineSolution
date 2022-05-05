using AutoMapper;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProducRepository _producRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProducRepository producRepository, IMapper mapper)
        {
            _mapper = mapper;
            _producRepository = producRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GeItems(int userId)
        {
            try
            {
                IEnumerable<CartItemDto> cartItemsDto = await _shoppingCartRepository.GetItems(userId);

                if (cartItemsDto == null)
                    return NoContent();

                return Ok(cartItemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            try
            {
                CartItemDto? cartItemDto = await _shoppingCartRepository.GetItem(id);
                if (cartItemDto == null)
                    return NoContent();

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                bool productExist = await _producRepository.ProductExist(cartItemToAddDto.ProductId);

                if (!productExist)
                    return NotFound();

                CartItem cartItem = _mapper.Map<CartItem>(cartItemToAddDto);

                CartItem newCartItem = await _shoppingCartRepository.AddItem(cartItem);
                if (newCartItem == null)
                    return NoContent();

                CartItemDto cartItemDto = await _shoppingCartRepository.GetItem(newCartItem.Id);

                string? a = "";

                return CreatedAtAction(nameof(GetItem), new { id = cartItemDto.Id }, cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                CartItemDto? cartItemDto = await _shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);
                if (cartItemDto == null)
                    return NotFound();

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                bool cartItem = await _shoppingCartRepository.DeleteItem(id);
                if (cartItem == false)
                    return NotFound();

                return cartItem;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
