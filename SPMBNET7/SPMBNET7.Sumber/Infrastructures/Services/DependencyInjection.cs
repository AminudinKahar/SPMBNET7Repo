using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Interfaces._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Sumber.EFRepository._01_Jadual;
using SPMBNET7.Sumber.EFRepository;
using SPMBNET7.Sumber.EFRepository._03_Sumber;
using SPMBNET7.Infrastructure.Carts._02_Akaun.Sessions;
using SPMBNET7.Infrastructure.Carts._91_Permohonan.Sessions;
using SPMBNET7.Infrastructure.Carts._03_Sumber.Sessions;

namespace SPMBNET7.App.Infrastructures.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddTransient<IRepository<JKW, int, string>, JKWRepository>();
            services.AddTransient<IRepository<JBank, int, string>, JBankRepository>();
            services.AddTransient<IRepository<JNegeri, int, string>, JNegeriRepository>();
            services.AddTransient<IRepository<SuPekerja, int, string>, SuPekerjaRepository>();
            services.AddTransient<ListViewIRepository<SuTanggunganPekerja, int>, SuTanggunganPekerjaRepository>();
            services.AddTransient<IRepository<JAgama, int, string>, JAgamaRepository>();
            services.AddTransient<IRepository<JBangsa, int, string>, JBangsaRepository>();
            services.AddTransient<IRepository<JCaraBayar, int, string>, JCaraBayarRepository>();
            services.AddTransient<IRepository<JJantina, int, string>, JJantinaRepository>();
            services.AddTransient<IRepository<JBahagian, int, string>, JBahagianRepository>();
            services.AddTransient<IRepository<JPelulus, int, string>, JPelulusRepository>();
            services.AddTransient<IRepository<JPenyemak, int, string>, JPenyemakRepository>();

            //PENDAHULUAN PELBAGAI
            services.AddTransient<IRepository<JTahapAktiviti, int, string>, JTahapAktivitiRepository>();
            services.AddTransient<IRepository<JSukan, int, string>, JSukanRepository>();

            //SKIM KECEMERLANGAN ATLET DAN ELAUN JURURULATIH
            services.AddTransient<IRepository<SuAtlet, int, string>, SuAtletRepository>();
            services.AddTransient<IRepository<SuJurulatih, int, string>, SuJurulatihRepository>();
            services.AddTransient<IRepository<SuProfil, int, string>, SuProfilAtletRepository>();
            services.AddTransient<IRepository<SuProfil, int, string>, SuProfilJurulatihRepository>();
            services.AddTransient<IRepository<SuProfil1, int, string>, SuProfil1Repository>();
            //SKIM KECEMERLANGAN ATLET DAN ELAUN JURURULATIH END

            services.AddTransient<CustomIRepository<string, int>, CustomRepository>();

            services.AddScoped(ss => SessionCartTerima.GetCart(ss));
            services.AddScoped(ss => SessionCartPendahuluan.GetCart(ss));
            services.AddScoped(ss => SessionCartPO.GetCart(ss));
            services.AddScoped(ss => SessionCartInden.GetCart(ss));
            services.AddScoped(ss => SessionCartPOLaras.GetCart(ss));
            services.AddScoped(ss => SessionCartJurnal.GetCart(ss));
            services.AddScoped(ss => SessionCartBelian.GetCart(ss));
            services.AddScoped(ss => SessionCartPV.GetCart(ss));
            services.AddScoped(ss => SessionCartPekerja.GetCart(ss));
            services.AddScoped(ss => SessionCartTunaiRuncit.GetCart(ss));
            services.AddScoped(ss => SessionCartTunaiCV.GetCart(ss));
            services.AddScoped(ss => SessionCartNotaMinta.GetCart(ss));
            services.AddScoped(ss => SessionCartWaran.GetCart(ss));
            services.AddScoped(ss => SessionCartAtlet.GetCart(ss));
            services.AddScoped(ss => SessionCartJurulatih.GetCart(ss));
            services.AddScoped(ss => SessionCartCimbEFT.GetCart(ss));
            services.AddScoped(ss => SessionCartInvois.GetCart(ss));
            services.AddScoped(ss => SessionCartPenyataPemungut.GetCart(ss));
            services.AddScoped(ss => SessionCartBankRecon.GetCart(ss));
            services.AddScoped(ss => SessionCartNotaDebitKreditBelian.GetCart(ss));

            services.AddAuthorization(options =>
            {
                //Menu Profil
                //Profil Atlet
                options.AddPolicy("SU001", policy => policy.RequireClaim("SU001"));
                options.AddPolicy("SU001C", policy => policy.RequireClaim("SU001C"));
                options.AddPolicy("SU001E", policy => policy.RequireClaim("SU001E"));
                options.AddPolicy("SU001D", policy => policy.RequireClaim("SU001D"));
                options.AddPolicy("SU001P", policy => policy.RequireClaim("SU001P"));
                options.AddPolicy("SU001B", policy => policy.RequireClaim("SU001B"));
                options.AddPolicy("SU001R", policy => policy.RequireClaim("SU001R"));
                options.AddPolicy("SU001T", policy => policy.RequireClaim("SU001T"));
                options.AddPolicy("SU001UT", policy => policy.RequireClaim("SU001UT"));
                //Profil Atlet End
                //Profil Jurulatih
                options.AddPolicy("SU002", policy => policy.RequireClaim("SU002"));
                options.AddPolicy("SU002C", policy => policy.RequireClaim("SU002C"));
                options.AddPolicy("SU002E", policy => policy.RequireClaim("SU002E"));
                options.AddPolicy("SU002D", policy => policy.RequireClaim("SU002D"));
                options.AddPolicy("SU002P", policy => policy.RequireClaim("SU002P"));
                options.AddPolicy("SU002B", policy => policy.RequireClaim("SU002B"));
                options.AddPolicy("SU002R", policy => policy.RequireClaim("SU002R"));
                options.AddPolicy("SU002T", policy => policy.RequireClaim("SU002T"));
                options.AddPolicy("SU002UT", policy => policy.RequireClaim("SU002UT"));
                //Profil Jurulatih End
                //Menu Daftar
                //Anggota
                options.AddPolicy("DF001", policy => policy.RequireClaim("DF001"));
                options.AddPolicy("DF001C", policy => policy.RequireClaim("DF001C"));
                options.AddPolicy("DF001E", policy => policy.RequireClaim("DF001E"));
                options.AddPolicy("DF001D", policy => policy.RequireClaim("DF001D"));
                options.AddPolicy("DF001B", policy => policy.RequireClaim("DF001B"));
                options.AddPolicy("DF001R", policy => policy.RequireClaim("DF001R"));
                //Anggota End
                //Atlet
                options.AddPolicy("DF005", policy => policy.RequireClaim("DF005"));
                options.AddPolicy("DF005C", policy => policy.RequireClaim("DF005C"));
                options.AddPolicy("DF005E", policy => policy.RequireClaim("DF005E"));
                options.AddPolicy("DF005D", policy => policy.RequireClaim("DF005D"));
                options.AddPolicy("DF005B", policy => policy.RequireClaim("DF005B"));
                options.AddPolicy("DF005R", policy => policy.RequireClaim("DF005R"));
                //Atlet End
                //Jurulatih
                options.AddPolicy("DF006", policy => policy.RequireClaim("DF006"));
                options.AddPolicy("DF006C", policy => policy.RequireClaim("DF006C"));
                options.AddPolicy("DF006E", policy => policy.RequireClaim("DF006E"));
                options.AddPolicy("DF006D", policy => policy.RequireClaim("DF006D"));
                options.AddPolicy("DF006B", policy => policy.RequireClaim("DF006B"));
                options.AddPolicy("DF006R", policy => policy.RequireClaim("DF006R"));
                //Jurulatih End
                //Laporan
                options.AddPolicy("LP001", policy => policy.RequireClaim("LP001"));
                //Laporan End
            });

            return services;
        }
    }
}
