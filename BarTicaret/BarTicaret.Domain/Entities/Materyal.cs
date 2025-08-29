using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarTicaret.Domain.Entities
{
    public class Materyal
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public List<UrunMateryal> UrunMateryaller { get; set; } = new();
    }
}
