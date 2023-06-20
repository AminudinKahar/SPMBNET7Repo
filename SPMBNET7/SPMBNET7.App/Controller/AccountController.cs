using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.App.Pages.ViewModels.Administrations;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces.Common;
using System.Data;
using System.Drawing;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace SPMBNET7.App.Controller
{
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string syscode = "MAIN";
        public const string modul = "SY001";
        public const string namamodul = "Maklumat Pengguna";

        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(
            ApplicationDbContext db,
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IRepository<SuPekerja, int, string> suPekerja,
            AppLogIRepository<AppLog, int> appLog,
            IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _suPekerjaRepo = suPekerja;
            _appLog = appLog;
            _webHostEnvironment=webHostEnvironment;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Register(string? returnurl = null)
        {
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                //create role
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Supervisor"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            //List<SelectListItem> listItems = new List<SelectListItem>();
            //var role = _roleManager.Roles.ToList();
            //foreach(IdentityRole item in role)
            //{
            //    listItems.Add(new SelectListItem()
            //    {
            //        Value = item.Name,
            //        Text = item.Name
            //    });
            //}
            List<SelectListItem> listItems = new List<SelectListItem>
            {
                //listItems.Add(new SelectListItem()
                //{
                //    Value = "Admin",
                //    Text = "Admin"
                //});
                new SelectListItem()
                {
                    Value = "User",
                    Text = "User"
                }
            };

            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = listItems
            };

            ViewBag.SuPekerja = await _suPekerjaRepo.GetAll();

            ViewBag.JBahagian = _db.JBahagian.ToList();

            return View(registerViewModel);
        }

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetEmailFromSuPekerja(int data)
        {
            try
            {
                var result = await _suPekerjaRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // redirect to login controller
        [HttpGet]
        public async Task<JsonResult> JsonLogOff()
        {
            try
            {
                await LogOff();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //redirect to login end

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnurl = null)
        {


            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (model.SuPekerjaId != null && model.SuPekerjaId != 0)
            {
                //check if user already exist in SuPekerja or not
                //if true then form is valid
                var pekerja = await _suPekerjaRepo.GetById((int)model.SuPekerjaId);
                if (pekerja != null && pekerja.Emel == model.Email)
                {
                    model.Nama = pekerja.Nama;
                    model.Password = "Spmb1234#";
                    // select multiple dropdownlist
                    if (model.SelectedJBahagianList != null)
                    {
                        model.JBahagianList = String.Join(",", model.SelectedJBahagianList);
                    }
                    else
                    {
                        TempData[SD.Error] = "Sila pilih Bahagian bagi pengguna ini.";
                        return View(model);
                    }
                    // select multiple dropdownlist end
                    if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            Nama = pekerja.Nama,
                            SuPekerjaId = model.SuPekerjaId,
                            JBahagianList = model.JBahagianList
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == "Admin")
                            {
                                await _userManager.AddToRoleAsync(user, "Admin");
                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user, "User");
                            }
                            //if (!User.IsInRole("Admin"))
                            //{
                            //    await _signInManager.SignInAsync(user, isPersistent: false);
                            //    return LocalRedirect(returnurl);
                            //}
                            //else
                            //{
                            TempData[SD.Success] = "Data pengguna berjaya ditambah.";
                            _appLog.Insert("Tambah", model.Email + " - " + pekerja.Nama, model.Email, 0, 0, model.SuPekerjaId, modul,syscode,namamodul, user);
                            _db.SaveChanges();
                            return RedirectToAction(nameof(UserController.Index), "User");
                            //}


                        }
                        AddErrors(result);

                    }
                }
                else
                {
                    TempData[SD.Error] = "Pengguna belum didaftar pada Jadual Anggota.";
                }

            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            //listItems.Add(new SelectListItem()
            //{
            //    Value = "Admin",
            //    Text = "Admin"
            //});
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });

            model.RoleList = listItems;

            ViewBag.SuPekerja = await _suPekerjaRepo.GetAll();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            HttpContext.Session.Remove("Username");
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Log_In()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync
                    (
                        model.Emel,
                        model.Katalaluan,
                        model.IngatSaya,
                        lockoutOnFailure: true
                    );

                if (result.Succeeded)
                {
                    var user = _db.applicationUsers.FirstOrDefault(b => b.UserName == model.Emel);
                    var roles = _db.UserRoles.FirstOrDefault(b => b.RoleId == "1f24d001-e893-491e-bbc1-974d2ee2e0f1");
                    if (!string.IsNullOrEmpty(user?.UserName))
                        HttpContext.Session.SetString("Username", user.UserName);
                    return LocalRedirect(returnurl);
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cubaan log masuk tidak sah");
                    return View(model);
                }

            }


            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Emel);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordError");
                }
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                await SendMail(model);

                //await _emailSender.SendEmailAsync(model.Emel, "Set Semula Katalaluan - Identity Manager",
                //    "Sila set semula katalaluan anda dengan melayari pautan ini: <a href=\"" + callbackUrl + "\">link</a>");
                //await _mailServices.SendEmailAsync(model.Emel, "Set Semula Katalaluan Sistem SPMB",
                //    "Sila set semula katalaluan anda dengan melayari pautan ini:<br> <a href=\"" + callbackUrl + "\">"+callbackUrl+"</a>");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        public async Task<int> SendMail(ForgotPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Emel);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user!);

            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user!.Id, code = code }, protocol: HttpContext.Request.Scheme);

            var profileName = _configuration["ProfileName"];

            var html = "<h4>Set Semula Katalaluan</h4>" +
                        "</ br>" +
                        "<p>Sila set semula katalaluan anda dengan melayari pautan ini:</p>" +
                        "<a href=" + callbackUrl + ">" + callbackUrl + "</a>";
            try
            {
                var query = "EXEC msdb.dbo.sp_send_dbmail " +
                            "@profile_name = '" + profileName + "', " +
                            "@recipients = '" + model.Emel + "', " +
                            "@body = '" + html + "', " +
                            "@body_format = 'HTML'," +
                            "@subject = 'Set Semula Katalaluan - Mesej Automatik'; ";

                var parameters = new DynamicParameters();
                parameters.Add("ProfileName", profileName, DbType.String);
                parameters.Add("Email", model.Emel, DbType.String);
                parameters.Add("CallbackUrl", callbackUrl, DbType.String);

                using (var connection = CreateConnection())
                {
                    return await connection.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet]
        public IActionResult ForgotPasswordError()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string? code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                model.Code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                AddErrors(result);

            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [Authorize(Roles = Init.allExceptSuperadminRole)]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _db.applicationUsers.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            ResetPasswordViewModel viewModel = new ResetPasswordViewModel();

            viewModel.Email = user?.Email ?? "";

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = Init.allRole)]
        public async Task<IActionResult> ProfileSetting()
        {
            var user = await _db.applicationUsers.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            ApplicationUserViewModel viewModel = new ApplicationUserViewModel();
            viewModel.id = user!.Id;
            viewModel.Nama = Regex.Replace(user.Nama, "[^a-zA-Z0-9_]+", "");
            viewModel.Id = user.Id;
            viewModel.GambarSediaAda = user.Tandatangan;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Init.allRole)]
        [SupportedOSPlatform("windows")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilSetting(string id, ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = await _db.applicationUsers.FirstOrDefaultAsync(x => x.Id == model.id);
                if (obj != null)
                {
                    if (model.Gambar != null)
                    {
                        if (model.GambarSediaAda != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img\\signature", model.GambarSediaAda);

                            if (Directory.Exists(filePath))
                            {
                                var image = Image.FromFile(filePath);

                                image.Dispose();

                                System.IO.File.Delete(filePath);
                            }

                        }

                    }

                    obj.Tandatangan = ProcessUploadedFile(model);

                    _db.Update(obj);
                    _appLog.Insert("SYSTEM", "SYSTEM - Kemaskini tandatangan", obj?.Email ?? "", 0, 0, obj?.SuPekerjaId ?? 0, modul, syscode, namamodul, obj);
                    await _db.SaveChangesAsync();
                    TempData[SD.Success] = "Kemaskini tandatangan berjaya";
                }
                
            }
            TempData[SD.Error] = "Kemaskini tandatangan gagal";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private string ProcessUploadedFile(ApplicationUserViewModel model)
        {
            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img\\signature");
            string str = Regex.Replace(model.Nama, "[^a-zA-Z0-9_]+", "");
            uniqueFileName = str + ".png";
            //uniqueFileName = model.Gambar.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                if (model.Gambar != null) model.Gambar.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Init.allExceptSuperadminRole)]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                int? pekerjaId = _db.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                if (user != null)
                {
                    model.Code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        TempData[SD.Success] = "Tukar Katalaluan berjaya..!";
                        _appLog.Insert("SYSTEM", "SYSTEM - tukar katalaluan", model.Email, 0, 0, pekerjaId, modul, syscode, namamodul, user);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    AddErrors(result);
                }

            }

            TempData[SD.Error] = "Tukar Katalaluan Gagal..!";
            return View(model);
        }
    }
}
