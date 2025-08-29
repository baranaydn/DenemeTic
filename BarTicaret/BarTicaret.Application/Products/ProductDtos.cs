using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarTicaret.Application.Products
{
    public record ProductCreateDto(string Ad, decimal Fiyat, int Adet, int KategoriId, List<int> MateryalIdleri);
    public record ProductDto(int Id, string Ad, decimal Fiyat, int Adet, string Kategori, List<string> Materyaller);

}
