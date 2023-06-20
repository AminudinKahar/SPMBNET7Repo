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
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels;
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces.Common;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    public class AkBelianController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "TG002";
        public const string namamodul = "Invois Pembekal";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBelian, int, string> _akBelianRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly IRepository<AkInden, int, string> _akIndenRepo;
        private readonly ListViewIRepository<AkBelian1, int> _akBelian1Repo;
        private readonly ListViewIRepository<AkBelian2, int> _akBelian2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly CustomIRepository<string, int> _customRepo;
        private CartBelian _cart;

        public AkBelianController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBelian, int, string> akBelian,
            IRepository<AkPembekal, int, string> akPembekal,
            IRepository<JKW, int, string> kwRepo,
            IRepository<AkPO, int, string> akPORepo,
            IRepository<AkInden, int, string> akIndenRepo,
            ListViewIRepository<AkBelian1, int> akBelian1Repository,
            ListViewIRepository<AkBelian2, int> akBelian2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AkPV, int, string> akPVRepository,
            CustomIRepository<string, int> customRepo,
            CartBelian cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akBelianRepo = akBelian;
            _akPembekalRepo = akPembekal;
            _kwRepo = kwRepo;
            _akPORepo = akPORepo;
            _akIndenRepo = akIndenRepo;
            _akBelian1Repo = akBelian1Repository;
            _akBelian2Repo = akBelian2Repository;
            _akCartaRepo = akCartaRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _akAkaunRepo = akAkaunRepository;
            _akPVRepo = akPVRepository;
            _customRepo = customRepo;
            _cart = cart;
        }


        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetKod(int data, string noInbois)
        {
            try
            {
                var result = await _context.AkBelian.FirstOrDefaultAsync(x => x.NoInbois == "IN/"+ data +"/"+noInbois);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // GET: AkBelian
        [Authorize(Policy = "TG002")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Invois", Value = "NoRujukan" },
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

            var akBelian = await _akBelianRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akBelian = await _akBelianRepo.GetAllIncludeDeletedItems();
            }

            //var akBelian = await _context.AkBelian.ToListAsync();

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akBelian = akBelian.Where(s => s.NoInbois.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akBelian = akBelian.Where(s => s.AkPembekal!.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akBelian = akBelian.Where(x => x.Tarikh >= date1
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

            List<AkBelianViewModel> viewModel = new List<AkBelianViewModel>();

            foreach (AkBelian item in akBelian)
            {
                var namaSykt = "";
                var alamat1 = "";

                if (item.AkPOId == null)
                {
                    namaSykt = item.AkPembekal?.NamaSykt ?? "";
                    alamat1 = item.AkPembekal?.Alamat1 ?? "";
                }
                else
                {
                    namaSykt = item.AkPO?.AkPembekal?.NamaSykt ?? "";
                    alamat1 = item.AkPO?.AkPembekal?.Alamat1 ?? "";
                }

                decimal jumlahPerihal = 0;
                if (item.AkBelian2 != null)
                {
                    foreach (AkBelian2 item2 in item.AkBelian2)
                    {
                        jumlahPerihal += item2.Amaun;
                    }
                }

                viewModel.Add(new AkBelianViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoInbois = item.NoInbois,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    NamaSykt = namaSykt,
                    Alamat1 = alamat1,
                    FlHapus = item.FlHapus,
                    FlPosting = item.FlPosting,
                    JumlahPerihal = jumlahPerihal
                }
                );
            }

            return View(viewModel);
        }

        // GET: AkBelian/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkBelian == null)
            {
                return NotFound();
            }

            // admin access
            var akBelian = await _akBelianRepo.GetByIdIncludeDeletedItems((int)id);

            if (akBelian == null)
            {
                return NotFound();
            }

            var kodObjekAkaunPemiutang = await _akCartaRepo.GetByIdIncludeDeletedItems(akBelian.KodObjekAPId);

            var akPO = new AkPO();
            if (akBelian.AkPOId != null)
            {
                akPO = await _akPORepo.GetByIdIncludeDeletedItems((int)akBelian.AkPOId);
            }
            else
            {
                akPO = new AkPO()
                {
                    NoPO = "-"
                };
            }

            var akInden = new AkInden();
            if (akBelian.AkIndenId != null)
            {
                akInden = await _akIndenRepo.GetByIdIncludeDeletedItems((int)akBelian.AkIndenId);
            }
            else
            {
                akInden = new AkInden()
                {
                    NoInden = "-"
                };
            }

            // check if linked with Nota Debit/ Kredit, show list of nota debit/kredit
            var akNota = _context.AkNotaDebitKreditBelian.Where(b => b.AkBelianId == id).ToList();

            // check if already paid, show list of PVs in AkBelian
            var akPV2 = _context.AkPV2.Include(b => b.AkPV).Where(b => b.AkBelianId == id).ToList();

            var pembekal = await _akPembekalRepo.GetByIdIncludeDeletedItems(akBelian.AkPembekalId);


            // admin access end

            // normal user access
            if (User.IsInRole("User"))
            {
                kodObjekAkaunPemiutang = await _akCartaRepo.GetById(akBelian.KodObjekAPId);

                if (akBelian.AkPOId != null)
                {
                    akPO = await _akPORepo.GetById((int)akBelian.AkPOId);
                }
                else
                {
                    akPO = new AkPO()
                    {
                        NoPO = "-"
                    };
                }

                if (akBelian.AkIndenId != null)
                {
                    akInden = await _akIndenRepo.GetById((int)akBelian.AkIndenId);
                }
                else
                {
                    akInden = new AkInden()
                    {
                        NoInden = "-"
                    };
                }

                pembekal = await _akPembekalRepo.GetById(akBelian.AkPembekalId);

            }
            //normal user access end

            AkBelianViewModel akBelianView = new AkBelianViewModel();

            //fill in view model AkPVViewModel from akPV
            akBelianView.AkPembekalId = akBelian.AkPembekalId;
            akBelianView.AkPO = akPO;
            akBelianView.AkInden = akInden;
            if (akPV2 != null)
            {
                foreach (var i in akPV2)
                {
                    akBelianView.JumlahPV += i.Amaun;
                }

                akBelianView.AkPV2 = akPV2;
            }

            if (akNota != null)
            {
                foreach (var i in akNota)
                {
                    if (i.FlJenis == 0)
                    {
                        akBelianView.JumlahNota += i.Jumlah;
                    }
                    else
                    {
                        akBelianView.JumlahNota -= i.Jumlah;
                    }
                }
                akBelianView.AkNotaDebitKreditBelian = akNota;
            }

            akBelianView.AkPembekal = pembekal;
            akBelianView.Id = akBelian.Id;
            akBelianView.Tahun = akBelian.Tahun;
            akBelianView.NoInbois = akBelian.NoInbois;
            akBelianView.Tarikh = akBelian.Tarikh;
            akBelianView.TarikhTerima = akBelian.TarikhTerima;
            akBelianView.TarikhKewanganTerima = akBelian.TarikhKewanganTerima;
            akBelianView.JKWId = akBelian.JKWId;
            akBelianView.JKW = akBelian.JKW;
            akBelianView.JBahagianId = akBelian.JBahagianId;
            akBelianView.JBahagian = akBelian.JBahagian;
            akBelianView.KodObjekAP = kodObjekAkaunPemiutang;
            akBelianView.Jumlah = akBelian.Jumlah;
            akBelianView.TarikhPosting = akBelian.TarikhPosting;
            akBelianView.FlPosting = akBelian.FlPosting;
            akBelianView.FlHapus = akBelian.FlHapus;

            if (akBelian.AkBelian2 != null)
            {
                foreach (AkBelian2 item in akBelian.AkBelian2)
                {
                    akBelianView.JumlahPerihal += item.Amaun;
                }
            }

            akBelianView.AkBelian1 = akBelian.AkBelian1;
            akBelianView.AkBelian2 = akBelian.AkBelian2;

            PopulateTable(id);
            return View(akBelianView);
        }
        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkPO> akPOList = _context.AkPO
                .Include(b => b.AkPembekal).ThenInclude(b => b!.JBank)
                .Include(b => b.JKW)
                .Include(b => b.AkPO1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPO2)
                .Where(b => b.FlPosting == 1)
                .OrderBy(b => b.Tarikh).ToList();
            ViewBag.AkPO = akPOList;

            List<AkInden> akIndenList = _context.AkInden
                .Include(b => b.AkPembekal).ThenInclude(b => b!.JBank)
                .Include(b => b.JKW)
                .Include(b => b.AkInden1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkInden2)
                .Where(b => b.FlPosting == 1)
                .OrderBy(b => b.Tarikh).ToList();
            ViewBag.AkInden = akIndenList;

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

            List<AkCarta> KodObjekAPList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4" && (b.Kod.Substring(0, 1) == "L"))
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.KodObjekAP = KodObjekAPList;

        }

        private void PopulateTable(int? id)
        {
            List<AkBelian1> akBelian1Table = _context.AkBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akBelian1 = akBelian1Table;

            List<AkBelian2> akBelian2Table = _context.AkBelian2
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Bil)
                .ToList();
            ViewBag.akBelian2 = akBelian2Table;
        }
        private void PopulateCart()
        {
            List<AkBelian1> lines1 = _cart.Lines1.ToList();

            foreach (AkBelian1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            List<AkBelian2> lines2 = _cart.Lines2.ToList();

            ViewBag.akBelian1 = lines1;
            ViewBag.akBelian2 = lines2;
        }

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkBelian1> tbl1 = new List<AkBelian1>();
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
            ViewBag.akBelian1 = tbl1;
            // table 1 end

            // table 2
            List<AkBelian2> tbl2 = new List<AkBelian2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    tbl2.Add(item);
                }
            }
            ViewBag.akBelian2 = tbl2;
            // table 2 end
        }
        // populate table from cart end
        // GET: AkBelian/Create
        [Authorize(Policy = "TG002C")]
        public IActionResult Create()
        {
            PopulateList();
            CartEmpty();
            return View();
        }

        private void PopulateCartFromDb(AkBelian akBelian)
        {
            List<AkBelian1> akBelian1Table = _context.AkBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkBelianId == akBelian.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkBelian1 item in akBelian1Table)
            {
                _cart.AddItem1(item.AkBelianId,
                               item.Amaun,
                               item.AkCartaId);
            }

            List<AkBelian2> akBelian2Table = _context.AkBelian2
                .Where(b => b.AkBelianId == akBelian.Id)
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkBelian2 item in akBelian2Table)
            {
                _cart.AddItem2(item.AkBelianId,
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
        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akBelian1 = new List<int>();
                ViewBag.akBelian2 = new List<int>();
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

        // on change no PO controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoPO(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkPO(id);
                var result = await _akPORepo.GetById(id);

                var akPOLaras = _context.AkPOLaras
                    .Include(x => x.AkPOLaras1)
                    .Where(x => x.AkPOId == id && x.FlPosting == 1).FirstOrDefault();

                List<AkPO1> akPO1Table = await _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                if (result.AkPO1 != null)
                {
                    foreach (AkPO1 item in akPO1Table)
                    {
                        if (item.Amaun != 0)
                        {
                            result.AkPO1.Add(item);
                        }
                    }
                }


                List<AkPO2> akPO2Table = await _context.AkPO2
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Bil)
                .ToListAsync();

                if (result.AkPO2 != null)
                {
                    foreach (AkPO2 item in akPO2Table)
                    {
                        if (akPOLaras != null)
                        {
                            item.Amaun = 0;
                            item.Harga = 0;
                            item.Kuantiti = 0;
                        }

                        result.AkPO2.Add(item);
                    }

                    result.AkPO2 = result.AkPO2.OrderBy(b => b.Bil).ToList();
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


            decimal Amaun = 0;
            decimal Kuantiti = 0;
            decimal Harga = 0;

            AkPOLaras akPOLaras = _context.AkPOLaras
                    .Include(x => x.AkPOLaras1)
                    .Where(x => x.AkPOId == id && x.FlPosting == 1).FirstOrDefault();

            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO1 item in akPO1Table)
            {

                item.AkPOId = 0;

                //if there is pelarasan PO
                if (akPOLaras != null && akPOLaras.AkPOLaras1 != null)
                {
                    foreach (var laras in akPOLaras.AkPOLaras1)
                    {
                        if (laras.AkCartaId == item.AkCartaId)
                        {
                            item.Amaun += laras.Amaun;
                        }
                    }
                }

                if (item.Amaun != 0)
                {
                    _cart.AddItem1(item.AkPOId,
                                   item.Amaun,
                                   item.AkCartaId);
                }
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                .AsNoTracking()
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Bil)
                .ToList();

            foreach (AkPO2 item in akPO2Table)
            {
                item.AkPOId = 0;
                if (akPOLaras == null)
                {
                    Amaun = item.Amaun;
                    Kuantiti = item.Kuantiti;
                    Harga = item.Harga;
                }

                _cart.AddItem2(item.AkPOId,
                               item.Indek,
                               item.Bil,
                               item.NoStok ?? "",
                               item.Perihal?.ToUpper() ?? "",
                               Kuantiti,
                               item.Unit ?? "",
                               Harga,
                               Amaun);
            }


        }
        //on change no PO controller end

        // on change no Inden controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoInden(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkInden(id);
                var result = await _akIndenRepo.GetById(id);

                List<AkInden1> akInden1Table = await _context.AkInden1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                if (result.AkInden1 != null)
                {
                    foreach (AkInden1 item in akInden1Table)
                    {
                        if (item.Amaun != 0)
                        {
                            result.AkInden1.Add(item);
                        }
                    }
                }

                List<AkInden2> akInden2Table = await _context.AkInden2
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Bil)
                .ToListAsync();

                if (result.AkInden2 != null)
                {
                    foreach (AkInden2 item in akInden2Table)
                    {
                        result.AkInden2.Add(item);
                    }

                    result.AkInden2 = result.AkInden2.OrderBy(b => b.Bil).ToList();
                }


                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCartFromAkInden(int id)
        {
            var user = _userManager.GetUserName(User);

            List<AkInden1> akInden1Table = _context.AkInden1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkInden1 item in akInden1Table)
            {

                item.AkIndenId = 0;

                if (item.Amaun != 0)
                {
                    _cart.AddItem1(item.AkIndenId,
                                   item.Amaun,
                                   item.AkCartaId);
                }
            }

            List<AkInden2> akInden2Table = _context.AkInden2
                .AsNoTracking()
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Bil)
                .ToList();

            foreach (AkInden2 item in akInden2Table)
            {
                item.AkIndenId = 0;

                _cart.AddItem2(item.AkIndenId,
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

        public async Task<JsonResult> SaveAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                if (akBelian1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akBelian1.AkBelianId,
                                    akBelian1.Amaun,
                                    akBelian1.AkCartaId);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                if (akBelian1 != null)
                {

                    _cart.RemoveItem1(akBelian1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                if (akBelian2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akBelian2.AkBelianId,
                                   akBelian2.Indek,
                                   akBelian2.Bil,
                                   akBelian2.NoStok ?? "",
                                   akBelian2.Perihal?.ToUpper()?? "",
                                   akBelian2.Kuantiti,
                                   akBelian2.Unit ?? "",
                                   akBelian2.Harga,
                                   akBelian2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                if (akBelian2 != null)
                {

                    _cart.RemoveItem2(akBelian2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // function  json Create end

        // get an item from cart akBelian1
        public JsonResult GetAnItemCartAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                AkBelian1 data = _cart.Lines1.Where(x => x.AkCartaId == akBelian1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akBelian1 end

        //save cart akBelian1
        public JsonResult SaveCartAkBelian1(AkBelian1 akBelian1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akBelian1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akBelian1.AkCartaId);

                    _cart.AddItem1(akBelian1.AkBelianId,
                                    akBelian1.Amaun,
                                    akBelian1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akBelian1 end

        // get all item from cart akBelian1
        public JsonResult GetAllItemCartAkBelian1()
        {

            try
            {
                List<AkBelian1> data = _cart.Lines1.ToList();

                foreach (AkBelian1 item in data)
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
        // get all item from cart akBelian1 end

        // get an item from cart akBelian2
        public JsonResult GetAnItemCartAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                AkBelian2 data = _cart.Lines2.Where(x => x.Indek == akBelian2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akBelian2 end

        //save cart akBelian2
        public JsonResult SaveCartAkBelian2(AkBelian2 akBelian2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.Indek == akBelian2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akBelian2.Indek);

                    _cart.AddItem2(akBelian2.AkBelianId,
                                   akBelian2.Indek,
                                   akBelian2.Bil,
                                   akBelian2.NoStok ?? "",
                                   akBelian2.Perihal?.ToUpper()?? "",
                                   akBelian2.Kuantiti,
                                   akBelian2.Unit?.ToUpper()?? "",
                                   akBelian2.Harga,
                                   akBelian2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akBelian2 end

        // get all item from cart akBelian2
        public JsonResult GetAllItemCartAkBelian2()
        {

            try
            {
                List<AkBelian2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akBelian2 end
        // POST: AkBelian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG002C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkBelian akBelian, decimal JumlahPerihal, string syscode)
        {
            AkBelian m = new AkBelian();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            if (user != null)
            {
                if (user.Email == "superadmin@idwal.com.my")
                {
                    akBelian.SuPekerjaMasukId = 1;
                }
                else
                {
                    akBelian.SuPekerjaMasukId = pekerjaId;
                }
            }


            var pembekal = await _akPembekalRepo.GetById(akBelian.AkPembekalId);
            if (pembekal == null)
            {
                TempData[SD.Error] = "Pembekal tidak wujud..!";
                PopulateTableFromCart();
                PopulateList();
                return View(akBelian);
            }

            var noRujukan = "IN/"+ pembekal.KodSykt.ToUpper() + "/" + akBelian.NoInbois.ToUpper();

            var akPo = new AkPO();
            if (akBelian.AkPOId != null)
                akPo = await _akPORepo.GetById((int)akBelian.AkPOId);

            var akInden = new AkInden();
            if (akBelian.AkIndenId != null)
                akInden = await _akIndenRepo.GetById((int)akBelian.AkIndenId);

            // checking for existing no rujukan
            var countNoRujukan = _context.AkBelian.Where(x => x.NoInbois == noRujukan && x.AkPembekalId == akBelian.AkPembekalId).Count();

            if (countNoRujukan > 0)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. No rujukan pendaftaran " + akBelian.NoInbois + " telah wujud";
                PopulateTableFromCart();
                PopulateList();
                return View(akBelian);
            }

            // checking for jumlah objek & jumlah perihal
            if (akBelian.Jumlah != JumlahPerihal)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
                PopulateTableFromCart();
                PopulateList();
                return View(akBelian);
            }

            if (ModelState.IsValid)
            {
                m.KodObjekAPId = akBelian.KodObjekAPId;
                m.JKWId = akBelian.JKWId;
                m.JBahagianId = akBelian.JBahagianId;
                m.Tahun = akBelian.Tahun;
                m.NoInbois = noRujukan;
                m.NoRujukan = noRujukan;
                m.Tarikh = akBelian.Tarikh;
                m.TarikhTerima = akBelian.TarikhTerima;
                m.TarikhKewanganTerima = akBelian.TarikhKewanganTerima;
                m.Jumlah = akBelian.Jumlah;
                m.FlPosting = 0;
                m.FlJenisTanggungan = akBelian.FlJenisTanggungan;

                switch (akBelian.FlJenisTanggungan)
                {
                    //PO (ada tanggungan)
                    case 1:
                        m.FlTanggungan = "1";
                        m.AkPOId = akBelian.AkPOId;
                        m.AkPembekalId = akPo.AkPembekalId;
                        break;
                    //Inden (ada tanggungan)
                    case 2:
                        m.FlTanggungan = "1";
                        m.AkIndenId = akBelian.AkIndenId;
                        m.AkPembekalId = akInden.AkPembekalId;
                        break;
                    // tanpa tanggungan
                    default:
                        m.FlTanggungan = "0";
                        m.AkPembekalId = akBelian.AkPembekalId;
                        break;
                }

                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkBelian1 = _cart.Lines1.ToArray();
                m.AkBelian2 = _cart.Lines2.ToArray();

                await _akBelianRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoInbois, m.NoInbois, 0, m.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + akBelian.NoInbois;
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Data gagal disimpan.";
            PopulateTableFromCart();
            PopulateList();
            return View(akBelian);
        }

        // GET: AkBelian/Edit/5
        [Authorize(Policy = "TG002E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkBelian == null)
            {
                return NotFound();
            }

            var akBelian = await _akBelianRepo.GetById((int)id);

            if (akBelian == null)
            {
                return NotFound();
            }

            var akPO = new AkPO();
            if (akBelian.AkPOId != null)
            {
                akPO = await _akPORepo.GetById((int)akBelian.AkPOId);
            }
            else
            {
                akPO = new AkPO()
                {
                    NoPO = "-"
                };
            }

            akBelian.AkPO = akPO;

            var akInden = new AkInden();
            if (akBelian.AkIndenId != null)
            {
                akInden = await _akIndenRepo.GetByIdIncludeDeletedItems((int)akBelian.AkIndenId);
            }
            else
            {
                akInden = new AkInden()
                {
                    NoInden = "-"
                };
            }

            akBelian.AkInden = akInden;

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akBelian);
            return View(akBelian);
        }

        // get latest Index number in AkBelian2
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

        // POST: AkBelian/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG002E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkBelian akBelian, decimal JumlahPerihal, string syscode)
        {
            if (id != akBelian.Id)
            {
                return NotFound();
            }

            if (akBelian.Jumlah != JumlahPerihal)
            {
                TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
                PopulateList();
                PopulateTableFromCart();
                return View(akBelian);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    AkBelian akBelianAsal = await _akBelianRepo.GetById(id);

                    // list of input that cannot be change
                    akBelian.Tahun = akBelianAsal.Tahun;
                    akBelian.JKWId = akBelianAsal.JKWId;
                    akBelian.JBahagianId = akBelianAsal.JBahagianId;
                    akBelian.NoInbois = akBelianAsal.NoInbois;
                    akBelian.NoRujukan = akBelianAsal.NoRujukan;
                    akBelian.FlJenisTanggungan = akBelianAsal.FlJenisTanggungan;
                    akBelian.FlTanggungan = akBelianAsal.FlTanggungan;

                    if (akBelianAsal.AkPOId != null)
                    {
                        akBelian.AkPOId = akBelianAsal.AkPOId;
                        akBelian.AkPembekalId = akBelianAsal.AkPembekalId;
                    }
                    if (akBelianAsal.AkIndenId != null)
                    {
                        akBelian.AkIndenId = akBelianAsal.AkIndenId;
                        akBelian.AkPembekalId = akBelianAsal.AkPembekalId;
                    }
                    akBelian.TarMasuk = akBelianAsal.TarMasuk;
                    akBelian.UserId = akBelianAsal.UserId;
                    akBelian.SuPekerjaMasukId = akBelianAsal.SuPekerjaMasukId;
                    akBelian.KodObjekAPId = akBelianAsal.KodObjekAPId;
                    decimal jumlahAsal = akBelianAsal.Jumlah;
                    // list of input that cannot be change end

                    if (akBelianAsal.AkBelian1 != null)
                    {
                        foreach (AkBelian1 item in akBelianAsal.AkBelian1)
                        {
                            var model = _context.AkBelian1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }

                    if (akBelianAsal.AkBelian2 != null)
                    {
                        foreach (AkBelian2 item in akBelianAsal.AkBelian2)
                        {
                            var model = _context.AkBelian2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }

                    _context.Entry(akBelianAsal).State = EntityState.Detached;

                    akBelian.AkBelian1 = _cart.Lines1.ToList();
                    akBelian.AkBelian2 = _cart.Lines2.ToList();

                    akBelian.UserIdKemaskini = user?.UserName ?? "";
                    akBelian.TarKemaskini = DateTime.Now;
                    akBelian.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akBelian);

                    //insert applog
                    if (jumlahAsal != akBelian.Jumlah)
                    {
                        _appLog.Insert("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(akBelian.Jumlah).ToString("#,##0.00"), akBelian.NoInbois, id, akBelian.Jumlah, pekerjaId, modul, syscode, namamodul, user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", akBelian.NoInbois, id, akBelian.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                    }

                    //insert applog end

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                    CartEmpty();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkBelianExists(akBelian.Id))
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
            TempData[SD.Warning] = "Data gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            return View(akBelian);
        }

        // GET: AkBelian/Delete/5
        [Authorize(Policy = "TG002D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkBelian == null)
            {
                return NotFound();
            }

            var akBelian = await _akBelianRepo.GetById((int)id);

            var kodObjekAkaunPemiutang = await _akCartaRepo.GetById(akBelian.KodObjekAPId);

            var akPO = new AkPO();
            if (akBelian.AkPOId != null)
            {
                akPO = await _akPORepo.GetById((int)akBelian.AkPOId);
            }
            else
            {
                akPO = new AkPO()
                {
                    NoPO = "-"
                };
            }

            var pembekal = await _akPembekalRepo.GetById(akBelian.AkPembekalId);

            if (akBelian == null)
            {
                return NotFound();
            }

            AkBelianViewModel akBelianView = new AkBelianViewModel();

            //fill in view model AkPVViewModel from akPV
            akBelianView.AkPembekalId = akBelian.AkPembekalId;
            akBelianView.AkPO = akPO;
            akBelianView.AkPembekal = pembekal;
            akBelianView.JBahagian = akBelian.JBahagian;
            akBelianView.Id = akBelian.Id;
            akBelianView.Tahun = akBelian.Tahun;
            akBelianView.NoInbois = akBelian.NoInbois;
            akBelianView.Tarikh = akBelian.Tarikh;
            akBelianView.TarikhTerima = akBelian.TarikhTerima;
            akBelianView.TarikhKewanganTerima = akBelian.TarikhKewanganTerima;
            akBelianView.JKW = akBelian.JKW;
            akBelianView.KodObjekAP = kodObjekAkaunPemiutang;
            akBelianView.Jumlah = akBelian.Jumlah;
            akBelianView.TarikhPosting = akBelian.TarikhPosting;
            akBelianView.FlPosting = akBelian.FlPosting;
            akBelianView.FlHapus = akBelian.FlHapus;

            if (akBelian.AkBelian2 != null)
            {
                foreach (AkBelian2 item in akBelian.AkBelian2)
                {
                    akBelianView.JumlahPerihal += item.Amaun;
                }
            }

            akBelianView.AkBelian1 = akBelian.AkBelian1;
            akBelianView.AkBelian2 = akBelian.AkBelian2;

            PopulateTable(id);
            return View(akBelianView);
        }

        // POST: AkBelian/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "TG002D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkBelian == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkBelian'  is null.");
            }
            var akBelian = await _context.AkBelian.FindAsync(id);
            if (akBelian != null)
            {
                // check if already posting redirect back
                if (akBelian.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }

                // check if already link with AkNotaDebitKreditBelian
                var akNota = await _context.AkNotaDebitKreditBelian.FirstOrDefaultAsync(b => b.AkBelianId == id);

                if (akNota != null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data terkait dengan nota debit/kredit " + akNota.NoRujukan + ".";
                    return RedirectToAction(nameof(Index));
                }

                // check if already link with akPV, Batal akPV included
                var akPV = await _akPVRepo.GetAll();
                var akPV2 = _context.AkPV2.ToList();
                var result = (from tbl2 in akPV2
                              join tbl in akPV
                              on tbl2.AkPVId equals tbl.Id into tbl2Tbl
                              from tbl2_tbl in tbl2Tbl
                              select new
                              {
                                  Id = tbl2.Id,
                                  AkPVId = tbl2.AkPVId,
                                  AkBelianId = tbl2.AkBelianId

                              }).Where(x => x.AkBelianId == id).FirstOrDefault();

                if (result != null)
                {
                    AkPV akPVItem = await _akPVRepo.GetById(result.AkPVId);
                    //duplicate id error
                    TempData[SD.Error] = "Data terkait dengan no baucer " + akPVItem.NoPV + ".";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                akBelian.UserIdKemaskini = user?.UserName ?? "";
                akBelian.TarKemaskini = DateTime.Now;
                akBelian.SuPekerjaKemaskiniId = pekerjaId;

                _context.AkBelian.Remove(akBelian);
                //insert applog
                _appLog.Insert("Hapus", "Hapus Data", akBelian.NoInbois, id, akBelian.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                //insert applog end
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AkBelianExists(int id)
        {
            return (_context.AkBelian?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool CurrentAkBelianExists(int akPembekalId, string noRujukan)
        {
            return _context.AkBelian.Any(e => e.AkPembekalId == akPembekalId && e.NoInbois == noRujukan);
        }

        // posting function
        [Authorize(Policy = "TG002T")]
        public async Task<IActionResult> Posting(int? id,string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                AkBelian akBelian = await _akBelianRepo.GetById((int)id);


                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == akBelian.NoInbois).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";
                    return RedirectToAction(nameof(Index));
                }

                if (akBelian.TarikhTerima == null || akBelian.TarikhKewanganTerima == null)
                {
                    TempData[SD.Error] = "Sila isi tarikh terima / tarikh kewangan terima untuk meneruskan operasi ini.";
                    return RedirectToAction(nameof(Index));
                }

                var kodPembekal = "";
                var penerima = "";

                if (akBelian.AkPembekalId != 0)
                {
                    kodPembekal = akBelian.AkPembekal?.KodSykt ?? "";
                    penerima = akBelian.AkPembekal?.NamaSykt ?? "";
                }

                if (akBelian.AkBelian1 != null)
                {
                    // if belian not include PO and Inden, check peruntukan
                    if (akBelian.AkPOId == null && akBelian.AkIndenId == null)
                    {

                        // check peruntukan
                        foreach (var item in akBelian.AkBelian1)
                        {
                            bool IsExistAbBukuVot = await _context.AbBukuVot
                                       .Where(x => x.Tahun == akBelian.Tahun && x.VotId == item.AkCartaId && x.JKWId == akBelian.JKWId && x.JBahagianId == akBelian.JBahagianId)
                                       .AnyAsync();

                            if (IsExistAbBukuVot == true)
                            {
                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akBelian.Tahun, item.AkCartaId, akBelian.JKWId, akBelian.JBahagianId);

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
                            // check peruntukan end
                        }

                    }

                    //posting operation start here

                    foreach (AkBelian1 item in akBelian.AkBelian1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVot = new AbBukuVot();
                        if (akBelian.AkPO != null || akBelian.AkInden != null)
                        {
                            //dengan tanggungan
                            abBukuVot = new AbBukuVot()
                            {
                                Tahun = akBelian.Tahun,
                                JKWId = akBelian.JKWId,
                                JBahagianId = akBelian.JBahagianId,
                                Tarikh = akBelian.Tarikh,
                                Kod = kodPembekal,
                                Penerima = penerima,
                                VotId = item.AkCartaId,
                                Rujukan = akBelian.NoInbois,
                                //Tanggungan = 0 - item.Amaun,
                                Liabiliti = item.Amaun

                            };
                        }
                        else
                        {
                            //tanpa tanggungan
                            abBukuVot = new AbBukuVot()
                            {
                                Tahun = akBelian.Tahun,
                                JKWId = akBelian.JKWId,
                                JBahagianId = akBelian.JBahagianId,
                                Tarikh = akBelian.Tarikh,
                                Kod = kodPembekal,
                                Penerima = penerima,
                                VotId = item.AkCartaId,
                                Rujukan = akBelian.NoInbois,
                                Liabiliti = item.Amaun
                            };

                        }

                        await _abBukuVotRepo.Insert(abBukuVot);

                        // insert into AbBukuVot end

                        //insert into akAkaun
                        AkAkaun akALiabiliti = new AkAkaun()
                        {
                            NoRujukan = akBelian.NoInbois,
                            JKWId = akBelian.JKWId,
                            JBahagianId = akBelian.JBahagianId,
                            AkCartaId1 = akBelian.KodObjekAPId,
                            AkCartaId2 = item.AkCartaId,
                            Tarikh = akBelian.Tarikh,
                            Kredit = item.Amaun,
                            AkPembekalId = akBelian.AkPembekalId
                        };

                        await _akAkaunRepo.Insert(akALiabiliti);

                        AkAkaun akAObjek = new AkAkaun()
                        {
                            NoRujukan = akBelian.NoInbois,
                            JKWId = akBelian.JKWId,
                            JBahagianId = akBelian.JBahagianId,
                            AkCartaId1 = item.AkCartaId,
                            AkCartaId2 = akBelian.KodObjekAPId,
                            Tarikh = akBelian.Tarikh,
                            Debit = item.Amaun,
                            AkPembekalId = akBelian.AkPembekalId
                        };

                        await _akAkaunRepo.Insert(akAObjek);
                    }

                    //update posting status in akTerima
                    akBelian.FlPosting = 1;
                    akBelian.TarikhPosting = DateTime.Now;
                    await _akBelianRepo.Update(akBelian);

                    //insert applog
                    _appLog.Insert("Posting", "Posting Data", akBelian.NoInbois, (int)id, akBelian.Jumlah, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya diluluskan.";
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

                AkBelian akBelian = await _akBelianRepo.GetById((int)id);

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akBelian.NoInbois).ToList();

                // check if already link with AkNotaDebitKreditBelian
                var akNota = await _context.AkNotaDebitKreditBelian.FirstOrDefaultAsync(b => b.AkBelianId == id);

                if (akNota != null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data terkait dengan nota debit/kredit " + akNota.NoRujukan + ".";
                    return RedirectToAction(nameof(Index));
                }

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == akBelian.NoInbois).ToList();
                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    var akPV = await _akPVRepo.GetAll();
                    var akPV2 = _context.AkPV2.ToList();
                    var result = (from tbl2 in akPV2
                                  join tbl in akPV
                                  on tbl2.AkPVId equals tbl.Id into tbl2Tbl
                                  from tbl2_tbl in tbl2Tbl
                                  select new
                                  {
                                      Id = tbl2.Id,
                                      AkPVId = tbl2.AkPVId,
                                      AkBelianId = tbl2.AkBelianId

                                  }).Where(x => x.AkBelianId == id).FirstOrDefault();

                    if (result != null)
                    {
                        AkPV akPVItem = await _akPVRepo.GetById(result.AkPVId);
                        //duplicate id error
                        TempData[SD.Error] = "Data terkait dengan no baucer " + akPVItem.NoPV + ".";
                    }
                    else
                    {
                        //unposting operation start here
                        //delete data from akAkaun
                        foreach (AkAkaun item in akAkaun)
                        {
                            await _akAkaunRepo.Delete(item.Id);
                        }

                        //delete data from abBukuVot
                        foreach (AbBukuVot item in abBukuVot)
                        {
                            await _abBukuVotRepo.Delete(item.Id);
                        }
                        //delete data from abBukuVot

                        //update posting status in akTerima
                        akBelian.FlPosting = 0;
                        akBelian.TarikhPosting = null;
                        await _akBelianRepo.Update(akBelian);

                        //insert applog
                        _appLog.Insert("UnPosting", "Batal Posting Data", akBelian.NoInbois, (int)id, akBelian.Jumlah, pekerjaId,modul, syscode, namamodul, user);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya batal kelulusan.";
                        //unposting operation end
                    }

                }


            }

            return RedirectToAction(nameof(Index));

        }

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

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "TG002R")]
        public async Task<IActionResult> RollBack(int id,string syscode)
        {
            var obj = await _akBelianRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            if (CurrentAkBelianExists(obj.AkPembekalId, obj.NoInbois) == false)
            {
                // Batal operation

                obj.FlHapus = 0;
                obj.UserIdKemaskini = user?.UserName ?? "";
                obj.TarKemaskini = DateTime.Now;
                obj.SuPekerjaKemaskiniId = pekerjaId;
                _context.AkBelian.Update(obj);

                // Batal operation end

                //insert applog
                _appLog.Insert("Rollback", "Rollback Data", obj.NoInbois, id, obj.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dikembalikan..!";

            }
            else
            {
                TempData[SD.Error] = "No Inbois telah wujud..!";

            }
            return RedirectToAction(nameof(Index));

        }
    }
}
