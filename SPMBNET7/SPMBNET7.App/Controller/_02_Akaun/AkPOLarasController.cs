using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using SPMBNET7.App.Data;
using SPMBNET7.App.Infrastructures.Services;
using SPMBNET7.App.Pages.PrintModels._02_Akaun;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels;
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Math;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    public class AkPOLarasController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "PT001";
        public const string namamodul = "Pelarasan Pesanan Tempatan";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly IRepository<AkPOLaras, int, string> _akPOLarasRepo;
        private readonly ListViewIRepository<AkPOLaras1, int> _akPOLaras1Repo;
        private readonly ListViewIRepository<AkPOLaras2, int> _akPOLaras2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkPembekal, int, string> _akpembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly UserServices _userService;
        private CartPOLaras _cart;

        public AkPOLarasController(ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPO, int, string> akPORepository,
            IRepository<AkPOLaras, int, string> akPOLarasRepository,
            ListViewIRepository<AkPOLaras1, int> akPOLaras1Repository,
            ListViewIRepository<AkPOLaras2, int> akPOLaras2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            UserServices userService,
            CartPOLaras cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akPORepo = akPORepository;
            _akPOLarasRepo = akPOLarasRepository;
            _akPOLaras1Repo = akPOLaras1Repository;
            _akPOLaras2Repo = akPOLaras2Repository;
            _akCartaRepo = akCartaRepository;
            _kwRepo = kwRepository;
            _akpembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _userService = userService;
            _cart = cart;
        }

        // GET: AkPOLaras
        [Authorize(Policy = "PT001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Rujukan", Value = "NoRujukan" },
                new SelectListItem() { Text = "Nama", Value = "Nama" }
            };

            if (!string.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akPOLaras = await _akPOLarasRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akPOLaras = await _akPOLarasRepo.GetAllIncludeDeletedItems();
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akPOLaras = akPOLaras.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Pembekal")
                    {
                        akPOLaras = akPOLaras.Where(s => s.AkPO!.AkPembekal!.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akPOLaras = akPOLaras.Where(x => x.Tarikh >= date1
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

            return View(akPOLaras);
        }

        // GET: AkPOLaras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkPOLaras == null)
            {
                return NotFound();
            }

            var akPOLaras = await _akPOLarasRepo.GetByIdIncludeDeletedItems((int)id);

            if (akPOLaras == null)
            {
                return NotFound();
            }

            AkPOLarasViewModel viewModel = new AkPOLarasViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkPOId = akPOLaras.AkPOId;
            viewModel.AkPO = akPOLaras.AkPO;
            viewModel.Id = akPOLaras.Id;
            viewModel.Tahun = akPOLaras.Tahun;
            viewModel.NoRujukan = akPOLaras.NoRujukan;
            viewModel.Tarikh = akPOLaras.Tarikh;
            viewModel.Tajuk = akPOLaras.Tajuk;
            viewModel.JKW = akPOLaras.JKW;
            viewModel.JKWId = akPOLaras.JKWId;
            viewModel.JBahagian = akPOLaras.JBahagian;
            viewModel.JBahagianId = akPOLaras.JBahagianId;
            viewModel.Jumlah = akPOLaras.Jumlah;
            viewModel.TarikhPosting = akPOLaras.TarikhPosting;
            viewModel.FlPosting = akPOLaras.FlPosting;
            viewModel.FlHapus = akPOLaras.FlHapus;
            viewModel.FlCetak = akPOLaras.FlCetak;

            if (akPOLaras.AkPOLaras2 != null)
            {
                foreach (AkPOLaras2 item in akPOLaras.AkPOLaras2)
                {
                    viewModel.JumlahPerihal += item.Amaun;
                }
            }
            
            viewModel.AkPOLaras1 = akPOLaras.AkPOLaras1;
            viewModel.AkPOLaras2 = akPOLaras.AkPOLaras2;

            PopulateTable(id);
            return View(viewModel);
        }

        private void PopulateTable(int? id)
        {
            List<AkPOLaras1> akPOLaras1Table = _context.AkPOLaras1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOLarasId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPOLaras1 = akPOLaras1Table;

            List<AkPOLaras2> akPOLaras2Table = _context.AkPOLaras2
                .Where(b => b.AkPOLarasId == id)
                .OrderBy(b => b.Bil)
                .ToList();
            ViewBag.akPOLaras2 = akPOLaras2Table;
        }

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkPOLaras1> tbl1 = new List<AkPOLaras1>();
            var cart1 = _cart.Lines1.ToList();

            if (cart1 != null && cart1.Count() > 0)
            {
                foreach (var item in cart1)
                {
                    var carta = _context.AkCarta.FirstOrDefault(b => b.Id == item.AkCartaId);
                    item.AkCarta = carta;
                    tbl1.Add(item);
                }
            }
            ViewBag.akPOLaras1 = tbl1;
            // table 1 end

            // table 2
            List<AkPOLaras2> tbl2 = new List<AkPOLaras2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    tbl2.Add(item);
                }
            }
            ViewBag.akPOLaras2 = tbl2;
            // table 2 end
        }
        // populate table from cart end

        // GET: AkPOLaras/Create
        [Authorize(Policy = "PT001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();
            var noRujukan = RunningNumber(year);

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;

            CartEmpty();
            PopulateList();
            return View();
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Kod).ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkPO> akPOList = _context.AkPO.Include(x => x.AkPembekal).Where(x => x.FlPosting == 1).ToList();
            ViewBag.AkPO = akPOList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akPO1 = new List<int>();
                ViewBag.akPO2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        //Function Running Number
        public string RunningNumber(string year)
        {

            string prefix = year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkPOLaras
                        .IgnoreQueryFilters()
                        .Where(x => x.Tahun == year)
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(7));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            return noRujukan;
        }

        [HttpPost]
        public JsonResult JsonGetKod(string year)
        {
            try
            {
                var result = "";
                if (year == null)
                {
                    result = "";
                }
                else
                {
                    result = RunningNumber(year);
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // on change no PO controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoPO(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkPO(id);
                var result = await _akPORepo.GetById(id);

                List<AkPO1> akPO1Table = await _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkPO1 item in akPO1Table)
                {
                    if (result.AkPO1 != null) result.AkPO1.Add(item);
                }

                List<AkPO2> akPO2Table = await _context.AkPO2
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Bil)
                .ToListAsync();

                foreach (AkPO2 item in akPO2Table)
                {
                    if (result.AkPO2 != null) result.AkPO2.Add(item);
                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCartFromAkPO(int id)
        {
            var user = _userManager.GetUserName(User);

            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO1 item in akPO1Table)
            {

                item.AkPOId = 0;

                _cart.AddItem1(item.AkPOId,
                               item.AkCartaId,
                               item.Amaun);
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                .AsNoTracking()
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Bil)
                .ToList();

            foreach (AkPO2 item in akPO2Table)
            {
                item.AkPOId = 0;


                item.Perihal = "PELARASAN -" + item.Perihal;

                _cart.AddItem2(item.AkPOId,
                               item.Indek,
                               item.Bil,
                               item.NoStok ?? "",
                               item.Perihal?.ToUpper() ?? "",
                               item.Kuantiti,
                               item.Unit ?? "",
                               item.Harga,
                               item.Amaun);
            }

        }
        //on change no PO controller end
        // POST: AkPOLaras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PT001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPOLaras akPOLaras, string syscode)
        {
            AkPOLaras m = new AkPOLaras();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akPOLaras.Tahun);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                m.JKWId = akPOLaras.JKWId;
                m.JBahagianId = akPOLaras.JBahagianId;
                m.NoRujukan = "PT/" + noRujukan;
                m.Tarikh = akPOLaras.Tarikh;
                m.Tajuk = akPOLaras.Tajuk?.ToUpper() ?? "";
                m.AkPOId = akPOLaras.AkPOId;
                m.TarikhPosting = akPOLaras.TarikhPosting;
                m.Jumlah = akPOLaras.Jumlah;
                m.FlPosting = 0;
                m.FlHapus = 0;
                m.FlCetak = 0;
                m.Tahun = akPOLaras.Tahun;
                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkPOLaras1 = _cart.Lines1.ToArray();
                m.AkPOLaras2 = _cart.Lines2.ToArray();

                await _akPOLarasRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoRujukan, m.NoRujukan, 0, m.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat Pelarasan Tanggungan berjaya ditambah. No Pendaftaran adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Data gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            return View(akPOLaras);
        }

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

        public async Task<JsonResult> SaveAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {
                if (akPOLaras1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akPOLaras1.AkPOLarasId,
                                    akPOLaras1.AkCartaId,
                                    akPOLaras1.Amaun
                                    );

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {
                if (akPOLaras1 != null)
                {

                    _cart.RemoveItem1(akPOLaras1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {
                if (akPOLaras2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akPOLaras2.AkPOLarasId,
                                   akPOLaras2.Indek,
                                   akPOLaras2.Bil,
                                   akPOLaras2.NoStok ?? "",
                                   akPOLaras2.Perihal?.ToUpper() ?? "",
                                   akPOLaras2.Kuantiti,
                                   akPOLaras2.Unit ?? "",
                                   akPOLaras2.Harga,
                                   akPOLaras2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {
                if (akPOLaras2 != null)
                {

                    _cart.RemoveItem2(akPOLaras2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akPOLaras1
        public JsonResult GetAnItemCartAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {
                AkPOLaras1 data = _cart.Lines1.Where(x => x.AkCartaId == akPOLaras1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPOLaras1 end

        //save cart akPOLaras1
        public JsonResult SaveCartAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {

                var akPOL1 = _cart.Lines1.Where(x => x.AkCartaId == akPOLaras1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akPOL1 != null)
                {
                    _cart.RemoveItem1(akPOLaras1.AkCartaId);

                    _cart.AddItem1(akPOLaras1.AkPOLarasId,
                                    akPOLaras1.AkCartaId,
                                    akPOLaras1.Amaun
                                    );
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPOLaras1 end

        // get all item from cart akPOLaras1
        public JsonResult GetAllItemCartAkPOLaras1()
        {

            try
            {
                List<AkPOLaras1> data = _cart.Lines1.ToList();

                foreach (AkPOLaras1 item in data)
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
        // get all item from cart akPOLaras1 end

        // get an item from cart akPOLaras2
        public JsonResult GetAnItemCartAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {
                AkPOLaras2 data = _cart.Lines2.Where(x => x.Indek == akPOLaras2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPOLaras2 end

        //save cart akPOLaras2
        public JsonResult SaveCartAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {

                var akPOL2 = _cart.Lines2.Where(x => x.Indek == akPOLaras2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akPOL2 != null)
                {
                    _cart.RemoveItem2(akPOLaras2.Indek);

                    _cart.AddItem2(akPOLaras2.AkPOLarasId,
                                   akPOLaras2.Indek,
                                   akPOLaras2.Bil,
                                   akPOLaras2.NoStok ?? "",
                                   akPOLaras2.Perihal?.ToUpper() ?? "",
                                   akPOLaras2.Kuantiti,
                                   akPOLaras2.Unit ?? "",
                                   akPOLaras2.Harga,
                                   akPOLaras2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPOLaras2 end

        // get all item from cart akPOLaras2
        public JsonResult GetAllItemCartAkPOLaras2()
        {

            try
            {
                List<AkPOLaras2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPOLaras2 end

        // function  json Create end
        // GET: AkPOLaras/Edit/5
        [Authorize(Policy = "PT001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkPOLaras == null)
            {
                return NotFound();
            }

            var akPOLaras = await _akPOLarasRepo.GetById((int)id);

            if (akPOLaras == null)
            {
                return NotFound();
            }
            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akPOLaras);
            return View(akPOLaras);
        }

        // get latest Index number in AkPOLaras2
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
        // get all item from cart akBelian1 end

        private void PopulateCartFromDb(AkPOLaras akPOLaras)
        {
            List<AkPOLaras1> akPOLaras1Table = _context.AkPOLaras1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOLarasId == akPOLaras.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPOLaras1 item in akPOLaras1Table)
            {
                _cart.AddItem1(item.AkPOLarasId,
                               item.AkCartaId,
                               item.Amaun);
            }

            List<AkPOLaras2> akPOLaras2Table = _context.AkPOLaras2
                .Where(b => b.AkPOLarasId == akPOLaras.Id)
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkPOLaras2 item in akPOLaras2Table)
            {
                _cart.AddItem2(item.AkPOLarasId,
                               item.Indek,
                               item.Bil,
                               item.NoStok ?? "",
                               item.Perihal?.ToUpper() ?? "",
                               item.Kuantiti,
                               item.Unit ?? "",
                               item.Harga,
                               item.Amaun);
            }
        }

        // POST: AkPOLaras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PT001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  AkPOLaras akPOLaras,decimal JumlahPerihal, string syscode)
        {
            if (id != akPOLaras.Id)
            {
                return NotFound();
            }

            if (akPOLaras.Jumlah != JumlahPerihal)
            {
                TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
                PopulateList();
                PopulateTableFromCart();
                return View(akPOLaras);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    AkPOLaras dataAsal = await _akPOLarasRepo.GetById(id);

                    // list of input that cannot be change
                    akPOLaras.Tahun = dataAsal.Tahun;
                    akPOLaras.JKWId = dataAsal.JKWId;
                    akPOLaras.JBahagianId = dataAsal.JBahagianId;
                    akPOLaras.NoRujukan = dataAsal.NoRujukan;
                    akPOLaras.TarMasuk = dataAsal.TarMasuk;
                    akPOLaras.UserId = dataAsal.UserId;
                    akPOLaras.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    akPOLaras.FlCetak = 0;
                    // list of input that cannot be change end

                    if (dataAsal.AkPOLaras1 != null)
                    {
                        foreach (AkPOLaras1 item in dataAsal.AkPOLaras1)
                        {
                            var model = _context.AkPOLaras1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    if (dataAsal.AkPOLaras2 != null)
                    {
                        foreach (AkPOLaras2 item in dataAsal.AkPOLaras2)
                        {
                            var model = _context.AkPOLaras2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    decimal jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akPOLaras.AkPOLaras1 = _cart.Lines1.ToList();
                    akPOLaras.AkPOLaras2 = _cart.Lines2.ToList();

                    akPOLaras.UserIdKemaskini = user?.UserName ?? "";
                    akPOLaras.TarKemaskini = DateTime.Now;
                    akPOLaras.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akPOLaras);

                    // insert applog
                    if (jumlahAsal != akPOLaras.Jumlah)
                    {
                        _appLog.Insert("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(akPOLaras.Jumlah).ToString("#,##0.00"), akPOLaras.NoRujukan, id, akPOLaras.Jumlah, pekerjaId, modul,syscode,namamodul,user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", akPOLaras.NoRujukan, id, akPOLaras.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPOLarasExists(akPOLaras.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData[SD.Success] = "Data berjaya diubah..!";
                CartEmpty();
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Warning] = "Data gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            return View(akPOLaras);
        }

        // GET: AkPOLaras/Delete/5
        [Authorize(Policy = "PT001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkPOLaras == null)
            {
                return NotFound();
            }

            var akPOLaras = await _akPOLarasRepo.GetByIdIncludeDeletedItems((int)id);


            if (akPOLaras == null)
            {
                return NotFound();
            }

            AkPOLarasViewModel viewModel = new AkPOLarasViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkPOId = akPOLaras.AkPOId;
            viewModel.AkPO = akPOLaras.AkPO;
            viewModel.Id = akPOLaras.Id;
            viewModel.Tahun = akPOLaras.Tahun;
            viewModel.NoRujukan = akPOLaras.NoRujukan;
            viewModel.Tarikh = akPOLaras.Tarikh;
            viewModel.Tajuk = akPOLaras.Tajuk;
            viewModel.JKW = akPOLaras.JKW;
            viewModel.JKWId = akPOLaras.JKWId;
            viewModel.JBahagian = akPOLaras.JBahagian;
            viewModel.JBahagianId = akPOLaras.JBahagianId;
            viewModel.Jumlah = akPOLaras.Jumlah;
            viewModel.TarikhPosting = akPOLaras.TarikhPosting;
            viewModel.FlPosting = akPOLaras.FlPosting;
            viewModel.FlHapus = akPOLaras.FlHapus;
            viewModel.FlCetak = akPOLaras.FlCetak;

            if (akPOLaras.AkPOLaras2 != null)
            {
                foreach (AkPOLaras2 item in akPOLaras.AkPOLaras2)
                {
                    viewModel.JumlahPerihal += item.Amaun;
                }
            }
            
            viewModel.AkPOLaras1 = akPOLaras.AkPOLaras1;
            viewModel.AkPOLaras2 = akPOLaras.AkPOLaras2;

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            return View(viewModel);
        }

        // POST: AkPOLaras/Delete/5
        [Authorize(Policy = "PT001D")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkPOLaras == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkPOLaras'  is null.");
            }
            var akPOLaras = await _context.AkPOLaras.FindAsync(id);
            if (akPOLaras != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                akPOLaras.UserIdKemaskini = user?.UserName ?? "";
                akPOLaras.TarKemaskini = DateTime.Now;
                akPOLaras.SuPekerjaKemaskiniId = pekerjaId;
                // check if already posting redirect back
                if (akPOLaras.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }
                akPOLaras.FlCetak = 0;
                _context.AkPOLaras.Update(akPOLaras);

                //insert applog
                _appLog.Insert("Hapus", akPOLaras.NoRujukan, akPOLaras.NoRujukan, 0, akPOLaras.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                _context.AkPOLaras.Remove(akPOLaras);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkPOLarasExists(int id)
        {
          return (_context.AkPOLaras?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // posting function
        [Authorize(Policy = "PT001T")]
        public async Task<IActionResult> Posting(int? id, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                AkPOLaras akPOLaras = await _akPOLarasRepo.GetById((int)id);

                //check for print
                if (akPOLaras.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                var abBukuVot = await _context.AbBukuVot.FirstOrDefaultAsync(x => x.Rujukan == "PT/" + akPOLaras.NoRujukan);
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";
                    return RedirectToAction(nameof(Index));

                }
                    //posting operation start here
                    if (akPOLaras.AkPOLaras1 != null)
                    {
                        foreach (AkPOLaras1 item in akPOLaras.AkPOLaras1)
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotPosting = new AbBukuVot()
                            {
                                Tahun = akPOLaras.Tahun,
                                JKWId = akPOLaras.JKWId,
                                JBahagianId = akPOLaras.JBahagianId,
                                Tarikh = akPOLaras.Tarikh,
                                Kod = akPOLaras.AkPO?.AkPembekal?.KodSykt ?? "",
                                Penerima = akPOLaras.AkPO?.AkPembekal?.NamaSykt ?? "",
                                VotId = item.AkCartaId,
                                Rujukan = akPOLaras.NoRujukan,
                                Tanggungan = item.Amaun
                            };

                            await _abBukuVotRepo.Insert(abBukuVotPosting);
                            // insert into AbBukuVot end

                        }
                    }
                    

                    //update posting status in akPO
                    akPOLaras.FlPosting = 1;
                    akPOLaras.TarikhPosting = DateTime.Now;
                    await _akPOLarasRepo.Update(akPOLaras);

                    //insert applog
                    _appLog.Insert("Posting", "Posting Data", akPOLaras.NoRujukan, (int)id, akPOLaras.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya diluluskan.";

            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        [Authorize(Policy = "PT001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _akPOLarasRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Batal operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkPOLaras.Update(obj);

            // Batal operation end

            //insert applog
            _appLog.Insert("Rollback", "Rollback Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId, modul, syscode, namamodul, user);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        // unposting function
        [Authorize(Policy = "PT001UT")]
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

                AkPOLaras akPOLaras = await _akPOLarasRepo.GetById((int)id);

                // cannot unposting cancelled document AkPO
                if (akPOLaras.AkPO != null && 
                    0 - akPOLaras.Jumlah == akPOLaras.AkPO.Jumlah && akPOLaras.AkPO.FlBatal == 1)
                {
                    TempData[SD.Error] = "Data terkait dengan pembatalan PO.";
                    return RedirectToAction(nameof(Index));
                }

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == akPOLaras.NoRujukan).ToList();
                if (abBukuVot == null || abBukuVot.Count() == 0)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    AkBelian akBelian = _context.AkBelian.Where(x => x.AkPOId == akPOLaras.AkPOId).FirstOrDefault();

                    if (akBelian != null)
                    {
                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada no Inbois " + akBelian.NoInbois.ToUpper() + ". Batal kelulusan tidak dibenarkan";
                    }
                    else
                    {
                        //unposting operation start here
                        //delete data from abBukuVot
                        foreach (AbBukuVot item in abBukuVot)
                        {
                            await _abBukuVotRepo.Delete(item.Id);
                        }
                        //delete data from abBukuVot end

                        //update posting status in akPOLaras
                        akPOLaras.FlPosting = 0;
                        akPOLaras.TarikhPosting = null;
                        await _akPOLarasRepo.Update(akPOLaras);

                        //insert applog
                        _appLog.Insert("UnPosting", "UnPosting Data", akPOLaras.NoRujukan, (int)id, akPOLaras.Jumlah, pekerjaId,modul, syscode, namamodul, user);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya batal kelulusan.";
                        //unposting operation end
                    }

                }

            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        //// POST: AkPOLaras/Cancel/5
        [Authorize(Policy = "PT001B")]
        public async Task<IActionResult> Cancel(int id,string syscode)
        {
            var obj = await _akPOLarasRepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == "PT/" + obj.NoRujukan).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // check if already linked with AkBelian
                AkBelian Belian = _context.AkBelian.Where(x => x.AkPOId == obj.AkPOId && x.FlBatal == 0).FirstOrDefault();

                if (Belian != null)
                {

                    //linkage id error
                    TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal tidak dibenarkan";
                    //}
                }
                else
                {

                    //unposting operation start here

                    //insert contra data into abBukuVot
                    if (obj.AkPOLaras1 != null)
                    {
                        foreach (AkPOLaras1 item in obj.AkPOLaras1)
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotCanceling = new AbBukuVot()
                            {
                                Tahun = obj.Tahun,
                                JKWId = obj.JKWId,
                                JBahagianId = obj.JBahagianId,
                                Tarikh = obj.Tarikh,
                                Kod = obj.AkPO?.AkPembekal?.KodSykt ?? "",
                                Penerima = obj.AkPO?.AkPembekal?.NamaSykt ?? "",
                                VotId = item.AkCartaId,
                                Rujukan = obj.NoRujukan,
                                Tanggungan = 0 - item.Amaun
                            };

                            await _abBukuVotRepo.Insert(abBukuVotCanceling);
                            // insert into AbBukuVot end

                        }
                    }
                    

                    //update AkPO

                    obj.FlBatal = 1;
                    obj.TarBatal = DateTime.Now;
                    await _akPOLarasRepo.Update(obj);

                    //insert applog
                    _appLog.Insert("Batal", "Batal Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dibatalkan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));
        }

        //// POST: AkPOLaras/Cancel/5
        [Authorize(Policy = "PT001B")]
        public async Task<IActionResult> CancelAll(int id,string syscode)
        {
            var obj = await _akPOLarasRepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == "PT/" + obj.NoRujukan).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // check if already linked with AkBelian
                AkBelian Belian = _context.AkBelian.Where(x => x.AkPOId == obj.AkPOId && x.FlBatal == 0).FirstOrDefault();

                if (Belian != null)
                {

                    //linkage id error
                    TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal tidak dibenarkan";
                    //}
                }
                else
                {

                    //unposting operation start here

                    // batal POLaras
                    //insert contra data into abBukuVot
                    if (obj.AkPOLaras1 != null)
                    {
                        foreach (AkPOLaras1 item in obj.AkPOLaras1)
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotCanceling = new AbBukuVot()
                            {
                                Tahun = obj.Tahun,
                                JKWId = obj.JKWId,
                                JBahagianId = obj.JBahagianId,
                                Tarikh = obj.Tarikh,
                                Kod = obj.AkPO?.AkPembekal?.KodSykt ?? "",
                                Penerima = obj.AkPO?.AkPembekal?.NamaSykt ?? "",
                                VotId = item.AkCartaId,
                                Rujukan = obj.NoRujukan + "-B",
                                Tanggungan = 0 - item.Amaun
                            };

                            await _abBukuVotRepo.Insert(abBukuVotCanceling);
                            // insert into AbBukuVot end

                        }
                    }
                    

                    //update AkPOLaras

                    obj.FlBatal = 1;
                    obj.TarBatal = DateTime.Now;
                    await _akPOLarasRepo.Update(obj);

                    // batal PO
                    //insert contra data into abBukuVot
                    if (obj.AkPO != null && obj.AkPO.AkPO1 != null)
                    {
                        foreach (AkPO1 item in obj.AkPO.AkPO1)
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotCanceling = new AbBukuVot()
                            {
                                Tahun = obj.AkPO.Tahun,
                                JKWId = obj.AkPO.JKWId,
                                JBahagianId = obj.AkPO.JBahagianId,
                                Tarikh = obj.AkPO.Tarikh,
                                Kod = obj.AkPO.AkPembekal?.KodSykt ?? "",
                                Penerima = obj.AkPO.AkPembekal?.NamaSykt ?? "",
                                VotId = item.AkCartaId,
                                Rujukan = "PO/" + obj.AkPO.NoPO + "-B",
                                Tanggungan = 0 - item.Amaun
                            };

                            await _abBukuVotRepo.Insert(abBukuVotCanceling);
                            // insert into AbBukuVot end

                        }

                        //update AkPO

                        obj.AkPO.FlBatal = 1;
                        obj.AkPO.TarBatal = DateTime.Now;
                        await _akPORepo.Update(obj.AkPO);
                    }
                    

                    //insert applog
                    _appLog.Insert("Batal", "Batal Semua Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dibatalkan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));
        }

        // printing pelarasan PO 
        [Authorize(Policy = "PT001P")]
        public async Task<IActionResult> PrintPdf(int id,string syscode)
        {
            AkPOLaras akPOLaras = await _akPOLarasRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akPOLaras.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(0 - akPOLaras.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akPOLaras.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            POLarasPrintModel data = new POLarasPrintModel();

            CompanyDetails company = await _userService.GetCompanyDetails();
            data.CompanyDetail = company;
            data.AkPOLaras = akPOLaras;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user?.UserName ?? "";

            //update cetak -> 1
            akPOLaras.FlCetak = 1;
            await _akPOLarasRepo.Update(akPOLaras);

            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", akPOLaras.NoRujukan, id, akPOLaras.Jumlah, pekerjaId,modul,syscode,namamodul,user);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("POLarasPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing pelarasan PO end

    }
}
