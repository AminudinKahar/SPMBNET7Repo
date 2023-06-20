using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Services;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._00_Sistem;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Sumber.Data.Seeds;

namespace SPMBNET7.Sumber.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<ExceptionLogger> ExceptionLogger { get; set; }
        public DbSet<HubConnection> HubConnection { get; set; }
        public DbSet<Notification> Notification { get; set; }

        //module
        public DbSet<JKW> JKW { get; set; }
        public DbSet<JCaraBayar> JCaraBayar { get; set; }
        public DbSet<SiModul> SiModul { get; set; }
        public DbSet<JBank> JBank { get; set; }
        public DbSet<JPTJ> JPTJ { get; set; }
        public DbSet<JNegeri> JNegeri { get; set; }
        public DbSet<AkBank> AkBank { get; set; }
        public DbSet<AkCarta> AkCarta { get; set; }
        public DbSet<JJenis> JJenis { get; set; }
        public DbSet<JParas> JParas { get; set; }
        public DbSet<AkAkaun> AkAkaun { get; set; }
        public DbSet<AkTerima> AkTerima { get; set; }
        public DbSet<AkTerima1> AkTerima1 { get; set; }
        public DbSet<AkTerima2> AkTerima2 { get; set; }
        public DbSet<AkTerima3> AkTerima3 { get; set; }
        public DbSet<AkPembekal> AkPembekal { get; set; }
        public DbSet<AkPO> AkPO { get; set; }
        public DbSet<AkPO1> AkPO1 { get; set; }
        public DbSet<AkPO2> AkPO2 { get; set; }
        public DbSet<AkInden> AkInden { get; set; }
        public DbSet<AkInden1> AkInden1 { get; set; }
        public DbSet<AkInden2> AkInden2 { get; set; }
        public DbSet<AkPOLaras> AkPOLaras { get; set; }
        public DbSet<AkPOLaras1> AkPOLaras1 { get; set; }
        public DbSet<AkPOLaras2> AkPOLaras2 { get; set; }
        public DbSet<AkJurnal> AkJurnal { get; set; }
        public DbSet<AkJurnal1> AkJurnal1 { get; set; }
        public DbSet<AppLog> AppLog { get; set; }
        public DbSet<AkBelian> AkBelian { get; set; }
        public DbSet<AkBelian1> AkBelian1 { get; set; }
        public DbSet<AkBelian2> AkBelian2 { get; set; }
        public DbSet<AkPV> AkPV { get; set; }
        public DbSet<AkPV1> AkPV1 { get; set; }
        public DbSet<AkPV2> AkPV2 { get; set; }
        public DbSet<AkPVGanda> AkPVGanda { get; set; }
        public DbSet<SuPekerja> SuPekerja { get; set; }
        public DbSet<SuTanggunganPekerja> SuTanggunganPekerja { get; set; }
        public DbSet<SuAtlet> SuAtlet { get; set; }
        public DbSet<SuJurulatih> SuJurulatih { get; set; }
        public DbSet<SuProfil> SuProfil { get; set; }
        public DbSet<SuProfil1> SuProfil1 { get; set; }
        public DbSet<JBangsa> JBangsa { get; set; }
        public DbSet<JAgama> JAgama { get; set; }
        public DbSet<AbBukuVot> AbBukuVot { get; set; }
        public DbSet<JSukan> JSukan { get; set; }
        public DbSet<JTahapAktiviti> JTahapAktiviti { get; set; }
        public DbSet<SpPendahuluanPelbagai> SpPendahuluanPelbagai { get; set; }
        public DbSet<SpPendahuluanPelbagai1> SpPendahuluanPelbagai1 { get; set; }
        public DbSet<SpPendahuluanPelbagai2> SpPendahuluanPelbagai2 { get; set; }
        public DbSet<JJantina> JJantina { get; set; }
        public DbSet<AkTunaiRuncit> AkTunaiRuncit { get; set; }
        public DbSet<AkTunaiPemegang> AkTunaiPemegang { get; set; }
        public DbSet<AkTunaiCV> AkTunaiCV { get; set; }
        public DbSet<AkTunaiCV1> AkTunaiCV1 { get; set; }
        public DbSet<AkTunaiLejar> AkTunaiLejar { get; set; }
        public DbSet<AkNotaMinta> AkNotaMinta { get; set; }
        public DbSet<AkNotaMinta1> AkNotaMinta1 { get; set; }
        public DbSet<AkNotaMinta2> AkNotaMinta2 { get; set; }
        public DbSet<JBahagian> JBahagian { get; set; }
        public DbSet<AbWaran> AbWaran { get; set; }
        public DbSet<AbWaran1> AbWaran1 { get; set; }
        public DbSet<JPelulus> JPelulus { get; set; }
        public DbSet<JPenyemak> JPenyemak { get; set; }
        public DbSet<JProfilKategori> JProfilKategori { get; set; }
        public DbSet<SiAppInfo> SiAppInfo { get; set; }
        public DbSet<AkCimbEFT> AkCimbEFT { get; set; }
        public DbSet<AkCimbEFT1> AkCimbEFT1 { get; set; }
        public DbSet<AkPenghutang> AkPenghutang { get; set; }
        public DbSet<AkInvois> AkInvois { get; set; }
        public DbSet<AkInvois1> AkInvois1 { get; set; }
        public DbSet<AkInvois2> AkInvois2 { get; set; }
        public DbSet<AkPenyataPemungut> AkPenyataPemungut { get; set; }
        public DbSet<AkPenyataPemungut1> AkPenyataPemungut1 { get; set; }
        public DbSet<AkPenyataPemungut2> AkPenyataPemungut2 { get; set; }
        public DbSet<AkBankRecon> AkBankRecon { get; set; }
        public DbSet<AkBankReconPenyataBank> AkBankReconPenyataBank { get; set; }
        public DbSet<AkPadananPenyata> AkPadananPenyata { get; set; }
        public DbSet<AkNotaDebitKreditBelian> AkNotaDebitKreditBelian { get; set; }
        public DbSet<AkNotaDebitKreditBelian1> AkNotaDebitKreditBelian1 { get; set; }
        public DbSet<AkNotaDebitKreditBelian2> AkNotaDebitKreditBelian2 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SeedQueryFilter();

            modelBuilder.SeedEntity();

            

        }
    }
}
