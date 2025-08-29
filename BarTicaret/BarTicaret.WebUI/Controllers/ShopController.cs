using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BarTicaret.Application.Products;
using System.ComponentModel.DataAnnotations;
using BarTicaret.WebUI.Services;

namespace BarTicaret.WebUI.Controllers;

public class ShopController : Controller
{
    private readonly IProductService _svc;
    private readonly IAuthService _auth;

    public ShopController(IProductService svc, IAuthService auth)
    {
        _svc = svc; _auth = auth;
    }

    public async Task<IActionResult> Index(string? materyal = null)
    {
        var items = await _svc.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(materyal))
            items = items.Where(p => p.Materyaller.Contains(materyal, StringComparer.OrdinalIgnoreCase)).ToList();

        ViewBag.Materyaller = new[] { "Demir", "Krom", "Bakir", "Plastik", "Pamuk", "Deri", "Metal" };
        return View(items);
    }

    [Authorize] // satin alma icin login gerekli
    public async Task<IActionResult> Buy(int id)
    {
        var product = (await _svc.GetAllAsync()).FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        return View(new BuyVm { ProductId = id, ProductName = product.Ad, Quantity = 1 });
    }

    [Authorize, HttpPost]
    public async Task<IActionResult> Buy(BuyVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        // Parola teyidi
        var username = User.Identity?.Name ?? "";
        if (!_auth.VerifyPassword(username, vm.Password))
        {
            ModelState.AddModelError("", "Şifre yanlış.");
            return View(vm);
        }

        try
        {
            var result = await _svc.SellAsync(vm.ProductId, vm.Quantity);
            TempData["ok"] = $"{vm.Quantity} adet satın alındı. Yeni stok: {result.Adet}";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(vm);
        }
    }
}

public class BuyVm
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    [Range(1, 999)] public int Quantity { get; set; }
    [Required] public string Password { get; set; } = "";
}
