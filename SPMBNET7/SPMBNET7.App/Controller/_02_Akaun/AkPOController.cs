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
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Math;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    public class AkPOController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "TG001";
        public const string namamodul = "Pesanan Tempatan";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly IRepository<AkNotaMinta, int, string> _akNotaMintaRepo;
        private readonly ListViewIRepository<AkPO1, int> _akPO1Repo;
        private readonly ListViewIRepository<AkPO2, int> _akPO2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkPembekal, int, string> _akpembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly AkPOLarasController _akPOLarasController;
        private readonly IRepository<AkPOLaras, int, string> _akPoLarasRepo;
        private readonly UserServices _userService;
        private CartPO _cart;

        public AkPOController(ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPO, int, string> akPORepository,
            IRepository<AkNotaMinta, int, string> akNotaMintaRepository,
            ListViewIRepository<AkPO1, int> akPO1Repository,
            ListViewIRepository<AkPO2, int> akPO2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JNegeri, int, string> negeriRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            CustomIRepository<string, int> customRepo,
            AkPOLarasController akPOLarasController,
            IRepository<AkPOLaras, int, string> akPOLarasRepository,
            UserServices userService,
            CartPO cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akPORepo = akPORepository;
            _akNotaMintaRepo = akNotaMintaRepository;
            _akPO1Repo = akPO1Repository;
            _akPO2Repo = akPO2Repository;
            _akCartaRepo = akCartaRepository;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akpembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _customRepo = customRepo;
            _cart = cart;
            _akPOLarasController = akPOLarasController;
            _akPoLarasRepo = akPOLarasRepository;
            _userService = userService;
        }

        //Function Running Number
        private string RunningNumber(int data, string tahun)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

            var kumpulanWang = kw?.Kod ?? "100";
            //var year = DateTime.Now.Year.ToString();
            var year = tahun;
            string prefix = "PO/" + year + "/" + kumpulanWang + "/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkPO
                .IgnoreQueryFilters()
                .Where(x => x.Tahun == year && x.JKWId == data)
                .Max(x => x.NoPO);
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
        public JsonResult JsonGetKod(AkPO data)
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

        // GET: AkPO
        [Authorize(Policy = "TG001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No PO", Value = "NoRujukan" },
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

            var akPO = await _akPORepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akPO = await _akPORepo.GetAllIncludeDeletedItems();
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akPO = akPO.Where(s => s.NoPO.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Pembekal")
                    {
                        akPO = akPO.Where(s => s.AkPembekal!.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akPO = akPO.Where(x => x.Tarikh >= date1
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

            return View(akPO);
        }


        // GET: AkPO/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkPO == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetByIdIncludeDeletedItems((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;

            if (akPO == null)
            {
                return NotFound();
            }

            decimal jumlahPerihal = 0;
            if (akPO.AkPO2 != null)
            {
                foreach (var item in akPO.AkPO2)
                {
                    jumlahPerihal = jumlahPerihal + item.Amaun;
                }
            }


            ViewBag.JumlahPerihal = jumlahPerihal;

            PopulateList();
            PopulateTable(id);
            return View(akPO);
        }

        // GET: AkPO/Create
        [Authorize(Policy = "TG001C")]
        public IActionResult Create()
        {
            var noRujukan = RunningNumber(1, DateTime.Now.ToString("yyyy"));
            ViewBag.NoRujukan = noRujukan;

            CartEmpty();
            PopulateList();
            return View();
        }

        public JsonResult GetAnItemCartAkPO1(AkPO1 akPO1)
        {

            try
            {
                AkPO1 data = _cart.Lines1.Where(x => x.AkCartaId == akPO1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPO1 end

        //save cart akPO1
        public async Task<JsonResult> SaveCartAkPO1(
            AkPO1 akPO1,
            string tahun,
            int jKWId,
            int jBahagianId)
        {

            try
            {

                var akP1 = _cart.Lines1.Where(x => x.AkCartaId == akPO1.AkCartaId).FirstOrDefault();

                if (akP1 != null)
                {
                    // check for baki peruntukan
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == tahun && x.VotId == akP1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                            .AnyAsync();

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akP1.AkCartaId, jKWId, jBahagianId);

                        if (sum < akPO1.Amaun)
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    else
                    {
                        return Json(new { result = "ERROR" });
                    }
                    // check for baki peruntukan end

                    _cart.RemoveItem1(akPO1.AkCartaId);

                    _cart.AddItem1(akPO1.AkPOId,
                                    akPO1.AkCartaId,
                                    akPO1.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPO1 end

        // get all item from cart akPO1
        public JsonResult GetAllItemCartAkPO1(AkPO1 akPO1)
        {

            try
            {
                List<AkPO1> data = _cart.Lines1.ToList();

                foreach (AkPO1 item in data)
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
        // get all item from cart akPO1 end

        // get an item from cart akPO2
        public JsonResult GetAnItemCartAkPO2(AkPO2 akPO2)
        {

            try
            {
                AkPO2 data = _cart.Lines2.Where(x => x.Indek == akPO2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPO2 end

        //save cart akPO2
        public JsonResult SaveCartAkPO2(AkPO2 akPO2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.Indek == akPO2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akPO2.Indek);

                    _cart.AddItem2(akPO2.AkPOId,
                               akPO2.Indek,
                               akPO2.Bil,
                               akPO2.NoStok ?? "",
                               akPO2.Perihal?.ToUpper() ?? "",
                               akPO2.Kuantiti,
                               akPO2.Unit ?? "",
                               akPO2.Harga,
                               akPO2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPO2 end

        // get all item from cart akPO2
        public JsonResult GetAllItemCartAkPO2()
        {

            try
            {
                List<AkPO2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPO2 end

        // on change no PO controller
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
        //on change no PO controller end

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

            List<AkNotaMinta> akNotaMintaList = _context.AkNotaMinta.Include(b => b.AkPO)
                .Where(x => x.FlPosting == 1 &&
                x.FlJenis == 0)
                .ToList();

            List<AkNotaMinta> akNMUpdated = new List<AkNotaMinta>();

            foreach (var item in akNotaMintaList)
            {
                decimal jumlahPO = 0;
                if (item.AkPO != null && item.AkPO.Count() > 0)
                {
                    foreach (var akPO in item.AkPO)
                    {
                        jumlahPO = jumlahPO + akPO.Jumlah;
                    }
                    if (jumlahPO == item.Jumlah)
                    {
                        continue;
                    }
                    else
                    {
                        akNMUpdated.Add(item);
                    }
                }
                else
                {
                    akNMUpdated.Add(item);
                }
            }

            ViewBag.AkNotaMinta = akNMUpdated;

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

            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO1 = akPO1Table;

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Bil)
                .ToList();
            ViewBag.akPO2 = akPO2Table;
        }
        private void PopulateCartFromDb(AkPO akPO)
        {
            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPO1 akPO1 in akPO1Table)
            {
                _cart.AddItem1(akPO1.AkPOId,
                                akPO1.AkCartaId,
                                akPO1.Amaun);
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.JPerihal)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkPO2 akPO2 in akPO2Table)
            {
                _cart.AddItem2(akPO2.AkPOId,
                               akPO2.Indek,
                               akPO2.Bil,
                               akPO2.NoStok ?? "",
                               akPO2.Perihal?.ToUpper() ?? "",
                               akPO2.Kuantiti,
                               akPO2.Unit ?? "",
                               akPO2.Harga,
                               akPO2.Amaun);
            }
        }

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkPO1> tbl1 = new List<AkPO1>();
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
            ViewBag.akPO1 = tbl1;
            // table 1 end

            // table 2
            List<AkPO2> tbl2 = new List<AkPO2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    tbl2.Add(item);
                }
            }
            ViewBag.akPO2 = tbl2;
            // table 2 end
        }
        // populate table from cart end

        public async Task<JsonResult> SaveAkPO1(
    AkPO1 akPO1,
    string tahun,
    int jKWId,
    int jBahagianId)
        {

            try
            {
                if (akPO1 != null)
                {
                    // check for baki peruntukan
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == tahun && x.VotId == akPO1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                            .AnyAsync();

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akPO1.AkCartaId, jKWId, jBahagianId);

                        if (sum < akPO1.Amaun)
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    else
                    {
                        return Json(new { result = "ERROR" });
                    }
                    // check for baki peruntukan end

                    _cart.AddItem1(akPO1.AkPOId,
                                   akPO1.AkCartaId,
                                   akPO1.Amaun);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akPO2.AkPOId,
                         akPO2.Indek,
                         akPO2.Bil,
                         akPO2.NoStok ?? "",
                         akPO2.Perihal?.ToUpper() ?? "",
                         akPO2.Kuantiti,
                         akPO2.Unit ?? "",
                         akPO2.Harga,
                         akPO2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null)
                {

                    _cart.RemoveItem1(akPO1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {

                    _cart.RemoveItem2(akPO2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkPO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPO akPO, string syscode)
        {
            AkPO m = new AkPO();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
            var pembekal = _context.AkPembekal.FirstOrDefault(x => x.Id == akPO.AkPembekalId);

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akPO.JKWId, akPO.Tahun);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                m.JKWId = akPO.JKWId;
                m.JBahagianId = akPO.JBahagianId;
                m.NoPO = noRujukan;
                m.Tarikh = akPO.Tarikh;
                m.AkNotaMintaId = akPO.AkNotaMintaId;
                m.TarikhPosting = akPO.TarikhPosting;
                m.AkPembekal = pembekal;
                m.Tajuk = akPO.Tajuk?.ToUpper() ?? "";
                m.Jumlah = akPO.Jumlah;
                m.FlPosting = 0;
                m.FlHapus = 0;
                m.FlCetak = 0;
                m.IsInKewangan = akPO.IsInKewangan;
                m.TarikhBekalan = akPO.TarikhBekalan;
                m.Tahun = akPO.Tahun;
                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkPO1 = _cart.Lines1.ToArray();
                m.AkPO2 = _cart.Lines2.ToArray();

                // check for baki peruntukan
                foreach (AkPO1 item in m.AkPO1)
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
                            PopulateTableFromCart();
                            ViewBag.NoRujukan = noRujukan;
                            return View(akPO);
                        }
                    }
                    else
                    {
                        TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta?.Kod;
                        PopulateList();
                        PopulateTableFromCart();
                        ViewBag.NoRujukan = noRujukan;

                        return View(akPO);
                    }
                }
                // check for baki peruntukan end

                await _akPORepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoPO, m.NoPO, 0, m.Jumlah, pekerjaId, modul, syscode, namamodul, user);
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
            return View(akPO);
        }

        // GET: AkPO/Edit/5
        [Authorize(Policy = "TG001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkPO == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;

            // check if already posting redirect back
            if (akPO.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akPO);
            return View(akPO);
        }

        // get latest Index number in AkPO2
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

        // POST: AkPO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkPO akPO, decimal JumlahPerihal, string syscode)
        {
            if (id != akPO.Id)
            {
                return NotFound();
            }

            if (akPO.Jumlah != JumlahPerihal)
            {
                TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
                PopulateList();
                PopulateTableFromCart();
                return View(akPO);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                    AkPO dataAsal = await _akPORepo.GetById(id);

                    // list of input that cannot be change
                    akPO.Tahun = dataAsal.Tahun;
                    akPO.JKWId = dataAsal.JKWId;
                    akPO.JBahagianId = dataAsal.JBahagianId;
                    akPO.AkNotaMintaId = dataAsal.AkNotaMintaId;
                    akPO.NoPO = dataAsal.NoPO;
                    akPO.TarMasuk = dataAsal.TarMasuk;
                    akPO.UserId = dataAsal.UserId;
                    akPO.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    akPO.FlCetak = 0;
                    // list of input that cannot be change end

                    if (dataAsal.AkPO1 != null)
                    {
                        foreach (AkPO1 item in dataAsal.AkPO1)
                        {
                            var model = _context.AkPO1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }

                    if (dataAsal.AkPO2 != null)
                    {
                        foreach (AkPO2 item in dataAsal.AkPO2)
                        {
                            var model = _context.AkPO2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }

                    decimal jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;


                    akPO.AkPO1 = _cart.Lines1.ToList();
                    akPO.AkPO2 = _cart.Lines2.ToList();

                    // check for baki peruntukan
                    foreach (AkPO1 item in _cart.Lines1)
                    {

                        bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == akPO.Tahun && x.VotId == item.AkCartaId && x.JKWId == akPO.JKWId && x.JBahagianId == akPO.JBahagianId)
                            .AnyAsync();

                        var carta = _context.AkCarta.Find(item.AkCartaId);

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akPO.Tahun, item.AkCartaId, akPO.JKWId, akPO.JBahagianId);

                            if (sum < item.Amaun)
                            {
                                TempData[SD.Error] = "Bajet untuk kod akaun " + carta?.Kod + " tidak mencukupi.";
                                PopulateList();
                                PopulateTableFromCart();

                                return View(akPO);
                            }
                        }
                        else
                        {
                            TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta?.Kod;
                            PopulateList();
                            PopulateTableFromCart();

                            return View(akPO);
                        }
                    }

                    // check for baki peruntukan end
                    akPO.UserIdKemaskini = user?.UserName ?? "";
                    akPO.TarKemaskini = DateTime.Now;
                    akPO.SuPekerjaKemaskiniId = pekerjaId;
                    akPO.FlCetak = 0;

                    _context.Update(akPO);

                    //insert applog
                    if (jumlahAsal != akPO.Jumlah)
                    {
                        _appLog.Insert("Ubah", "RM" +  Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(akPO.Jumlah).ToString("#,##0.00"), akPO.NoPO, id, akPO.Jumlah, pekerjaId, modul, syscode, namamodul, user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", akPO.NoPO, id, akPO.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                    }
                    //insert applog end
                    
                    TempData[SD.Success] = "Data berjaya diubah..!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPOExists(akPO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                CartEmpty();
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Data gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            return View(akPO);
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

        // GET: AkPO/Delete/5
        [Authorize(Policy = "TG001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkPO == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetById((int)id);
            if (akPO == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);
            return View(akPO);
        }

        // POST: AkPO/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "TG001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkPO == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkPO'  is null.");
            }
            var akPO = await _context.AkPO.FindAsync(id);
            if (akPO != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                akPO.UserIdKemaskini = user?.UserName ?? "";
                akPO.TarKemaskini = DateTime.Now;
                akPO.SuPekerjaKemaskiniId = pekerjaId;
                // check if already posting redirect back
                if (akPO.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }
                akPO.FlCetak = 0;
                _context.AkPO.Update(akPO);

                _context.AkPO.Remove(akPO);

                //insert applog
                _appLog.Insert("Hapus", akPO.NoPO, akPO.NoPO, id, akPO.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                //insert applog end
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
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

        // posting function
        [Authorize(Policy = "TG001T")]
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

                AkPO akPO = await _akPORepo.GetById((int)id);

                //check for print
                if (akPO.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PO/" + akPO.NoPO)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

                }

                // check for baki peruntukan
                if (akPO.AkPO1 != null)
                {
                    foreach (AkPO1 item in akPO.AkPO1)
                    {
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == akPO.Tahun && x.VotId == item.AkCartaId && x.JKWId == akPO.JKWId && x.JBahagianId == akPO.JBahagianId)
                                .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akPO.Tahun, item.AkCartaId, akPO.JKWId, akPO.JBahagianId);

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

                        //posting operation start here
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotPosting = new AbBukuVot()
                        {
                            Tahun = akPO.Tahun,
                            JKWId = akPO.JKWId,
                            JBahagianId = akPO.JBahagianId,
                            Tarikh = akPO.Tarikh,
                            Kod = akPO.AkPembekal?.KodSykt ?? "",
                            Penerima = akPO.AkPembekal?.NamaSykt ?? "",
                            VotId = item.AkCartaId,
                            Rujukan = akPO.NoPO,
                            Tanggungan = item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotPosting);
                        // insert into AbBukuVot end

                    }
                }

                // check for baki peruntukan end

                //update AkNotaMinta
                if (akPO.AkNotaMintaId != null)
                {
                    //var noPO = "PO/" + akPO.NoPO;
                    var noPO = akPO.NoPO;
                    var tarikhPO = DateTime.Now;

                    AkNotaMinta akNM = await _akNotaMintaRepo.GetById((int)akPO.AkNotaMintaId);

                    akNM.NoCAS = noPO;
                    akNM.TarikhSeksyenKewangan = tarikhPO;

                    await _akNotaMintaRepo.Update(akNM);
                }

                //update AkNotaMinta end

                //update posting status in akPO
                akPO.FlPosting = 1;
                akPO.TarikhPosting = DateTime.Now;
                await _akPORepo.Update(akPO);

                //insert applog
                _appLog.Insert("Posting", "Posting Data", akPO.NoPO, (int)id, akPO.Jumlah, pekerjaId, modul, syscode, namamodul, user);

                //insert applog end

                await _context.SaveChangesAsync();


                TempData[SD.Success] = "Data berjaya diluluskan.";


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "TG001UT")]
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

                AkPO akPO = await _akPORepo.GetById((int)id);

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == akPO.NoPO).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    // check if already linked with AkBelian
                    AkBelian Belian = _context.AkBelian.Where(x => x.AkPOId == id).FirstOrDefault();

                    if (Belian != null)
                    {
                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal kelulusan tidak dibenarkan";
                        //}
                    }

                    // check if already linked with AkPOLaras
                    AkPOLaras akPOLaras = _context.AkPOLaras.Where(x => x.AkPOId == id).FirstOrDefault();

                    if (akPOLaras != null)
                    {
                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada No Pelarasan Tanggungan " + akPOLaras.NoRujukan.ToUpper() + ". Batal kelulusan tidak dibenarkan";
                    }
                    else
                    {
                        //unposting operation start here
                        //delete data from akAkaun
                        foreach (AbBukuVot item in abBukuVot)
                        {
                            await _abBukuVotRepo.Delete(item.Id);
                        }

                        //update posting status in akPO

                        //update AkNotaMinta

                        if (akPO.AkNotaMintaId != null)
                        {
                            AkNotaMinta akNM = await _akNotaMintaRepo.GetById((int)akPO.AkNotaMintaId);

                            akNM.NoCAS = "";
                            akNM.TarikhSeksyenKewangan = null;

                            await _akNotaMintaRepo.Update(akNM);
                        }

                        //update AkNotaMinta end

                        akPO.FlPosting = 0;
                        akPO.TarikhPosting = null;
                        await _akPORepo.Update(akPO);

                        //insert applog
                        _appLog.Insert("UnPosting", "UnPosting Data", akPO.NoPO, (int)id, akPO.Jumlah, pekerjaId, modul, syscode, namamodul, user);

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
        //// POST: AkPO/Cancel/5
        [Authorize(Policy = "TG001B")]
        public async Task<IActionResult> Cancel(int id, string syscode)
        {
            var obj = await _akPORepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PO/" + obj.NoPO)).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }

            // check if already linked with AkBelian
            AkBelian Belian = _context.AkBelian.Where(x => x.AkPOId == id && x.FlBatal == 0).FirstOrDefault();

            if (Belian != null)
            {

                //linkage id error
                TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal tidak dibenarkan";
                //}
            }
            else
            {
                // check if already linked with AkPOLaras
                AkPOLaras akPOLaras = _context.AkPOLaras.Where(x => x.AkPOId == id && x.FlBatal == 0).FirstOrDefault();

                if (akPOLaras != null)
                {
                    //linkage id error
                    TempData[SD.Error] = "Data terkait pada No Pelarasan Tanggungan " + akPOLaras.NoRujukan.ToUpper() + ". Batal tidak dibenarkan";
                }
                else
                {
                    //unposting operation start here
                    if (obj.AkPO1 != null)
                    {
                        //insert contra data into abBukuVot
                        foreach (AkPO1 item in obj.AkPO1)
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
                                Rujukan = "PT/"+ _akPOLarasController.RunningNumber(DateTime.Now.ToString("yyyy")),
                                Tanggungan = 0 - item.Amaun
                            };

                            await _abBukuVotRepo.Insert(abBukuVotCanceling);
                            // insert into AbBukuVot end

                        }
                    }

                    //update AkPO

                    obj.FlBatal = 1;
                    obj.TarBatal = DateTime.Now;
                    await _akPORepo.Update(obj);

                    AkPOLaras l = new AkPOLaras();
                    l.JKWId = obj.JKWId;
                    l.JBahagianId = obj.JBahagianId;
                    l.NoRujukan = "PT/"+ _akPOLarasController.RunningNumber(DateTime.Now.ToString("yyyy"));
                    l.Tarikh = DateTime.Now;
                    l.AkPOId = obj.Id;
                    l.TarikhPosting = DateTime.Now;
                    l.Jumlah = 0 - obj.Jumlah;
                    l.FlPosting = 1;
                    l.FlHapus = 0;
                    l.FlCetak = 1;
                    l.Tahun = DateTime.Now.ToString("yyyy");
                    l.UserId = user?.UserName ?? "";
                    l.TarMasuk = DateTime.Now;
                    l.SuPekerjaMasukId = pekerjaId;

                    List<AkPOLaras1> akPOLaras1 = new List<AkPOLaras1>();
                    if (obj.AkPO1 != null)
                    {
                        foreach (AkPO1 item in obj.AkPO1)
                        {
                            akPOLaras1.Add(new AkPOLaras1
                            {
                                AkCartaId = item.AkCartaId,
                                Amaun = 0 - item.Amaun
                            });
                        }
                    }
                    
                    l.AkPOLaras1 = akPOLaras1;

                    List<AkPOLaras2> akPOLaras2 = new List<AkPOLaras2>();

                    if (obj.AkPO2 != null)
                    {
                        foreach (AkPO2 item in obj.AkPO2)
                        {
                            akPOLaras2.Add(new AkPOLaras2
                            {
                                Indek = item.Indek,
                                Bil = item.Bil,
                                NoStok = item.NoStok,
                                Perihal = "PELARASAN - " + item.Perihal,
                                Kuantiti = item.Kuantiti,
                                Unit = item.Unit,
                                Harga = 0 - item.Harga,
                                Amaun = 0 - item.Amaun

                            });
                        }
                    }
                    

                    l.AkPOLaras2 = akPOLaras2;

                    await _akPoLarasRepo.Insert(l);

                    //insert applog
                    _appLog.Insert("Batal", "Batal Data", obj.NoPO, (int)id, obj.Jumlah, pekerjaId, modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dibatalkan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "TG001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _akPORepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

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

            _context.AkPO.Update(obj);

            // Batal operation end

            //insert applog
            _appLog.Insert("Rollback", "Rollback Data", obj.NoPO, (int)id, obj.Jumlah, pekerjaId,modul,syscode,namamodul,user);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        // printing pesanan tempatan by akPO.Id
        [Authorize(Policy = "TG001P")]
        public async Task<IActionResult> PrintPdf(int id, string syscode)
        {
            AkPO akPO = await _akPORepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akPO.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(0 - akPO.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akPO.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user!.Email);
            var pekerja = _context.SuPekerja.FirstOrDefault(x => x.Id == namaUser!.SuPekerjaId);
            var jawatan = "Super Admin";
            if (pekerja != null)
            {
                jawatan = pekerja.Jawatan;
            }

            POPrintModel data = new POPrintModel();

            akPO.AkPO2 = akPO.AkPO2?.OrderBy(p => p.Bil).ToList();

            data.CompanyDetail = await _userService.GetCompanyDetails();
            data.AkPO = akPO;
            //data.AkPO.JNegeri = negeri;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = namaUser?.Nama ?? "";
            data.Jawatan = jawatan;

            //update cetak -> 1
            akPO.FlCetak = 1;
            await _akPORepo.Update(akPO);

            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", akPO.NoPO, id, akPO.Jumlah, pekerjaId,modul,syscode,namamodul,user);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("POPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing pesanan tempatan end

        private bool AkPOExists(int id)
        {
            return (_context.AkPO?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
