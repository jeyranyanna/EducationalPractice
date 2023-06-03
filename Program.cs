using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HSEPractice2.Areas.Identity.Data;
using HSEPractice2;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WaterparkContextConnection") ?? throw new InvalidOperationException("Connection string 'WaterparkContextConnection' not found.");

builder.Services.AddDbContext<WaterparkContext>(options =>
    options.UseNpgsql(connectionString));

// Проверка почты не обязательна
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WaterparkContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();

#region Авторизация
AddAuthorizationPolicies(builder.Services);

void AddAuthorizationPolicies(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });

    services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
        options.AddPolicy(Constants.Policies.RequireDirector, policy => policy.RequireRole(Constants.Roles.Director));
        options.AddPolicy(Constants.Policies.RequireCashier, policy => policy.RequireRole(Constants.Roles.Cashier));
        options.AddPolicy(Constants.Policies.RequireAccountant, policy => policy.RequireRole(Constants.Roles.Accountant));
        options.AddPolicy(Constants.Policies.RequireInstructor, policy => policy.RequireRole(Constants.Roles.Instructor));
        options.AddPolicy(Constants.Policies.RequireAnimator, policy => policy.RequireRole(Constants.Roles.Animator));
        options.AddPolicy(Constants.Policies.RequireClient, policy => policy.RequireRole(Constants.Roles.Client));
    });
}

#endregion

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();// для работы логина и входа

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "dashboard",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
        .RequireAuthorization(); // Требуется авторизация для использования маршрута "dashboard"
});

//app.MapControllerRoute(
//    name: "default", 
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
