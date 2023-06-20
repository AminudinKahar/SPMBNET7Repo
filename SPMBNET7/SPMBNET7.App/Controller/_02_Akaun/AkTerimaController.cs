using System;
using System.Collections.Generic;
using System.Data;
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
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels;
using SPMBNET7.CoreBusiness._Enums;
using Microsoft.Build.Evaluation;
using Rotativa.AspNetCore;
using SPMBNET7.Infrastructure.Math;
using SPMBNET7.App.Pages.PrintModels._02_Akaun;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    public class AkTerimaController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "PR001";
        public const string namamodul = "Penerimaan";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly ListViewIRepository<AkTerima1, int> _akTerima1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly ListViewIRepository<AkTerima2, int> _akTerima2Repo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPPRepo;
        private readonly IRepository<AkPenghutang, int, string> _akPenghutangRepo;
        private readonly IRepository<AkInvois, int, string> _akInvoisRepo;
        private readonly UserServices _userService;
        private CartTerima _cart;

        public AkTerimaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTerima, int, string> akTerimaRepository,
            ListViewIRepository<AkTerima1, int> akTerima1Repository,
            ListViewIRepository<AkTerima2, int> akTerima2Repository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<JNegeri, int, string> negeriRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<SpPendahuluanPelbagai, int, string> spPPRepo,
            IRepository<AkPenghutang, int, string> akPenghutangRepo,
            IRepository<AkInvois, int, string> akInvoisRepo,
            UserServices userService,
            CartTerima cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akBankRepo = akBankRepository;
            _akTerimaRepo = akTerimaRepository;
            _akTerima1Repo = akTerima1Repository;
            _akTerima2Repo = akTerima2Repository;
            _akCartaRepo = akCartaRepository;
            _akAkaunRepo = akAkaunRepository;
            _spPPRepo = spPPRepo;
            _akPenghutangRepo = akPenghutangRepo;
            _akInvoisRepo = akInvoisRepo;
            _userService = userService;
            _cart = cart;
        }

        // GET: AkTerima
        [Authorize(Policy = "PR001")]
        // GET: AkTerima
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

            var akTerima = await _akTerimaRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akTerima = await _akTerimaRepo.GetAllIncludeDeletedItems();
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akTerima = akTerima.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akTerima = akTerima.Where(s => s.Nama.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akTerima = akTerima.Where(x => x.Tarikh >= date1
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

            List<AkTerimaViewModel> viewModel = new List<AkTerimaViewModel>();
            foreach (AkTerima item in akTerima)
            {
                decimal jumlahUrusniaga = 0;
                if (item.AkTerima2 != null)
                {
                    foreach (AkTerima2 item2 in item.AkTerima2)
                    {
                        jumlahUrusniaga += item2.Amaun;
                    }
                }

                viewModel.Add(new AkTerimaViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoRujukan = item.NoRujukan,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    Nama = item.Nama?.ToUpper() ?? "",
                    FlHapus = item.FlHapus,
                    FlPosting = item.FlPosting,
                    FlCetak = item.FlCetak,
                    JumlahUrusniaga = jumlahUrusniaga
                });
            }

            return View(viewModel);
        }

        // GET: AkTerima/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkTerima == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetByIdIncludeDeletedItems((int)id);

            // normal user access
            if (User.IsInRole("User"))
            {
                akTerima = await _akTerimaRepo.GetById((int)id);
            }

            if (akTerima == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akTerima);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<SpPendahuluanPelbagai> spList = _context.SpPendahuluanPelbagai.Where(b => b.FlPosting == 1).OrderBy(b => b.NoPermohonan).ToList();

            List<SpPendahuluanPelbagai> spListUpdated = new List<SpPendahuluanPelbagai>();

            foreach (var item in spList)
            {
                var ExistAkTerimaWithSp = _context.AkTerima.Any(b => b.SpPendahuluanPelbagaiId == item.Id);

                if (ExistAkTerimaWithSp == true)
                {
                    continue;

                }
                else
                {
                    var ExistAkPVWithSp = _context.AkPV.Any(b => b.SpPendahuluanPelbagaiId == item.Id);
                    if (ExistAkPVWithSp == true)
                    {
                        spListUpdated.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            ViewBag.SpPendahuluanPelbagai = spListUpdated;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

            List<JCaraBayar> jCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = jCaraBayarList;

            List<AkPenghutang> akPenghutangList = _context.AkPenghutang
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPenghutang = akPenghutangList;

            List<AkInvois> akInvoisList = _context.AkInvois
                .Where(b => b.FlPosting == 1)
                .OrderBy(b => b.Tarikh).ToList();

            foreach (var item in akInvoisList)
            {
                item.NoInbois = item.NoInbois.Substring(3);
            }
            ViewBag.AkInvois = akInvoisList;

        }

        private void PopulateTable(int? id)
        {
            List<AkTerima1> akTerima1Table = _context.AkTerima1
                .Include(b => b.AkCarta)
                .Where(b => b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima1 = akTerima1Table;

            List<AkTerima2> akTerima2Table = _context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(b => b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima2 = akTerima2Table;

            List<AkTerima3> akTerima3Table = _context.AkTerima3
                .Include(b => b.AkInvois)
                .Where(b => b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima3 = akTerima3Table;
        }
        private void PopulateCart()
        {
            List<AkTerima1> lines1 = _cart.Lines1.ToList();

            foreach (AkTerima1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            ViewBag.akTerima1 = lines1;

            List<AkTerima2> lines2 = _cart.Lines2.ToList();

            foreach (AkTerima2 item in lines2)
            {
                var jCaraBayar = _context.JCaraBayar.Where(x => x.Id == item.JCaraBayarId).FirstOrDefault();
                item.JCaraBayar = jCaraBayar;
            }

            ViewBag.akTerima2 = _cart.Lines2.ToList();
        }

        private void PopulateCartFromDb(AkTerima akTerima)
        {
            List<AkTerima1> akTerima1Table = _context.AkTerima1
                .Include(b => b.AkCarta)
                .Where(b => b.AkTerimaId == akTerima.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkTerima1 akTerima1 in akTerima1Table)
            {
                _cart.AddItem1(akTerima1.AkTerimaId,
                               akTerima1.Amaun,
                               akTerima1.AkCartaId);
            }

            ViewBag.akTerima1 = akTerima1Table;

            List<AkTerima2> akTerima2Table = _context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(b => b.AkTerimaId == akTerima.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkTerima2 akTerima2 in akTerima2Table)
            {
                _cart.AddItem2(akTerima2.AkTerimaId,
                               akTerima2.JCaraBayarId,
                               akTerima2.Amaun,
                               akTerima2.NoCek ?? "",
                               akTerima2.JenisCek,
                               akTerima2.KodBankCek ?? "",
                               akTerima2.TempatCek?.ToUpper() ?? "",
                               akTerima2.NoSlip ?? "",
                               akTerima2.TarSlip,
                               akTerima2.AkPenyataPemungutId);
            }

            ViewBag.akTerima2 = akTerima2Table;
        }

        private string GetNoRujukan(int data, string year)
        {
            var ptj = _context.JPTJ.Include(b => b.JKW).FirstOrDefault(x => x.JKWId == data);

            var initial = ptj?.JKW?.Kod.Substring(0, 1) ?? "1" + ptj?.Kod ?? "01";

            string prefix = "RR/" + initial + year;

            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTerima
                       .IgnoreQueryFilters()
                       .Where(x => x.Tahun == year && x.JKWId == data)
                       .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(10));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            return noRujukan;
        }


        // GET: AkTerima/Create
        [Authorize(Policy = "PR001C")]
        public IActionResult CreateByJenis(string jenis)
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();
            var data = 1;

            ViewBag.NoRujukan = GetNoRujukan(data, year);
            // get latest no rujukan running number end
            ViewBag.Jenis = jenis;

            PopulateList();
            CartEmpty();
            return View(jenis);
        }

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

        // on change NoKP controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoKP(string data)
        {
            try
            {
                var result = await _context.AkTerima
                    .Where(x => x.NoKp == data)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    result = new AkTerima
                    {
                        Nama = "",
                        Alamat1 = "",
                        Alamat2 = "",
                        Alamat3 = "",
                        Poskod = "",
                        Bandar = "",
                        Tel = "",
                        Emel = ""
                    };
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change NoKP controller end

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

        public JsonResult GetCaraBayar(JCaraBayar jCaraBayar)
        {
            try
            {
                var result = _context.JCaraBayar.Where(b => b.Id == jCaraBayar.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        // on change CaraBayar controller
        [HttpPost]
        public async Task<JsonResult> JsonGetCaraBayar(int data)
        {
            try
            {
                var result = await _context.JCaraBayar.FindAsync(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change CaraBayar controller end

        // on change pendahuluan
        [HttpPost]
        public async Task<JsonResult> JsonGetPendahuluan(int data, int AkTerimaId)
        {
            try
            {
                CartEmpty();
                var result = await _spPPRepo.GetById(data);

                if (result != null && result.FlPosting == 1)
                {
                    _cart.AddItem1(AkTerimaId,
                                result.JumLulus,
                                (int)result.AkCartaId!);
                }


                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change pendahuluan end

        // get an item from cart akTerima1
        public JsonResult GetAnItemCartAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                AkTerima1 data = _cart.Lines1.Where(x => x.AkCartaId == akTerima1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akTerima1 end

        //save cart akTerima1
        public JsonResult SaveCartAkTerima1(AkTerima1 akTerima1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akTerima1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akTerima1.AkCartaId);

                    _cart.AddItem1(akTerima1.AkTerimaId,
                                    akTerima1.Amaun,
                                    akTerima1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akTerima1 end

        // get all item from cart akTerima1
        public JsonResult GetAllItemCartAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                List<AkTerima1> data = _cart.Lines1.ToList();

                foreach (AkTerima1 item in data)
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
        // get all item from cart akTerima1 end

        // get an item from cart akTerima2
        public JsonResult GetAnItemCartAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                AkTerima2 data = _cart.Lines2.Where(x => x.JCaraBayarId == akTerima2.JCaraBayarId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akTerima2 end

        //save cart akTerima2
        public JsonResult SaveCartAkTerima2(AkTerima2 akTerima2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.JCaraBayarId == akTerima2.JCaraBayarId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akTerima2.JCaraBayarId);


                    _cart.AddItem2(akTerima2.AkTerimaId,
                                   akTerima2.JCaraBayarId,
                                   akTerima2.Amaun,
                                   akTerima2.NoCek ?? "",
                                   akTerima2.JenisCek,
                                   akTerima2.KodBankCek ?? "",
                                   akTerima2.TempatCek?.ToUpper() ?? "",
                                   akTerima2.NoSlip ?? "",
                                   akTerima2.TarSlip,
                                   akTerima2.AkPenyataPemungutId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akTerima2 end

        // get all item from cart akTerima2
        public JsonResult GetAllItemCartAkTerima2()
        {

            try
            {
                List<AkTerima2> data = _cart.Lines2.ToList();

                foreach (AkTerima2 item in data)
                {
                    var jCaraBayar = _context.JCaraBayar.Find(item.JCaraBayarId);

                    item.JCaraBayar = jCaraBayar;

                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akTerima2 end

        // get an item from cart akTerima3
        public JsonResult GetAnItemCartAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                AkTerima3 data = _cart.Lines3.Where(x => x.AkInvoisId == akTerima3.AkInvoisId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akTerima3 end

        //save cart akTerima3
        public JsonResult SaveCartAkTerima3(AkTerima3 akTerima3)
        {

            try
            {

                var akT3 = _cart.Lines3.Where(x => x.AkInvoisId == akTerima3.AkInvoisId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT3 != null && akT3.AkInvoisId != null)
                {
                    _cart.RemoveItem3((int)akT3.AkInvoisId);

                    _cart.AddItem3(akTerima3.AkTerimaId,
                                    akTerima3.AkInvoisId,
                                    akTerima3.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akTerima3 end

        // get all item from cart akTerima3
        public JsonResult GetAllItemCartAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                List<AkTerima3> data = _cart.Lines3.ToList();

                foreach (AkTerima3 item in data)
                {
                    var akInvois = _context.AkInvois.Find(item.AkInvoisId);

                    item.AkInvois = akInvois;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akTerima3 end

        // POST: AkTerima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "PR001C")]
        public async Task<IActionResult> CreateByJenis(
            AkTerima akTerima,
            string syscode,
            decimal JumlahUrusniaga,
            string jenis,
            int FlKategoriPenerima = 0)
        {

            // note:
            // FlJenisTerima = 0 ( Am )
            // FlJenisTerima = 1 ( Inbois )
            // FlJenisTerima = 2 ( Gaji )
            // FlJenisTerima = 3 ( Pendahuluan )
            // FlJenisTerima = 4 ( Panjar )
            // ..
            // FlKategoriPenerima = 0 ( Am / Lain - lain )
            // FlKategoriPenerima = 1 ( pembekal )
            // FlKategoriPenerima = 2 ( pekerja )
            // ..

            AkTerima m = new AkTerima();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
            var spPendahuluan = _context.SpPendahuluanPelbagai.Find(akTerima.SpPendahuluanPelbagaiId);

            if (akTerima.FlJenisTerima == JenisBaucer.Inbois)
            {
                akTerima.FlKategoriPembayar = KategoriPembayar.Penghutang;
            }

            if (spPendahuluan != null)
            {
                var pekerja = _context.SuPekerja.Find(spPendahuluan.SuPekerjaId);
                akTerima.FlKategoriPembayar = KategoriPembayar.Pekerja;
            }


            // checking for jumlah objek & jumlah perihal
            if (akTerima.Jumlah != JumlahUrusniaga)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
                //PopulateCart();
                CartEmpty();
                PopulateList();
                PopulateTableFromCart();
                return View(jenis, akTerima);
            }

            // get latest no rujukan running number  

            string noRujukan = GetNoRujukan(akTerima.JKWId, akTerima.Tahun);

            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {


                m.JKWId = akTerima.JKWId;
                m.JBahagianId = akTerima.JBahagianId;
                m.JNegeriId = akTerima.JNegeriId;
                m.AkBankId = akTerima.AkBankId;
                m.Tahun = akTerima.Tahun;
                m.NoRujukan = noRujukan;
                m.Tarikh = akTerima.Tarikh;
                m.Jumlah = akTerima.Jumlah;
                m.FlCetak = 0;
                m.FlPosting = 0;
                m.KodPembayar = akTerima.KodPembayar;
                m.NoKp = akTerima.NoKp;
                m.Nama = akTerima.Nama;
                m.Alamat1 = akTerima.Alamat1;
                m.Alamat2 = akTerima.Alamat2;
                m.Alamat3 = akTerima.Alamat3;
                m.Poskod = akTerima.Poskod;
                m.Bandar = akTerima.Bandar;
                m.Tel = akTerima.Tel;
                m.Emel = akTerima.Emel;
                m.Sebab = akTerima.Sebab;

                m.FlKategoriPembayar = akTerima.FlKategoriPembayar;

                if (akTerima.FlKategoriPembayar == KategoriPembayar.Penghutang)
                {
                    m.AkPenghutangId = akTerima.AkPenghutangId;
                }

                m.FlJenisTerima = akTerima.FlJenisTerima;
                if (spPendahuluan != null)
                {
                    m.SpPendahuluanPelbagaiId = akTerima.SpPendahuluanPelbagaiId;
                }

                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;
                //m.TarKemaskini = akTerima.TarKemaskini;

                m.AkTerima1 = _cart.Lines1.ToArray();
                m.AkTerima2 = _cart.Lines2.ToArray();
                m.AkTerima3 = _cart.Lines3.ToArray();

                await _akTerimaRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoRujukan, m.NoRujukan, 0, m.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }

            TempData[SD.Error] = "Data gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            return View(jenis, akTerima);
        }

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkTerima1> akT1 = new List<AkTerima1>();
            var cart1 = _cart.Lines1.ToList();

            if (cart1 != null && cart1.Count() > 0)
            {
                foreach (var item in cart1)
                {
                    akT1.Add(item);
                }
            }
            ViewBag.akTerima1 = akT1;
            // table 1 end

            // table 2
            List<AkTerima2> akT2 = new List<AkTerima2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    akT2.Add(item);
                }
            }
            ViewBag.akTerima2 = akT2;
            // table 2 end

            // table 2
            List<AkTerima3> akT3 = new List<AkTerima3>();
            var cart3 = _cart.Lines3.ToList();

            if (cart3 != null && cart3.Count() > 0)
            {
                foreach (var item in cart3)
                {
                    akT3.Add(item);
                }
            }
            ViewBag.akTerima3 = akT3;
            // table 2 end
        }
        // populate table from cart end
        // GET: AkTerima/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkTerima == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetById((int)id);

            // check if already posting redirect back
            if (akTerima.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            if (akTerima == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akTerima);
            return View(akTerima);
        }

        // POST: AkTerima/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "PR001E")]
        public async Task<IActionResult> Edit(
            int id,
            AkTerima akTerima,
            decimal JumlahUrusniaga,
            string syscode)
        {
            if (id != akTerima.Id)
            {
                return NotFound();
            }

            if (akTerima.Jumlah == JumlahUrusniaga)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                        AkTerima dataAsal = await _akTerimaRepo.GetById(id);

                        // list of input that cannot be change
                        akTerima.Tahun = dataAsal.Tahun;
                        akTerima.JKWId = dataAsal.JKWId;
                        akTerima.FlJenisTerima = dataAsal.FlJenisTerima;
                        akTerima.FlKategoriPembayar = dataAsal.FlKategoriPembayar;
                        akTerima.AkPenghutangId = dataAsal.AkPenghutangId;
                        //akTerima.JBahagianId = dataAsal.JBahagianId;
                        akTerima.NoRujukan = dataAsal.NoRujukan;
                        akTerima.Nama = dataAsal.Nama;
                        akTerima.TarMasuk = dataAsal.TarMasuk;
                        akTerima.UserId = dataAsal.UserId;
                        akTerima.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                        akTerima.FlCetak = 0;
                        // list of input that cannot be change end

                        if (dataAsal.AkTerima1 != null)
                        {
                            foreach (AkTerima1 item in dataAsal.AkTerima1)
                            {
                                var model = _context.AkTerima1.FirstOrDefault(b => b.Id == item.Id);
                                if (model != null)
                                {
                                    _context.Remove(model);
                                }
                            }
                        }
                        
                        if (dataAsal.AkTerima2 != null)
                        {
                            foreach (AkTerima2 item in dataAsal.AkTerima2)
                            {
                                var model = _context.AkTerima2.FirstOrDefault(b => b.Id == item.Id);
                                if (model != null)
                                {
                                    _context.Remove(model);
                                }
                            }
                        }
                        
                        if (dataAsal.AkTerima3 != null)
                        {
                            foreach (AkTerima3 item in dataAsal.AkTerima3)
                            {
                                var model = _context.AkTerima3.FirstOrDefault(b => b.Id == item.Id);
                                if (model != null)
                                {
                                    _context.Remove(model);
                                }
                            }
                        }
                        

                        decimal jumlahAsal = dataAsal.Jumlah;
                        _context.Entry(dataAsal).State = EntityState.Detached;

                        akTerima.AkTerima1 = _cart.Lines1.ToList();
                        akTerima.AkTerima2 = _cart.Lines2.ToList();
                        akTerima.AkTerima3 = _cart.Lines3.ToList();

                        akTerima.UserIdKemaskini = user?.UserName ?? "";
                        akTerima.TarKemaskini = DateTime.Now;
                        akTerima.SuPekerjaKemaskiniId = pekerjaId;

                        _context.Update(akTerima);

                        //insert applog
                        if (jumlahAsal != akTerima.Jumlah)
                        {
                            _appLog.Insert("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                                Convert.ToDecimal(akTerima.Jumlah).ToString("#,##0.00"), akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId, modul, syscode, namamodul,user);

                        }
                        else
                        {
                            _appLog.Insert("Ubah", "Ubah Data", akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                        }
                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkTerimaExists(akTerima.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    CartEmpty();
                    // checking for jumlah objek & jumlah perihal
                    if (akTerima.Jumlah != JumlahUrusniaga)
                    {
                        TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Urusniaga";
                    }
                    else
                    {
                        TempData[SD.Success] = "Data berjaya diubah..!";
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Urusniaga";
            PopulateList();
            PopulateTable(id);
            PopulateTableFromCart();
            return View(akTerima);
        }
        // GET: AkTerima/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkTerima == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetById((int)id);

            if (akTerima == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akTerima);
            return View(akTerima);
        }

        // POST: AkTerima/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string sebabHapus, string syscode)
        {
            if (_context.AkTerima == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkTerima'  is null.");
            }
            var akTerima = await _context.AkTerima.FindAsync(id);
            if (akTerima != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                akTerima.UserIdKemaskini = user?.UserName ?? "";
                akTerima.TarKemaskini = DateTime.Now;
                akTerima.SuPekerjaKemaskiniId = pekerjaId;

                akTerima.SebabHapus = sebabHapus?.ToUpper() ?? "";
                // check if already posting redirect back
                if (akTerima.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }
                akTerima.FlCetak = 0;
                _context.AkTerima.Update(akTerima);

                _context.AkTerima.Remove(akTerima);

                //insert applog
                _appLog.Insert("Hapus", "Hapus Data : " + sebabHapus ?? "", akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "PR001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _akTerimaRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Rollback operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkTerima.Update(obj);

            // Rollback operation end

            //insert applog
            _appLog.Insert("Rollback", "Rollback Data", obj.NoRujukan, id, obj.Jumlah, pekerjaId,modul, syscode, namamodul, user);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        // posting function
        [Authorize(Policy = "PR001T")]
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

                AkTerima akTerima = await _akTerimaRepo.GetById((int)id);

                //check if data print status is printed or not
                if (akTerima.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila Cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                // check if data print status is printed or not end
                
                //checking if jumlah objek equal to jumlah perihal 
                decimal jumlahPerihal = 0;
                if (akTerima.AkTerima2 != null) 
                {
                    foreach (AkTerima2 item in akTerima.AkTerima2)
                    {
                        jumlahPerihal += item.Amaun;
                    }
                }

                if (akTerima.Jumlah != jumlahPerihal)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Jumlah Objek tidak sama dengan Jumlah Perihal.";
                    return RedirectToAction(nameof(Index));
                }
                // checking end

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == akTerima.NoRujukan).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

                }
                else
                {
                    //posting operation start here
                    //insert into akAkaun

                    if (akTerima.AkTerima1 != null && akTerima.AkBank != null)
                    {
                        foreach (AkTerima1 item in akTerima.AkTerima1)
                        {
                            AkAkaun akAKodBank = new AkAkaun()
                            {
                                NoRujukan = akTerima.NoRujukan,
                                JKWId = akTerima.JKWId,
                                JBahagianId = akTerima.JBahagianId,
                                AkCartaId1 = akTerima.AkBank.AkCartaId,
                                AkCartaId2 = item.AkCartaId,
                                Tarikh = akTerima.Tarikh,
                                Tahun = akTerima.Tahun,
                                Debit = item.Amaun,
                                AkPenghutangId = akTerima.AkPenghutangId,
                                JSukanId = akTerima.SpPendahuluanPelbagai?.JSukanId
                            };
                            await _akAkaunRepo.Insert(akAKodBank);

                            AkAkaun akAObjek = new AkAkaun()
                            {
                                NoRujukan = akTerima.NoRujukan,
                                JKWId = akTerima.JKWId,
                                JBahagianId = akTerima.JBahagianId,
                                AkCartaId1 = item.AkCartaId,
                                AkCartaId2 = akTerima.AkBank.AkCartaId,
                                Tarikh = akTerima.Tarikh,
                                Tahun = akTerima.Tahun,
                                Kredit = item.Amaun,
                                AkPenghutangId = akTerima.AkPenghutangId,
                                JSukanId = akTerima.SpPendahuluanPelbagai?.JSukanId
                            };

                            await _akAkaunRepo.Insert(akAObjek);
                        }
                    }


                    //update posting status in akTerima
                    akTerima.FlPosting = 1;
                    akTerima.TarikhPosting = DateTime.Now;
                    await _akTerimaRepo.Update(akTerima);

                    //insert applog
                    _appLog.Insert("Posting", "Posting Data", akTerima.NoRujukan, (int)id, akTerima.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "PR001UT")]
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

                AkTerima akTerima = await _akTerimaRepo.GetById((int)id);

                // if already exist in Penyata Pemungut, declare error
                if (akTerima.AkTerima2 != null)
                {
                    foreach (var akTerima2 in akTerima.AkTerima2)
                    {
                        if (!string.IsNullOrEmpty(akTerima2.NoSlip) || !string.IsNullOrEmpty(akTerima2.TarSlip?.ToString("dd/MM/yyyy")))
                        {
                            bool IsExistPenyataPemungut = await _context.AkPenyataPemungut.AnyAsync(b => b.NoDokumen == akTerima2.NoSlip);
                            if (IsExistPenyataPemungut == true)
                            {
                                TempData[SD.Error] = "Data terlibat dengan Penyata Pemungut " + akTerima2.NoSlip + ". Batal Posting tidak dibenarkan.";
                                return RedirectToAction(nameof(Index));
                            }

                        };
                    }
                }
                
                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akTerima.NoRujukan).ToList();
                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    //unposting operation start here
                    //delete data from akAkaun
                    foreach (AkAkaun item in akAkaun)
                    {
                        await _akAkaunRepo.Delete(item.Id);
                    }

                    //update posting status in akTerima
                    akTerima.FlPosting = 0;
                    akTerima.TarikhPosting = null;
                    //akTerima.TarikhPosting = null;
                    await _akTerimaRepo.Update(akTerima);

                    //insert applog
                    _appLog.Insert("UnPosting", "UnPosting Data", akTerima.NoRujukan, (int)id, akTerima.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // printing resit rasmi by akTerima.Id
        [Authorize(Policy = "PR001P")]
        public async Task<IActionResult> PrintPdf(int id, string syscode)
        {
            AkTerima akTerima = await _akTerimaRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akTerima.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(0 - akTerima.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akTerima.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
            string penyedia = "SuperAdmin";

            if (akTerima.SuPekerjaMasukId != null)
            {
                penyedia = _context.SuPekerja.FirstOrDefault(b => b.Id == akTerima.SuPekerjaMasukId)?.Nama ?? "";

            }

            TerimaanPrintModel data = new TerimaanPrintModel();

            CompanyDetails company = await _userService.GetCompanyDetails();
            data.CompanyDetail = company;
            data.AkTerima = akTerima;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.penyedia = penyedia;
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user!.Email);

            data.username = namaUser?.Nama ?? "";

            //update cetak -> 1
            akTerima.FlCetak = 1;
            await _akTerimaRepo.Update(akTerima);

            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId,modul,syscode,namamodul,user);
            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("TerimaanPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };

        }
        // printing resit rasmi end

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetPenghutang(int data)
        {
            try
            {
                var result = await _akPenghutangRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetInboisDikeluarkan(int data)
        {
            try
            {
                var result = await _context.AkInvois.Include(b => b.AkPenghutang).Where(x => x.AkPenghutangId == data).ToListAsync();

                if (result.Count() == 0)
                {
                    return Json(new { result = "Error" });
                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // on change inbois controller
        [HttpPost]
        public async Task<JsonResult> JsonGetAkInvois(int data)
        {
            try
            {
                //_cart.Clear3();
                var result = await _akInvoisRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        //on change inbois controller end

        // json empty Cart controller
        [HttpPost]
        public JsonResult JsonEmptyCart()
        {
            try
            {
                CartEmpty();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        // json empty cart end

        //function json Create akTerima3
        public JsonResult GetAkInvois(AkInvois akInvois)
        {
            try
            {

                var result = _context.AkInvois
                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                    .Where(b => b.Id == akInvois.Id)
                    .FirstOrDefault();

                //if (result!= null)
                //{
                //    PopulateCartAkPV1(result.Id);
                //}
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        private bool AkTerimaExists(int id)
        {
            return (_context.AkTerima?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akTerima1 = new List<int>();
                ViewBag.akTerima2 = new List<int>();
                ViewBag.akTerima3 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();
                _cart.Clear3();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null)
                {
                    _cart.AddItem1(akTerima1.AkTerimaId,
                                   akTerima1.Amaun,
                                   akTerima1.AkCartaId);
                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                bool isCek = false;

                if (akTerima2 != null)
                {
                    var caraBayar = _context.JCaraBayar.FirstOrDefault(b => b.Id == akTerima2.JCaraBayarId);

                    if (caraBayar != null)
                    {
                        isCek = caraBayar.Perihal.Contains("CEK");

                        if (isCek == true)
                        {
                            if (akTerima2.JenisCek == 0)
                            {
                                return Json(new { result = "ERRORCEK", message = "Sila pilih jenis cek." });
                            }

                        }
                        var user = await _userManager.GetUserAsync(User);


                        _cart.AddItem2(akTerima2.AkTerimaId,
                                       akTerima2.JCaraBayarId,
                                       akTerima2.Amaun,
                                       akTerima2.NoCek ?? "",
                                       akTerima2.JenisCek,
                                       akTerima2.KodBankCek ?? "",
                                       akTerima2.TempatCek?.ToUpper() ?? "",
                                       akTerima2.NoSlip ?? "",
                                       akTerima2.TarSlip,
                                       null);
                    }
                    
                }

                return Json(new { result = "OK", isCek = isCek });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                if (akTerima3 != null)
                {

                    // add akTerima3 into cart lines3
                    _cart.AddItem3(akTerima3.AkTerimaId,
                                   akTerima3.AkInvoisId,
                                   akTerima3.Amaun);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null)
                {

                    _cart.RemoveItem1(akTerima1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                if (akTerima2 != null)
                {

                    _cart.RemoveItem2(akTerima2.JCaraBayarId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                if (akTerima3 != null && akTerima3.AkInvoisId != null)
                {
                    _cart.RemoveItem3((int)akTerima3.AkInvoisId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

    }
}
