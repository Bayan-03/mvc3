using EventMVC3.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ≈⁄œ«œ Àﬁ«›…  œ⁄„ «· ﬁÊÌ„ «·„Ì·«œÌ („À· en-US √Ê ar-EG)
var cultureInfo = new CultureInfo("en-US"); // √Ê "ar-EG" ≈–« √—œ  ⁄—÷ ⁄—»Ì ·ﬂ‰ »«· ﬁÊÌ„ «·„Ì·«œÌ

//  ⁄ÌÌ‰ «·Àﬁ«›… ﬂ≈⁄œ«œ «› —«÷Ì
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = new List<CultureInfo> { cultureInfo },
    SupportedUICultures = new List<CultureInfo> { cultureInfo }
};

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EventContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("myConnection")));

var app = builder.Build();

//  ›⁄Ì· «·≈⁄œ«œ«  «·„Õ·Ì…
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
