using BarTicaret.Application.Abstractions;
using BarTicaret.Domain.Entities;

namespace BarTicaret.Application.Lookup;
public class LookupService : ILookupService
{
    private readonly IRepository<Kategori> _cat;
    private readonly IRepository<Materyal> _mat;
    public LookupService(IRepository<Kategori> cat, IRepository<Materyal> mat) { _cat = cat; _mat = mat; }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
        => (await _cat.ListAsync()).Select(x => new CategoryDto(x.Id, x.Ad)).ToList();

    public async Task<IReadOnlyList<MaterialDto>> GetMaterialsAsync()
        => (await _mat.ListAsync()).Select(x => new MaterialDto(x.Id, x.Ad)).ToList();
}
