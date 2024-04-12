using Syncfusion.Blazor;
using PruebaTecnicaABSolutions.Services;
using Microsoft.AspNetCore.Authentication.Cookies; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<IUserServices,UserServices>();
builder.Services.AddTransient<IEncriptService,EncriptService>();
builder.Services.AddTransient<IMenuCategoriesService, MenuCategoriesService>();
builder.Services.AddTransient<IMenuItemsService, MenuItemsService>();
builder.Services.AddTransient<IBusinessService, BusinessService>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(option =>
{
    option.LoginPath = "/Access/Login";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    option.AccessDeniedPath = "/Access/Privacy";
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToController("Index", "Home");



app.Run();
