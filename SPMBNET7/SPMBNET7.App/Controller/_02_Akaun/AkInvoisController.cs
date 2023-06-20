﻿using System;
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
    public class AkInvoisController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "IN001";
        public const string namamodul = "Inbois Dikeluarkan";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkInvois, int, string> _akInvoisRepo;
        private readonly IRepository<AkPenghutang, int, string> _akPenghutangRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly ListViewIRepository<AkInvois1, int> _akInvois1Repo;
        private readonly ListViewIRepository<AkInvois2, int> _akInvois2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly UserServices _userService;
        private CartInvois _cart;

        public AkInvoisController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkInvois, int, string> akInvois,
            IRepository<AkPenghutang, int, string> akPenghutang,
            IRepository<JKW, int, string> kwRepo,
            ListViewIRepository<AkInvois1, int> akInvois1Repository,
            ListViewIRepository<AkInvois2, int> akInvois2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AkTerima, int, string> akTerimaRepository,
            UserServices userService,
            CartInvois cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akInvoisRepo = akInvois;
            _akPenghutangRepo = akPenghutang;
            _kwRepo = kwRepo;
            _akInvois1Repo = akInvois1Repository;
            _akInvois2Repo = akInvois2Repository;
            _akCartaRepo = akCartaRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _akAkaunRepo = akAkaunRepository;
            _akTerimaRepo = akTerimaRepository;
            _userService = userService;
            _cart = cart;
        }

        // GET: AkInvois
        [Authorize(Policy = "IN001")]
        public async Task<IActionResult> Index(string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Inbois", Value = "NoRujukan" },
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

            var akInvois = await _akInvoisRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akInvois = await _akInvoisRepo.GetAllIncludeDeletedItems();
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akInvois = akInvois.Where(s => s.NoInbois.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akInvois = akInvois.Where(s => s.AkPenghutang!.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akInvois = akInvois.Where(x => x.Tarikh >= date1
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

            List<AkInvoisViewModel> viewModel = new List<AkInvoisViewModel>();

            if (akInvois != null)
            {
                foreach (AkInvois item in akInvois)
                {
                    var namaSykt = "";
                    var alamat1 = "";

                    namaSykt = item.AkPenghutang?.NamaSykt ?? "";
                    alamat1 = item.AkPenghutang?.Alamat1 ?? "";

                    decimal jumlahPerihal = 0;

                    if (item.AkInvois2 != null)
                    {
                        foreach (AkInvois2 item2 in item.AkInvois2)
                        {
                            jumlahPerihal += item2.Amaun;
                        }
                    }
                    
                    viewModel.Add(new AkInvoisViewModel
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
                        FlCetak = item.FlCetak,
                        FlStatusSemak = item.FlStatusSemak,
                        FlStatusLulus = item.FlStatusLulus,
                        JumlahPerihal = jumlahPerihal
                    }
                    );
                }
            }
            

            List<JPenyemak> penyemak = _context.JPenyemak
                .Include(x => x.SuPekerja)
                .Where(x => x.IsInvois == true).OrderBy(b => b.SuPekerja!.Nama).ToList();
            ViewBag.JPenyemak = penyemak;

            List<JPelulus> pelulus = _context.JPelulus
                .Include(x => x.SuPekerja)
                .Where(x => x.IsInvois == true).OrderBy(b => b.SuPekerja!.Nama).ToList();
            ViewBag.JPelulus = pelulus;

            return View(viewModel);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkPenghutang> akPenghutangList = _context.AkPenghutang
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPenghutang = akPenghutangList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

            List<AkCarta> KodObjekAPList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4" && (b.Kod.Substring(0, 1) == "A"))
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.KodObjekAP = KodObjekAPList;

        }

        private void PopulateTable(int? id)
        {
            List<AkInvois1> akInvois1Table = _context.AkInvois1
                .Include(b => b.AkCarta)
                .Where(b => b.AkInvoisId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akInvois1 = akInvois1Table;

            List<AkInvois2> akInvois2Table = _context.AkInvois2
                .Where(b => b.AkInvoisId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akInvois2 = akInvois2Table;
        }

        private void PopulateCart()
        {
            List<AkInvois1> lines1 = _cart.Lines1.ToList();

            foreach (AkInvois1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            List<AkInvois2> lines2 = _cart.Lines2.ToList();

            ViewBag.akInvois1 = lines1;
            ViewBag.akInvois2 = lines2;
        }

        private void PopulateCartFromDb(AkInvois akInvois)
        {
            List<AkInvois1> akInvois1Table = _context.AkInvois1
                .Include(b => b.AkCarta)
                .Where(b => b.AkInvoisId == akInvois.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkInvois1 item in akInvois1Table)
            {
                _cart.AddItem1(item.AkInvoisId,
                               item.Amaun,
                               item.AkCartaId);
            }

            List<AkInvois2> akInvois2Table = _context.AkInvois2
                .Where(b => b.AkInvoisId == akInvois.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkInvois2 item in akInvois2Table)
            {
                _cart.AddItem2(item.AkInvoisId,
                               item.Indek,
                               item.Baris,
                               item.Bil ?? "",
                               item.NoStok ?? "",
                               item.Perihal?.ToUpper()?? "",
                               item.Kuantiti,
                               item.Unit?.ToUpper()?? "",
                               item.Harga,
                               item.Amaun);
            }
        }


        // GET: AkInvois/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkInvois == null)
            {
                return NotFound();
            }

            // admin access
            var akInvois = await _akInvoisRepo.GetByIdIncludeDeletedItems((int)id);

            var kodObjekAkaunPemiutang = await _akCartaRepo.GetByIdIncludeDeletedItems(akInvois.KodObjekAPId);


            // check if already paid, show list of PVs in AkBelian
            var akTerima3 = _context.AkTerima3.Include(b => b.AkTerima).Where(b => b.AkInvoisId == id).ToList();

            var penghutang = await _akPenghutangRepo.GetByIdIncludeDeletedItems(akInvois.AkPenghutangId);

            if (akInvois == null)
            {
                return NotFound();
            }
            // admin access end

            // normal user access
            if (User.IsInRole("User"))
            {
                kodObjekAkaunPemiutang = await _akCartaRepo.GetById(akInvois.KodObjekAPId);

                penghutang = await _akPenghutangRepo.GetById(akInvois.AkPenghutangId);

                if (akInvois == null)
                {
                    return NotFound();
                }
            }
            //normal user access end

            AkInvoisViewModel viewModel = new AkInvoisViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkPenghutangId = akInvois.AkPenghutangId;
            if (akTerima3 != null)
            {
                foreach (var i in akTerima3)
                {
                    viewModel.JumlahTerimaan += i.Amaun;
                }

                viewModel.AkTerima3 = akTerima3;
            }
            viewModel.AkPenghutang = penghutang;
            viewModel.Id = akInvois.Id;
            viewModel.Tahun = akInvois.Tahun;
            viewModel.NoInbois = akInvois.NoInbois;
            viewModel.Tarikh = akInvois.Tarikh;
            viewModel.JKWId = akInvois.JKWId;
            viewModel.JKW = akInvois.JKW;
            viewModel.JBahagianId = akInvois.JBahagianId;
            viewModel.JBahagian = akInvois.JBahagian;
            viewModel.KodObjekAP = kodObjekAkaunPemiutang;
            viewModel.Jumlah = akInvois.Jumlah;
            viewModel.TarikhPosting = akInvois.TarikhPosting;
            viewModel.FlPosting = akInvois.FlPosting;
            viewModel.FlHapus = akInvois.FlHapus;

            if (akInvois.AkInvois2 != null)
            {
                foreach (AkInvois2 item in akInvois.AkInvois2)
                {
                    viewModel.JumlahPerihal += item.Amaun;
                }
            }
            
            viewModel.AkInvois1 = akInvois.AkInvois1;
            viewModel.AkInvois2 = akInvois.AkInvois2;

            PopulateTable(id);
            return View(viewModel);
        }

        // GET: AkInvois/Create
        [Authorize(Policy = "IN001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number 
            var year = DateTime.Now.Year.ToString();
            var data = 1;

            ViewBag.NoRujukan = GetNoRujukan(data, year);
            // get latest no rujukan running number end

            PopulateList();
            CartEmpty();
            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akInvois1 = new List<int>();
                ViewBag.akInvois2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // on change kod penghutang controller
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

        // get an item from cart akInvois1
        public JsonResult GetAnItemCartAkInvois1(AkInvois1 akInvois1)
        {

            try
            {
                AkInvois1 data = _cart.Lines1.Where(x => x.AkCartaId == akInvois1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akInvois1 end

        //save cart akInvois1
        public JsonResult SaveCartAkInvois1(AkInvois1 akInvois1)
        {

            try
            {

                var ak1 = _cart.Lines1.Where(x => x.AkCartaId == akInvois1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (ak1 != null)
                {
                    _cart.RemoveItem1(akInvois1.AkCartaId);

                    _cart.AddItem1(akInvois1.AkInvoisId,
                                    akInvois1.Amaun,
                                    akInvois1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akInvois1 end

        // get all item from cart akInvois1
        public JsonResult GetAllItemCartAkInvois1()
        {

            try
            {
                List<AkInvois1> data = _cart.Lines1.ToList();

                foreach (AkInvois1 item in data)
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
        // get all item from cart akInvois1 end

        // get an item from cart akInvois2
        public JsonResult GetAnItemCartAkInvois2(AkBelian2 akInvois2)
        {

            try
            {
                AkInvois2 data = _cart.Lines2.Where(x => x.Indek == akInvois2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akBelian2 end

        //save cart akInvois2
        public JsonResult SaveCartAkInvois2(AkInvois2 akInvois2)
        {

            try
            {

                var ak2 = _cart.Lines2.Where(x => x.Indek == akInvois2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (ak2 != null)
                {
                    _cart.RemoveItem2(akInvois2.Indek);

                    _cart.AddItem2(akInvois2.AkInvoisId,
                                   akInvois2.Indek,
                                   akInvois2.Baris,
                                   akInvois2.Bil ?? "",
                                   akInvois2.NoStok ?? "",
                                   akInvois2.Perihal?.ToUpper()?? "",
                                   akInvois2.Kuantiti,
                                   akInvois2.Unit?.ToUpper()?? "",
                                   akInvois2.Harga,
                                   akInvois2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akBelian2 end

        // get all item from cart akInvois2
        public JsonResult GetAllItemCartAkInvois2()
        {

            try
            {
                List<AkInvois2> data = _cart.Lines2.OrderBy(b => b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akInvois2 end

        private string GetNoRujukan(int data, string year)
        {
            var ptj = _context.JPTJ.Include(b => b.JKW).FirstOrDefault(x => x.JKWId == data);

            var initial = ptj?.JKW?.Kod.Substring(0,1) ?? "1" + ptj?.Kod ?? "01" ;

            string prefix = year + "/" + initial + "/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkInvois
                       .IgnoreQueryFilters()
                       .Where(x => x.Tahun == year && x.JKWId == data)
                       .Max(x => x.NoInbois);

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

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkInvois1> akInvois1 = new List<AkInvois1>();
            var cart1 = _cart.Lines1.ToList();

            if (cart1 != null && cart1.Count() > 0)
            {
                foreach (var item in cart1)
                {
                    akInvois1.Add(item);
                }
            }
            ViewBag.akInvois1 = akInvois1;
            // table 1 end

            // table 2
            List<AkInvois2> akInvois2 = new List<AkInvois2>();
            var cart2 = _cart.Lines2.ToList();

            if (cart2 != null && cart2.Count() > 0)
            {
                foreach (var item in cart2)
                {
                    akInvois2.Add(item);
                }
            }
            ViewBag.akInvois2 = akInvois2;
            // table 2 end
        }
        // populate table from cart end

        // POST: AkInvois/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "IN001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkInvois akInvois, decimal JumlahPerihal, string syscode)
        {
            AkInvois m = new AkInvois();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var penghutang = await _akPenghutangRepo.GetById(akInvois.AkPenghutangId);
            if (penghutang == null)
            {
                TempData[SD.Error] = "Penghutang tidak wujud..!";
                //PopulateCart();
                PopulateList();
                PopulateTableFromCart();
                CartEmpty();
                return View(akInvois);
            }

            var noRujukan = "DI/"+ GetNoRujukan(akInvois.JKWId, akInvois.Tahun);

            // checking for jumlah objek & jumlah perihal
            if (akInvois.Jumlah != JumlahPerihal)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
                PopulateList();
                PopulateTableFromCart();
                return View(akInvois);
            }

            if (ModelState.IsValid)
            {
                m.KodObjekAPId = akInvois.KodObjekAPId;
                m.JKWId = akInvois.JKWId;
                m.JBahagianId = akInvois.JBahagianId;
                m.Tahun = akInvois.Tahun;
                m.NoInbois = noRujukan;
                m.Tarikh = akInvois.Tarikh;
                m.Jumlah = akInvois.Jumlah;
                m.FlPosting = 0;

                m.AkPenghutangId = akInvois.AkPenghutangId;

                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkInvois1 = _cart.Lines1.ToArray();
                m.AkInvois2 = _cart.Lines2.ToArray();

                await _akInvoisRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.NoInbois, m.NoInbois, 0, m.Jumlah, pekerjaId, modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Maklumat gagal disimpan.";
            PopulateList();
            PopulateTableFromCart();
            return View(akInvois);
        }

        // GET: AkInvois/Edit/5
        [Authorize(Policy = "SP001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkInvois == null)
            {
                return NotFound();
            }

            var akInvois = await _akInvoisRepo.GetById((int)id);

            if (akInvois == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akInvois);
            return View(akInvois);
        }

        // get latest Index number in AkInvois2
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

        // POST: AkInvois/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "IN001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  AkInvois akInvois,decimal JumlahPerihal, string syscode)
        {
            if (id != akInvois.Id)
            {
                return NotFound();
            }

            if (akInvois.Jumlah == JumlahPerihal)
            {
                TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
                PopulateList();
                PopulateTableFromCart();
                return View(akInvois);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    AkInvois dataAsal = await _akInvoisRepo.GetById(id);

                    // list of input that cannot be change
                    akInvois.Tahun = dataAsal.Tahun;
                    akInvois.JKWId = dataAsal.JKWId;
                    akInvois.JBahagianId = dataAsal.JBahagianId;
                    akInvois.NoInbois = dataAsal.NoInbois;
                    akInvois.AkPenghutangId = dataAsal.AkPenghutangId;
                    akInvois.TarMasuk = dataAsal.TarMasuk;
                    akInvois.UserId = dataAsal.UserId;
                    akInvois.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    akInvois.KodObjekAPId = dataAsal.KodObjekAPId;
                    decimal jumlahAsal = dataAsal.Jumlah;
                    // list of input that cannot be change end

                    if (dataAsal.AkInvois1 != null)
                    {
                        foreach (AkInvois1 item in dataAsal.AkInvois1)
                        {
                            var model = _context.AkInvois1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    if (dataAsal.AkInvois2 != null)
                    {
                        foreach (AkInvois2 item in dataAsal.AkInvois2)
                        {
                            var model = _context.AkInvois2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akInvois.AkInvois1 = _cart.Lines1.ToList();
                    akInvois.AkInvois2 = _cart.Lines2.ToList();

                    akInvois.UserIdKemaskini = user?.UserName ?? "";
                    akInvois.TarKemaskini = DateTime.Now;
                    akInvois.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akInvois);

                    //insert applog
                    if (jumlahAsal != akInvois.Jumlah)
                    {
                        _appLog.Insert("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(akInvois.Jumlah).ToString("#,##0.00"), akInvois.NoInbois, id, akInvois.Jumlah, pekerjaId, modul,syscode,namamodul,user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", akInvois.NoInbois, id, akInvois.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                    }

                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkInvoisExists(akInvois.Id))
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
            return View(akInvois);
        }

        // GET: AkInvois/Delete/5
        [Authorize(Policy = "IN001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkInvois == null)
            {
                return NotFound();
            }

            var akInvois = await _akInvoisRepo.GetById((int)id);

            if (akInvois == null)
            {
                return NotFound();
            }

            AkInvoisViewModel viewModel = new AkInvoisViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkPenghutangId = akInvois.AkPenghutangId;
            viewModel.AkPenghutang = akInvois.AkPenghutang;
            viewModel.JBahagian = akInvois.JBahagian;
            viewModel.Id = akInvois.Id;
            viewModel.Tahun = akInvois.Tahun;
            viewModel.NoInbois = akInvois.NoInbois;
            viewModel.Tarikh = akInvois.Tarikh;
            viewModel.JKW = akInvois.JKW;
            viewModel.KodObjekAP = akInvois.KodObjekAP;
            viewModel.Jumlah = akInvois.Jumlah;
            viewModel.TarikhPosting = akInvois.TarikhPosting;
            viewModel.FlPosting = akInvois.FlPosting;
            viewModel.FlHapus = akInvois.FlHapus;

            if (akInvois.AkInvois2 != null)
            {
                foreach (AkInvois2 item in akInvois.AkInvois2)
                {
                    viewModel.JumlahPerihal += item.Amaun;
                }
            }
            
            viewModel.AkInvois1 = akInvois.AkInvois1;
            viewModel.AkInvois2 = akInvois.AkInvois2;

            PopulateTable(id);
            return View(viewModel);
        }

        // POST: AkInvois/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "TG002D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkInvois == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkInvois'  is null.");
            }
            var akInvois = await _context.AkInvois.FindAsync(id);
            if (akInvois != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                // check if already posting redirect back
                if (akInvois.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }

                // check if already link with akPV, Batal akPV included
                var akTerima = await _akTerimaRepo.GetAll();
                var akTerima3 = _context.AkTerima3.ToList();
                var result = (from tbl3 in akTerima3
                              join tbl in akTerima
                              on tbl3.AkTerimaId equals tbl.Id into tbl3Tbl
                              from tbl3_tbl in tbl3Tbl
                              select new
                              {
                                  Id = tbl3.Id,
                                  AkTerimaId = tbl3.AkTerimaId,
                                  AkInvoisId = tbl3.AkInvoisId

                              }).Where(x => x.AkInvoisId == id).FirstOrDefault();

                if (result != null)
                {
                    AkTerima akTerimaItem = await _akTerimaRepo.GetById(result.AkTerimaId);
                    //duplicate id error
                    TempData[SD.Error] = "Data terkait dengan no Resit " + akTerimaItem.NoRujukan + ".";
                    return RedirectToAction(nameof(Index));
                }

                akInvois.UserIdKemaskini = user?.UserName ?? "";
                akInvois.TarKemaskini = DateTime.Now;
                akInvois.SuPekerjaKemaskiniId = pekerjaId;

                _context.AkInvois.Remove(akInvois);
                //insert applog
                _appLog.Insert("Hapus", "Hapus Data", akInvois.NoInbois, id, akInvois.Jumlah, pekerjaId, modul,syscode,namamodul,user);
                //insert applog end
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkInvoisExists(int id)
        {
          return (_context.AkInvois?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool CurrentAkInvoisExists(int akPenghutangId, string noRujukan)
        {
            return _context.AkInvois.Any(e => e.AkPenghutangId == akPenghutangId && e.NoInbois == noRujukan);
        }

        // function  json Create
        public JsonResult GetCarta(AkCarta akCarta)
        {
            try
            {
                var result = _context.AkCarta.FirstOrDefault(b => b.Id == akCarta.Id);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public async Task<JsonResult> SaveAkInvois1(AkInvois1 akInvois1)
        {

            try
            {
                if (akInvois1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akInvois1.AkInvoisId,
                                    akInvois1.Amaun,
                                    akInvois1.AkCartaId);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkInvois1(AkInvois1 akInvois1)
        {

            try
            {
                if (akInvois1 != null)
                {

                    _cart.RemoveItem1(akInvois1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkInvois2(AkInvois2 akInvois2)
        {

            try
            {
                if (akInvois2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akInvois2.AkInvoisId,
                                   akInvois2.Indek,
                                   akInvois2.Baris,
                                   akInvois2.Bil ?? "",
                                   akInvois2.NoStok ?? "",
                                   akInvois2.Perihal?.ToUpper()?? "",
                                   akInvois2.Kuantiti,
                                   akInvois2.Unit?.ToUpper()?? "",
                                   akInvois2.Harga,
                                   akInvois2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkInvois2(AkInvois2 akInvois2)
        {

            try
            {
                if (akInvois2 != null)
                {

                    _cart.RemoveItem2(akInvois2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // function  json Create end

        // Semak function
        [HttpPost, ActionName("Semak")]
        [Authorize(Policy = "IN001T")]
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

                AkInvois sp = await _akInvoisRepo.GetById((int)id);

                //check for print
                if (sp.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal disemak. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                //semak operation start here
                //update semak status
                sp.FlStatusSemak = 1;
                sp.TarSemak = tarikhSemak;

                sp.JPenyemakId = penyemakId;


                await _akInvoisRepo.Update(sp);

                //insert applog
                _appLog.Insert("Posting", "Semak Data", sp.NoInbois, (int)id, sp.Jumlah, pekerjaId,modul, syscode, namamodul, user);

                //insert applog end

                await _context.SaveChangesAsync();

                TempData[SD.Success] = "Data berjaya disemak.";
            }

            return RedirectToAction(nameof(Index));

        }
        // Semak function end

        // posting function
        [Authorize(Policy = "IN001T")]
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

                AkInvois akInvois = await _akInvoisRepo.GetById((int)id);

                List<AkInvois1>? akI1 = new List<AkInvois1>();
                if (akInvois.AkInvois1 != null) akI1 = akInvois.AkInvois1.ToList();

                var akAkaun = await _context.AkAkaun.FirstOrDefaultAsync(x => x.NoRujukan == akInvois.NoInbois);
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

                }
                else
                {
                    //posting operation start here
                    var kodPembekal = "";
                    var penerima = "";

                    if (akInvois.AkPenghutangId != 0)
                    {
                        kodPembekal = akInvois.AkPenghutang?.KodSykt ?? "";
                        penerima = akInvois.AkPenghutang?.NamaSykt ?? "";
                    }

                    foreach (AkInvois1 item in akI1)
                    {

                        //insert into akAkaun
                        AkAkaun akHutang = new AkAkaun()
                        {
                            NoRujukan = akInvois.NoInbois,
                            JKWId = akInvois.JKWId,
                            JBahagianId = akInvois.JBahagianId,
                            AkCartaId1 = akInvois.KodObjekAPId,
                            AkCartaId2 = item.AkCartaId,
                            Tarikh = akInvois.Tarikh,
                            Debit = item.Amaun,
                            AkPenghutangId = akInvois.AkPenghutangId
                        };

                        await _akAkaunRepo.Insert(akHutang);

                        AkAkaun akAObjek = new AkAkaun()
                        {
                            NoRujukan = akInvois.NoInbois,
                            JKWId = akInvois.JKWId,
                            JBahagianId = akInvois.JBahagianId,
                            AkCartaId1 = item.AkCartaId,
                            AkCartaId2 = akInvois.KodObjekAPId,
                            Tarikh = akInvois.Tarikh,
                            Kredit = item.Amaun,
                            AkPenghutangId = akInvois.AkPenghutangId
                        };

                        await _akAkaunRepo.Insert(akAObjek);
                    }

                    //update posting status in akTerima
                    akInvois.FlPosting = 1;
                    akInvois.TarikhPosting = DateTime.Now;
                    await _akInvoisRepo.Update(akInvois);

                    //insert applog
                    _appLog.Insert("Posting", "Posting Data", akInvois.NoInbois, (int)id, akInvois.Jumlah, pekerjaId, modul, syscode, namamodul, user);

                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan.";

                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "IN001UT")]
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

                AkInvois akInvois = await _akInvoisRepo.GetById((int)id);

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akInvois.NoInbois).ToList();

                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    var akTerima = await _akTerimaRepo.GetAll();
                    var akTerima3 = _context.AkTerima3.ToList();
                    var result = (from tbl3 in akTerima3
                                  join tbl in akTerima
                                  on tbl3.AkTerimaId equals tbl.Id into tbl3Tbl
                                  from tbl3_tbl in tbl3Tbl
                                  select new
                                  {
                                      Id = tbl3.Id,
                                      AkTerimaId = tbl3.AkTerimaId,
                                      AkInvoisId = tbl3.AkInvoisId

                                  }).Where(x => x.AkInvoisId == id).FirstOrDefault();

                    if (result != null)
                    {
                        AkTerima akTerimaItem = await _akTerimaRepo.GetById(result.AkTerimaId);
                        //duplicate id error
                        TempData[SD.Error] = "Data terkait dengan no Resit " + akTerimaItem.NoRujukan + ".";
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
                        akInvois.FlPosting = 0;
                        akInvois.TarikhPosting = null;
                        await _akInvoisRepo.Update(akInvois);

                        //insert applog
                        _appLog.Insert("UnPosting", "Batal Posting Data", akInvois.NoInbois, (int)id, akInvois.Jumlah, pekerjaId,modul,syscode,namamodul,user);

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
        // POST: AkPV/Cancel/5
        [Authorize(Policy = "IN001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _akInvoisRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            if (CurrentAkInvoisExists(obj.AkPenghutangId, obj.NoInbois) == false)
            {
                // Batal operation

                obj.FlHapus = 0;
                obj.UserIdKemaskini = user?.UserName ??"";
                obj.TarKemaskini = DateTime.Now;
                obj.SuPekerjaKemaskiniId = pekerjaId;
                _context.AkInvois.Update(obj);

                // Batal operation end

                //insert applog
                _appLog.Insert("Rollback", "Rollback Data", obj.NoInbois, id, obj.Jumlah, pekerjaId,modul, syscode,namamodul,user);
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

        // printing NInvois
        [Authorize(Policy = "IN001P")]
        public async Task<IActionResult> PrintPdf(int id, string syscode)
        {
            AkInvois akI = await _akInvoisRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akI.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(0 - akI.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akI.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            InvoisPrintModel data = new InvoisPrintModel();

            CompanyDetails company = await _userService.GetCompanyDetails();

            if (akI.AkInvois2 != null)
            {
                foreach (AkInvois2 item in akI.AkInvois2)
                {
                    data.JumlahPerihal += item.Amaun;
                }
            }
            
            data.CompanyDetail = company;
            data.akInvois = akI;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.username = user?.UserName ?? "";

            //update cetak -> 1
            akI.FlCetak = 1;
            await _akInvoisRepo.Update(akI);

            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", akI.NoInbois, id, akI.Jumlah, pekerjaId,modul,syscode,namamodul,user);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("InvoisPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing Invois end

    }
}
