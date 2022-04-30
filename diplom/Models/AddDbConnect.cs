using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Cloth.Models;
using Microsoft.AspNetCore.Identity;
using Cloth.Models.ViewModel;

namespace Cloth.Models
{
    public class AddDbConnect : IdentityDbContext<AppUser>
    {
        public AddDbConnect(DbContextOptions<AddDbConnect> option) : base(option) { }

        public static async Task CreateAdminUser(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email,
                };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Commentaries> Commentaries { get; set; }
        public DbSet<ClientsData> Clients { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }
        public DbSet<Remains> Remains { get; set; }
        public DbSet<Deliveries> Deliveries { get; set; }
        public DbSet<CartLine> CartLine { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //продукт - бренд - категория - доп инфа - комментарии - остатки
            modelBuilder.Entity<Products>()
                .HasOne(b => b.Brands)
                .WithMany(a => a.Products)
                .HasForeignKey(b => b.BrandId);

            modelBuilder.Entity<Products>()
                .HasOne(b => b.Categories)
                .WithMany(a => a.Products)
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<Products>()
                .HasOne(b => b.Options)
                .WithMany(a => a.Products)
                .HasForeignKey(b => b.OptionsId);

            modelBuilder.Entity<Commentaries>()
                .HasOne(b => b.Product)
                .WithMany(a => a.Commentaries)
                .HasForeignKey(b => b.ProductId);

            modelBuilder.Entity<Remains>()
                .HasOne(a => a.Products)
                .WithMany(a => a.Remains)
                .HasForeignKey(a => a.ProductId);

            ////заказ - продукты - кредитка - клиент
            //modelBuilder.Entity<Order>()
            //    .HasMany(b => b.Products)
            //    .WithMany(a => a.Orders)
            //    .UsingEntity(b => b.ToTable("OrderProducts"));

            //modelBuilder.Entity<Order>()
            //    .HasOne(b => b.CreditCard)
            //    .WithMany(a => a.Orders)
            //    .HasForeignKey(b => b.CardId).OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Order>()
            //    .HasOne(b => b.ClientsData)
            //    .WithMany(a => a.Orders)
            //    .HasForeignKey(b => b.ClientId).OnDelete(DeleteBehavior.NoAction);

            ////остатки - продукты
            //modelBuilder.Entity<Products>()
            //    .HasOne(b => b.Remains)
            //    .WithMany(a => a.Products)
            //    .HasForeignKey(b => b.RemainsId);

            ////поставщики - склад
            //modelBuilder.Entity<Deliveries>()
            //    .HasOne(b => b.Warehouse)
            //    .WithMany(a => a.Deliveries)
            //    .HasForeignKey(b => b.WarehouseId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
