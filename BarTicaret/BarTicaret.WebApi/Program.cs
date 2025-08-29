using BarTicaret.Infrastructure;
using BarTicaret.Application.Products;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (DbContext + Repo)
builder.Services.AddInfrastructure();

// Application Servisleri
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // T�rk�e a��klamalar vs. burada �zelle�tirilebilir
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
