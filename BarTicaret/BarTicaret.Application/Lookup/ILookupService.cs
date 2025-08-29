namespace BarTicaret.Application.Lookup;
public interface ILookupService
{
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync();
    Task<IReadOnlyList<MaterialDto>> GetMaterialsAsync();
}
