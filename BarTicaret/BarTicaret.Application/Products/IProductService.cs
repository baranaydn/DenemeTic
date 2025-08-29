using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarTicaret.Application.Products
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetAllAsync();
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task<ProductDto> SellAsync(int productId, int quantity);
        Task DeleteAsync(int id);

    }
}
