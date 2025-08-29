using Microsoft.EntityFrameworkCore;
using BarTicaret.Domain.Entities;

namespace BarTicaret.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Urun> Urunler => Set<Urun>();
        public DbSet<Kategori> Kategoriler => Set<Kategori>();
        public DbSet<Materyal> Materyaller => Set<Materyal>();
        public DbSet<UrunMateryal> UrunMateryaller => Set<UrunMateryal>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // N-N anahtar
            modelBuilder.Entity<UrunMateryal>()
                .HasKey(x => new { x.UrunId, x.MateryalId });

            // ---- SEEDLER (TEKİL VE BİR KEZ) ----

            // Kategoriler
            modelBuilder.Entity<Kategori>().HasData(
                new Kategori { Id = 1, Ad = "Giyim" },
                new Kategori { Id = 2, Ad = "Aksesuar" },
                new Kategori { Id = 3, Ad = "Elektronik" }
            );

            // Materyaller (tek blok, benzersiz Id'ler)
            modelBuilder.Entity<Materyal>().HasData(
                new Materyal { Id = 1, Ad = "Pamuk" },
                new Materyal { Id = 2, Ad = "Deri" },
                new Materyal { Id = 3, Ad = "Metal" },
                new Materyal { Id = 4, Ad = "Plastik" },
                new Materyal { Id = 5, Ad = "Demir" },
                new Materyal { Id = 6, Ad = "Krom" },
                new Materyal { Id = 7, Ad = "Bakir" }
            );

            // Ürünler
            modelBuilder.Entity<Urun>().HasData(
                new Urun { Id = 1, Ad = "Pamuk Tshirt", Fiyat = 299.90m, Adet = 50, Aktif = true, KategoriId = 1 },
                new Urun { Id = 2, Ad = "Deri Cuzdan", Fiyat = 649.00m, Adet = 20, Aktif = true, KategoriId = 2 },
                new Urun { Id = 3, Ad = "Kulaklik", Fiyat = 899.00m, Adet = 15, Aktif = true, KategoriId = 3 }
            );

            // Ürün–Materyal bağları (benzersiz birleşik anahtar)
            modelBuilder.Entity<UrunMateryal>().HasData(
                new UrunMateryal { UrunId = 1, MateryalId = 1 }, // Tshirt -> Pamuk
                new UrunMateryal { UrunId = 2, MateryalId = 2 }, // Cuzdan -> Deri
                new UrunMateryal { UrunId = 3, MateryalId = 5 }, // Kulaklik -> Demir
                new UrunMateryal { UrunId = 3, MateryalId = 6 }, // Kulaklik -> Krom
                new UrunMateryal { UrunId = 3, MateryalId = 7 }  // Kulaklik -> Bakir
            );
        }
    }
}
