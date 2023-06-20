using Microsoft.AspNetCore.Identity;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._00_Sistem;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;

namespace SPMBNET7.Sumber.Data.Seeds
{
    public static class SeedData
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            var results = userManager.FindByEmailAsync(Init.superAdminEmail).Result;
            if (results == null)
            {
                var user = new ApplicationUser
                {
                    UserName = Init.superAdminEmail,
                    Email = Init.superAdminEmail,
                    Nama = Init.superAdminName
                };

                IdentityResult result = userManager.CreateAsync(user, Init.superAdminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Init.superAdminName).Wait();
                }
            }
            else
            {
                userManager.AddToRoleAsync(results, Init.superAdminName).Wait();

                if (db.UserClaims.FirstOrDefault(uc => uc.UserId == results.Id) == null)
                {
                    List<IdentityUserClaim<string>> claimForUser = new List<IdentityUserClaim<string>>()
                        {
                            new IdentityUserClaim<string>{ UserId = results.Id, ClaimType = "PR001", ClaimValue = "PR001 Penerimaan"},
                            new IdentityUserClaim<string>{ UserId = results.Id, ClaimType = "PV001", ClaimValue = "PV001 Baucer Pembayaran"},
                            new IdentityUserClaim<string>{ UserId = results.Id, ClaimType = "LP001", ClaimValue = "LP001 Laporan - laporan Daftar"}
                        };

                    db.UserClaims.AddRangeAsync(claimForUser).Wait();

                    db.SaveChanges();
                }

            }
        }
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.SiAppInfo.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                CompanyDetails company = new CompanyDetails();

                context.SiAppInfo.AddRange(
                    new SiAppInfo
                    {
                        KodSistem = company.KodSistem,
                        TarVersi = DateTime.Today,
                        NamaSyarikat = company.NamaSyarikat,
                        NoPendaftaran = company.NoPendaftaran,
                        AlamatSyarikat1 = company.AlamatSyarikat1,
                        AlamatSyarikat2 = company.AlamatSyarikat2,
                        AlamatSyarikat3 = company.AlamatSyarikat3,
                        Bandar = company.Bandar,
                        Poskod = company.Poskod,
                        Daerah = company.Daerah,
                        Negeri = company.Negeri,
                        TelSyarikat = company.TelSyarikat,
                        FaksSyarikat = company.FaksSyarikat,
                        EmelSyarikat = company.EmelSyarikat,
                        TarMula = DateTime.Today,
                        CompanyLogoWeb = company.CompanyLogoWeb,
                        CompanyLogoPrintPDF = company.CompanyLogoPrintPDF

                    }
                );
            }

            //// Look for any JKW.
            if (context.JKW.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JKW.AddRange(
                    new JKW
                    {
                        Kod = "100",
                        Perihal = "KUMPULAN WANG UTAMA"
                    }
                );
            }


            if (context.JCaraBayar.Any())
            {
                //return;
            }
            else
            {
                context.JCaraBayar.AddRange(
                    new JCaraBayar
                    {
                        Kod = "T",
                        Perihal = "TUNAI"
                    },
                    new JCaraBayar
                    {
                        Kod = "C",
                        Perihal = "CEK / WANG POS"
                    },
                    new JCaraBayar
                    {
                        Kod = "M",
                        Perihal = "MAKLUMAN KREDIT"
                    },
                    new JCaraBayar
                    {
                        Kod = "E",
                        Perihal = "EFT"
                    },
                    new JCaraBayar
                    {
                        Kod = "I",
                        Perihal = "IBG"
                    },
                    new JCaraBayar
                    {
                        Kod = "K",
                        Perihal = "KAD KREDIT"
                    },
                    new JCaraBayar
                    {
                        Kod = "JP",
                        Perihal = "JOMPAY"
                    }
                );
            }
            if (context.Roles.Any())
            {
                //context.Roles.AddRange(
                //    new IdentityRole { Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper() }
                //    );
            }
            else
            {
                context.Roles.AddRange(
                    new IdentityRole { Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper() },
                   new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
                    new IdentityRole { Name = "Supervisor", NormalizedName = "Supervisor".ToUpper() },
                    new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() }
                    );
            }

            if (context.SiModul.Any())
            {
                //return;
            }
            else
            {
                context.SiModul.AddRange(
                    new SiModul
                    {
                        FuncId = "SY001",
                        FuncName = "Maklumat Pengguna",
                        Model = "User"
                    },
                    new SiModul
                    {
                        FuncId = "SY002",
                        FuncName = "Maklumat Tahap Pengguna",
                        Model = "Roles"
                    },
                    new SiModul
                    {
                        FuncId = "SY003",
                        FuncName = "Log Transaksi Pengguna",
                        Model = "AppLog"
                    },
                    new SiModul
                    {
                        FuncId = "SY004",
                        FuncName = "Info Syarikat",
                        Model = "SiAppInfo"
                    },
                    new SiModul
                    {
                        FuncId = "JD006",
                        FuncName = "Jadual Kumpulan Wang",
                        Model = "JKW"
                    },
                    new SiModul
                    {
                        FuncId = "JD002",
                        FuncName = "Jadual Bahagian",
                        Model = "JBahagian"
                    },
                    new SiModul
                    {
                        FuncId = "JD009",
                        FuncName = "Jadual Akaun Bank",
                        Model = "AkBank"
                    },
                    new SiModul
                    {
                        FuncId = "JD004",
                        FuncName = "Jadual Bank",
                        Model = "JBank"
                    },
                    new SiModul
                    {
                        FuncId = "JD005",
                        FuncName = "Jadual Cara Bayar",
                        Model = "JCaraBayar"
                    },
                    new SiModul
                    {
                        FuncId = "JD001",
                        FuncName = "Jadual Agama",
                        Model = "JAgama"
                    },
                    new SiModul
                    {
                        FuncId = "JD003",
                        FuncName = "Jadual Bangsa",
                        Model = "JBangsa"
                    },
                    new SiModul
                    {
                        FuncId = "JD008",
                        FuncName = "Jadual Sukan",
                        Model = "JSukan"
                    },
                    new SiModul
                    {
                        FuncId = "JD013",
                        FuncName = "Jadual Tahap Aktiviti",
                        Model = "JTahapAktiviti"
                    },
                    new SiModul
                    {
                        FuncId = "JD007",
                        FuncName = "Jadual Negeri",
                        Model = "JNegeri"
                    },
                    new SiModul
                    {
                        FuncId = "JD011",
                        FuncName = "Jadual Penyemak",
                        Model = "JPenyemak"
                    },
                    new SiModul
                    {
                        FuncId = "JD010",
                        FuncName = "Jadual Pelulus",
                        Model = "JPelulus"
                    },
                    new SiModul
                    {
                        FuncId = "JD012",
                        FuncName = "Jadual Profil Kategori",
                        Model = "JProfilKategori"
                    },
                    new SiModul
                    {
                        FuncId = "DF001",
                        FuncName = "Daftar Anggota",
                        Model = "SuPekerja"
                    },
                    new SiModul
                    {
                        FuncId = "DF002",
                        FuncName = "Daftar Pembekal",
                        Model = "AkPembekal"
                    },
                    new SiModul
                    {
                        FuncId = "DF003",
                        FuncName = "Daftar Penghutang",
                        Model = "AkPenghutang"
                    },
                    new SiModul
                    {
                        FuncId = "DF004",
                        FuncName = "Daftar P. Tunai Runcit",
                        Model = "AkTunaiRuncit",
                        FuncIdOld = "TR001"
                    },
                    new SiModul
                    {
                        FuncId = "DF005",
                        FuncName = "Daftar Atlet",
                        Model = "SuAtlet"
                    },
                    new SiModul
                    {
                        FuncId = "DF006",
                        FuncName = "Daftar Jurulatih",
                        Model = "SuJurulatih"
                    },
                    new SiModul
                    {
                        FuncId = "BJ001",
                        FuncName = "Belanjawan Waran",
                        Model = "AbWaran"
                    },
                    new SiModul
                    {
                        FuncId = "AK001",
                        FuncName = "Carta Akaun",
                        Model = "AkCarta"
                    },
                    new SiModul
                    {
                        FuncId = "AK002",
                        FuncName = "Lejar Am",
                        Model = "AkAkaun"
                    },
                    new SiModul
                    {
                        FuncId = "AK003",
                        FuncName = "Buku Vot",
                        Model = "AbBukuVot"
                    },
                    new SiModul
                    {
                        FuncId = "SU001",
                        FuncName = "Profil Atlet",
                        Model = "SuProfil"
                    },
                    new SiModul
                    {
                        FuncId = "SU002",
                        FuncName = "Profil Jurulatih",
                        Model ="SuProfil"
                    },
                    new SiModul
                    {
                        FuncId = "SP001",
                        FuncName = "Pendahuluan Pelbagai",
                        Model = "SpPendahuluanPelbagai"
                    },
                    new SiModul
                    {
                        FuncId = "IN001",
                        FuncName = "Invois Dikeluarkan",
                        Model = "AkInvois"
                    },
                    new SiModul
                    {
                        FuncId = "PR001",
                        FuncName = "Penerimaan",
                        Model = "AkTerima"
                    },
                    new SiModul
                    {
                        FuncId = "PR002",
                        FuncName = "Penyata Pemungut",
                        Model = "AkPenyataPemungut"
                    },
                    new SiModul
                    {
                        FuncId = "NM001",
                        FuncName = "Nota Minta",
                        Model = "AkNotaMinta"
                    },
                    new SiModul
                    {
                        FuncId = "TG001",
                        FuncName = "Pesanan Tempatan",
                        Model = "AkPO"
                    },
                    new SiModul
                    {
                        FuncId = "TG003",
                        FuncName = "Inden Kerja",
                        Model = "AkInden"
                    },
                    new SiModul
                    {
                        FuncId = "PT001",
                        FuncName = "Pelarasan Pesanan Tempatan",
                        Model = "AkPOLaras"
                    },
                    new SiModul
                    {
                        FuncId = "TG002",
                        FuncName = "Invois Pembekal",
                        Model = "AkBelian"
                    },
                    new SiModul
                    {
                        FuncId = "PV001",
                        FuncName = "Baucer Pembayaran",
                        Model = "AkPV"
                    },
                    new SiModul
                    {
                        FuncId = "PV002",
                        FuncName = "Biz Channel",
                        Model = "AkCimbEFT"
                    },
                    new SiModul
                    {
                        FuncId = "JU001",
                        FuncName = "Baucer Jurnal",
                        Model = "AkJurnal"
                    },
                    new SiModul
                    {
                        FuncId = "TR001",
                        FuncName = "Tunai Keluar",
                        Model = "AkTunaiCV",
                        FuncIdOld = "TR002"
                    },
                    new SiModul
                    {
                        FuncId = "LPN001",
                        FuncName = "Laporan Daftar Bil / Nota Minta"
                    },
                    new SiModul
                    {
                        FuncId = "LPV001",
                        FuncName = "Laporan Daftar Baucer"
                    },
                    new SiModul
                    {
                        FuncId = "LPR001",
                        FuncName = "Laporan Daftar Resit"
                    }
                );
            }

            if (context.JJantina.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JJantina.AddRange(
                    new JJantina
                    {
                        Perihal = "LELAKI"
                    },
                    new JJantina
                    {
                        Perihal = "PEREMPUAN"
                    }
                );
            }

            if (context.JBank.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JBank.AddRange(
                    new JBank
                    {
                        Kod = "BIMB",
                        Nama = "BANK ISLAM MALAYSIA BERHAD",
                        KodEFT = "01"
                    },
                    new JBank
                    {
                        Kod = "BMMB",
                        Nama = "BANK MUAMALAT MALAYSIA BERHAD",
                        KodEFT = "02"
                    },
                    new JBank
                    {
                        Kod = "MBB",
                        Nama = "MALAYAN BANKING BERHAD",
                        KodEFT = "03"
                    }
                );
            }

            if (context.JNegeri.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JNegeri.AddRange(
                    new JNegeri
                    {
                        Kod = "01",
                        Perihal = "JOHOR"
                    },
                    new JNegeri
                    {
                        Kod = "02",
                        Perihal = "KEDAH"
                    },
                    new JNegeri
                    {
                        Kod = "03",
                        Perihal = "KELANTAN"
                    },
                    new JNegeri
                    {
                        Kod = "04",
                        Perihal = "MELAKA"
                    },
                    new JNegeri
                    {
                        Kod = "05",
                        Perihal = "NEGERI SEMBILAN"
                    },
                    new JNegeri
                    {
                        Kod = "06",
                        Perihal = "PAHANG"
                    },
                    new JNegeri
                    {
                        Kod = "07",
                        Perihal = "PULAU PINANG"
                    },
                    new JNegeri
                    {
                        Kod = "08",
                        Perihal = "PERAK"
                    },
                    new JNegeri
                    {
                        Kod = "09",
                        Perihal = "PERLIS"
                    },
                    new JNegeri
                    {
                        Kod = "10",
                        Perihal = "SELANGOR"
                    },
                    new JNegeri
                    {
                        Kod = "11",
                        Perihal = "TERENGGANU"
                    },
                    new JNegeri
                    {
                        Kod = "12",
                        Perihal = "SABAH"
                    },
                    new JNegeri
                    {
                        Kod = "13",
                        Perihal = "SARAWAK"
                    },
                    new JNegeri
                    {
                        Kod = "14",
                        Perihal = "WILAYAH PERSEKUTUAN (KUALA LUMPUR)"
                    },
                    new JNegeri
                    {
                        Kod = "15",
                        Perihal = "WILAYAH PERSEKUTUAN (LABUAN)"
                    },
                    new JNegeri
                    {
                        Kod = "16",
                        Perihal = "WILAYAH PERSEKUTUAN (PUTRAJAYA)"
                    }
                );
            }

            if (context.JJenis.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JJenis.AddRange(
                    new JJenis
                    {
                        Kod = "L",
                        Nama = "LIABILITI"
                    },

                    new JJenis
                    {
                        Kod = "E",
                        Nama = "EKUITI"
                    },

                    new JJenis
                    {
                        Kod = "B",
                        Nama = "BELANJA"
                    },
                    new JJenis
                    {
                        Kod = "A",
                        Nama = "ASET"
                    },
                    new JJenis
                    {
                        Kod = "H",
                        Nama = "HASIL"
                    }

                );
            }

            if (context.JParas.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JParas.AddRange(
                    new JParas
                    {
                        Kod = "1"
                    },

                    new JParas
                    {
                        Kod = "2",
                    },

                    new JParas
                    {
                        Kod = "3"
                    },
                    new JParas
                    {
                        Kod = "4"
                    }

                );
            }

            if (context.JAgama.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JAgama.AddRange(
                    new JAgama
                    {
                        Perihal = "ISLAM"
                    },

                    new JAgama
                    {
                        Perihal = "BUDDHA"
                    },

                    new JAgama
                    {
                        Perihal = "KRISTIAN"
                    },
                    new JAgama
                    {
                        Perihal = "HINDU"
                    },
                    new JAgama
                    {
                        Perihal = "TIADA AGAMA"
                    },
                    new JAgama
                    {
                        Perihal = "LAIN-LAIN"
                    }

                );
            }

            if (context.JBangsa.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JBangsa.AddRange(
                    new JBangsa
                    {
                        Perihal = "MELAYU",
                    },

                    new JBangsa
                    {
                        Perihal = "CINA"
                    },

                    new JBangsa
                    {
                        Perihal = "INDIA"
                    },
                    new JBangsa
                    {
                        Perihal = "LAIN-LAIN"
                    }

                );
            }

            context.SaveChanges();

            // add table with relationship
            // JPTJ
            if (context.JPTJ.Any())
            {
                
            }
            else
            {
                if (context.JKW.Any())
                {
                    var jkw = context.JKW.FirstOrDefault();

                    if (jkw != null)
                    {
                        context.JPTJ.AddRange(
                        new JPTJ
                        {
                            Kod = "101",
                            Perihal = "PUSAT TANGGUNGJAWAB UTAMA",
                            JKWId = jkw.Id

                        }
                    );
                    }
                    //return;   // DB has been seeded
                    context.SaveChanges();
                }
            }

            // JBahagian
            if (context.JBahagian.Any())
            {
                
            }
            else
            {
                if (context.JPTJ.Any())
                {
                    var jptj = context.JPTJ.FirstOrDefault();

                    if (jptj != null && jptj.JKWId != null)
                    {
                        context.JBahagian.AddRange(
                        new JBahagian
                        {
                            Kod = "101",
                            Perihal = "BAHAGIAN UTAMA",
                            JPTJId = jptj.Id,
                            JKWId = (int)jptj.JKWId

                        }
                    );
                    }
                    //return;   // DB has been seeded
                    context.SaveChanges();
                }
            }

            if (context.AkCarta.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                var jenisL = context.JJenis.FirstOrDefault(j => j.Kod == "L");
                var jenisE = context.JJenis.FirstOrDefault(j => j.Kod == "E");
                var jenisB = context.JJenis.FirstOrDefault(j => j.Kod == "B");
                var jenisA = context.JJenis.FirstOrDefault(j => j.Kod == "A");
                var jenisH = context.JJenis.FirstOrDefault(j => j.Kod == "H");

                var paras1 = context.JParas.FirstOrDefault(j => j.Kod == "1");
                var paras2 = context.JParas.FirstOrDefault(j => j.Kod == "2");
                var paras3 = context.JParas.FirstOrDefault(j => j.Kod == "3");
                var paras4 = context.JParas.FirstOrDefault(j => j.Kod == "4");

                var jkw = context.JKW.FirstOrDefault();

                context.AkCarta.AddRange(
                    // Aset
                    new AkCarta
                    {
                        Kod = "A10000",
                        Perihal = "ASET SEMASA",
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras1.Id
                    },
                    new AkCarta
                    {
                        Kod = "A11000",
                        Perihal = "WANG TUNAI DAN BAKI BANK",
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras2.Id
                    },
                    new AkCarta
                    {
                        Kod = "A11100",
                        Perihal = "WANG TUNAI DAN BAKI BANK",
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras3.Id
                    },
                    new AkCarta
                    {
                        Kod = "A11101",
                        Perihal = "AKAUN BANK UTAMA",
                        DebitKredit = "D",
                        UmumDetail = "D",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id
                    },
                    //
                    // Belanja
                    new AkCarta
                    {
                        Kod = "B10000",
                        Perihal = "GAJI DAN UPAH",
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras1.Id
                    },
                    new AkCarta
                    {
                        Kod = "B11000",
                        Perihal = "GAJI DAN UPAH",
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras2.Id
                    },
                    new AkCarta
                    {
                        Kod = "B11100",
                        Perihal = "GAJI DAN UPAH KAKITANGAN",
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras3.Id
                    },
                    new AkCarta
                    {
                        Kod = "B11101",
                        Perihal = "GAJI DAN UPAH - KAKITANGAN",
                        DebitKredit = "D",
                        UmumDetail = "D",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id
                    },
                    // LIABILITI
                    new AkCarta
                    {
                        Kod = "L10000",
                        Perihal = "LIABILITI SEMASA",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras1.Id
                    },
                    new AkCarta
                    {
                        Kod = "L11000",
                        Perihal = "AKAUN BELUM BAYAR",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras2.Id
                    },
                    new AkCarta
                    {
                        Kod = "L11100",
                        Perihal = "AKAUN BELUM BAYAR",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras3.Id
                    },
                    new AkCarta
                    {
                        Kod = "L11101",
                        Perihal = "AKAUN BELUM BAYAR",
                        DebitKredit = "K",
                        UmumDetail = "D",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id
                    },
                    // EKUITI
                    new AkCarta
                    {
                        Kod = "E10000",
                        Perihal = "EKUITI",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras1.Id
                    },
                    new AkCarta
                    {
                        Kod = "E11000",
                        Perihal = "RIZAB",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras2.Id
                    },
                    new AkCarta
                    {
                        Kod = "E11100",
                        Perihal = "RIZAB",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras3.Id
                    },
                    new AkCarta
                    {
                        Kod = "E11101",
                        Perihal = "RIZAB PENILAIAN SEMULA TANAH",
                        DebitKredit = "K",
                        UmumDetail = "D",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id
                    },
                    // HASIL
                    new AkCarta
                    {
                        Kod = "H70000",
                        Perihal = "HASIL",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras1.Id
                    },
                    new AkCarta
                    {
                        Kod = "H71000",
                        Perihal = "PELBAGAI TERIMAAN UNTUK PERKHIDMATAN",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras2.Id
                    },
                    new AkCarta
                    {
                        Kod = "H71100",
                        Perihal = "KOMISEN ATAS SUMBANGAN",
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras3.Id
                    },
                    new AkCarta
                    {
                        Kod = "H71101",
                        Perihal = "KOMISEN ATAS SUMBANGAN",
                        DebitKredit = "K",
                        UmumDetail = "D",
                        Baki = 0,
                        IsBajet = true,
                        JKWId = jkw.Id,
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id
                    }
                );
            }
        }
    }
}
