using InnerPeace;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<TimeZoneInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<TimeZoneInterceptor>();

    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(interceptor)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging();
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "UserScheme";         
        options.DefaultAuthenticateScheme = "UserScheme";
        options.DefaultChallengeScheme = "UserScheme";
    })
    .AddCookie("DoctorScheme", options =>
    {
        options.LoginPath = "/Account/doctor/login";
        options.AccessDeniedPath = "/Account/access-denied"; 
        options.Cookie.Name = "DoctorCookie";
    })
    .AddCookie("UserScheme", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/access-denied"; 
        options.Cookie.Name = "UserCookie"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();