using Microsoft.EntityFrameworkCore;
using Cloth.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
string ConnectionString =  builder.Configuration.GetConnectionString("MyConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AddDbConnect>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AddDbConnect>().AddDefaultTokenProviders();
builder.Services.AddMvc(s => s.EnableEndpointRouting = false);

builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMemoryCache();
builder.Services.AddSession();



builder.Host.UseDefaultServiceProvider(options => options.ValidateScopes = false) ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePages();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

//app.UseMvcWithDefaultRoute();

app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: null,
        template: "{Category}/Page{productPage}",
        defaults: new {controller = "Home", action = "Catalog" }
        );
    routes.MapRoute(
        name: null,
        template: "Page{productPage:int}",
        defaults: new {controller = "Home", action = "Catalog", productPage = 1}
        );
    routes.MapRoute(
        name : null,
        template: "{category}",
        defaults: new {controller = "Home", action = "Catalog", productPage = 1}
        );
    routes.MapRoute(
        name: null,
        template: "",
        defaults: new {controller = "Home", action = "Index", productPage = 1}
        );
    routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
});

AddDbConnect.CreateAdminUser(app.Services, app.Configuration).Wait();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
