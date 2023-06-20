using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rotativa.AspNetCore;
using SPMBNET7.App.Data;
using SPMBNET7.App.Data.Infrastructure;
using SPMBNET7.App.Data.Seeds;
using SPMBNET7.App.EFRepository;
using SPMBNET7.App.EFRepository._01_Jadual;
using SPMBNET7.App.EFRepository._02_Akaun;
using SPMBNET7.App.EFRepository._03_Sumber;
using SPMBNET7.App.EFRepository._91_Permohonan;
using SPMBNET7.App.Infrastructures.Hubs;
using SPMBNET7.App.Infrastructures.Services;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions;
using SPMBNET7.Infrastructure.Carts._03_Sumber.Sessions;
using SPMBNET7.Infrastructure.Carts._91_Permohonan.Sessions;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory()
});

var env = builder.Environment;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddSignalR();

// DI 
builder.Services.AddTransient<NotificationHub>();
builder.Services.AddTransient<SubscribeNotificationTableDependency>();

builder.Services.AddTransient<UserServices, UserServices>();

builder.Services.AddDbContext<ApplicationDbContext>(
                options => {
                    options.UseSqlServer(connectionString);
                    options.UseTriggers(triggerOptions =>
                    {
                        triggerOptions.AddTrigger<SoftDeleteTrigger>();

                    });

                });


builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider); ;

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireLowercase = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);
    opt.Lockout.MaxFailedAccessAttempts = 3;
}
);
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.AccessDeniedPath = new PathString("/Home/Accessdenied");
    opt.ExpireTimeSpan = TimeSpan.FromSeconds(600);
    opt.LoginPath = "/Account/Login";
    opt.SlidingExpiration = true;
    opt.LogoutPath = "/Account/LogOff";
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

builder.Services.AddInfrastructure();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Pages/Partials/{0}.cshtml" + RazorViewEngine.ViewExtension);
});

var app = builder.Build();

Configure(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // debug shown if error (developer mode on)
    app.UseDeveloperExceptionPage();
    // debug shown if error (developer mode on) end
    //
    // debug hide if error (developer mode off)
    //app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
    // debug hide if error (developer mode off) end

    app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");
RotativaConfiguration.Setup(env.ContentRootPath, "wwwroot/plugins/Rotativa");

app.Run();


void Configure(WebApplication host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
   

    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        //if (dbContext.Database.IsSqlServer())
        //{
        //    dbContext.Database.Migrate();
        //}

        SeedData.Initialize(dbContext);

        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        SeedData.SeedUsers(userManager, dbContext);

        
    }
    catch
    {
        throw;
    }
}