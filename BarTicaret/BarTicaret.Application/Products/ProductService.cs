using BarTicaret.Application.Abstractions;
using BarTicaret.Domain.Entities;

namespace BarTicaret.Application.Products;

public class ProductService : IProductService
{
    private readonly IRepository<Urun> _urunRepo;
    private readonly IRepository<Kategori> _kategoriRepo;
    private readonly IRepository<Materyal> _materyalRepo;

    public ProductService(
        IRepository<Urun> urunRepo,
        IRepository<Kategori> kategoriRepo,
        IRepository<Materyal> materyalRepo)
    {
        _urunRepo = urunRepo;
        _kategoriRepo = kategoriRepo;
        _materyalRepo = materyalRepo;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var urunler = await _urunRepo.ListAsync();

        // basit donusum (gercek projede Include gerekebilir, simdilik basic)
        return urunler.Select(u => new ProductDto(
            u.Id,
            u.Ad,
            u.Fiyat,
            u.Adet,
            u.Kategori?.Ad ?? "Bilinmiyor",
            u.UrunMateryaller.Select(um => um.Materyal.Ad).ToList()
        )).ToList();
    }

    public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Ad))
            throw new ArgumentException("Urun adi bos olamaz.");
        if (dto.Fiyat <= 0)
            throw new ArgumentException("Fiyat 0'dan buyuk olmali.");
        if (dto.Adet < 0)
            throw new ArgumentException("Adet negatif olamaz.");

        var kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId)
            ?? throw new ArgumentException("Kategori bulunamadi.");

        var materyaller = new List<Materyal>();
        foreach (var mid in dto.MateryalIdleri.Distinct())
        {
            var m = await _materyalRepo.GetByIdAsync(mid);
            if (m != null) materyaller.Add(m);
        }

        var urun = new Urun
        {
            Ad = dto.Ad,
            Fiyat = dto.Fiyat,
            Adet = dto.Adet,
            Aktif = true,
            KategoriId = kategori.Id
        };

        urun = await _urunRepo.AddAsync(urun);

        // urun-materyal baglantilari
        urun.UrunMateryaller = materyaller
            .Select(m => new UrunMateryal { UrunId = urun.Id, MateryalId = m.Id })
            .ToList();

        await _urunRepo.UpdateAsync(urun);

        return new ProductDto(
            urun.Id,
            urun.Ad,
            urun.Fiyat,
            urun.Adet,
            kategori.Ad,
            materyaller.Select(m => m.Ad).ToList()
        ); }
        public async Task<ProductDto> SellAsync(int productId, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Adet 0'dan buyuk olmali.");
        var urun = await _urunRepo.GetByIdAsync(productId) ?? throw new ArgumentException("Urun bulunamadi.");
        if (urun.Adet < quantity) throw new InvalidOperationException("Yeterli stok yok.");

        urun.Adet -= quantity;
        await _urunRepo.UpdateAsync(urun);

        // kategori/materyal bilgileri repository'de Include ile geliyor
        return new ProductDto(
            urun.Id, urun.Ad, urun.Fiyat, urun.Adet,
            urun.Kategori?.Ad ?? "Bilinmiyor",
            urun.UrunMateryaller.Select(um => um.Materyal.Ad).ToList()
        );
    }

    public async Task DeleteAsync(int id)
    {
        var urun = await _urunRepo.GetByIdAsync(id) ?? throw new ArgumentException("Urun bulunamadi.");
        await _urunRepo.DeleteAsync(urun);
    }

}

