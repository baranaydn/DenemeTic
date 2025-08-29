using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BarTicaret.Application.Abstractions;
using BarTicaret.Infrastructure.Data;
using BarTicaret.Infrastructure.Repositories;

namespace BarTicaret.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // InMemory DB (hızlı deneme için)
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("BarTicaretDb"));

            // Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            return services;
        }
    }
}
