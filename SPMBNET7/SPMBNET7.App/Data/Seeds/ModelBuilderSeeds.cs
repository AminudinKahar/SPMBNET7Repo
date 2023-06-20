using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness.Models.Modules._04_Aset;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;

namespace SPMBNET7.App.Data.Seeds
{
    public static class ModelBuilderSeeds
    {
        public static void SeedQueryFilter(this ModelBuilder modelBuilder)
        {
            //load item without soft delete
            modelBuilder.Entity<JKW>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JPTJ>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JBahagian>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JCaraBayar>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkBank>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JBank>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JNegeri>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JAgama>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JBangsa>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JSukan>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JTahapAktiviti>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JJantina>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPembekal>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<SuPekerja>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkCarta>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkTunaiPemegang>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JJenis>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JParas>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JPelulus>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JPenyemak>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JProfilKategori>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPenghutang>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);

            //Terimaan
            modelBuilder.Entity<AkTerima>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPenyataPemungut>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            // Terimaan End

            //Baucer
            modelBuilder.Entity<AkPV>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkJurnal>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            // Baucer End
            //Tanggungan
            modelBuilder.Entity<AkNotaMinta>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPO>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkInden>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPOLaras>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkBelian>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Tanggungan End
            //Tunai Runcit
            modelBuilder.Entity<AkTunaiCV>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Tunai Runcit End
            //Permohonan
            modelBuilder.Entity<SpPendahuluanPelbagai>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Permohonan End
            //Belanjawan
            modelBuilder.Entity<AbWaran>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Belanjawan end
            //Profil Atlet & Jurulatih
            modelBuilder.Entity<SuProfil>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Profil Atlet & Jurulatih end
            //Invois
            modelBuilder.Entity<AkInvois>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Invois end
            //Nota Debit Kredit Belian
            modelBuilder.Entity<AkNotaDebitKreditBelian>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Nota Debit Kredit Belian end
            //BankRecon
            modelBuilder.Entity<AkBankRecon>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //BankRecon end
        }
        public static void SeedEntity(this ModelBuilder modelBuilder)
        {
            //AsAset
            modelBuilder.Entity<AsAset>()
                .HasOne(e => e.JBahagian)
                .WithMany(e => e.AsAset)
                .HasForeignKey(e => e.JBahagianId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AsAset>()
                .HasOne(e => e.JKW)
                .WithMany(e => e.AsAset)
                .HasForeignKey(e => e.JKWId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            //SuAtlet
            modelBuilder.Entity<SuAtlet>()
                .HasOne(e => e.JBank)
                .WithMany(c => c.SuAtlet)
                .HasForeignKey(m => m.JBankId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<SuAtlet>()
                .HasOne(e => e.JCaraBayar)
                .WithMany(c => c.SuAtlet)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<SuAtlet>()
                .HasOne(e => e.JNegeri)
                .WithMany(c => c.SuAtlet)
                .HasForeignKey(m => m.JNegeriId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<SuAtlet>()
                .HasOne(e => e.JSukan)
                .WithMany(c => c.SuAtlet)
                .HasForeignKey(m => m.JSukanId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //SuAtlet end

            //SuJurulatih
            modelBuilder.Entity<SuJurulatih>()
                .HasOne(e => e.JBank)
                .WithMany(c => c.SuJurulatih)
                .HasForeignKey(m => m.JBankId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<SuJurulatih>()
                .HasOne(e => e.JCaraBayar)
                .WithMany(c => c.SuJurulatih)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<SuJurulatih>()
                .HasOne(e => e.JNegeri)
                .WithMany(c => c.SuJurulatih)
                .HasForeignKey(m => m.JNegeriId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<SuJurulatih>()
                .HasOne(e => e.JSukan)
                .WithMany(c => c.SuJurulatih)
                .HasForeignKey(m => m.JSukanId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //SuJurulatih end

            //SuPekerja
            modelBuilder.Entity<SuTanggunganPekerja>()
                .HasOne(e => e.SuPekerja)
                .WithMany(c => c.SuTanggungan)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //SuPekerja end

            //AkInden
            modelBuilder.Entity<AkInden1>()
                .HasOne(e => e.AkCarta)
                .WithMany(c => c.AkInden1)
                .HasForeignKey(m => m.AkCartaId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkInden end

            //AkBankRecon
            modelBuilder.Entity<AkBankReconPenyataBank>()
                .HasOne(e => e.AkBankRecon)
                .WithMany(c => c.AkBankReconPenyataBank)
                .HasForeignKey(m => m.AkBankReconId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkBankRecon end

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.JBank)
                .WithMany(c => c.AkBank)
                ;

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.JKW)
                .WithMany(c => c.AkBank)
                .OnDelete(DeleteBehavior.Restrict);

            //AbWaran
            modelBuilder.Entity<AbWaran>()
                .HasOne(e => e.JKW)
                .WithMany(c => c.AbWaran)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AbWaran>()
                .HasOne(e => e.JBahagian)
                .WithMany(c => c.AbWaran)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AbWaran1>()
                .HasOne(e => e.AkCarta)
                .WithMany(c => c.AbWaran1)
                .HasForeignKey(m => m.AkCartaId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AbWaran End

            //AbBukuVot
            modelBuilder.Entity<AbBukuVot>()
                .HasOne(e => e.JBahagian)
                .WithMany(c => c.abBukuVot)
                .HasForeignKey(m => m.JBahagianId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            modelBuilder.Entity<AbBukuVot>()
                .HasOne(e => e.JKW)
                .WithMany(c => c.AbBukuVot)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            modelBuilder.Entity<AbBukuVot>()
                .HasOne(e => e.Vot)
                .WithMany(c => c.Vot)
                .HasForeignKey(m => m.VotId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AbBukuVot end

            modelBuilder.Entity<AkCarta>()
                .HasOne(e => e.JKW)
                .WithMany(c => c.AkCarta)
                .OnDelete(DeleteBehavior.Restrict);

            //AkAkaun
            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.AkCarta1)
                    .WithMany(t => t.AkAkaun1)
                    .HasForeignKey(m => m.AkCartaId1)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.AkCarta2!)
                    .WithMany(t => t.AkAkaun2)
                    .HasForeignKey(m => m.AkCartaId2)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkAkaun)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkAkaun)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkAkaun end

            //AkTerima
            modelBuilder.Entity<AkTerima>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima>()
                    .HasOne(m => m.JNegeri)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.JNegeriId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima>()
               .HasOne(m => m.SpPendahuluanPelbagai!)
               .WithMany(t => t.AkTerima)
               .HasForeignKey(m => m.SpPendahuluanPelbagaiId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkTerima1>()
               .HasOne(m => m.AkCarta)
               .WithMany(t => t.AkTerima1)
               .HasForeignKey(m => m.AkCartaId)
               .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkTerima1>()
               .HasOne(m => m.AkTerima)
               .WithMany(t => t.AkTerima1)
               .HasForeignKey(m => m.AkTerimaId)
               .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkTerima2>()
               .HasOne(m => m.AkTerima)
               .WithMany(t => t.AkTerima2)
               .HasForeignKey(m => m.AkTerimaId)
               .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkTerima2>()
               .HasOne(m => m.JCaraBayar)
               .WithMany(t => t.AkTerima2)
               .HasForeignKey(m => m.JCaraBayarId)
               .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkTerima3>()
                .HasOne(m => m.AkInvois)
                .WithMany(t => t.AkTerima3)
                .HasForeignKey(m => m.AkInvoisId)
                .OnDelete(DeleteBehavior.ClientNoAction).IsRequired(false);

            modelBuilder.Entity<AkTerima3>()
                .HasOne(m => m.AkTerima)
                .WithMany(t => t.AkTerima3)
                .HasForeignKey(m => m.AkTerimaId)
                .OnDelete(DeleteBehavior.ClientNoAction).IsRequired(false);
            //AkTerima end
            //AkPO
            modelBuilder.Entity<AkPO>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkPO)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPO>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkPO)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPO>()
                .HasOne(m => m.AkNotaMinta)
                .WithMany(t => t.AkPO)
                .HasForeignKey(m => m.AkNotaMintaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPO>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkPO)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkPO1>()
                    .HasOne(m => m.AkCarta)
                    .WithMany(t => t.AkPO1)
                    .HasForeignKey(m => m.AkCartaId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkPO end
            //AkInden
            modelBuilder.Entity<AkInden>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkInden)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkInden>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkInden)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkInden>()
                .HasOne(m => m.AkNotaMinta)
                .WithMany(t => t.AkInden)
                .HasForeignKey(m => m.AkNotaMintaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkInden>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkInden)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkInden end
            //AkPOLaras
            modelBuilder.Entity<AkPOLaras>()
                    .HasOne(m => m.AkPO)
                    .WithMany(t => t.AkPOLaras)
                    .HasForeignKey(m => m.AkPOId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .IsRequired();

            modelBuilder.Entity<AkPOLaras>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkPOLaras)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPOLaras1>()
                    .HasOne(m => m.AkCarta)
                    .WithMany(t => t.AkPOLaras1)
                    .HasForeignKey(m => m.AkCartaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            //AkPOLaras end
            //AkNotaMinta
            modelBuilder.Entity<AkNotaMinta>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkNotaMinta)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkNotaMinta>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkNotaMinta)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkNotaMinta>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkNotaMinta)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkNotaMinta1>()
                    .HasOne(m => m.AkCarta)
                    .WithMany(t => t.AkNotaMinta1)
                    .HasForeignKey(m => m.AkCartaId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkNotaMinta end
            //AkJurnal
            modelBuilder.Entity<AkJurnal>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkJurnal)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkJurnal1>()
                    .HasOne(m => m.AkCartaDebit)
                    .WithMany(t => t.AkJurnalDebit)
                    .HasForeignKey(m => m.AkCartaDebitId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkJurnal1>()
                    .HasOne(m => m.AkCartaKredit!)
                    .WithMany(t => t.AkJurnalKredit)
                    .HasForeignKey(m => m.AkCartaKreditId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkJurnal1>()
                    .HasOne(m => m.JBahagianDebit)
                    .WithMany(t => t.AkJurnalDebit)
                    .HasForeignKey(m => m.JBahagianDebitId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkJurnal1>()
                    .HasOne(m => m.JBahagianKredit!)
                    .WithMany(t => t.AkJurnalKredit)
                    .HasForeignKey(m => m.JBahagianKreditId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkJurnal end
            //AkBelian

            // Temporal Table
            modelBuilder.Entity<AkBelian>().Property(x => x.ValidFromUTC)
                            .IsRequired()
                            .ValueGeneratedOnAddOrUpdate()
                            .HasDefaultValueSql("SYSUTCDATETIME()");

            modelBuilder.Entity<AkBelian>().Property(x => x.ValidToUTC)
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999')");
            //Temporal Table end

            modelBuilder.Entity<AkBelian>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkBelian)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkBelian>()
                .HasOne(m => m.AkPO)
                .WithMany(t => t.AkBelian)
                .HasForeignKey(m => m.AkPOId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkBelian>()
                .HasOne(m => m.AkPembekal)
                .WithMany(t => t.AkBelian)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkBelian>()
                    .HasOne(m => m.KodObjekAP)
                    .WithMany(t => t.AkBelian)
                    .HasForeignKey(m => m.KodObjekAPId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkBelian>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkBelian)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkBelian1>()
                .HasOne(e => e.AkCarta)
                .WithMany(c => c.AkBelian1)
                .HasForeignKey(m => m.AkCartaId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkBelian end

            //AkInvois
            modelBuilder.Entity<AkInvois>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkInvois)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkInvois>()
                .HasOne(m => m.AkPenghutang)
                .WithMany(t => t.AkInvois)
                .HasForeignKey(m => m.AkPenghutangId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkInvois>()
                    .HasOne(m => m.KodObjekAP)
                    .WithMany(t => t.AkInvois)
                    .HasForeignKey(m => m.KodObjekAPId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkInvois>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkInvois)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkInvois1>()
                    .HasOne(m => m.AkCarta)
                    .WithMany(t => t.AkInvois1)
                    .HasForeignKey(m => m.AkCartaId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkInvois end

            //AkNotaDebitKreditBelian
            modelBuilder.Entity<AkNotaDebitKreditBelian>()
                .HasOne(m => m.JBahagian)
                .WithMany(m => m.AkNotaDebitKreditBelian)
                .HasForeignKey(m => m.JBahagianId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkNotaDebitKreditBelian>()
                .HasOne(m => m.AkBelian)
                .WithMany(m => m.AkNotaDebitKreditBelian)
                .HasForeignKey(m => m.AkBelianId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkNotaDebitKreditBelian1>()
                .HasOne(m => m.AkCarta)
                .WithMany(m => m.AkNotaDebitKreditBelian1)
                .HasForeignKey(m => m.AkCartaId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            //AkNotaDebitKreditBelian End

            //AkPV
            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkPV2>()
                .HasOne(m => m.AkBelian)
                .WithMany(t => t.AkPV2)
                .HasForeignKey(m => m.AkBelianId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.AkPembekal!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.SuPekerja!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkPV)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPV>()
                    .HasOne(m => m.JBank)
                    .WithMany(t => t.AkPV)
                    .HasForeignKey(m => m.JBankId)
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.AkTunaiRuncit!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.AkTunaiRuncitId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.SpPendahuluanPelbagai!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.SpPendahuluanPelbagaiId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkPV)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.JCaraBayar!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV1>()
                .HasOne(m => m.AkCarta)
                .WithMany(t => t.AkPV1)
                .HasForeignKey(m => m.AkCartaId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPV2>()
                .HasOne(m => m.AkBelian)
                .WithMany(t => t.AkPV2)
                .HasForeignKey(m => m.AkBelianId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPV2>()
                .HasOne(m => m.AkPV)
                .WithMany(t => t.AkPV2)
                .HasForeignKey(m => m.AkPVId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPVGanda>()
                .HasOne(m => m.JCaraBayar)
                .WithMany(t => t.AkPVGanda)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPVGanda>()
                .HasOne(m => m.JBank)
                .WithMany(t => t.AkPVGanda)
                .HasForeignKey(m => m.JBankId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPVGanda>()
                .HasOne(m => m.AkPV)
                .WithMany(t => t.AkPVGanda)
                .HasForeignKey(m => m.AkPVId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            //AKPV end

            //Biz Channel (CIMB EFT)

            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.AkPembekal)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.SuPekerja)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.SuAtlet)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.SuAtletId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.SuJurulatih)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.SuJurulatihId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                    .HasOne(m => m.JBank)
                    .WithMany(t => t.AkCimbEFT1)
                    .HasForeignKey(m => m.JBankId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkCimbEFT1>()
                    .HasOne(m => m.AkPV)
                    .WithMany(t => t.AkCimbEFT1)
                    .HasForeignKey(m => m.AkPVId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkCimbEFT1>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkCimbEFT1)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkCimbEFT1>()
                    .HasOne(m => m.JBank)
                    .WithMany(t => t.AkCimbEFT1)
                    .HasForeignKey(m => m.JBankId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkCimbEFT1>()
                    .HasOne(m => m.SuPekerja)
                    .WithMany(t => t.AkCimbEFT1)
                    .HasForeignKey(m => m.SuPekerjaId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkCimbEFT>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkCimbEFT)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            //Biz Channel (CIMB EFT) end

            //Penyata Pemungut

            modelBuilder.Entity<AkPenyataPemungut>()
                .HasOne(m => m.SuPekerja)
                .WithMany(t => t.AkPenyataPemungut)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPenyataPemungut>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkPenyataPemungut)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPenyataPemungut>()
                .HasOne(m => m.JCaraBayar)
                .WithMany(t => t.AkPenyataPemungut)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPenyataPemungut1>()
                .HasOne(m => m.AkCarta)
                .WithMany(t => t.AkPenyataPemungut1)
                .HasForeignKey(m => m.AkCartaId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPenyataPemungut1>()
                .HasOne(m => m.AkPenyataPemungut)
                .WithMany(t => t.AkPenyataPemungut1)
                .HasForeignKey(m => m.AkPenyataPemungutId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPenyataPemungut1>()
                .HasOne(m => m.JBahagian)
                .WithMany(t => t.AkPenyataPemungut1)
                .HasForeignKey(m => m.JBahagianId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            modelBuilder.Entity<AkPenyataPemungut2>()
                .HasOne(m => m.AkPenyataPemungut)
                .WithMany(t => t.AkPenyataPemungut2)
                .HasForeignKey(m => m.AkPenyataPemungutId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            //Penyata Pemungut end

            //SPPENDAHULUAN
            modelBuilder.Entity<SpPendahuluanPelbagai>()
                    .HasOne(m => m.SuPekerja!)
                    .WithMany(t => t.SpPendahuluanPelbagai)
                    .HasForeignKey(m => m.SuPekerjaId)
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SpPendahuluanPelbagai>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.SpPendahuluanPelbagai)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SpPendahuluanPelbagai1>()
                    .HasOne(m => m.JJantina)
                    .WithMany(t => t.SpPendahuluanPelbagai1)
                    .HasForeignKey(m => m.JJantinaId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //SPPENDAHULUAN END

            //AkBank
            modelBuilder.Entity<AkBank>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkBank)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkBank end

            //AkTunaiRuncit
            modelBuilder.Entity<AkTunaiRuncit>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkTunaiRuncit)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<AkTunaiRuncit>()
                    .HasOne(m => m.AkCarta)
                    .WithMany(t => t.AkTunaiRuncit)
                    .HasForeignKey(m => m.AkCartaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            modelBuilder.Entity<AkTunaiRuncit>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkTunaiRuncit)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            //AkTunaiRuncit end
            //AkTunaiLejar
            modelBuilder.Entity<AkTunaiLejar>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkTunaiLejar)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkTunaiLejar>()
                    .HasOne(m => m.AkCarta)
                    .WithMany(t => t.AkTunaiLejar)
                    .HasForeignKey(m => m.AkCartaId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkTunaiLejar>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkTunaiLejar)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkTunaiLejar end

            //AkTunaiCV
            modelBuilder.Entity<AkTunaiCV>()
                .HasOne(m => m.AkPembekal!)
                .WithMany(t => t.AkTunaiCV)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkTunaiCV>()
                .HasOne(m => m.SuPekerja!)
                .WithMany(t => t.AkTunaiCV)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkTunaiCV1>()
                .HasOne(m => m.AkCarta)
                .WithMany(t => t.AkTunaiCV1)
                .HasForeignKey(m => m.AkCartaId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            //AkTunaiCV end

            //AkJurnal
            modelBuilder.Entity<AkJurnal>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkJurnal)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkJurnal1>()
                   .HasOne(m => m.AkCartaKredit)
                   .WithMany(t => t.AkJurnalKredit)
                   .HasForeignKey(m => m.AkCartaKreditId)
                   .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkJurnal1>()
                   .HasOne(m => m.AkCartaDebit)
                   .WithMany(t => t.AkJurnalDebit)
                   .HasForeignKey(m => m.AkCartaDebitId)
                   .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkJurnal1>()
                   .HasOne(m => m.AkJurnal)
                   .WithMany(t => t.AkJurnal1)
                   .HasForeignKey(m => m.AkJurnalId)
                   .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkJurnal1>()
                   .HasOne(m => m.JBahagianKredit)
                   .WithMany(t => t.AkJurnalKredit)
                   .HasForeignKey(m => m.JBahagianKreditId)
                   .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<AkJurnal1>()
                   .HasOne(m => m.JBahagianDebit)
                   .WithMany(t => t.AkJurnalDebit)
                   .HasForeignKey(m => m.JBahagianDebitId)
                   .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //AkJurnal End

            // SUPROFIL
            modelBuilder.Entity<SuProfil>()
            .HasOne(m => m.JKW)
            .WithMany(t => t.SuProfil)
            .HasForeignKey(m => m.JKWId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<SuProfil>()
            .HasOne(m => m.AkCarta)
            .WithMany(t => t.SuProfil)
            .HasForeignKey(m => m.AkCartaId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<SuProfil>()
            .HasOne(m => m.JBahagian)
            .WithMany(t => t.SuProfil)
            .HasForeignKey(m => m.JBahagianId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<SuProfil1>()
            .HasOne(m => m.JCaraBayar)
            .WithMany(t => t.SuProfil1)
            .HasForeignKey(m => m.JCaraBayarId)
            .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<SuProfil1>()
            .HasOne(m => m.JSukan)
            .WithMany(t => t.SuProfil1)
            .HasForeignKey(m => m.JSukanId)
            .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            // SUPROFIL END

            //set default value
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan1).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan2).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan3).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan4).HasDefaultValue("");
        }
    }
}
