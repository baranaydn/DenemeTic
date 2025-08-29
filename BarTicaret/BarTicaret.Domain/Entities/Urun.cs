using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarTicaret.Domain.Entities
{
    public class Urun
    {
        public int Id { get; set; }

        /// <summary>Urun adi (ASCII, UI'da Turkce gosteririz)</summary>
        public string Ad { get; set; } = default!;

        /// <summary>Fiyat (KDV, doviz vs. basitlik icin yok)</summary>
        public decimal Fiyat { get; set; }

        /// <summary>Stoktaki adet</summary>
        public int Adet { get; set; }

        /// <summary>Listeleme icin aktif/pasif</summary>
        public bool Aktif { get; set; } = true;

        // Kategori iliskisi (1 Kategori - N Urun)
        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; } = default!;

        // Materyaller (N-N)
        public List<UrunMateryal> UrunMateryaller { get; set; } = new();
    }
}
