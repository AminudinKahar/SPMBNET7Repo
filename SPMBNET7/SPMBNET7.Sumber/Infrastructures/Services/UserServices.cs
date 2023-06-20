using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._00_Sistem;

namespace SPMBNET7.Sumber.Infrastructures.Services
{
    public class UserServices
    {
        private ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserServices(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<CompanyDetails> GetCompanyDetails()
        {
            CompanyDetails company = new CompanyDetails();

            SiAppInfo appInfo = await _db.SiAppInfo.FirstOrDefaultAsync();

            if (appInfo != null)
            {

                company.KodSistem = appInfo.KodSistem;
                company.NamaSyarikat = appInfo.NamaSyarikat;
                company.NoPendaftaran = appInfo.NoPendaftaran;
                company.AlamatSyarikat1 = appInfo.AlamatSyarikat1;
                company.AlamatSyarikat2 = appInfo.AlamatSyarikat2;
                company.AlamatSyarikat3 = appInfo.AlamatSyarikat3;
                company.Bandar = appInfo.Bandar;
                company.Poskod = appInfo.Poskod;
                company.Daerah = appInfo.Daerah;
                company.Negeri = appInfo.Negeri;
                company.TelSyarikat = appInfo.TelSyarikat;
                company.FaksSyarikat = appInfo.FaksSyarikat;
                company.EmelSyarikat = appInfo.EmelSyarikat;
                company.CompanyLogoWeb = "MainLogo_Syarikat.webp";
                company.CompanyLogoPrintPDF = "MainLogo_Syarikat.png";
                company.TarMula = appInfo.TarMula;
            };

            return company;
        }
        public async Task Impersonate(string userId)
        {

            ApplicationUser user = (ApplicationUser)await _userManager.FindByIdAsync(userId);

            if (user != null) 
                await _signInManager.SignInAsync(user, false);
        }

        public bool IsUnderMaintainance()
        {
            SiAppInfo appInfo = _db.SiAppInfo.FirstOrDefault();

            if (appInfo != null)
            {
                if (appInfo.FlMaintainance == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
