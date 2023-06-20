using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.Sumber.Data.Infrastructure;
using SPMBNET7.Sumber.Data.Seeds;
using SPMBNET7.Sumber.EFRepository;
using SPMBNET7.Sumber.EFRepository._01_Jadual;
using SPMBNET7.Sumber.EFRepository._03_Sumber;
using SPMBNET7.Sumber.Infrastructures.Hubs;
using SPMBNET7.Sumber.Infrastructures.Services;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions;
using SPMBNET7.Infrastructure.Carts._03_Sumber.Sessions;
using SPMBNET7.Infrastructure.Carts._91_Permohonan.Sessions;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces.Common;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory()
});

var env = builder.Environment;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Connection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddSession();
builder.Services.AddMemoryCache();
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

builder.Services.AddTransient<IRepository<JKW, int, string>, JKWRepository>();
builder.Services.AddTransient<IRepository<JBank, int, string>, JBankRepository>();
builder.Services.AddTransient<IRepository<JNegeri, int, string>, JNegeriRepository>();
builder.Services.AddTransient<AppLogIRepository<AppLog, int>, AppLogRepository>();
builder.Services.AddTransient<IRepository<SuPekerja, int, string>, SuPekerjaRepository>();
builder.Services.AddTransient<ListViewIRepository<SuTanggunganPekerja, int>, SuTanggunganPekerjaRepository>();
builder.Services.AddTransient<IRepository<JAgama, int, string>, JAgamaRepository>();
builder.Services.AddTransient<IRepository<JBangsa, int, string>, JBangsaRepository>();
builder.Services.AddTransient<IRepository<JCaraBayar, int, string>, JCaraBayarRepository>();
builder.Services.AddTransient<IRepository<JJantina, int, string>, JJantinaRepository>();
builder.Services.AddTransient<IRepository<JBahagian, int, string>, JBahagianRepository>();
builder.Services.AddTransient<IRepository<JPTJ, int, string>, JPTJRepository>();
builder.Services.AddTransient<IRepository<JPelulus, int, string>, JPelulusRepository>();
builder.Services.AddTransient<IRepository<JPenyemak, int, string>, JPenyemakRepository>();


//PENDAHULUAN PELBAGAI
builder.Services.AddTransient<IRepository<JTahapAktiviti, int, string>, JTahapAktivitiRepository>();
builder.Services.AddTransient<IRepository<JSukan, int, string>, JSukanRepository>();
//PENDAHULUAN PELBAGAI END

//SKIM KECEMERLANGAN ATLET DAN ELAUN JURURULATIH
builder.Services.AddTransient<IRepository<SuAtlet, int, string>, SuAtletRepository>();
builder.Services.AddTransient<IRepository<SuJurulatih, int, string>, SuJurulatihRepository>();
builder.Services.AddTransient<IRepository<SuProfil, int, string>, SuProfilAtletRepository>();
builder.Services.AddTransient<IRepository<SuProfil, int, string>, SuProfilJurulatihRepository>();
builder.Services.AddTransient<IRepository<SuProfil1, int, string>, SuProfil1Repository>();
//SKIM KECEMERLANGAN ATLET DAN ELAUN JURURULATIH END

builder.Services.AddScoped(ss => SessionCartTerima.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartPendahuluan.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartPO.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartInden.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartPOLaras.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartJurnal.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartBelian.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartPV.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartPekerja.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartTunaiRuncit.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartTunaiCV.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartNotaMinta.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartWaran.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartAtlet.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartJurulatih.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartCimbEFT.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartInvois.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartPenyataPemungut.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartBankRecon.GetCart(ss));
builder.Services.AddScoped(ss => SessionCartNotaDebitKreditBelian.GetCart(ss));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


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