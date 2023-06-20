using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPMBNET7.App.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlRequest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TraceIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HubConnection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubConnection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JAgama",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JAgama", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JBangsa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JBangsa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KodEFT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JBank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JCaraBayar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JCaraBayar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JJantina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perihal2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JJantina", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JJenis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JJenis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JKW",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JKW", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JNegeri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JNegeri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JParas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JParas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JProfilKategori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KadarGeran = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JProfilKategori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JSukan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsElit = table.Column<bool>(type: "bit", nullable: false),
                    IsPembangunan = table.Column<bool>(type: "bit", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JSukan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JTahapAktiviti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JTahapAktiviti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PushNotification",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    NotificationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotification", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SiAppInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodSistem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarVersi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NamaSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoPendaftaran = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlamatSyarikat1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlamatSyarikat2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlamatSyarikat3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Poskod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Daerah = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Negeri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaksSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmelSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMula = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyLogoWeb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogoPrintPDF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlMaintainance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiAppInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiModul",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FuncName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuncIdOld = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiModul", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JPTJ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JPTJ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JPTJ_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkPembekal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodSykt = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NamaSykt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NoPendaftaran = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Bandar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AkaunBank = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BakiAwal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPembekal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPembekal_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPembekal_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPenghutang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodSykt = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NamaSykt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NoPendaftaran = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Bandar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AkaunBank = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BakiAwal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenghutang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenghutang_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPenghutang_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuPekerja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoGaji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoKp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    TelefonRumah = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonBimbit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusKahwin = table.Column<int>(type: "int", nullable: false),
                    BilAnak = table.Column<int>(type: "int", nullable: false),
                    GajiPokok = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarikhMasukKerja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBerhentiKerja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarikhPencen = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    JAgamaId = table.Column<int>(type: "int", nullable: true),
                    JBangsaId = table.Column<int>(type: "int", nullable: true),
                    Jawatan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: true),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuPekerja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuPekerja_JAgama_JAgamaId",
                        column: x => x.JAgamaId,
                        principalTable: "JAgama",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuPekerja_JBangsa_JBangsaId",
                        column: x => x.JBangsaId,
                        principalTable: "JBangsa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuPekerja_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuPekerja_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuPekerja_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkCarta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DebitKredit = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UmumDetail = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Catatan1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Catatan2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Baki = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsBajet = table.Column<bool>(type: "bit", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JJenisId = table.Column<int>(type: "int", nullable: false),
                    JParasId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkCarta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkCarta_JJenis_JJenisId",
                        column: x => x.JJenisId,
                        principalTable: "JJenis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkCarta_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkCarta_JParas_JParasId",
                        column: x => x.JParasId,
                        principalTable: "JParas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuAtlet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodAtlet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoKp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FlStatus = table.Column<int>(type: "int", nullable: false),
                    TarikhAktif = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBerhenti = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    JAgamaId = table.Column<int>(type: "int", nullable: true),
                    JBangsaId = table.Column<int>(type: "int", nullable: true),
                    Jawatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: false),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuAtlet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JAgama_JAgamaId",
                        column: x => x.JAgamaId,
                        principalTable: "JAgama",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuAtlet_JBangsa_JBangsaId",
                        column: x => x.JBangsaId,
                        principalTable: "JBangsa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuAtlet_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuJurulatih",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodJurulatih = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoKp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FlStatus = table.Column<int>(type: "int", nullable: false),
                    TarikhAktif = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBerhenti = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    JAgamaId = table.Column<int>(type: "int", nullable: true),
                    JBangsaId = table.Column<int>(type: "int", nullable: true),
                    Jawatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: false),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsJSMBakat = table.Column<bool>(type: "bit", nullable: false),
                    IsJSMPelapis = table.Column<bool>(type: "bit", nullable: false),
                    IsSukma = table.Column<bool>(type: "bit", nullable: false),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuJurulatih", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JAgama_JAgamaId",
                        column: x => x.JAgamaId,
                        principalTable: "JAgama",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JBangsa_JBangsaId",
                        column: x => x.JBangsaId,
                        principalTable: "JBangsa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JBahagian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JPTJId = table.Column<int>(type: "int", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JBahagian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JBahagian_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JBahagian_JPTJ_JPTJId",
                        column: x => x.JPTJId,
                        principalTable: "JPTJ",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LgDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LgModule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LgOperation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LgNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRujukan = table.Column<int>(type: "int", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SysCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLog_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tandatangan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    JBahagianList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JPelulus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: false),
                    MinAmaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaksAmaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsNotaMinta = table.Column<bool>(type: "bit", nullable: false),
                    IsPO = table.Column<bool>(type: "bit", nullable: false),
                    IsBelian = table.Column<bool>(type: "bit", nullable: false),
                    IsPV = table.Column<bool>(type: "bit", nullable: false),
                    IsPendahuluan = table.Column<bool>(type: "bit", nullable: false),
                    IsInvois = table.Column<bool>(type: "bit", nullable: false),
                    IsLaporanBukuVot = table.Column<bool>(type: "bit", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JPelulus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JPelulus_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JPenyemak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: false),
                    MinAmaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaksAmaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsNotaMinta = table.Column<bool>(type: "bit", nullable: false),
                    IsPO = table.Column<bool>(type: "bit", nullable: false),
                    IsBelian = table.Column<bool>(type: "bit", nullable: false),
                    IsPV = table.Column<bool>(type: "bit", nullable: false),
                    IsPendahuluan = table.Column<bool>(type: "bit", nullable: false),
                    IsInvois = table.Column<bool>(type: "bit", nullable: false),
                    IsLaporanBukuVot = table.Column<bool>(type: "bit", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JPenyemak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JPenyemak_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuTanggunganPekerja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hubungan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoKP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuTanggunganPekerja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuTanggunganPekerja_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbBukuVot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Penerima = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VotId = table.Column<int>(type: "int", nullable: false),
                    Rujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tanggungan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tbs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Baki = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rizab = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Liabiliti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Belanja = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbBukuVot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbBukuVot_AkCarta_VotId",
                        column: x => x.VotId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbBukuVot_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbBukuVot_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbWaran",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlJenisWaran = table.Column<int>(type: "int", nullable: false),
                    FlJenisPindahan = table.Column<int>(type: "int", nullable: false),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbWaran", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbWaran_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbWaran_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkAkaun",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    AkCartaId1 = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AkCartaId2 = table.Column<int>(type: "int", nullable: true),
                    NoRujukan = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NoSlip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TarSlip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tunai = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Bulan = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Ganding = table.Column<int>(type: "int", maxLength: 1, nullable: true),
                    JSukanId = table.Column<int>(type: "int", nullable: true),
                    AkPembekalId = table.Column<int>(type: "int", nullable: true),
                    AkPenghutangId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkAkaun", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkAkaun_AkCarta_AkCartaId1",
                        column: x => x.AkCartaId1,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkAkaun_AkCarta_AkCartaId2",
                        column: x => x.AkCartaId2,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkAkaun_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkAkaun_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    NoAkaun = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    IsBajet = table.Column<bool>(type: "bit", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBank_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkBank_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkBank_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkBank_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiRuncit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KaunterPanjar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HadMaksimum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiRuncit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiRuncit_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiRuncit_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiRuncit_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuProfil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bulan = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlKategori = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuProfil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuProfil_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaMinta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlJenis = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    NoSiri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoCAS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarikhSeksyenKewangan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JPenyemakId = table.Column<int>(type: "int", nullable: true),
                    FlStatusSemak = table.Column<int>(type: "int", nullable: false),
                    TarSemak = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JPelulusId = table.Column<int>(type: "int", nullable: true),
                    FlStatusLulus = table.Column<int>(type: "int", nullable: false),
                    TarLulus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    AkPembekalId = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaMinta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta_JPelulus_JPelulusId",
                        column: x => x.JPelulusId,
                        principalTable: "JPelulus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkNotaMinta_JPenyemak_JPenyemakId",
                        column: x => x.JPenyemakId,
                        principalTable: "JPenyemak",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpPendahuluanPelbagai",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPermohonan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JenisPermohonan = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktiviti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tempat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarSedia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JumKeseluruhan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JPenyemakId = table.Column<int>(type: "int", nullable: true),
                    FlStatusSokong = table.Column<int>(type: "int", nullable: false),
                    TarSokong = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JumSokong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JPelulusId = table.Column<int>(type: "int", nullable: true),
                    FlStatusLulus = table.Column<int>(type: "int", nullable: false),
                    TarLulus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JumLulus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkCartaId = table.Column<int>(type: "int", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    JTahapAktivitiId = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPendahuluanPelbagai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JPelulus_JPelulusId",
                        column: x => x.JPelulusId,
                        principalTable: "JPelulus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JPenyemak_JPenyemakId",
                        column: x => x.JPenyemakId,
                        principalTable: "JPenyemak",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JTahapAktiviti_JTahapAktivitiId",
                        column: x => x.JTahapAktivitiId,
                        principalTable: "JTahapAktiviti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AbWaran1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbWaranId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TK = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbWaran1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbWaran1_AbWaran_AbWaranId",
                        column: x => x.AbWaranId,
                        principalTable: "AbWaran",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbWaran1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbWaran1_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkBankRecon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Bulan = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    BakiPenyata = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    FlMuatNaik = table.Column<int>(type: "int", nullable: false),
                    TarMuatNaik = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsKunci = table.Column<bool>(type: "bit", nullable: false),
                    TarKunci = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBankRecon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBankRecon_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkCimbEFT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPBI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarJana = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarBayar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NamaFail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BilPV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlKategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    FlStatus = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkCimbEFT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkPenyataPemungut",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoDokumen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoSlip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarSlip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: false),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlJenisCek = table.Column<int>(type: "int", nullable: false),
                    BilTerima = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenyataPemungut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkJurnal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    AkTunaiRuncitId = table.Column<int>(type: "int", nullable: true),
                    NoJurnal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JumDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JumKredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Catatan1 = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, defaultValue: ""),
                    Catatan2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: ""),
                    Catatan3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: ""),
                    Catatan4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: ""),
                    Posting = table.Column<int>(type: "int", nullable: false),
                    Cetak = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlJenisJurnal = table.Column<int>(type: "int", nullable: false),
                    FlKategoriPenerima = table.Column<int>(type: "int", nullable: false),
                    FlTunai = table.Column<int>(type: "int", nullable: false),
                    TarTunai = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAKB = table.Column<bool>(type: "bit", nullable: false),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkJurnal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkJurnal_AkTunaiRuncit_AkTunaiRuncitId",
                        column: x => x.AkTunaiRuncitId,
                        principalTable: "AkTunaiRuncit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkJurnal_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkJurnal_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiCV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTunaiRuncitId = table.Column<int>(type: "int", nullable: false),
                    KategoriPenerima = table.Column<int>(type: "int", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    NoCV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    AkPembekalId = table.Column<int>(type: "int", nullable: true),
                    NoKP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Penerima = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiCV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTunaiCV_AkTunaiRuncit_AkTunaiRuncitId",
                        column: x => x.AkTunaiRuncitId,
                        principalTable: "AkTunaiRuncit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiLejar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AkTunaiRuncitId = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Baki = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rekup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiLejar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiLejar_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiLejar_AkTunaiRuncit_AkTunaiRuncitId",
                        column: x => x.AkTunaiRuncitId,
                        principalTable: "AkTunaiRuncit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTunaiLejar_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiLejar_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiPemegang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTunaiRuncitId = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiPemegang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiPemegang_AkTunaiRuncit_AkTunaiRuncitId",
                        column: x => x.AkTunaiRuncitId,
                        principalTable: "AkTunaiRuncit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTunaiPemegang_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuProfil1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuProfilId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmaunSebelum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tunggakan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuAtletId = table.Column<int>(type: "int", nullable: true),
                    SuJurulatihId = table.Column<int>(type: "int", nullable: true),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: true),
                    NoCekEFT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TarCekEFT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuProfil1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuProfil1_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil1_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil1_SuAtlet_SuAtletId",
                        column: x => x.SuAtletId,
                        principalTable: "SuAtlet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuProfil1_SuJurulatih_SuJurulatihId",
                        column: x => x.SuJurulatihId,
                        principalTable: "SuJurulatih",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuProfil1_SuProfil_SuProfilId",
                        column: x => x.SuProfilId,
                        principalTable: "SuProfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkInden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoInden = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBekalan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    TempohSiap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhSiap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    IsInKewangan = table.Column<bool>(type: "bit", nullable: false),
                    AkPembekalId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    AkNotaMintaId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInden_AkNotaMinta_AkNotaMintaId",
                        column: x => x.AkNotaMintaId,
                        principalTable: "AkNotaMinta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkInden_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInden_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInden_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaMinta1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaMintaId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaMinta1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta1_AkNotaMinta_AkNotaMintaId",
                        column: x => x.AkNotaMintaId,
                        principalTable: "AkNotaMinta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaMinta2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaMintaId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaMinta2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta2_AkNotaMinta_AkNotaMintaId",
                        column: x => x.AkNotaMintaId,
                        principalTable: "AkNotaMinta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBekalan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    TempohSiap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhSiap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    IsInKewangan = table.Column<bool>(type: "bit", nullable: false),
                    AkPembekalId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    AkNotaMintaId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPO_AkNotaMinta_AkNotaMintaId",
                        column: x => x.AkNotaMintaId,
                        principalTable: "AkNotaMinta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPO_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPO_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPO_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkPV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoPV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoKP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NoCekAtauEFT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TarCekAtauEFT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    NoRekup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlJenisBaucer = table.Column<int>(type: "int", nullable: false),
                    FlKategoriPenerima = table.Column<int>(type: "int", nullable: false),
                    denganTanggungan = table.Column<bool>(type: "bit", nullable: false),
                    IsGanda = table.Column<bool>(type: "bit", nullable: false),
                    IsAKB = table.Column<bool>(type: "bit", nullable: false),
                    JPenyemakId = table.Column<int>(type: "int", nullable: true),
                    FlStatusSemak = table.Column<int>(type: "int", nullable: false),
                    TarSemak = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JPelulusId = table.Column<int>(type: "int", nullable: true),
                    FlStatusLulus = table.Column<int>(type: "int", nullable: false),
                    TarLulus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    JBankId = table.Column<int>(type: "int", nullable: true),
                    AkPembekalId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: true),
                    AkTunaiRuncitId = table.Column<int>(type: "int", nullable: true),
                    SpPendahuluanPelbagaiId = table.Column<int>(type: "int", nullable: true),
                    SuProfilId = table.Column<int>(type: "int", nullable: true),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    FlTunai = table.Column<int>(type: "int", nullable: false),
                    TarTunai = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPV_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPV_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_AkTunaiRuncit_AkTunaiRuncitId",
                        column: x => x.AkTunaiRuncitId,
                        principalTable: "AkTunaiRuncit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPV_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPV_JPelulus_JPelulusId",
                        column: x => x.JPelulusId,
                        principalTable: "JPelulus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_JPenyemak_JPenyemakId",
                        column: x => x.JPenyemakId,
                        principalTable: "JPenyemak",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                        column: x => x.SpPendahuluanPelbagaiId,
                        principalTable: "SpPendahuluanPelbagai",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV_SuProfil_SuProfilId",
                        column: x => x.SuProfilId,
                        principalTable: "SuProfil",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkTerima",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KodPembayar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NoKp = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Bandar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sebab = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    FlJenisTerima = table.Column<int>(type: "int", nullable: false),
                    FlKategoriPembayar = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    SpPendahuluanPelbagaiId = table.Column<int>(type: "int", nullable: true),
                    AkPenghutangId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTerima_AkPenghutang_AkPenghutangId",
                        column: x => x.AkPenghutangId,
                        principalTable: "AkPenghutang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTerima_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTerima_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTerima_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTerima_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                        column: x => x.SpPendahuluanPelbagaiId,
                        principalTable: "SpPendahuluanPelbagai",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpPendahuluanPelbagai1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpPendahuluanPelbagaiId = table.Column<int>(type: "int", nullable: false),
                    BilAtl = table.Column<int>(type: "int", nullable: false),
                    BilJul = table.Column<int>(type: "int", nullable: false),
                    BilPeg = table.Column<int>(type: "int", nullable: false),
                    BilTek = table.Column<int>(type: "int", nullable: false),
                    BilUru = table.Column<int>(type: "int", nullable: false),
                    Jumlah = table.Column<int>(type: "int", nullable: false),
                    JJantinaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPendahuluanPelbagai1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai1_JJantina_JJantinaId",
                        column: x => x.JJantinaId,
                        principalTable: "JJantina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai1_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                        column: x => x.SpPendahuluanPelbagaiId,
                        principalTable: "SpPendahuluanPelbagai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpPendahuluanPelbagai2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpPendahuluanPelbagaiId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Baris = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kadar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bulan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPendahuluanPelbagai2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai2_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                        column: x => x.SpPendahuluanPelbagaiId,
                        principalTable: "SpPendahuluanPelbagai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkBankReconPenyataBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    AkBankReconId = table.Column<int>(type: "int", nullable: false),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KodTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerihalTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoDokumen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Baki = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPadan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBankReconPenyataBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBankReconPenyataBank_AkBankRecon_AkBankReconId",
                        column: x => x.AkBankReconId,
                        principalTable: "AkBankRecon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkPenyataPemungut1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    AkPenyataPemungutId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenyataPemungut1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut1_AkPenyataPemungut_AkPenyataPemungutId",
                        column: x => x.AkPenyataPemungutId,
                        principalTable: "AkPenyataPemungut",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut1_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkJurnal1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkJurnalId = table.Column<int>(type: "int", nullable: false),
                    Indeks = table.Column<int>(type: "int", nullable: false),
                    JBahagianDebitId = table.Column<int>(type: "int", nullable: true),
                    AkCartaDebitId = table.Column<int>(type: "int", nullable: true),
                    JBahagianKreditId = table.Column<int>(type: "int", nullable: true),
                    AkCartaKreditId = table.Column<int>(type: "int", nullable: true),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkJurnal1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkJurnal1_AkCarta_AkCartaDebitId",
                        column: x => x.AkCartaDebitId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkJurnal1_AkCarta_AkCartaKreditId",
                        column: x => x.AkCartaKreditId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkJurnal1_AkJurnal_AkJurnalId",
                        column: x => x.AkJurnalId,
                        principalTable: "AkJurnal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkJurnal1_JBahagian_JBahagianDebitId",
                        column: x => x.JBahagianDebitId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkJurnal1_JBahagian_JBahagianKreditId",
                        column: x => x.JBahagianKreditId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiCV1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTunaiCVId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiCV1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTunaiCV1_AkTunaiCV_AkTunaiCVId",
                        column: x => x.AkTunaiCVId,
                        principalTable: "AkTunaiCV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkInden1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkIndenId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInden1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInden1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInden1_AkInden_AkIndenId",
                        column: x => x.AkIndenId,
                        principalTable: "AkInden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkInden2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkIndenId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInden2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInden2_AkInden_AkIndenId",
                        column: x => x.AkIndenId,
                        principalTable: "AkInden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkBelian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhTerima = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarikhKewanganTerima = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoInbois = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlTanggungan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlJenisTanggungan = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    AkPOId = table.Column<int>(type: "int", nullable: true),
                    AkIndenId = table.Column<int>(type: "int", nullable: true),
                    KodObjekAPId = table.Column<int>(type: "int", nullable: false),
                    AkPembekalId = table.Column<int>(type: "int", nullable: false),
                    ValidFromUTC = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ValidToUTC = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999')"),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBelian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBelian_AkCarta_KodObjekAPId",
                        column: x => x.KodObjekAPId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkBelian_AkInden_AkIndenId",
                        column: x => x.AkIndenId,
                        principalTable: "AkInden",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkBelian_AkPO_AkPOId",
                        column: x => x.AkPOId,
                        principalTable: "AkPO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkBelian_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkBelian_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkBelian_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkInvois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoInbois = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    JPenyemakId = table.Column<int>(type: "int", nullable: true),
                    FlStatusSemak = table.Column<int>(type: "int", nullable: false),
                    TarSemak = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JPelulusId = table.Column<int>(type: "int", nullable: true),
                    FlStatusLulus = table.Column<int>(type: "int", nullable: false),
                    TarLulus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    AkPOId = table.Column<int>(type: "int", nullable: true),
                    KodObjekAPId = table.Column<int>(type: "int", nullable: false),
                    AkPenghutangId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInvois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInvois_AkCarta_KodObjekAPId",
                        column: x => x.KodObjekAPId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_AkPO_AkPOId",
                        column: x => x.AkPOId,
                        principalTable: "AkPO",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkInvois_AkPenghutang_AkPenghutangId",
                        column: x => x.AkPenghutangId,
                        principalTable: "AkPenghutang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_JPelulus_JPelulusId",
                        column: x => x.JPelulusId,
                        principalTable: "JPelulus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkInvois_JPenyemak_JPenyemakId",
                        column: x => x.JPenyemakId,
                        principalTable: "JPenyemak",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkPO1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPOId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPO1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPO1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPO1_AkPO_AkPOId",
                        column: x => x.AkPOId,
                        principalTable: "AkPO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPO2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPOId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPO2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPO2_AkPO_AkPOId",
                        column: x => x.AkPOId,
                        principalTable: "AkPO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPOLaras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoRujukan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    AkPOId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPOLaras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPOLaras_AkPO_AkPOId",
                        column: x => x.AkPOId,
                        principalTable: "AkPO",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPOLaras_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPOLaras_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkCimbEFT1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    AkCimbEFTId = table.Column<int>(type: "int", nullable: false),
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    FlPenerimaEFT = table.Column<int>(type: "int", nullable: false),
                    AkPembekalId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    SuJurulatihId = table.Column<int>(type: "int", nullable: true),
                    SuAtletId = table.Column<int>(type: "int", nullable: true),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JBankId = table.Column<int>(type: "int", nullable: true),
                    FlStatus = table.Column<int>(type: "int", nullable: true),
                    AkBankId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkCimbEFT1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkCimbEFT_AkCimbEFTId",
                        column: x => x.AkCimbEFTId,
                        principalTable: "AkCimbEFT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_SuAtlet_SuAtletId",
                        column: x => x.SuAtletId,
                        principalTable: "SuAtlet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_SuJurulatih_SuJurulatihId",
                        column: x => x.SuJurulatihId,
                        principalTable: "SuJurulatih",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkPV1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPV1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPV1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV1_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPVGanda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    FlKategoriPenerima = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    SuAtletId = table.Column<int>(type: "int", nullable: true),
                    SuJurulatihId = table.Column<int>(type: "int", nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoKp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoAkaun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JBankId = table.Column<int>(type: "int", nullable: true),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCekAtauEFT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarCekAtauEFT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: true),
                    FlTunai = table.Column<int>(type: "int", nullable: false),
                    TarTunai = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPVGanda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPVGanda_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPVGanda_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPVGanda_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPVGanda_SuAtlet_SuAtletId",
                        column: x => x.SuAtletId,
                        principalTable: "SuAtlet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPVGanda_SuJurulatih_SuJurulatihId",
                        column: x => x.SuJurulatihId,
                        principalTable: "SuJurulatih",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPVGanda_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkTerima1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTerimaId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTerima1_AkTerima_AkTerimaId",
                        column: x => x.AkTerimaId,
                        principalTable: "AkTerima",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkTerima2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTerimaId = table.Column<int>(type: "int", nullable: false),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCek = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    JenisCek = table.Column<int>(type: "int", nullable: false),
                    KodBankCek = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    TempatCek = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NoSlip = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TarSlip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AkPenyataPemungutId = table.Column<int>(type: "int", nullable: true),
                    FlTunai = table.Column<int>(type: "int", nullable: false),
                    TarTunai = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima2_AkTerima_AkTerimaId",
                        column: x => x.AkTerimaId,
                        principalTable: "AkTerima",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTerima2_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkPadananPenyata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkBankReconPenyataBankId = table.Column<int>(type: "int", nullable: false),
                    FlJenis = table.Column<int>(type: "int", nullable: false),
                    AkPVId = table.Column<int>(type: "int", nullable: true),
                    AkPVGandaId = table.Column<int>(type: "int", nullable: true),
                    AkTerima2Id = table.Column<int>(type: "int", nullable: true),
                    AkJurnalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPadananPenyata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPadananPenyata_AkBankReconPenyataBank_AkBankReconPenyataBankId",
                        column: x => x.AkBankReconPenyataBankId,
                        principalTable: "AkBankReconPenyataBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkBelian1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkBelianId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBelian1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBelian1_AkBelian_AkBelianId",
                        column: x => x.AkBelianId,
                        principalTable: "AkBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkBelian1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkBelian2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkBelianId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBelian2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBelian2_AkBelian_AkBelianId",
                        column: x => x.AkBelianId,
                        principalTable: "AkBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaDebitKreditBelian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AkBelianId = table.Column<int>(type: "int", nullable: false),
                    FlJenis = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkCartaId = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaDebitKreditBelian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian_AkBelian_AkBelianId",
                        column: x => x.AkBelianId,
                        principalTable: "AkBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkPV2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HavePO = table.Column<bool>(type: "bit", nullable: false),
                    AkBelianId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPV2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPV2_AkBelian_AkBelianId",
                        column: x => x.AkBelianId,
                        principalTable: "AkBelian",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPV2_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkInvois1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkInvoisId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInvois1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInvois1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois1_AkInvois_AkInvoisId",
                        column: x => x.AkInvoisId,
                        principalTable: "AkInvois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkInvois2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkInvoisId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Baris = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInvois2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInvois2_AkInvois_AkInvoisId",
                        column: x => x.AkInvoisId,
                        principalTable: "AkInvois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkTerima3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTerimaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkInvoisId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima3_AkInvois_AkInvoisId",
                        column: x => x.AkInvoisId,
                        principalTable: "AkInvois",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTerima3_AkTerima_AkTerimaId",
                        column: x => x.AkTerimaId,
                        principalTable: "AkTerima",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkPOLaras1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPOLarasId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPOLaras1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPOLaras1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPOLaras1_AkPOLaras_AkPOLarasId",
                        column: x => x.AkPOLarasId,
                        principalTable: "AkPOLaras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPOLaras2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPOLarasId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPOLaras2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPOLaras2_AkPOLaras_AkPOLarasId",
                        column: x => x.AkPOLarasId,
                        principalTable: "AkPOLaras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPenyataPemungut2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    AkPenyataPemungutId = table.Column<int>(type: "int", nullable: false),
                    AkTerima2Id = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenyataPemungut2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut2_AkPenyataPemungut_AkPenyataPemungutId",
                        column: x => x.AkPenyataPemungutId,
                        principalTable: "AkPenyataPemungut",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut2_AkTerima2_AkTerima2Id",
                        column: x => x.AkTerima2Id,
                        principalTable: "AkTerima2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkJurnalAkPadananPenyata",
                columns: table => new
                {
                    AkJurnalId = table.Column<int>(type: "int", nullable: false),
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkJurnalAkPadananPenyata", x => new { x.AkJurnalId, x.AkPadananPenyataId });
                    table.ForeignKey(
                        name: "FK_AkJurnalAkPadananPenyata_AkJurnal_AkJurnalId",
                        column: x => x.AkJurnalId,
                        principalTable: "AkJurnal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkJurnalAkPadananPenyata_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPadananPenyataAkTerima2",
                columns: table => new
                {
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false),
                    AkTerima2Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPadananPenyataAkTerima2", x => new { x.AkPadananPenyataId, x.AkTerima2Id });
                    table.ForeignKey(
                        name: "FK_AkPadananPenyataAkTerima2_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPadananPenyataAkTerima2_AkTerima2_AkTerima2Id",
                        column: x => x.AkTerima2Id,
                        principalTable: "AkTerima2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPVAkPadananPenyata",
                columns: table => new
                {
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPVAkPadananPenyata", x => new { x.AkPVId, x.AkPadananPenyataId });
                    table.ForeignKey(
                        name: "FK_AkPVAkPadananPenyata_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPVAkPadananPenyata_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPVGandaAkPadananPenyata",
                columns: table => new
                {
                    AkPVGandaId = table.Column<int>(type: "int", nullable: false),
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPVGandaAkPadananPenyata", x => new { x.AkPVGandaId, x.AkPadananPenyataId });
                    table.ForeignKey(
                        name: "FK_AkPVGandaAkPadananPenyata_AkPVGanda_AkPVGandaId",
                        column: x => x.AkPVGandaId,
                        principalTable: "AkPVGanda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPVGandaAkPadananPenyata_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaDebitKreditBelian1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaDebitKreditBelianId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaDebitKreditBelian1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian1_AkNotaDebitKreditBelian_AkNotaDebitKreditBelianId",
                        column: x => x.AkNotaDebitKreditBelianId,
                        principalTable: "AkNotaDebitKreditBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaDebitKreditBelian2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaDebitKreditBelianId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaDebitKreditBelian2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian2_AkNotaDebitKreditBelian_AkNotaDebitKreditBelianId",
                        column: x => x.AkNotaDebitKreditBelianId,
                        principalTable: "AkNotaDebitKreditBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsAset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    JenisAset = table.Column<int>(type: "int", nullable: false),
                    NoAset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkInvois2Id = table.Column<int>(type: "int", nullable: false),
                    AkBelian2Id = table.Column<int>(type: "int", nullable: true),
                    AkInden2Id = table.Column<int>(type: "int", nullable: true),
                    AkNotaDebitKreditBelian2Id = table.Column<int>(type: "int", nullable: true),
                    AkNotaMinta2Id = table.Column<int>(type: "int", nullable: true),
                    AkPO2Id = table.Column<int>(type: "int", nullable: true),
                    AkPOLaras2Id = table.Column<int>(type: "int", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsAset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsAset_AkBelian2_AkBelian2Id",
                        column: x => x.AkBelian2Id,
                        principalTable: "AkBelian2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AsAset_AkInden2_AkInden2Id",
                        column: x => x.AkInden2Id,
                        principalTable: "AkInden2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AsAset_AkInvois2_AkInvois2Id",
                        column: x => x.AkInvois2Id,
                        principalTable: "AkInvois2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsAset_AkNotaDebitKreditBelian2_AkNotaDebitKreditBelian2Id",
                        column: x => x.AkNotaDebitKreditBelian2Id,
                        principalTable: "AkNotaDebitKreditBelian2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AsAset_AkNotaMinta2_AkNotaMinta2Id",
                        column: x => x.AkNotaMinta2Id,
                        principalTable: "AkNotaMinta2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AsAset_AkPO2_AkPO2Id",
                        column: x => x.AkPO2Id,
                        principalTable: "AkPO2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AsAset_AkPOLaras2_AkPOLaras2Id",
                        column: x => x.AkPOLaras2Id,
                        principalTable: "AkPOLaras2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AsAset_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsAset_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_JBahagianId",
                table: "AbBukuVot",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_JKWId",
                table: "AbBukuVot",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_VotId",
                table: "AbBukuVot",
                column: "VotId");

            migrationBuilder.CreateIndex(
                name: "IX_AbWaran_JBahagianId",
                table: "AbWaran",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AbWaran_JKWId",
                table: "AbWaran",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AbWaran1_AbWaranId",
                table: "AbWaran1",
                column: "AbWaranId");

            migrationBuilder.CreateIndex(
                name: "IX_AbWaran1_AkCartaId",
                table: "AbWaran1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AbWaran1_JBahagianId",
                table: "AbWaran1",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_AkCartaId1",
                table: "AkAkaun",
                column: "AkCartaId1");

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_AkCartaId2",
                table: "AkAkaun",
                column: "AkCartaId2");

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_JBahagianId",
                table: "AkAkaun",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_JKWId",
                table: "AkAkaun",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_AkCartaId",
                table: "AkBank",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_JBahagianId",
                table: "AkBank",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_JBankId",
                table: "AkBank",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_JKWId",
                table: "AkBank",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBankRecon_AkBankId",
                table: "AkBankRecon",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBankReconPenyataBank_AkBankReconId",
                table: "AkBankReconPenyataBank",
                column: "AkBankReconId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_AkIndenId",
                table: "AkBelian",
                column: "AkIndenId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_AkPembekalId",
                table: "AkBelian",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_AkPOId",
                table: "AkBelian",
                column: "AkPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_JBahagianId",
                table: "AkBelian",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_JKWId",
                table: "AkBelian",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_KodObjekAPId",
                table: "AkBelian",
                column: "KodObjekAPId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian1_AkBelianId",
                table: "AkBelian1",
                column: "AkBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian1_AkCartaId",
                table: "AkBelian1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian2_AkBelianId",
                table: "AkBelian2",
                column: "AkBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCarta_JJenisId",
                table: "AkCarta",
                column: "JJenisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCarta_JKWId",
                table: "AkCarta",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCarta_JParasId",
                table: "AkCarta",
                column: "JParasId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT_AkBankId",
                table: "AkCimbEFT",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT_SuPekerjaId",
                table: "AkCimbEFT",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkBankId",
                table: "AkCimbEFT1",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkCimbEFTId",
                table: "AkCimbEFT1",
                column: "AkCimbEFTId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkPembekalId",
                table: "AkCimbEFT1",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkPVId",
                table: "AkCimbEFT1",
                column: "AkPVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_JBankId",
                table: "AkCimbEFT1",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_SuAtletId",
                table: "AkCimbEFT1",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_SuJurulatihId",
                table: "AkCimbEFT1",
                column: "SuJurulatihId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_SuPekerjaId",
                table: "AkCimbEFT1",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_AkNotaMintaId",
                table: "AkInden",
                column: "AkNotaMintaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_AkPembekalId",
                table: "AkInden",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_JBahagianId",
                table: "AkInden",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_JKWId",
                table: "AkInden",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden1_AkCartaId",
                table: "AkInden1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden1_AkIndenId",
                table: "AkInden1",
                column: "AkIndenId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden2_AkIndenId",
                table: "AkInden2",
                column: "AkIndenId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_AkPenghutangId",
                table: "AkInvois",
                column: "AkPenghutangId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_AkPOId",
                table: "AkInvois",
                column: "AkPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JBahagianId",
                table: "AkInvois",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JKWId",
                table: "AkInvois",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JPelulusId",
                table: "AkInvois",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JPenyemakId",
                table: "AkInvois",
                column: "JPenyemakId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_KodObjekAPId",
                table: "AkInvois",
                column: "KodObjekAPId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois1_AkCartaId",
                table: "AkInvois1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois1_AkInvoisId",
                table: "AkInvois1",
                column: "AkInvoisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois2_AkInvoisId",
                table: "AkInvois2",
                column: "AkInvoisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal_AkTunaiRuncitId",
                table: "AkJurnal",
                column: "AkTunaiRuncitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal_JBahagianId",
                table: "AkJurnal",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal_JKWId",
                table: "AkJurnal",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_AkCartaDebitId",
                table: "AkJurnal1",
                column: "AkCartaDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_AkCartaKreditId",
                table: "AkJurnal1",
                column: "AkCartaKreditId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_AkJurnalId",
                table: "AkJurnal1",
                column: "AkJurnalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_JBahagianDebitId",
                table: "AkJurnal1",
                column: "JBahagianDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_JBahagianKreditId",
                table: "AkJurnal1",
                column: "JBahagianKreditId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnalAkPadananPenyata_AkPadananPenyataId",
                table: "AkJurnalAkPadananPenyata",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian_AkBelianId",
                table: "AkNotaDebitKreditBelian",
                column: "AkBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian_AkCartaId",
                table: "AkNotaDebitKreditBelian",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian_JBahagianId",
                table: "AkNotaDebitKreditBelian",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian1_AkCartaId",
                table: "AkNotaDebitKreditBelian1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian1_AkNotaDebitKreditBelianId",
                table: "AkNotaDebitKreditBelian1",
                column: "AkNotaDebitKreditBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian2_AkNotaDebitKreditBelianId",
                table: "AkNotaDebitKreditBelian2",
                column: "AkNotaDebitKreditBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_AkPembekalId",
                table: "AkNotaMinta",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JBahagianId",
                table: "AkNotaMinta",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JKWId",
                table: "AkNotaMinta",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JPelulusId",
                table: "AkNotaMinta",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JPenyemakId",
                table: "AkNotaMinta",
                column: "JPenyemakId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta1_AkCartaId",
                table: "AkNotaMinta1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta1_AkNotaMintaId",
                table: "AkNotaMinta1",
                column: "AkNotaMintaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta2_AkNotaMintaId",
                table: "AkNotaMinta2",
                column: "AkNotaMintaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPadananPenyata_AkBankReconPenyataBankId",
                table: "AkPadananPenyata",
                column: "AkBankReconPenyataBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPadananPenyataAkTerima2_AkTerima2Id",
                table: "AkPadananPenyataAkTerima2",
                column: "AkTerima2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AkPembekal_JBankId",
                table: "AkPembekal",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPembekal_JNegeriId",
                table: "AkPembekal",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenghutang_JBankId",
                table: "AkPenghutang",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenghutang_JNegeriId",
                table: "AkPenghutang",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut_AkBankId",
                table: "AkPenyataPemungut",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut_JCaraBayarId",
                table: "AkPenyataPemungut",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut_SuPekerjaId",
                table: "AkPenyataPemungut",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut1_AkCartaId",
                table: "AkPenyataPemungut1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut1_AkPenyataPemungutId",
                table: "AkPenyataPemungut1",
                column: "AkPenyataPemungutId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut1_JBahagianId",
                table: "AkPenyataPemungut1",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut2_AkPenyataPemungutId",
                table: "AkPenyataPemungut2",
                column: "AkPenyataPemungutId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut2_AkTerima2Id",
                table: "AkPenyataPemungut2",
                column: "AkTerima2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO_AkNotaMintaId",
                table: "AkPO",
                column: "AkNotaMintaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO_AkPembekalId",
                table: "AkPO",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO_JBahagianId",
                table: "AkPO",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO_JKWId",
                table: "AkPO",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO1_AkCartaId",
                table: "AkPO1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO1_AkPOId",
                table: "AkPO1",
                column: "AkPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO2_AkPOId",
                table: "AkPO2",
                column: "AkPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras_AkPOId",
                table: "AkPOLaras",
                column: "AkPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras_JBahagianId",
                table: "AkPOLaras",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras_JKWId",
                table: "AkPOLaras",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras1_AkCartaId",
                table: "AkPOLaras1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras1_AkPOLarasId",
                table: "AkPOLaras1",
                column: "AkPOLarasId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras2_AkPOLarasId",
                table: "AkPOLaras2",
                column: "AkPOLarasId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_AkBankId",
                table: "AkPV",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_AkPembekalId",
                table: "AkPV",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_AkTunaiRuncitId",
                table: "AkPV",
                column: "AkTunaiRuncitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JBahagianId",
                table: "AkPV",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JBankId",
                table: "AkPV",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JCaraBayarId",
                table: "AkPV",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JKWId",
                table: "AkPV",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JPelulusId",
                table: "AkPV",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JPenyemakId",
                table: "AkPV",
                column: "JPenyemakId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SpPendahuluanPelbagaiId",
                table: "AkPV",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SuPekerjaId",
                table: "AkPV",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SuProfilId",
                table: "AkPV",
                column: "SuProfilId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV1_AkCartaId",
                table: "AkPV1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV1_AkPVId",
                table: "AkPV1",
                column: "AkPVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV2_AkBelianId",
                table: "AkPV2",
                column: "AkBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV2_AkPVId",
                table: "AkPV2",
                column: "AkPVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVAkPadananPenyata_AkPadananPenyataId",
                table: "AkPVAkPadananPenyata",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_AkPVId",
                table: "AkPVGanda",
                column: "AkPVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_JBankId",
                table: "AkPVGanda",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_JCaraBayarId",
                table: "AkPVGanda",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_SuAtletId",
                table: "AkPVGanda",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_SuJurulatihId",
                table: "AkPVGanda",
                column: "SuJurulatihId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_SuPekerjaId",
                table: "AkPVGanda",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGandaAkPadananPenyata_AkPadananPenyataId",
                table: "AkPVGandaAkPadananPenyata",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_AkBankId",
                table: "AkTerima",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_AkPenghutangId",
                table: "AkTerima",
                column: "AkPenghutangId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_JBahagianId",
                table: "AkTerima",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_JKWId",
                table: "AkTerima",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_JNegeriId",
                table: "AkTerima",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_SpPendahuluanPelbagaiId",
                table: "AkTerima",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima1_AkCartaId",
                table: "AkTerima1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima1_AkTerimaId",
                table: "AkTerima1",
                column: "AkTerimaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima2_AkTerimaId",
                table: "AkTerima2",
                column: "AkTerimaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima2_JCaraBayarId",
                table: "AkTerima2",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima3_AkInvoisId",
                table: "AkTerima3",
                column: "AkInvoisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima3_AkTerimaId",
                table: "AkTerima3",
                column: "AkTerimaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_AkPembekalId",
                table: "AkTunaiCV",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_AkTunaiRuncitId",
                table: "AkTunaiCV",
                column: "AkTunaiRuncitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_SuPekerjaId",
                table: "AkTunaiCV",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV1_AkCartaId",
                table: "AkTunaiCV1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV1_AkTunaiCVId",
                table: "AkTunaiCV1",
                column: "AkTunaiCVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_AkCartaId",
                table: "AkTunaiLejar",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_AkTunaiRuncitId",
                table: "AkTunaiLejar",
                column: "AkTunaiRuncitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_JBahagianId",
                table: "AkTunaiLejar",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_JKWId",
                table: "AkTunaiLejar",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPemegang_AkTunaiRuncitId",
                table: "AkTunaiPemegang",
                column: "AkTunaiRuncitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPemegang_SuPekerjaId",
                table: "AkTunaiPemegang",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiRuncit_AkCartaId",
                table: "AkTunaiRuncit",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiRuncit_JBahagianId",
                table: "AkTunaiRuncit",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiRuncit_JKWId",
                table: "AkTunaiRuncit",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLog_SuPekerjaId",
                table: "AppLog",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_AkBelian2Id",
                table: "AsAset",
                column: "AkBelian2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_AkInden2Id",
                table: "AsAset",
                column: "AkInden2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_AkInvois2Id",
                table: "AsAset",
                column: "AkInvois2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_AkNotaDebitKreditBelian2Id",
                table: "AsAset",
                column: "AkNotaDebitKreditBelian2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_AkNotaMinta2Id",
                table: "AsAset",
                column: "AkNotaMinta2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_AkPO2Id",
                table: "AsAset",
                column: "AkPO2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_AkPOLaras2Id",
                table: "AsAset",
                column: "AkPOLaras2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_JBahagianId",
                table: "AsAset",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AsAset_JKWId",
                table: "AsAset",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SuPekerjaId",
                table: "AspNetUsers",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JBahagian_JKWId",
                table: "JBahagian",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_JBahagian_JPTJId",
                table: "JBahagian",
                column: "JPTJId");

            migrationBuilder.CreateIndex(
                name: "IX_JPelulus_SuPekerjaId",
                table: "JPelulus",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_JPenyemak_SuPekerjaId",
                table: "JPenyemak",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_JPTJ_JKWId",
                table: "JPTJ",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_AkCartaId",
                table: "SpPendahuluanPelbagai",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JBahagianId",
                table: "SpPendahuluanPelbagai",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JKWId",
                table: "SpPendahuluanPelbagai",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JNegeriId",
                table: "SpPendahuluanPelbagai",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JPelulusId",
                table: "SpPendahuluanPelbagai",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JPenyemakId",
                table: "SpPendahuluanPelbagai",
                column: "JPenyemakId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JSukanId",
                table: "SpPendahuluanPelbagai",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JTahapAktivitiId",
                table: "SpPendahuluanPelbagai",
                column: "JTahapAktivitiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_SuPekerjaId",
                table: "SpPendahuluanPelbagai",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai1_JJantinaId",
                table: "SpPendahuluanPelbagai1",
                column: "JJantinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai1_SpPendahuluanPelbagaiId",
                table: "SpPendahuluanPelbagai1",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai2_SpPendahuluanPelbagaiId",
                table: "SpPendahuluanPelbagai2",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JAgamaId",
                table: "SuAtlet",
                column: "JAgamaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JBangsaId",
                table: "SuAtlet",
                column: "JBangsaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JBankId",
                table: "SuAtlet",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JCaraBayarId",
                table: "SuAtlet",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JNegeriId",
                table: "SuAtlet",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JSukanId",
                table: "SuAtlet",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JAgamaId",
                table: "SuJurulatih",
                column: "JAgamaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JBangsaId",
                table: "SuJurulatih",
                column: "JBangsaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JBankId",
                table: "SuJurulatih",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JCaraBayarId",
                table: "SuJurulatih",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JNegeriId",
                table: "SuJurulatih",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JSukanId",
                table: "SuJurulatih",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SuPekerja_JAgamaId",
                table: "SuPekerja",
                column: "JAgamaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuPekerja_JBangsaId",
                table: "SuPekerja",
                column: "JBangsaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuPekerja_JBankId",
                table: "SuPekerja",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_SuPekerja_JCaraBayarId",
                table: "SuPekerja",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_SuPekerja_JNegeriId",
                table: "SuPekerja",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil_AkCartaId",
                table: "SuProfil",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil_JBahagianId",
                table: "SuProfil",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil_JKWId",
                table: "SuProfil",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_JCaraBayarId",
                table: "SuProfil1",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_JSukanId",
                table: "SuProfil1",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_SuAtletId",
                table: "SuProfil1",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_SuJurulatihId",
                table: "SuProfil1",
                column: "SuJurulatihId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_SuProfilId",
                table: "SuProfil1",
                column: "SuProfilId");

            migrationBuilder.CreateIndex(
                name: "IX_SuTanggunganPekerja_SuPekerjaId",
                table: "SuTanggunganPekerja",
                column: "SuPekerjaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbBukuVot");

            migrationBuilder.DropTable(
                name: "AbWaran1");

            migrationBuilder.DropTable(
                name: "AkAkaun");

            migrationBuilder.DropTable(
                name: "AkBelian1");

            migrationBuilder.DropTable(
                name: "AkCimbEFT1");

            migrationBuilder.DropTable(
                name: "AkInden1");

            migrationBuilder.DropTable(
                name: "AkInvois1");

            migrationBuilder.DropTable(
                name: "AkJurnal1");

            migrationBuilder.DropTable(
                name: "AkJurnalAkPadananPenyata");

            migrationBuilder.DropTable(
                name: "AkNotaDebitKreditBelian1");

            migrationBuilder.DropTable(
                name: "AkNotaMinta1");

            migrationBuilder.DropTable(
                name: "AkPadananPenyataAkTerima2");

            migrationBuilder.DropTable(
                name: "AkPenyataPemungut1");

            migrationBuilder.DropTable(
                name: "AkPenyataPemungut2");

            migrationBuilder.DropTable(
                name: "AkPO1");

            migrationBuilder.DropTable(
                name: "AkPOLaras1");

            migrationBuilder.DropTable(
                name: "AkPV1");

            migrationBuilder.DropTable(
                name: "AkPV2");

            migrationBuilder.DropTable(
                name: "AkPVAkPadananPenyata");

            migrationBuilder.DropTable(
                name: "AkPVGandaAkPadananPenyata");

            migrationBuilder.DropTable(
                name: "AkTerima1");

            migrationBuilder.DropTable(
                name: "AkTerima3");

            migrationBuilder.DropTable(
                name: "AkTunaiCV1");

            migrationBuilder.DropTable(
                name: "AkTunaiLejar");

            migrationBuilder.DropTable(
                name: "AkTunaiPemegang");

            migrationBuilder.DropTable(
                name: "AppLog");

            migrationBuilder.DropTable(
                name: "AsAset");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ExceptionLogger");

            migrationBuilder.DropTable(
                name: "HubConnection");

            migrationBuilder.DropTable(
                name: "JProfilKategori");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PushNotification");

            migrationBuilder.DropTable(
                name: "SiAppInfo");

            migrationBuilder.DropTable(
                name: "SiModul");

            migrationBuilder.DropTable(
                name: "SpPendahuluanPelbagai1");

            migrationBuilder.DropTable(
                name: "SpPendahuluanPelbagai2");

            migrationBuilder.DropTable(
                name: "SuProfil1");

            migrationBuilder.DropTable(
                name: "SuTanggunganPekerja");

            migrationBuilder.DropTable(
                name: "AbWaran");

            migrationBuilder.DropTable(
                name: "AkCimbEFT");

            migrationBuilder.DropTable(
                name: "AkJurnal");

            migrationBuilder.DropTable(
                name: "AkPenyataPemungut");

            migrationBuilder.DropTable(
                name: "AkTerima2");

            migrationBuilder.DropTable(
                name: "AkPVGanda");

            migrationBuilder.DropTable(
                name: "AkPadananPenyata");

            migrationBuilder.DropTable(
                name: "AkTunaiCV");

            migrationBuilder.DropTable(
                name: "AkBelian2");

            migrationBuilder.DropTable(
                name: "AkInden2");

            migrationBuilder.DropTable(
                name: "AkInvois2");

            migrationBuilder.DropTable(
                name: "AkNotaDebitKreditBelian2");

            migrationBuilder.DropTable(
                name: "AkNotaMinta2");

            migrationBuilder.DropTable(
                name: "AkPO2");

            migrationBuilder.DropTable(
                name: "AkPOLaras2");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "JJantina");

            migrationBuilder.DropTable(
                name: "AkTerima");

            migrationBuilder.DropTable(
                name: "AkPV");

            migrationBuilder.DropTable(
                name: "SuAtlet");

            migrationBuilder.DropTable(
                name: "SuJurulatih");

            migrationBuilder.DropTable(
                name: "AkBankReconPenyataBank");

            migrationBuilder.DropTable(
                name: "AkInvois");

            migrationBuilder.DropTable(
                name: "AkNotaDebitKreditBelian");

            migrationBuilder.DropTable(
                name: "AkPOLaras");

            migrationBuilder.DropTable(
                name: "AkTunaiRuncit");

            migrationBuilder.DropTable(
                name: "SpPendahuluanPelbagai");

            migrationBuilder.DropTable(
                name: "SuProfil");

            migrationBuilder.DropTable(
                name: "AkBankRecon");

            migrationBuilder.DropTable(
                name: "AkPenghutang");

            migrationBuilder.DropTable(
                name: "AkBelian");

            migrationBuilder.DropTable(
                name: "JSukan");

            migrationBuilder.DropTable(
                name: "JTahapAktiviti");

            migrationBuilder.DropTable(
                name: "AkBank");

            migrationBuilder.DropTable(
                name: "AkInden");

            migrationBuilder.DropTable(
                name: "AkPO");

            migrationBuilder.DropTable(
                name: "AkCarta");

            migrationBuilder.DropTable(
                name: "AkNotaMinta");

            migrationBuilder.DropTable(
                name: "JJenis");

            migrationBuilder.DropTable(
                name: "JParas");

            migrationBuilder.DropTable(
                name: "AkPembekal");

            migrationBuilder.DropTable(
                name: "JBahagian");

            migrationBuilder.DropTable(
                name: "JPelulus");

            migrationBuilder.DropTable(
                name: "JPenyemak");

            migrationBuilder.DropTable(
                name: "JPTJ");

            migrationBuilder.DropTable(
                name: "SuPekerja");

            migrationBuilder.DropTable(
                name: "JKW");

            migrationBuilder.DropTable(
                name: "JAgama");

            migrationBuilder.DropTable(
                name: "JBangsa");

            migrationBuilder.DropTable(
                name: "JBank");

            migrationBuilder.DropTable(
                name: "JCaraBayar");

            migrationBuilder.DropTable(
                name: "JNegeri");
        }
    }
}
