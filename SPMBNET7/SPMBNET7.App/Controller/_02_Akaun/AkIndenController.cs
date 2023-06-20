using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
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
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Math;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    [Authorize(Policy = "TG003")]
    public class AkIndenController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "TG003";
        public const string namamodul = "Inden Kerja";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkInden, int, string> _akIndenRepo;
        private readonly IRepository<AkNotaMinta, int, string> _akNotaMintaRepo;
        private readonly ListViewIRepository<AkInden1, int> _akInden1Repo;
        private readonly ListViewIRepository<AkInden2, int> _akInden2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkPembekal, int, string> _akpembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly UserServices _userService;
        private CartInden _cart;

        public AkIndenController(ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkInden, int, string> akIndenRepository,
            IRepository<AkNotaMinta, int, string> akNotaMintaRepository,
            ListViewIRepository<AkInden1, int> akInden1Repository,
            ListViewIRepository<AkInden2, int> akInden2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JNegeri, int, string> negeriRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            CustomIRepository<string, int> customRepo,
            UserServices userService,
            CartInden cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akIndenRepo = akIndenRepository;
            _akNotaMintaRepo = akNotaMintaRepository;
            _akInden1Repo = akInden1Repository;
            _akInden2Repo = akInden2Repository;
            _akCartaRepo = akCartaRepository;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akpembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _customRepo = customRepo;
            _userService = userService;
            _cart = cart;
        }

        [HttpPost]
        public JsonResult GetMaklumat(AkPembekal akPembekal)
        {
            try
            {
                var result = _context.AkPembekal.Where(b => b.Id == akPembekal.Id).Include(x => x.JBank).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

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

        //Function Running Number
        private string RunningNumber(int JKWId,string Tahun)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == JKWId);

            var kumpulanWang = kw?.Kod ?? "100";
            //var year = DateTime.Now.Year.ToString();
            var year = Tahun;
            string prefix = "IK/" + year + "/" + kumpulanWang + "/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkInden
                .IgnoreQueryFilters()
                .Where(x => x.Tahun == year && x.JKWId == JKWId)
                .Max(x => x.NoInden);
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

        [HttpPost]
        public JsonResult JsonGetKod(AkInden data)
        {
            try
            {
                var result = "";
                if (data == null)
                {
                    result = "";
                }
                else
                {
                    result = RunningNumber(data.JKWId, data.Tahun);
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // GET: AkInden
        [Authorize(Policy = "TG003")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Inden", Value = "NoRujukan" },
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

            var akInden = await _akIndenRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akInden = await _akIndenRepo.GetAllIncludeDeletedItems();
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akInden = akInden.Where(s => s.NoInden.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Pembekal")
                    {
                        akInden = akInden.Where(s => s.AkPembekal!.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akInden = akInden.Where(x => x.Tarikh >= date1
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

            return View(akInden);
        }

        // GET: AkInden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkInden == null)
            {
                return NotFound();
            }

            var akInden = await _akIndenRepo.GetByIdIncludeDeletedItems((int)id);
            var kw = await _kwRepo.GetById(akInden.JKWId);
            akInden.JKW = kw;

            if (akInden == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akInden);
        }

        // on change no Inden controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoNotaMinta(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkNotaMinta(id);
                var result = await _akNotaMintaRepo.GetById(id);

                List<AkNotaMinta1> akNotaMinta1Table = await _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                if (result.AkNotaMinta1 != null)
                {
                    foreach (AkNotaMinta1 item in akNotaMinta1Table)
                    {
                        result.AkNotaMinta1.Add(item);
                    }
                }
                    

                List<AkNotaMinta2> akNotaMinta2Table = await _context.AkNotaMinta2
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Bil)
                .ToListAsync();

                if (result.AkNotaMinta2 != null)
                {
                    foreach (AkNotaMinta2 item in akNotaMinta2Table)
                    {
                        result.AkNotaMinta2.Add(item);
                    }

                    result.AkNotaMinta2 = result.AkNotaMinta2.OrderBy(b => b.Bil).ToList();

                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCartFromAkNotaMinta(int id)
        {
            var user = _userManager.GetUserName(User);

            List<AkNotaMinta1> akNotaMinta1Table = _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkNotaMinta1 item in akNotaMinta1Table)
            {

                item.AkNotaMintaId = 0;

                _cart.AddItem1(item.AkNotaMintaId,
                                item.AkCartaId,
                               item.Amaun
                               );
            }

            List<AkNotaMinta2> akNotaMinta2Table = _context.AkNotaMinta2
                .AsNoTracking()
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Bil)
                .ToList();

            foreach (AkNotaMinta2 item in akNotaMinta2Table)
            {
                item.AkNotaMintaId = 0;

                _cart.AddItem2(item.AkNotaMintaId,
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
        //on change no Inden controller end

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkPembekal> PembekalList = _context.AkPembekal.OrderBy(b => b.Id).ToList();
            ViewBag.AkPembekal = PembekalList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkNotaMinta> akNotaMintaList = _context.AkNotaMinta
                .Where(x => x.FlPosting == 1 &&
                x.FlJenis == 1)
                .ToList();
            ViewBag.AkNotaMinta = akNotaMintaList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

        }

        private void PopulateTable(int? id)
        {

            List<AkInden1> akInden1Table = _context.AkInden1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akInden1 = akInden1Table;

            List<AkInden2> akInden2Table = _context.AkInden2
                //.Include(b => b.AkCarta)
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Bil)
                .ToList();
            ViewBag.akInden2 = akInden2Table;
        }

        private void PopulateCart(AkInden akInden)
        {
            List<AkInden1> akInden1Table = _context.AkInden1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenId == akInden.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkInden1 akInden1 in akInden1Table)
            {
                _cart.AddItem1(akInden1.AkIndenId,
                                akInden1.AkCartaId,
                                akInden1.Amaun);
            }

            List<AkInden2> akInden2Table = _context.AkInden2
                //.Include(b => b.JPerihal)
                .Where(b => b.AkIndenId == akInden.Id)
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkInden2 akInden2 in akInden2Table)
            {
                _cart.AddItem2(akInden2.AkIndenId,
                               akInden2.Indek,
                               akInden2.Bil,
                               akInden2.NoStok ?? "",
                               akInden2.Perihal?.ToUpper() ?? "",
                               akInden2.Kuantiti,
                               akInden2.Unit ?? "",
                               akInden2.Harga,
                               akInden2.Amaun);
            }
        }
        // GET: AkInden/Create
        [Authorize(Policy = "TG003C")]
        public IActionResult Create()
        {

            ViewBag.NoRujukan = RunningNumber(1,DateTime.Now.ToString("yyyy"));
            CartEmpty();
            PopulateList();
            return View();
        }

        public JsonResult GetAnItemCartAkInden1(AkInden1 akInden1)
        {

            try
            {
                AkInden1 data = _cart.Lines1.Where(x => x.AkCartaId == akInden1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        //save cart akInden1
        public async Task<JsonResult> SaveCartAkInden1(
            AkInden1 akInden1,
            string tahun,
            int jKWId,
            int jBahagianId)
        {

            try
            {

                var akI1 = _cart.Lines1.Where(x => x.AkCartaId == akInden1.AkCartaId).FirstOrDefault();

                if (akI1 != null)
                {
                    // check for baki peruntukan
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == tahun && x.VotId == akI1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                            .AnyAsync();

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akI1.AkCartaId, jKWId, jBahagianId);

                        if (sum < akInden1.Amaun)
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    else
                    {
                        return Json(new { result = "ERROR" });
                    }
                    // check for baki peruntukan end

                    _cart.RemoveItem1(akInden1.AkCartaId);

                    _cart.AddItem1(akInden1.AkIndenId,
                                    akInden1.AkCartaId,
                                    akInden1.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akInden1 end

        // get all item from cart akInden1
        public JsonResult GetAllItemCartAkInden1(AkInden1 akInden1)
        {

            try
            {
                List<AkInden1> data = _cart.Lines1.ToList();

                foreach (AkInden1 item in data)
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
        // get all item from cart akInden1 end

        // get an item from cart akInden2
        public JsonResult GetAnItemCartAkInden2(AkInden2 akInden2)
        {

            try
            {
                AkInden2 data = _cart.Lines2.Where(x => x.Indek == akInden2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akInden2 end

        //save cart akInden2
        public JsonResult SaveCartAkInden2(AkInden2 akInden2)
        {

            try
            {

                var akI2 = _cart.Lines2.Where(x => x.Indek == akInden2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akI2 != null)
                {
                    _cart.RemoveItem2(akInden2.Indek);

                    _cart.AddItem2(akInden2.AkIndenId,
                                   akInden2.Indek,
                                   akInden2.Bil,
                                   akInden2.NoStok ?? "",
                                   akInden2.Perihal?.ToUpper() ?? "",
                                   akInden2.Kuantiti,
                                   akInden2.Unit ?? "",
                                   akInden2.Harga,
                                   akInden2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPO2 end

        // get all item from cart akInden2
        public JsonResult GetAllItemCartAkInden2()
        {

            try
            {
                List<AkInden2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akInden2 end
        // POST: AkInden/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG003C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkInden akInden,string syscode)
        {
            AkInden m = new AkInden();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var pembekal = _context.AkPembekal.FirstOrDefault(x => x.Id == akInden.AkPembekalId);

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akInden.JKWId,akInden.Tahun);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                m.JKWId = akInden.JKWId;
                m.JBahagianId = akInden.JBahagianId;
                m.NoInden = noRujukan;
                m.Tarikh = akInden.Tarikh;
                m.Tajuk = akInden.Tajuk?.ToUpper() ?? "";
                m.TarikhBekalan = akInden.TarikhBekalan;
                m.AkNotaMintaId = akInden.AkNotaMintaId;
                m.TarikhPosting = akInden.TarikhPosting;
                m.AkPembekal = pembekal;
                m.Jumlah = akInden.Jumlah;
                m.FlPosting = 0;
                m.FlHapus = 0;
                m.FlCetak = 0;
                m.IsInKewangan = akInden.IsInKewangan;
                m.Tahun = akInden.Tahun;
                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkInden1 = _cart.Lines1.ToArray();
                m.AkInden2 = _cart.Lines2.ToArray();

                // check for baki peruntukan
                foreach (AkInden1 item in m.AkInden1)
                {
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                        .Where(x => x.Tahun == m.Tahun && x.VotId == item.AkCartaId && x.JKWId == m.JKWId && x.JBahagianId == m.JBahagianId)
                        .AnyAsync();

                    var carta = _context.AkCarta.Find(item.AkCartaId);

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(m.Tahun, item.AkCartaId, m.JKWId, m.JBahagianId);

                        if (sum < item.Amaun)
                        {
                            TempData[SD.Error] = "Bajet untuk kod akaun " + carta?.Kod + " tidak mencukupi.";
                            PopulateList();
                            CartEmpty();
                            PopulateTableFromCart();
                            ViewBag.NoRujukan = noRujukan;
                            return View(akInden);
                        }
                    }
                    else
                    {
                        TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta?.Kod;
                        PopulateList();
                        CartEmpty();
                        PopulateTableFromCart();
                        ViewBag.NoRujukan = noRujukan;
                        return View(akInden);
                    }
                }
                // check for baki peruntukan end

                await _akIndenRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoInden, m.NoInden, 0, m.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat Pesanan Tempatan berjaya ditambah. No Pendaftaran adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Data gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            ViewBag.NoRujukan = noRujukan;
            return View(akInden);
        }

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkInden1> tbl1 = new List<AkInden1>();
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
            ViewBag.akInden1 = tbl1;
            // table 1 end

            // table 2
            List<AkInden2> tbl2 = new List<AkInden2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    tbl2.Add(item);
                }
            }
            ViewBag.akInden2 = tbl2;
            // table 2 end
        }
        // populate table from cart end


        // GET: AkInden/Edit/5
        [Authorize(Policy = "TG003E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkInden == null)
            {
                return NotFound();
            }

            var akInden = await _akIndenRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akInden.JKWId);
            akInden.JKW = kw;

            // check if already posting redirect back
            if (akInden.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(akInden);
            return View(akInden);
        }

        // get latest Index number in AkInden2
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
        // POST: AkInden/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG003E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkInden akInden, decimal JumlahPerihal, string syscode)
        {
            if (id != akInden.Id)
            {
                return NotFound();
            }
            if (akInden.Jumlah != JumlahPerihal)
            {
                TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
                PopulateList();
                PopulateTableFromCart();
                return View(akInden);
            }
                if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                    AkInden dataAsal = await _akIndenRepo.GetById(id);

                    // list of input that cannot be change
                    akInden.Tahun = dataAsal.Tahun;
                    akInden.JKWId = dataAsal.JKWId;
                    akInden.JBahagianId = dataAsal.JBahagianId;
                    akInden.AkNotaMintaId = dataAsal.AkNotaMintaId;
                    akInden.NoInden = dataAsal.NoInden;
                    akInden.TarMasuk = dataAsal.TarMasuk;
                    akInden.UserId = dataAsal.UserId;
                    akInden.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    akInden.FlCetak = 0;
                    // list of input that cannot be change end

                    if (dataAsal.AkInden1 != null)
                    {
                        foreach (AkInden1 item in dataAsal.AkInden1)
                        {
                            var model = _context.AkInden1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    if (dataAsal.AkInden2 != null)
                    {
                        foreach (AkInden2 item in dataAsal.AkInden2)
                        {
                            var model = _context.AkInden2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    decimal jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;


                    akInden.AkInden1 = _cart.Lines1.ToList();
                    akInden.AkInden2 = _cart.Lines2.ToList();

                    // check for baki peruntukan
                    foreach (AkInden1 item in _cart.Lines1)
                    {

                        bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == akInden.Tahun && x.VotId == item.AkCartaId && x.JKWId == akInden.JKWId && x.JBahagianId == akInden.JBahagianId)
                            .AnyAsync();

                        var carta = _context.AkCarta.Find(item.AkCartaId);

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akInden.Tahun, item.AkCartaId, akInden.JKWId, akInden.JBahagianId);

                            if (sum < item.Amaun)
                            {
                                TempData[SD.Error] = "Bajet untuk kod akaun " + carta?.Kod + " tidak mencukupi.";
                                PopulateList();
                                PopulateTableFromCart();
                                return View(akInden);
                            }
                        }
                        else
                        {
                            TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta?.Kod;
                            PopulateList();
                            PopulateTableFromCart() ;
                            PopulateTable(id);

                            return View(akInden);
                        }
                    }
                    // check for baki peruntukan end

                    akInden.UserIdKemaskini = user?.UserName ?? "";
                    akInden.TarKemaskini = DateTime.Now;
                    akInden.SuPekerjaKemaskiniId = pekerjaId;
                    akInden.FlCetak = 0;

                    _context.Update(akInden);

                    //insert applog
                    if (jumlahAsal != akInden.Jumlah)
                    {
                        _appLog.Insert("Ubah", "RM" +  Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(akInden.Jumlah).ToString("#,##0.00"), akInden.NoInden, id, akInden.Jumlah, pekerjaId,modul, syscode,namamodul,user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", akInden.NoInden, id, akInden.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                    }
                    //insert applog end
                    CartEmpty();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkIndenExists(akInden.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Data gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            PopulateTable(id);

            return View(akInden);
        }

        // GET: AkInden/Delete/5
        [Authorize(Policy = "TG003D")] 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkInden == null)
            {
                return NotFound();
            }

            var akInden = await _akIndenRepo.GetById((int)id);
            if (akInden == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akInden);
        }

        // POST: AkInden/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "TG003D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkInden == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkInden'  is null.");
            }
            var akInden = await _context.AkInden.FindAsync(id);
            if (akInden != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                akInden.UserIdKemaskini = user?.UserName ?? "";
                akInden.TarKemaskini = DateTime.Now;
                akInden.SuPekerjaKemaskiniId = pekerjaId;
                // check if already posting redirect back
                if (akInden.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }
                akInden.FlCetak = 0;
                _context.AkInden.Update(akInden);

                _context.AkInden.Remove(akInden);

                //insert applog
                _appLog.Insert("Hapus", akInden.NoInden, akInden.NoInden, id, akInden.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool AkIndenExists(int id)
        {
          return (_context.AkInden?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akInden1 = new List<int>();
                ViewBag.akInden2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkInden1(
            AkInden1 akInden1,
            string tahun,
            int jKWId,
            int jBahagianId)
        {

            try
            {
                if (akInden1 != null)
                {
                    // check for baki peruntukan
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == tahun && x.VotId == akInden1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                            .AnyAsync();

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akInden1.AkCartaId, jKWId, jBahagianId);

                        if (sum < akInden1.Amaun)
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    else
                    {
                        return Json(new { result = "ERROR" });
                    }
                    // check for baki peruntukan end

                    _cart.AddItem1(akInden1.AkIndenId,
                                   akInden1.AkCartaId,
                                   akInden1.Amaun);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkInden2(AkInden2 akInden2)
        {

            try
            {
                if (akInden2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akInden2.AkIndenId,
                         akInden2.Indek,
                         akInden2.Bil,
                         akInden2.NoStok ?? "",
                         akInden2.Perihal?.ToUpper() ?? "",
                         akInden2.Kuantiti,
                         akInden2.Unit ?? "",
                         akInden2.Harga,
                         akInden2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkInden1(AkInden1 akInden1)
        {

            try
            {
                if (akInden1 != null)
                {

                    _cart.RemoveItem1(akInden1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkInden2(AkInden2 akInden2)
        {

            try
            {
                if (akInden2 != null)
                {

                    _cart.RemoveItem2(akInden2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // posting function
        [Authorize(Policy = "TG003T")]
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

                AkInden akInden = await _akIndenRepo.GetById((int)id);

                //check for print
                if (akInden.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end


                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("IK/" + akInden.NoInden)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";
                    return RedirectToAction(nameof(Index));
                }

                // check for baki peruntukan
                if (akInden.AkInden1 != null)
                {
                    foreach (AkInden1 item in akInden.AkInden1)
                    {
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == akInden.Tahun && x.VotId == item.AkCartaId && x.JKWId == akInden.JKWId && x.JBahagianId == akInden.JBahagianId)
                                .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akInden.Tahun, item.AkCartaId, akInden.JKWId, akInden.JBahagianId);

                            if (sum < item.Amaun)
                            {
                                TempData[SD.Error] = "Bajet untuk kod akaun " + item.AkCarta?.Kod + " tidak mencukupi.";
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + item.AkCarta?.Kod;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    //posting operation start here

                    foreach (AkInden1 item in akInden.AkInden1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotPosting = new AbBukuVot()
                        {
                            Tahun = akInden.Tahun,
                            JKWId = akInden.JKWId,
                            JBahagianId = akInden.JBahagianId,
                            Tarikh = akInden.Tarikh,
                            Kod = akInden.AkPembekal?.KodSykt ?? "",
                            Penerima = akInden.AkPembekal?.NamaSykt ?? "",
                            VotId = item.AkCartaId,
                            Rujukan = akInden.NoInden,
                            Tanggungan = item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotPosting);
                        // insert into AbBukuVot end

                    }

                    //update AkNotaMinta
                    if (akInden.AkNotaMintaId != null)
                    {
                        var noInden = akInden.NoInden;
                        var tarikhInden = DateTime.Now;

                        AkNotaMinta akNM = await _akNotaMintaRepo.GetById((int)akInden.AkNotaMintaId);

                        akNM.NoCAS = noInden;
                        akNM.TarikhSeksyenKewangan = tarikhInden;

                        await _akNotaMintaRepo.Update(akNM);
                    }

                    //update AkNotaMinta end

                    //update posting status in akPO
                    akInden.FlPosting = 1;
                    akInden.TarikhPosting = DateTime.Now;
                    await _akIndenRepo.Update(akInden);

                    //insert applog
                    _appLog.Insert("Posting", "Posting Data", akInden.NoInden, (int)id, akInden.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan.";
                }
                
                // check for baki peruntukan end

                    
                }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "TG003UT")]
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

                AkInden akInden = await _akIndenRepo.GetById((int)id);

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == akInden.NoInden).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";
                    return RedirectToAction(nameof(Index));
                }
                
                    // check if already linked with AkBelian
                    AkBelian Belian = _context.AkBelian.Where(x => x.AkIndenId == id).FirstOrDefault();

                    if (Belian != null)
                    {

                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal kelulusan tidak dibenarkan";
                    return RedirectToAction(nameof(Index));
                    //}
                }

                        //unposting operation start here
                        //delete data from akAkaun
                        foreach (AbBukuVot item in abBukuVot)
                        {
                            await _abBukuVotRepo.Delete(item.Id);
                        }

                        //update posting status in akPO

                        //update AkNotaMinta

                        if (akInden.AkNotaMintaId != null)
                        {
                            AkNotaMinta akNM = await _akNotaMintaRepo.GetById((int)akInden.AkNotaMintaId);

                            akNM.NoCAS = "";
                            akNM.TarikhSeksyenKewangan = null;

                            await _akNotaMintaRepo.Update(akNM);
                        }

                        //update AkNotaMinta end

                        akInden.FlPosting = 0;
                        akInden.TarikhPosting = null;
                        await _akIndenRepo.Update(akInden);

                        //insert applog
                        _appLog.Insert("UnPosting", "UnPosting Data", akInden.NoInden, (int)id, akInden.Jumlah, pekerjaId, modul, syscode, namamodul, user);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya batal kelulusan.";
                        //unposting operation end

            }

            return RedirectToAction(nameof(Index));

        }

        //// POST: AkPO/Cancel/5
        [Authorize(Policy = "TG003B")]
        public async Task<IActionResult> Cancel(int id, string syscode)
        {
            var obj = await _akIndenRepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("IK/" + obj.NoInden)).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // check if already linked with AkBelian
                AkBelian Belian = _context.AkBelian.Where(x => x.AkIndenId == id && x.FlBatal == 0).FirstOrDefault();

                if (Belian != null)
                {

                    //linkage id error
                    TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal tidak dibenarkan";
                    return RedirectToAction(nameof(Index));
                    //}
                }

                if (obj.AkInden1 != null)
                {
                    //insert contra data into abBukuVot
                    foreach (AkInden1 item in obj.AkInden1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotCanceling = new AbBukuVot()
                        {
                            Tahun = obj.Tahun,
                            JKWId = obj.JKWId,
                            JBahagianId = obj.JBahagianId,
                            Tarikh = obj.Tarikh,
                            Kod = obj.AkPembekal?.KodSykt ?? "",
                            Penerima = obj.AkPembekal?.NamaSykt ?? "",
                            VotId = item.AkCartaId,
                            Rujukan = "IK/"+obj.NoInden,
                            Tanggungan = 0 - item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotCanceling);
                        // insert into AbBukuVot end

                    }

                    //update AkPO

                    obj.FlBatal = 1;
                    obj.TarBatal = DateTime.Now;
                    await _akIndenRepo.Update(obj);

                    //insert applog
                    _appLog.Insert("Batal", "Batal Data", obj.NoInden, (int)id, obj.Jumlah, pekerjaId, modul, syscode, namamodul, user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dibatalkan.";
                    //unposting operation end
                }

            }

            return RedirectToAction(nameof(Index));
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "TG003R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _akIndenRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            //check if Nota Minta exist && Posting == 1 or not
            if (obj.AkNotaMintaId != null)
            {
                var nm = await _context.AkNotaMinta.FirstOrDefaultAsync(x => x.Id == obj.AkNotaMintaId && x.FlPosting == 1);

                if (nm == null)
                {
                    TempData[SD.Error] = "Nota minta belum posting / tidak wujud..!";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Batal operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkInden.Update(obj);

            // Batal operation end

            //insert applog
            _appLog.Insert("Rollback", "Rollback Data", obj.NoInden, (int)id, obj.Jumlah, pekerjaId, modul, syscode, namamodul, user);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        // printing resit rasmi by akPO.Id
        [Authorize(Policy = "TG003P")]
        public async Task<IActionResult> PrintPdf(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            AkInden akInden = await _akIndenRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akInden.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(0 - akInden.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akInden.Jumlah)).ToUpper();
            }

            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user!.Email);
            var pekerja = _context.SuPekerja.FirstOrDefault(x => x.Id == namaUser!.SuPekerjaId);
            var jawatan = "Super Admin";
            if (pekerja != null)
            {
                jawatan = pekerja.Jawatan;
            }

            IndenPrintModel data = new IndenPrintModel();

            CompanyDetails company = await _userService.GetCompanyDetails();
            data.CompanyDetail = company;
            data.AkInden = akInden;
            //data.AkPO.JNegeri = negeri;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = namaUser?.Nama ?? "";
            data.Jawatan = jawatan;

            //update cetak -> 1
            akInden.FlCetak = 1;
            await _akIndenRepo.Update(akInden);

            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", akInden.NoInden, id, akInden.Jumlah, pekerjaId,modul,syscode,namamodul,user);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("IndenPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing inden kerja end
    }
}
