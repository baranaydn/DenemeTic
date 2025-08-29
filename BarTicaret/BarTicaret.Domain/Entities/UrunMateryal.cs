using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarTicaret.Domain.Entities
{
    public class UrunMateryal
    {
        public int UrunId { get; set; }
        public Urun Urun { get; set; } = default!;

        public int MateryalId { get; set; }
        public Materyal Materyal { get; set; } = default!;
    }
}
