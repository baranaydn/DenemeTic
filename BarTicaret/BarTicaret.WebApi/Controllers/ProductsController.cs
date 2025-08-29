using Microsoft.AspNetCore.Mvc;
using BarTicaret.Application.Products;

namespace BarTicaret.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _svc;

        public ProductsController(IProductService svc)
        {
            _svc = svc;
        }

        /// <summary>Tum urunleri getirir</summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll()
        {
            var result = await _svc.GetAllAsync();
            return Ok(result);
        }

        /// <summary>Yeni urun olusturur</summary>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductCreateDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }
    }
}
