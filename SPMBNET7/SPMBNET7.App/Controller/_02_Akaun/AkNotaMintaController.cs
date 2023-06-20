using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.App.Infrastructures.Services;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels;
using Microsoft.Build.Evaluation;
using Rotativa.AspNetCore;
using SPMBNET7.Infrastructure.Math;
using SPMBNET7.App.Pages.PrintModels._02_Akaun;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    public class AkNotaMintaController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "NM001";
        public const string namamodul = "Nota Minta";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkNotaMinta, int, string> _akNotaMintaRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly UserServices _userService;
        private CartNotaMinta _cart;

        public AkNotaMintaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkNotaMinta, int, string> akNotaMintaRepository,
            IRepository<AkPembekal, int, string> akPembekal,
            IRepository<JKW, int, string> kwRepo,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            UserServices userService,
            CartNotaMinta cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akNotaMintaRepo = akNotaMintaRepository;
            _akPembekalRepo = akPembekal;
            _kwRepo = kwRepo;
            _abBukuVotRepo = abBukuVotRepository;
            _akCartaRepo = akCartaRepository;
            _userService = userService;
            _cart = cart;
        }

        // GET: AkNotaMinta
        [Authorize(Policy = "NM001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Nota Minta", Value = "NoRujukan" },
                new SelectListItem() { Text = "Nama", Value = "Nama" },
                new SelectListItem() { Text = "No Siri", Value = "NoSiri" }
            };

            if (!string.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }


            var akNotaMinta = await _akNotaMintaRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akNotaMinta = await _akNotaMintaRepo.GetAllIncludeDeletedItems();
            }
            else
            {
                akNotaMinta = akNotaMinta.Where(b => b.UserId == User.Identity!.Name).ToList();
            }

            //var akNotaMinta = await _context.akNotaMinta.ToListAsync();

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akNotaMinta = akNotaMinta.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akNotaMinta = akNotaMinta.Where(s => s.AkPembekal!.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "NoSiri")
                    {
                        foreach (var i in akNotaMinta)
                        {
                            if (string.IsNullOrEmpty(i.NoSiri))
                            {
                                i.NoSiri = "";
                            }
                        }

                        akNotaMinta = akNotaMinta.Where(s => s.NoSiri!.Contains(searchString.ToUpper())).ToList();
                    }


                    ViewBag.SearchData1 = searchString;

                }

                // searching with '%like%' condition end

                // searching with date range condition
                if (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2))
                {
                    if (searchColumn == "Tarikh")
                    {
                        DateTime date1 = DateTime.Parse(searchDate1);
                        DateTime date2 = DateTime.Parse(searchDate2).AddHours(23.99);
                        akNotaMinta = akNotaMinta.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "Tarikh");
            }

            List<AkNotaMintaViewModel> viewModel = new List<AkNotaMintaViewModel>();


            foreach (AkNotaMinta item in akNotaMinta)
            {
                decimal jumlahPerihal = 0;
                if (item.AkNotaMinta2 != null)
                {
                    foreach (AkNotaMinta2 item2 in item.AkNotaMinta2)
                    {
                        jumlahPerihal += item2.Amaun;
                    }
                }
                
                viewModel.Add(new AkNotaMintaViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoRujukan = item.NoRujukan,
                    NoSiri = item.NoSiri,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    NamaSykt = item.AkPembekal?.NamaSykt ?? "",
                    Alamat1 = item.AkPembekal?.Alamat1 ?? "",
                    Tajuk = item.Tajuk,
                    FlHapus = item.FlHapus,
                    FlCetak = item.FlCetak,
                    FlPosting = item.FlPosting,
                    FlStatusSemak = item.FlStatusSemak,
                    FlStatusLulus = item.FlStatusLulus,
                    JumlahPerihal = jumlahPerihal,
                    UserId = item.UserId
                }
                );
            }

            List<JPenyemak> penyemak = _context.JPenyemak
                .Include(x => x.SuPekerja)
                .Where(x => x.IsNotaMinta == true).OrderBy(b => b.SuPekerja!.Nama).ToList();
            ViewBag.JPenyemak = penyemak;

            List<JPelulus> pelulus = _context.JPelulus
                .Include(x => x.SuPekerja)
                .Where(x => x.IsNotaMinta == true).OrderBy(b => b.SuPekerja!.Nama).ToList();
            ViewBag.JPelulus = pelulus;

            return View(viewModel);
        }

        private void PopulateList(int? pekerjaId)
        {
            var user = _context.applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.UserName == User.Identity!.Name);

            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            string[] arr = user!.JBahagianList.Split(',');
            List<JBahagian> bahagianList = new List<JBahagian>();

            if (user.UserName != Init.superAdminEmail)
            {
                foreach (var item in arr)
                {
                    var bahagian = _context.JBahagian.FirstOrDefault(x => x.Id == int.Parse(item));

                    if (bahagian != null) bahagianList.Add(bahagian);

                }
            }
            else
            {
                bahagianList.AddRange(_context.JBahagian.ToList());
            }

            ViewBag.JBahagian = bahagianList;

            var pekerja = _context.SuPekerja.FirstOrDefault(b => b.Id == pekerjaId);

            if (User.IsInRole("SuperAdmin"))
            {
                ViewBag.IdPekerja = 1;
                ViewBag.NamaPekerja = "SuperAdmin";
            }
            else
            {
                ViewBag.IdPekerja = pekerjaId;
                ViewBag.NamaPekerja = pekerja?.Nama ?? "";
            }

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4" && (b.Kod.Substring(0, 1) == "B" || b.Kod.Substring(0, 1) == "A"))
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

        }

        private void PopulateTable(int? id)
        {
            List<AkNotaMinta1> akNotaMinta1Table = _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akNotaMinta1 = akNotaMinta1Table;

            List<AkNotaMinta2> akNotaMinta2Table = _context.AkNotaMinta2
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Bil)
                .ToList();
            ViewBag.akNotaMinta2 = akNotaMinta2Table;
        }
        private void PopulateCart()
        {
            List<AkNotaMinta1> lines1 = _cart.Lines1.ToList();

            foreach (AkNotaMinta1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            List<AkNotaMinta2> lines2 = _cart.Lines2.ToList();

            ViewBag.akNotaMinta1 = lines1;
            ViewBag.akNotaMinta2 = lines2;
        }

        private void PopulateCartFromDb(AkNotaMinta akNotaMinta)
        {
            List<AkNotaMinta1> akNotaMinta1Table = _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == akNotaMinta.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkNotaMinta1 item in akNotaMinta1Table)
            {
                _cart.AddItem1(item.AkNotaMintaId,
                               item.AkCartaId,
                               item.Amaun
                               );
            }

            List<AkNotaMinta2> akNotaMinta2Table = _context.AkNotaMinta2
                .Where(b => b.AkNotaMintaId == akNotaMinta.Id)
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkNotaMinta2 item in akNotaMinta2Table)
            {
                _cart.AddItem2(item.AkNotaMintaId,
                               item.Indek,
                               item.Bil,
                               item.NoStok ?? "",
                               item.Perihal?.ToUpper()?? "",
                               item.Kuantiti,
                               item.Unit ?? "",
                               item.Harga,
                               item.Amaun);
            }
        }

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkNotaMinta1> tbl1 = new List<AkNotaMinta1>();
            var cart1 = _cart.Lines1.ToList();

            if (cart1 != null && cart1.Count() > 0)
            {
                foreach (var item in cart1)
                {
                    tbl1.Add(item);
                }
            }
            ViewBag.akNotaMinta1 = tbl1;
            // table 1 end

            // table 2
            List<AkNotaMinta2> tbl2 = new List<AkNotaMinta2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    tbl2.Add(item);
                }
            }
            ViewBag.akNotaMinta2 = tbl2;
            // table 2 end
        }
        // populate table from cart end

        // GET: AkNotaMinta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkNotaMinta == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetByIdIncludeDeletedItems((int)id);

            // normal user access
            if (User.IsInRole("User"))
            {
                akNotaMinta = await _akNotaMintaRepo.GetById((int)id);
            }

            AkNotaMintaViewModel viewModel = new AkNotaMintaViewModel();

            viewModel.Id = akNotaMinta.Id;
            viewModel.AkPembekalId = akNotaMinta.AkPembekalId;
            viewModel.AkPembekal = akNotaMinta.AkPembekal;
            viewModel.Tahun = akNotaMinta.Tahun;
            viewModel.Tarikh = akNotaMinta.Tarikh;
            viewModel.JKW = akNotaMinta.JKW;
            viewModel.JKWId = akNotaMinta.JKWId;
            viewModel.JBahagian = akNotaMinta.JBahagian;
            viewModel.JBahagianId = akNotaMinta.JBahagianId;
            viewModel.NoRujukan = akNotaMinta.NoRujukan.Substring(3);
            viewModel.Tajuk = akNotaMinta.Tajuk;
            viewModel.NoSiri = akNotaMinta.NoSiri;
            viewModel.NoCAS = akNotaMinta.NoCAS;
            viewModel.TarikhSeksyenKewangan = akNotaMinta.TarikhSeksyenKewangan;
            viewModel.FlStatusSemak = akNotaMinta.FlStatusSemak;
            viewModel.FlPosting = akNotaMinta.FlPosting;
            viewModel.FlCetak = akNotaMinta.FlCetak;
            viewModel.FlHapus = akNotaMinta.FlHapus;
            viewModel.FlJenis = akNotaMinta.FlJenis;

            viewModel.Jumlah = akNotaMinta.Jumlah;
            viewModel.AkNotaMinta1 = akNotaMinta.AkNotaMinta1;
            if (akNotaMinta.AkNotaMinta2 != null)
            {
                foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
                {
                    viewModel.JumlahPerihal += item.Amaun;
                }
                viewModel.AkNotaMinta2 = akNotaMinta.AkNotaMinta2.OrderBy(b => b.Bil).ToList();
            }
            
            if (akNotaMinta == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            PopulateList(akNotaMinta.SuPekerjaMasukId);
            return View(viewModel);
        }

        private string GetNoRujukan(int data, string year)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

            var kumpulanWang = kw?.Kod ?? "100";

            string prefix = year + "/" + kumpulanWang + "/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkNotaMinta
                       .IgnoreQueryFilters()
                       .Where(x => x.Tahun == year && x.JKWId == data)
                       .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(12));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            return noRujukan;
        }

        // GET: AkNotaMinta/Create
        [Authorize(Policy = "NM001C")]
        public async Task<IActionResult> Create()
        {
            AkNotaMinta sp = new AkNotaMinta();

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            if (pekerjaId == null)
            {
                sp.SuPekerjaMasukId = 1;
            }
            else
            {
                sp.SuPekerjaMasukId = pekerjaId;
            }

            // get latest no rujukan running number 
            var year = DateTime.Now.Year.ToString();
            var data = 1;

            ViewBag.NoRujukan = GetNoRujukan(data, year);
            // get latest no rujukan running number end

            PopulateList(sp.SuPekerjaMasukId);
            CartEmpty();
            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akNotaMinta1 = new List<int>();
                ViewBag.akNotaMinta2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetPembekal(int data)
        {
            try
            {
                var result = await _akPembekalRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // get an item from cart akNotaMinta1
        public JsonResult GetAnItemCartAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {
                AkNotaMinta1 data = _cart.Lines1.Where(x => x.AkCartaId == akNotaMinta1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akNotaMinta1 end

        //save cart akNotaMinta1
        public JsonResult SaveCartAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akNotaMinta1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akNotaMinta1.AkCartaId);

                    _cart.AddItem1(akNotaMinta1.AkNotaMintaId,
                                    akNotaMinta1.AkCartaId,
                                    akNotaMinta1.Amaun
                                    );
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akNotaMinta1 end

        // get all item from cart akNotaMinta1
        public JsonResult GetAllItemCartAkNotaMinta1()
        {

            try
            {
                List<AkNotaMinta1> data = _cart.Lines1.ToList();

                foreach (AkNotaMinta1 item in data)
                {
                    var akCarta = _context.AkCarta.Find(item.AkCartaId);

                    item.AkCarta = akCarta;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akNotaMinta1 end

        // get an item from cart akNotaMinta2
        public JsonResult GetAnItemCartAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {

            try
            {
                AkNotaMinta2 data = _cart.Lines2.Where(x => x.Indek == akNotaMinta2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akNotaMinta2 end

        //save cart akNotaMinta2
        public JsonResult SaveCartAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.Indek == akNotaMinta2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akNotaMinta2.Indek);

                    _cart.AddItem2(akNotaMinta2.AkNotaMintaId,
                                   akNotaMinta2.Indek,
                                   akNotaMinta2.Bil,
                                   akNotaMinta2.NoStok ?? "",
                                   akNotaMinta2.Perihal?.ToUpper()?? "",
                                   akNotaMinta2.Kuantiti,
                                   akNotaMinta2.Unit ?? "",
                                   akNotaMinta2.Harga,
                                   akNotaMinta2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akNotaMinta2 end

        // get all item from cart akNotaMinta2
        public JsonResult GetAllItemCartAkNotaMinta2()
        {

            try
            {
                List<AkNotaMinta2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akNotaMinta2 end

        // function json get no rujukan (running number)
        [HttpPost]
        public JsonResult JsonGetKod(int data, string year)
        {
            try
            {
                var result = "";
                if (data == 0)
                {
                    result = "";
                }
                else
                {
                    // get latest no rujukan running number  
                    result = GetNoRujukan(data, year);
                    // get latest no rujukan running number end
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        // function json get no rujukan (running number) end

        // function  json Create
        public JsonResult GetCarta(AkCarta akCarta)
        {
            try
            {
                var result = _context.AkCarta.Where(b => b.Id == akCarta.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public async Task<JsonResult> SaveAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {
                if (akNotaMinta1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akNotaMinta1.AkNotaMintaId,
                                    akNotaMinta1.AkCartaId,
                                    akNotaMinta1.Amaun
                                    );

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {
                if (akNotaMinta1 != null)
                {

                    _cart.RemoveItem1(akNotaMinta1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {
            var akNota = akNotaMinta2;
            try
            {
                if (akNotaMinta2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akNotaMinta2.AkNotaMintaId,
                                   akNotaMinta2.Indek,
                                   akNotaMinta2.Bil,
                                   akNotaMinta2.NoStok ?? "",
                                   akNotaMinta2.Perihal?.ToUpper()?? "",
                                   akNotaMinta2.Kuantiti,
                                   akNotaMinta2.Unit ?? "",
                                   akNotaMinta2.Harga,
                                   akNotaMinta2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {

            try
            {
                if (akNotaMinta2 != null)
                {

                    _cart.RemoveItem2(akNotaMinta2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkNotaMinta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "NM001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkNotaMinta akNotaMinta, decimal JumlahPerihal,string syscode)
        {
            AkNotaMinta m = new AkNotaMinta();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var noRujukan = GetNoRujukan(akNotaMinta.JKWId, akNotaMinta.Tahun);

            if (user != null)
            {
                if (user.Email == "superadmin@idwal.com.my")
                {
                    akNotaMinta.SuPekerjaMasukId = 1;
                }
                else
                {
                    akNotaMinta.SuPekerjaMasukId = pekerjaId;
                }
            }

            if (akNotaMinta.AkPembekalId == 0)
            {
                TempData[SD.Error] = "Sila pilih kod pembekal";
                ViewBag.NoRujukan = noRujukan;
                PopulateTableFromCart();
                PopulateList(pekerjaId);
                return View(akNotaMinta);
            }

            if (ModelState.IsValid)
            {
                m.FlJenis = akNotaMinta.FlJenis;
                m.JKWId = akNotaMinta.JKWId;
                m.JBahagianId = akNotaMinta.JBahagianId;
                m.Tahun = akNotaMinta.Tahun;
                m.Tajuk = akNotaMinta.Tajuk?.ToUpper() ?? "";
                m.AkPembekalId = akNotaMinta.AkPembekalId;
                m.NoRujukan = "NM/" + noRujukan;
                m.Tarikh = akNotaMinta.Tarikh;
                m.Jumlah = akNotaMinta.Jumlah;
                m.FlPosting = 0;
                m.FlCetak = 0;
                m.FlHapus = 0;

                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkNotaMinta1 = _cart.Lines1.ToArray();
                m.AkNotaMinta2 = _cart.Lines2.ToArray();

                await _akNotaMintaRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoRujukan, m.NoRujukan, 0, m.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Data gagal disimpan.";
            ViewBag.NoRujukan = noRujukan;
            PopulateTableFromCart();
            PopulateList(pekerjaId);
            return View(akNotaMinta);
        }

        // GET: AkNotaMinta/Edit/5
        [Authorize(Policy = "NM001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkNotaMinta == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

            if (akNotaMinta == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateTable(id);
            PopulateList(akNotaMinta.SuPekerjaMasukId);
            PopulateCartFromDb(akNotaMinta);
            return View(akNotaMinta);
        }

        // get latest Index number in AkNotaMinta2
        public JsonResult GetLatestIndexNumberPerihal()
        {

            try
            {
                var data = _cart.Lines2.Max(x => x.Indek);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkNotaMinta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkNotaMinta akNotaMinta, decimal JumlahPerihal, string syscode)
        {
            if (id != akNotaMinta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    AkNotaMinta dataAsal = await _akNotaMintaRepo.GetById(id);

                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    // list of input that cannot be change
                    akNotaMinta.FlJenis = dataAsal.FlJenis;
                    akNotaMinta.Tahun = dataAsal.Tahun;
                    akNotaMinta.JKWId = dataAsal.JKWId;
                    akNotaMinta.JBahagianId = dataAsal.JBahagianId;
                    akNotaMinta.NoRujukan = dataAsal.NoRujukan;
                    akNotaMinta.NoCAS = dataAsal.NoCAS;
                    akNotaMinta.TarikhSeksyenKewangan = dataAsal.TarikhSeksyenKewangan;
                    akNotaMinta.NoSiri = dataAsal.NoSiri;
                    akNotaMinta.TarMasuk = dataAsal.TarMasuk;
                    akNotaMinta.UserId = dataAsal.UserId;
                    akNotaMinta.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    akNotaMinta.FlCetak = 0;
                    // list of input that cannot be change end

                    decimal jumlahAsal = 0;
                    if (dataAsal.AkNotaMinta2 != null)
                    {
                        foreach (AkNotaMinta2 item in dataAsal.AkNotaMinta2)
                        {
                            var model = _context.AkNotaMinta2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                jumlahAsal += model.Amaun;
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akNotaMinta.AkNotaMinta1 = dataAsal.AkNotaMinta1;
                    akNotaMinta.AkNotaMinta2 = _cart.Lines2.ToList();

                    akNotaMinta.TarSemak = null;
                    akNotaMinta.JPenyemakId = null;
                    akNotaMinta.FlStatusSemak = 0;

                    akNotaMinta.TarLulus = null;
                    akNotaMinta.JPelulusId = null;
                    akNotaMinta.FlStatusLulus = 0;

                    akNotaMinta.UserIdKemaskini = user?.UserName ?? "";
                    akNotaMinta.TarKemaskini = DateTime.Now;
                    akNotaMinta.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akNotaMinta);

                    //insert applog
                    if (jumlahAsal != JumlahPerihal)
                    {
                        _appLog.Insert("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(JumlahPerihal).ToString("#,##0.00"), akNotaMinta.NoRujukan, id, JumlahPerihal, pekerjaId,modul,syscode,namamodul,user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", akNotaMinta.NoRujukan, id, JumlahPerihal, pekerjaId,modul,syscode,namamodul,user);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkNotaMintaExists(akNotaMinta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                CartEmpty();
                TempData[SD.Success] = "Data berjaya diubah..!";
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Warning] = "Data tidak lengkap. Sila cuba sekali lagi";
            PopulateList(akNotaMinta.SuPekerjaMasukId);
            PopulateTableFromCart();
            return View(akNotaMinta);
        }

        // GET: AkNotaMinta/Edit/5
        [Authorize(Policy = "NM001E1")]
        public async Task<IActionResult> EditKewangan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

            AkNotaMintaViewModel viewModel = new AkNotaMintaViewModel();

            viewModel.Id = akNotaMinta.Id;
            viewModel.AkPembekalId = akNotaMinta.AkPembekalId;
            viewModel.AkPembekal = akNotaMinta.AkPembekal;
            viewModel.Tahun = akNotaMinta.Tahun;
            viewModel.Tarikh = akNotaMinta.Tarikh;
            viewModel.JKW = akNotaMinta.JKW;
            viewModel.JKWId = akNotaMinta.JKWId;
            viewModel.JBahagian = akNotaMinta.JBahagian;
            viewModel.JBahagianId = akNotaMinta.JBahagianId;
            viewModel.NoRujukan = akNotaMinta.NoRujukan.Substring(3);
            viewModel.Tajuk = akNotaMinta.Tajuk;
            viewModel.NoSiri = akNotaMinta.NoSiri;
            viewModel.NoCAS = akNotaMinta.NoCAS;
            viewModel.TarikhSeksyenKewangan = akNotaMinta.TarikhSeksyenKewangan;
            viewModel.FlPosting = akNotaMinta.FlPosting;
            viewModel.FlCetak = akNotaMinta.FlCetak;
            viewModel.FlHapus = akNotaMinta.FlHapus;

            viewModel.Jumlah = akNotaMinta.Jumlah;
            viewModel.AkNotaMinta1 = akNotaMinta.AkNotaMinta1;
            if (akNotaMinta.AkNotaMinta2 != null)
            {
                foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
                {
                    viewModel.JumlahPerihal += item.Amaun;
                }
            }
            
            viewModel.AkNotaMinta2 = akNotaMinta.AkNotaMinta2;

            if (akNotaMinta == null)
            {
                return NotFound();
            }

            // get latest no rujukan running number if not existed 
            if (string.IsNullOrEmpty(akNotaMinta.NoSiri))
            {
                var year = DateTime.Now.Year.ToString().Substring(2);
                var month = DateTime.Now.Month.ToString();
                string prefix = "/" + month + "/" + year;
                int x = 1;
                string noRujukan = "0000" + prefix;

                var LatestNoRujukan = _context.AkNotaMinta.Where(x => x.NoSiri!.EndsWith(prefix))
                            .Max(x => x.NoSiri);

                if (LatestNoRujukan == null)
                {
                    noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                }
                else
                {
                    x = int.Parse(LatestNoRujukan.Substring(0, 4));
                    x++;
                    noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                }
                ViewBag.NoSiri = noRujukan;
            }
            else
            {
                ViewBag.NoSiri = akNotaMinta.NoSiri;
            }

            // get latest no rujukan running number end


            CartEmpty();
            PopulateTable(id);
            PopulateList(akNotaMinta.SuPekerjaMasukId);
            PopulateCartFromDb(akNotaMinta);
            return View(viewModel);
        }

        // POST: AkNotaMinta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "NM001E1")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKewangan(int id, AkNotaMinta akNotaMinta, decimal JumlahPerihal,string syscode)
        {
            if (id != akNotaMinta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    AkNotaMinta dataAsal = await _akNotaMintaRepo.GetById(id);

                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    // get latest no rujukan running number if not existed 
                    if (string.IsNullOrEmpty(dataAsal.NoSiri))
                    {
                        var year = DateTime.Now.Year.ToString().Substring(2);
                        var month = DateTime.Now.Month.ToString();
                        string prefix = "/" + month + "/" + year;
                        int x = 1;
                        string noRujukan = "0000" + prefix;

                        var LatestNoRujukan = _context.AkNotaMinta.Where(x => x.NoSiri!.EndsWith(prefix))
                                    .Max(x => x.NoSiri);

                        if (LatestNoRujukan == null)
                        {
                            noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                        }
                        else
                        {
                            x = int.Parse(LatestNoRujukan.Substring(0, 4));
                            x++;
                            noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                        }
                        akNotaMinta.NoSiri = noRujukan;
                    }
                    else
                    {
                        akNotaMinta.NoSiri = dataAsal.NoSiri;
                    }

                    // get latest no rujukan running number end

                    // list of input that cannot be change
                    akNotaMinta.FlJenis = dataAsal.FlJenis;
                    akNotaMinta.Tahun = dataAsal.Tahun;
                    akNotaMinta.JKWId = dataAsal.JKWId;
                    akNotaMinta.NoRujukan = dataAsal.NoRujukan;
                    akNotaMinta.AkPembekalId = dataAsal.AkPembekalId;
                    akNotaMinta.Tajuk = dataAsal.Tajuk;
                    akNotaMinta.NoCAS = dataAsal.NoCAS;
                    akNotaMinta.TarikhSeksyenKewangan = dataAsal.TarikhSeksyenKewangan;
                    akNotaMinta.TarMasuk = dataAsal.TarMasuk;
                    akNotaMinta.UserId = dataAsal.UserId;
                    akNotaMinta.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    if (dataAsal.AkNotaMinta1 != null)
                    {
                        foreach (AkNotaMinta1 item in dataAsal.AkNotaMinta1)
                        {
                            var model = _context.AkNotaMinta1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    var jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akNotaMinta.AkNotaMinta1 = _cart.Lines1.ToList();
                    akNotaMinta.AkNotaMinta2 = dataAsal.AkNotaMinta2;

                    akNotaMinta.UserIdKemaskini = user?.UserName ?? "";
                    akNotaMinta.TarKemaskini = DateTime.Now;
                    akNotaMinta.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akNotaMinta);

                    //insert applog
                    if (jumlahAsal != akNotaMinta.Jumlah)
                    {
                        _appLog.Insert("Ubah", "Ubah Bahagian Kewangan RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(akNotaMinta.Jumlah).ToString("#,##0.00"), akNotaMinta.NoRujukan, id, akNotaMinta.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data Ubah Bahagian Kewangan", akNotaMinta.NoRujukan, id, akNotaMinta.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                    }

                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkNotaMintaExists(akNotaMinta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                CartEmpty();
                TempData[SD.Success] = "Data berjaya diubah..!";
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Warning] = "Data tidak lengkap. Sila cuba sekali lagi";
            PopulateList(akNotaMinta.SuPekerjaMasukId);
            PopulateTableFromCart();
            return View(akNotaMinta);
        }


        // GET: AkNotaMinta/Delete/5
        [Authorize(Policy = "NM001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkNotaMinta == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

            AkNotaMintaViewModel viewModel = new AkNotaMintaViewModel();

            viewModel.Id = akNotaMinta.Id;
            viewModel.AkPembekalId = akNotaMinta.AkPembekalId;
            viewModel.AkPembekal = akNotaMinta.AkPembekal;
            viewModel.Tahun = akNotaMinta.Tahun;
            viewModel.Tarikh = akNotaMinta.Tarikh;
            viewModel.JKW = akNotaMinta.JKW;
            viewModel.JKWId = akNotaMinta.JKWId;
            viewModel.JBahagian = akNotaMinta.JBahagian;
            viewModel.JBahagianId = akNotaMinta.JBahagianId;
            viewModel.NoRujukan = akNotaMinta.NoRujukan.Substring(3);
            viewModel.Tajuk = akNotaMinta.Tajuk;
            viewModel.NoSiri = akNotaMinta.NoSiri;
            viewModel.NoCAS = akNotaMinta.NoCAS;
            viewModel.TarikhSeksyenKewangan = akNotaMinta.TarikhSeksyenKewangan;
            viewModel.FlPosting = akNotaMinta.FlPosting;
            viewModel.FlCetak = akNotaMinta.FlCetak;
            viewModel.FlHapus = akNotaMinta.FlHapus;
            viewModel.FlJenis = akNotaMinta.FlJenis;

            viewModel.Jumlah = akNotaMinta.Jumlah;
            viewModel.AkNotaMinta1 = akNotaMinta.AkNotaMinta1;
            if (akNotaMinta.AkNotaMinta2 != null)
            {
                foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
                {
                    viewModel.JumlahPerihal += item.Amaun;
                }
            }
            
            viewModel.AkNotaMinta2 = akNotaMinta.AkNotaMinta2;

            if (akNotaMinta == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            PopulateList(akNotaMinta.SuPekerjaMasukId);
            return View(viewModel);
        }

        // POST: AkNotaMinta/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "NM001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkNotaMinta == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkNotaMinta'  is null.");
            }
            var akNotaMinta = await _context.AkNotaMinta.FindAsync(id);
            if (akNotaMinta != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                akNotaMinta.UserIdKemaskini = user?.UserName ?? "";
                akNotaMinta.TarKemaskini = DateTime.Now;
                akNotaMinta.SuPekerjaKemaskiniId = pekerjaId;

                akNotaMinta.FlCetak = 0;
                _context.AkNotaMinta.Update(akNotaMinta);

                _context.AkNotaMinta.Remove(akNotaMinta);

                //insert applog
                _appLog.Insert("Hapus", "Hapus Data", akNotaMinta.NoRujukan, id, akNotaMinta.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Semak function
        [HttpPost, ActionName("Semak")]
        [Authorize(Policy = "NM001T")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Semak(int? id, int penyemakId, DateTime? tarikhSemak, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {

                if (tarikhSemak == null)
                {
                    TempData[SD.Error] = "Tarikh Semak diperlukan.";
                    return RedirectToAction(nameof(Index));

                }

                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                var namaUser = _context.
                    applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.Email == user!.UserName);


                AkNotaMinta nm = await _akNotaMintaRepo.GetById((int)id);

                //check for print
                if (nm.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal disemak. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                //semak operation start here
                //update semak status
                nm.FlStatusSemak = 1;
                nm.TarSemak = tarikhSemak;

                nm.JPenyemakId = penyemakId;


                await _akNotaMintaRepo.Update(nm);

                //insert applog
                _appLog.Insert("Posting", "Semak Data", nm.NoRujukan, (int)id, nm.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                //insert applog end

                await _context.SaveChangesAsync();

                TempData[SD.Success] = "Data berjaya disemak.";
            }

            return RedirectToAction(nameof(Index));

        }
        // Semak function end

        // posting function
        [Authorize(Policy = "NM001T")]
        public async Task<IActionResult> Posting(int? id, int pelulusId, DateTime? tarikhLulus, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                AkNotaMinta akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

                //check for print
                if (akNotaMinta.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                if (tarikhLulus == null)
                {
                    TempData[SD.Error] = "Tarikh Lulus diperlukan.";
                    return RedirectToAction(nameof(Index));

                }

                var akPO = await _context.AkPO.Where(x => x.AkNotaMintaId == id && x.FlHapus == 0).FirstOrDefaultAsync();
                if (akPO != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

                }
                else
                {
                    //posting operation start here

                    //update posting status in akTerima
                    akNotaMinta.FlStatusLulus = 1;
                    akNotaMinta.TarLulus = tarikhLulus;
                    akNotaMinta.JPelulusId = pelulusId;

                    akNotaMinta.FlPosting = 1;
                    akNotaMinta.TarikhPosting = DateTime.Now;
                    await _akNotaMintaRepo.Update(akNotaMinta);

                    //insert applog
                    _appLog.Insert("Posting", "Posting Data", akNotaMinta.NoRujukan, (int)id, akNotaMinta.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan. Sila terus ke modul Pesanan Tempatan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "TG002UT")]
        public async Task<IActionResult> UnPosting(int? id, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                AkNotaMinta akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

                AkPO akPO = await _context.AkPO.FirstOrDefaultAsync(x => x.AkNotaMintaId == id);

                if (akPO != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data terkait pada Pesanan Tempatan " + akPO.NoPO?.ToUpper() + ". Batal kelulusan tidak dibenarkan";

                }
                else
                {
                    //unposting operation start here

                    //update posting status in akTerima
                    akNotaMinta.FlStatusLulus = 0;
                    akNotaMinta.TarLulus = null;
                    akNotaMinta.JPelulusId = null;

                    akNotaMinta.FlPosting = 0;
                    akNotaMinta.TarikhPosting = null;
                    await _akNotaMintaRepo.Update(akNotaMinta);

                    //insert applog
                    _appLog.Insert("UnPosting", "UnPosting Data", akNotaMinta.NoRujukan, (int)id, akNotaMinta.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan dari Pesanan Tempatan / Inden Kerja.";
                    //unposting operation end
                }

            }
            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "PR001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _akNotaMintaRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Rollback operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.TarSemak = null;
            obj.JPenyemakId = null;
            obj.FlStatusSemak = 0;

            obj.TarLulus = null;
            obj.JPelulusId = null;
            obj.FlStatusLulus = 0;

            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkNotaMinta.Update(obj);

            // Rollback operation end

            //insert applog
            _appLog.Insert("Rollback", "Rollback Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId,modul,syscode,namamodul,user);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }


        // printing Nota Minta
        [Authorize(Policy = "NM001P")]
        public async Task<IActionResult> PrintPdf(int id, int penyemakId, int pelulusId,string syscode)
        {
            AkNotaMinta akNotaMinta = await _akNotaMintaRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akNotaMinta.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(0 - akNotaMinta.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akNotaMinta.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
            var namaUser = _context.applicationUsers.FirstOrDefault(x => x.Email == user!.Email);

            NotaMintaPrintModel data = new NotaMintaPrintModel();


            if (akNotaMinta.TarikhSeksyenKewangan != null)
            {
                data.TarikhKewangan= akNotaMinta.TarikhSeksyenKewangan.ToString()!;
            }
            else
            {
                data.TarikhKewangan = "";
            }

            CompanyDetails company = await _userService.GetCompanyDetails();

            if (akNotaMinta.AkNotaMinta2 != null)
            {
                foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
                {
                    data.JumlahPerihal += item.Amaun;
                }
            }
            
            data.CompanyDetail = company;
            data.AkNotaMinta = akNotaMinta;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.username = namaUser?.Nama ?? "";

            //update cetak -> 1
            akNotaMinta.FlCetak = 1;
            await _akNotaMintaRepo.Update(akNotaMinta);

            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", akNotaMinta.NoRujukan, id, akNotaMinta.Jumlah, pekerjaId,modul,syscode,namamodul,user);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("NotaMintaPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing Nota Minta end

        private bool AkNotaMintaExists(int id)
        {
          return (_context.AkNotaMinta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
