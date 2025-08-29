using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BarTicaret.Application.Products;
using BarTicaret.Application.Lookup;

namespace BarTicaret.WebUI.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IProductService _svc;
    private readonly ILookupService _lookup;

    public AdminController(IProductService svc, ILookupService lookup)
    { _svc = svc; _lookup = lookup; }

    public async Task<IActionResult> Index()
        => View(await _svc.GetAllAsync());

    public async Task<IActionResult> Create()
    {
        var cats = await _lookup.GetCategoriesAsync();
        var mats = await _lookup.GetMaterialsAsync();
        var vm = new AdminCreateVm
        {
            Categories = cats.Select(c => new SelectListItem(c.Ad, c.Id.ToString())).ToList(),
            Materials = mats.Select(m => new SelectListItem(m.Ad, m.Id.ToString())).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdminCreateVm vm)
    {
        if (!ModelState.IsValid)
        {
            await FillLists(vm);
            return View(vm);
        }

        var dto = new ProductCreateDto(vm.Ad, vm.Fiyat, vm.Adet, vm.KategoriId, vm.SeciliMateryalIdleri?.ToList() ?? new());
        try
        {
            await _svc.CreateAsync(dto);
            TempData["ok"] = "Ürün eklendi";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await FillLists(vm);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        TempData["ok"] = "Silindi";
        return RedirectToAction(nameof(Index));
    }

    private async Task FillLists(AdminCreateVm vm)
    {
        var cats = await _lookup.GetCategoriesAsync();
        var mats = await _lookup.GetMaterialsAsync();
        vm.Categories = cats.Select(c => new SelectListItem(c.Ad, c.Id.ToString(), c.Id == vm.KategoriId)).ToList();
        vm.Materials = mats.Select(m => new SelectListItem(m.Ad, m.Id.ToString(),
                             vm.SeciliMateryalIdleri?.Contains(m.Id) ?? false)).ToList();
    }
}

public class AdminCreateVm
{
    public string Ad { get; set; } = "";
    public decimal Fiyat { get; set; }
    public int Adet { get; set; }
    public int KategoriId { get; set; }

    // çoklu seçim
    public int[]? SeciliMateryalIdleri { get; set; }

    // dropdown verileri
    public List<SelectListItem> Categories { get; set; } = new();
    public List<SelectListItem> Materials { get; set; } = new();
}
