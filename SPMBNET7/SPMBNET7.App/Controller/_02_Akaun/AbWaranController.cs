﻿using System;
using System.Collections.Generic;
using System.Data;
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
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Math;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AbWaranController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "BJ001";
        public const string namamodul = "Waran";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AbWaran, int, string> _abWaranRepo;
        private readonly ListViewIRepository<AbWaran1, int> _abWaran1Repo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JBahagian, int, string> _jBahagianRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPPRepo;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly UserServices _userService;
        private CartWaran _cart;

        public AbWaranController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AbWaran, int, string> abWaranRepo,
            ListViewIRepository<AbWaran1, int> abWaran1Repo,
            IRepository<JKW, int, string> jkwRepo,
            IRepository<JBahagian, int, string> jBahagianRepo,
            IRepository<AkCarta, int, string> akCartaRepo,
            IRepository<AbBukuVot, int, string> abBukuVotRepo,
            IRepository<AkPO, int, string> akPORepo,
            IRepository<AkPV, int, string> akPVRepo,
            IRepository<AkTerima, int, string> akTerimaRepo,
            IRepository<SpPendahuluanPelbagai, int, string> spPPRepo,
            CustomIRepository<string, int> customrepo,
            UserServices userService,
            CartWaran cart)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _abWaranRepo = abWaranRepo;
            _abWaran1Repo = abWaran1Repo;
            _kwRepo = jkwRepo;
            _jBahagianRepo = jBahagianRepo;
            _akCartaRepo = akCartaRepo;
            _abBukuVotRepo = abBukuVotRepo;
            _akPORepo = akPORepo;
            _akPVRepo = akPVRepo;
            _akTerimaRepo = akTerimaRepo;
            _spPPRepo = spPPRepo;
            _customRepo = customrepo;
            _userService = userService;
            _cart = cart;
        }

        // GET: AbWaran
        [Authorize(Policy = "BJ001")]
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
                new SelectListItem() { Text = "Tahun", Value = "Tahun" }
            };

            if (!string.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var abWaran = await _abWaranRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                abWaran = await _abWaranRepo.GetAllIncludeDeletedItems();
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        abWaran = abWaran.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Tahun")
                    {
                        abWaran = abWaran.Where(s => s.Tahun.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        abWaran = abWaran.Where(x => x.Tarikh >= date1
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

            return View(abWaran);
        }

        // GET: AbWaran/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AbWaran == null)
            {
                return NotFound();
            }

            var abWaran = await _abWaranRepo.GetByIdIncludeDeletedItems((int)id);

            if (abWaran == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            return View(abWaran);
        }

        private void PopulateTable(int? id)
        {
            List<AbWaran1> table1 = _context.AbWaran1
                .Include(b => b.AkCarta)
                .Where(b => b.AbWaranId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.abWaran1 = table1;
        }

        private void PopulateTableFromCart()
        {
            // table 1
            List<AbWaran1> tbl1 = new List<AbWaran1>();
            var cart1 = _cart.Lines1.ToList();

            if (cart1 != null && cart1.Count() > 0)
            {
                foreach (var item in cart1)
                {
                    tbl1.Add(item);
                }
            }
            ViewBag.abWaran1 = tbl1;
            // table 1 end

        }
        // populate table from cart end

        // GET: AbWaran/Create
        [Authorize(Policy = "BJ001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();

            ViewBag.NoRujukan = GetNoRujukan(year);
            // get latest no rujukan running number end

            PopulateList();
            CartEmpty();
            return View();
        }

        private string GetNoRujukan(string year)
        {
            string prefix = "WR/" + year + "/";
            int x = 1;
            string noRujukan = prefix + "0000";

            var LatestNoRujukan = _context.AbWaran
                       .IgnoreQueryFilters()
                       .Where(x => x.Tahun == year)
                       .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "0000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(9));
                x++;
                noRujukan = string.Format("{0:" + prefix + "0000}", x);
            }
            return noRujukan;
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Kod).ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
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
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // on change no PO controller
        [HttpPost]
        public JsonResult JsonGetKod(string year)
        {
            try
            {
                var result = GetNoRujukan(year);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // function  json Create
        public JsonResult GetCarta(int id, int id2)
        {
            try
            {
                var result = _context.AkCarta.Where(b => b.Id == id).FirstOrDefault();

                var bahagian = _context.JBahagian.FirstOrDefault(b => b.Id == id2);

                return Json(new { result = "OK", record = result, bahagian = bahagian });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public async Task<JsonResult> SaveAbWaran1(
            AbWaran1 abWaran1,
            string tahun,
            int jKWId)
        {

            try
            {
                if (abWaran1 != null)
                {
                    // check for baki peruntukan
                    if (abWaran1.TK == "-")
                    {
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == tahun && x.VotId == abWaran1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == abWaran1.JBahagianId)
                                .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, abWaran1.AkCartaId, jKWId, abWaran1.JBahagianId);

                            if (sum < abWaran1.Amaun)
                            {
                                return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak mencukupi." });
                            }
                        }
                        else
                        {
                            return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak wujud" });
                        }
                    }
                    // check for baki peruntukan end

                    var cart = _cart.Lines1
                        .FirstOrDefault(b => b.AkCartaId == abWaran1.AkCartaId
                        && b.JBahagianId == abWaran1.JBahagianId);

                    if (cart != null)
                    {
                        return Json(new { result = "ERROR", message = "Bahagian dan Kod Akaun telah wujud." });
                    }
                    _cart.AddItem1(abWaran1.AbWaranId,
                                abWaran1.Amaun,
                                abWaran1.AkCartaId,
                                abWaran1.JBahagianId,
                                abWaran1.TK
                                );


                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAbWaran1(AbWaran1 abWaran1)
        {

            try
            {
                if (abWaran1 != null)
                {

                    _cart.RemoveItem1(abWaran1.AkCartaId, (int)abWaran1.JBahagianId!);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart abWaran1
        public JsonResult GetAnItemCartAbWaran1(AbWaran1 abWaran1)
        {

            try
            {
                AbWaran1 data = _cart.Lines1.Where(x => x.AkCartaId == abWaran1.AkCartaId && x.JBahagianId == abWaran1.JBahagianId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart AbWaran1 end

        //save cart AbWaran1
        public async Task<JsonResult> SaveCartAbWaran1(
            AbWaran1 abWaran1,
            string tahun,
            int jKWId)
        {
            try
            {

                var abW1 = _cart.Lines1.Where(x => x.AkCartaId == abWaran1.AkCartaId && x.JBahagianId == abWaran1.JBahagianId).FirstOrDefault();

                if (abW1 != null)
                {
                    // check for baki peruntukan
                    if (abWaran1.TK == "-")
                    {
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == tahun && x.VotId == abWaran1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == abWaran1.JBahagianId)
                                .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, abWaran1.AkCartaId, jKWId, abWaran1.JBahagianId);

                            if (sum < abWaran1.Amaun)
                            {
                                return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak mencukupi." });
                            }
                        }
                        else
                        {
                            return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak wujud" });
                        }
                    }
                    // check for baki peruntukan end

                    _cart.RemoveItem1(abW1.AkCartaId, (int)abW1.JBahagianId!);

                    _cart.AddItem1(abWaran1.AbWaranId,
                                    abWaran1.Amaun,
                                    abWaran1.AkCartaId,
                                    abWaran1.JBahagianId,
                                    abWaran1.TK
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
        public JsonResult GetAllItemCartAbWaran1()
        {

            try
            {
                List<AbWaran1> data = _cart.Lines1.ToList();

                foreach (AbWaran1 item in data)
                {
                    var akCarta = _context.AkCarta.Find(item.AkCartaId);

                    item.AkCarta = akCarta;

                    var bahagian = _context.JBahagian.Find(item.JBahagianId);

                    item.JBahagian = bahagian;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPOLaras1 end

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

        // json Check Jumlah controller
        [HttpPost]
        public JsonResult JsonCheckJumlahFromCart()
        {
            try
            {
                var abWaran1 = _cart.Lines1.ToList();

                var result = "OK";

                decimal total = 0;
                foreach (var item in abWaran1)
                {
                    if (item.TK == "+")
                    {
                        total += item.Amaun;
                    }
                    else
                    {
                        total -= item.Amaun;
                    }
                }

                if (total != 0)
                {
                    result = "ERROR";
                }
                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        // POST: AbWaran/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "BJ001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AbWaran abWaran, string syscode)
        {
            AbWaran m = new AbWaran();
            abWaran.NoRujukan = GetNoRujukan(abWaran.Tahun);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            if (abWaran.FlJenisPindahan == 0)
            {
                foreach (var item in _cart.Lines1)
                {
                    abWaran.JBahagianId = item.JBahagianId;
                }
            }

            // check start
            // check if Tahun, FlJenisWaran ,JBahagianId, JKWId already exist or not 
            if (abWaran.FlJenisWaran == 0)
            {

                var w = await _context.AbWaran.Where(x => x.Tahun == abWaran.Tahun
                                                                && x.FlJenisWaran == abWaran.FlJenisWaran
                                                                && x.JKWId == abWaran.JKWId
                                                                && x.JBahagianId == abWaran.JBahagianId)
                                    .FirstOrDefaultAsync();

                if (w != null)
                {
                    TempData[SD.Error] = "Data bagi Tahun, Jenis Waran, Kump. Wang dan Bahagian telah wujud.";
                    ViewBag.NoRujukan = GetNoRujukan(abWaran.Tahun);
                    PopulateTableFromCart();
                    PopulateList();

                    return View(abWaran);
                }
            }

            // check if jenis Waran pindah, total value should be 0, if not return view(abWaran)
            if (abWaran.FlJenisWaran == 2)
            {


                var abWaran1 = _cart.Lines1.ToList();

                decimal total = 0;
                foreach (var item in abWaran1)
                {
                    if (item.TK == "+")
                    {
                        total += item.Amaun;
                    }
                    else if (item.TK == "-")
                    {
                        total -= item.Amaun;
                    }
                }

                if (total != 0)
                {
                    TempData[SD.Error] = "Jumlah Tambah dan Kurang tidak sama.";
                    ViewBag.NoRujukan = GetNoRujukan(abWaran.Tahun);
                    PopulateTableFromCart();
                    PopulateList();

                    return View(abWaran);
                }
            }
            // check end

            if (ModelState.IsValid)
            {
                if (abWaran != null)
                {
                    m.FlJenisWaran = abWaran.FlJenisWaran;
                    m.FlJenisPindahan = abWaran.FlJenisPindahan;

                    m.Tahun = abWaran.Tahun;
                    m.Tarikh = abWaran.Tarikh;
                    m.NoRujukan = GetNoRujukan(abWaran.Tahun);
                    m.JKWId = abWaran.JKWId;
                    m.JBahagianId = abWaran.JBahagianId;
                    m.Jumlah = abWaran.Jumlah;
                    m.Catatan = abWaran.Catatan?.ToUpper();
                    m.UserId = user?.UserName ?? "";
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    m.AbWaran1 = _cart.Lines1.ToArray();

                    // check for baki peruntukan
                    foreach (AbWaran1 item in m.AbWaran1)
                    {
                        if (item.TK == "-")
                        {
                            bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == m.Tahun && x.VotId == item.AkCartaId && x.JKWId == m.JKWId && x.JBahagianId == item.JBahagianId)
                                .AnyAsync();

                            var carta = _context.AkCarta.Find(item.AkCartaId);

                            if (IsExistAbBukuVot == true)
                            {
                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(m.Tahun, item.AkCartaId, m.JKWId, item.JBahagianId);

                                if (sum < item.Amaun)
                                {
                                    TempData[SD.Error] = "Bajet untuk kod akaun " + carta?.Kod ?? "" + " tidak mencukupi.";
                                    ViewBag.NoRujukan = GetNoRujukan(abWaran.Tahun);
                                    PopulateTableFromCart();
                                    PopulateList();

                                    return View(abWaran);
                                }
                            }
                            else
                            {
                                TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta?.Kod ?? "";
                                ViewBag.NoRujukan = GetNoRujukan(abWaran.Tahun);
                                PopulateTableFromCart();
                                PopulateList();

                                return View(abWaran);
                            }

                        }
                    }
                    // check for baki peruntukan end

                    await _abWaranRepo.Insert(m);

                    //insert applog
                    _appLog.Insert("Tambah", m.NoRujukan, m.NoRujukan, 0, abWaran.Jumlah, pekerjaId,modul,syscode,namamodul,user);
                    //insert applog end
                    await _abWaranRepo.Save();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No Pendaftaran adalah " + m.NoRujukan;

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData[SD.Error] = "Maklumat tidak lengkap. Sila cuba sekali lagi.";
                    ViewBag.NoRujukan = GetNoRujukan(abWaran?.Tahun ?? DateTime.Now.ToString("yyyy"));
                    PopulateTableFromCart();
                    PopulateList();

                    return View(abWaran);
                }
            }

            ViewBag.NoRujukan = GetNoRujukan(abWaran?.Tahun ?? DateTime.Now.ToString("yyyy"));
            PopulateList();
            CartEmpty();

            return View(abWaran);
        }

        // GET: AbWaran/Edit/5
        [Authorize(Policy = "BJ001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AbWaran == null)
            {
                return NotFound();
            }

            var abWaran = await _abWaranRepo.GetById((int)id);

            if (abWaran == null)
            {
                return NotFound();
            }
            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(abWaran);
            return View(abWaran);
        }

        private void PopulateCartFromDb(AbWaran abWaran)
        {
            List<AbWaran1> table1 = _context.AbWaran1
                .Include(b => b.AkCarta)
                .Where(b => b.AbWaranId == abWaran.Id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AbWaran1 item in table1)
            {
                _cart.AddItem1(item.AbWaranId,
                               item.Amaun,
                               item.AkCartaId,
                               item.JBahagianId,
                               item.TK);
            }

        }

        // POST: AbWaran/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "BJ001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AbWaran abWaran, string syscode)
        {
            if (id != abWaran.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    AbWaran dataAsal = await _abWaranRepo.GetById(id);

                    // list of input that cannot be change
                    abWaran.Tahun = dataAsal.Tahun;
                    abWaran.JKWId = dataAsal.JKWId;
                    abWaran.JBahagianId = dataAsal.JBahagianId;
                    abWaran.FlJenisWaran = dataAsal.FlJenisWaran;
                    abWaran.TarMasuk = dataAsal.TarMasuk;
                    abWaran.UserId = dataAsal.UserId;
                    abWaran.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    abWaran.FlCetak = 0;
                    // list of input that cannot be change end

                    decimal jumlahAsal = dataAsal.Jumlah;

                    if (dataAsal != null && dataAsal.AbWaran1 != null)
                    {
                        foreach (AbWaran1 item in dataAsal.AbWaran1)
                        {
                            var model = _context.AbWaran1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        _context.Entry(dataAsal).State = EntityState.Detached;
                    }
                    
                    abWaran.AbWaran1 = _cart.Lines1.ToList();

                    // check for baki peruntukan
                    foreach (AbWaran1 item in _cart.Lines1)
                    {

                        if (item.TK == "-")
                        {
                            bool IsExistAbBukuVot = await _context.AbBukuVot
                               .Where(x => x.Tahun == abWaran.Tahun && x.VotId == item.AkCartaId && x.JKWId == abWaran.JKWId && x.JBahagianId == item.JBahagianId)
                               .AnyAsync();

                            var carta = _context.AkCarta.Find(item.AkCartaId);

                            if (IsExistAbBukuVot == true)
                            {
                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(abWaran.Tahun, item.AkCartaId, abWaran.JKWId, item.JBahagianId);

                                if (sum < item.Amaun)
                                {
                                    TempData[SD.Error] = "Bajet untuk kod akaun " + carta?.Kod + " tidak mencukupi.";
                                    PopulateList();
                                    PopulateTableFromCart();

                                    return View(abWaran);
                                }
                            }
                            else
                            {
                                TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta?.Kod;
                                PopulateList();
                                PopulateTableFromCart();

                                return View(abWaran);
                            }
                        }
                    }
                    // check for baki peruntukan end

                    abWaran.UserIdKemaskini = user?.UserName ?? "";
                    abWaran.TarKemaskini = DateTime.Now;
                    abWaran.SuPekerjaKemaskiniId = pekerjaId;
                    abWaran.Catatan = abWaran.Catatan?.ToUpper();

                    _context.Update(abWaran);
                    // insert applog

                    if (jumlahAsal != abWaran.Jumlah)
                    {

                       _appLog.Insert("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(abWaran.Jumlah).ToString("#,##0.00"), abWaran.NoRujukan, id, abWaran.Jumlah, pekerjaId, modul, syscode, namamodul, user);

                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", abWaran.NoRujukan, id, abWaran.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                    }
                    //insert applog end

                    TempData[SD.Success] = "Data berjaya diubah..!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbWaranExists(abWaran.Id))
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
            PopulateList();
            PopulateTableFromCart();
            return View(abWaran);
        }

        // GET: AbWaran/Delete/5
        [Authorize("BJ001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AbWaran == null)
            {
                return NotFound();
            }

            var abWaran = await _abWaranRepo.GetById((int)id);

            if (abWaran == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            return View(abWaran);
        }

        // POST: AbWaran/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize("BJ001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AbWaran == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AbWaran'  is null.");
            }
            var abWaran = await _context.AbWaran.FindAsync(id);
            if (abWaran != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                abWaran.UserIdKemaskini = user?.UserName ?? "";
                abWaran.TarKemaskini = DateTime.Now;
                abWaran.SuPekerjaKemaskiniId = pekerjaId;
                // check if already posting redirect back
                if (abWaran.FlPosting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }
                abWaran.FlCetak = 0;
                _context.AbWaran.Update(abWaran);

                //insert applog

                _appLog.Insert("Hapus", abWaran.NoRujukan, abWaran.NoRujukan, id, abWaran.Jumlah, pekerjaId, modul, syscode, namamodul, user);
                //insert applog end

                _context.AbWaran.Remove(abWaran);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";

            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool AbWaranExists(int id)
        {
          return (_context.AbWaran?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "BJ001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            var obj = await _abWaranRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Batal operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            _context.AbWaran.Update(obj);

            // Batal operation end

            //insert applog
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            _appLog.Insert("Rollback", "Rollback Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId,modul, syscode, namamodul, user);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        // posting function
        [Authorize(Policy = "BJ001T")]
        public async Task<IActionResult> Posting(int? id, string syscode)
        {
            // your code starts here
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                AbWaran obj = await _abWaranRepo.GetById((int)id);

                var jenisWaran = "";

                switch (obj.FlJenisWaran)
                {
                    case 0:
                        jenisWaran = "PERUNTUKAN ASAL";
                        obj.Tarikh = Convert.ToDateTime("01/01/" + obj.Tahun);
                        break;
                    case 1:
                        jenisWaran = "PERUNTUKAN TAMBAH/ TARIK BALIK";
                        break;
                    default:
                        jenisWaran = "PERUNTUKAN PINDAHAN";
                        break;
                }
                //check for print
                if (obj.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<AbWaran1> abWaran1 = obj.AbWaran1?.ToList();

                if (abWaran1 != null)
                {
                    // check for total in / out kod objek (only for WPP)
                    if (obj.FlJenisWaran == 2)
                    {

                        decimal total = 0;

                        foreach (var item in abWaran1)
                        {

                            if (item.TK == "-")
                            {
                                total -= item.Amaun;
                            }
                            else
                            {
                                total += item.Amaun;
                            }
                        }

                        if (total != 0)
                        {
                            TempData[SD.Error] = "Data gagal diluluskan. Jumlah Masuk / Keluar tidak sama.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    // check for total in / out kod objek end

                    // check for baki peruntukan
                    foreach (AbWaran1 item in abWaran1)
                    {

                        int? bahagianId = obj.JBahagianId;

                        if (bahagianId == null)
                        {
                            bahagianId = item.JBahagianId;
                        }

                        if (item.TK == "-")
                        {
                            bool IsExistAbBukuVot = await _context.AbBukuVot
                                   .Where(x => x.Tahun == obj.Tahun && x.VotId == item.AkCartaId && x.JKWId == obj.JKWId && x.JBahagianId == bahagianId)
                                   .AnyAsync();

                            if (IsExistAbBukuVot == true)
                            {
                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(obj.Tahun, item.AkCartaId, obj.JKWId, bahagianId);

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
                    }
                    // check for baki peruntukan end

                    var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(obj.NoRujukan)).FirstOrDefaultAsync();
                    if (abBukuVot != null)
                    {

                        //duplicate id error
                        TempData[SD.Error] = "Data gagal diluluskan.";

                    }
                    else
                    {
                        //posting operation start here

                        foreach (AbWaran1 item in abWaran1)
                        {
                            if (obj.FlJenisPindahan == 1)
                            {
                                if (item.TK == "+")
                                {
                                    //insert into AbBukuVot
                                    AbBukuVot abBukuVotPosting = new AbBukuVot()
                                    {
                                        Tahun = obj.Tahun,
                                        JKWId = obj.JKWId,
                                        JBahagianId = item.JBahagianId,
                                        Tarikh = obj.Tarikh,
                                        //Kod = "",
                                        Penerima = jenisWaran,
                                        VotId = item.AkCartaId,
                                        Rujukan = obj.NoRujukan,
                                        Kredit = item.Amaun
                                    };
                                    await _abBukuVotRepo.Insert(abBukuVotPosting);
                                }
                                else
                                {
                                    //insert into AbBukuVot
                                    AbBukuVot abBukuVotPosting = new AbBukuVot()
                                    {
                                        Tahun = obj.Tahun,
                                        JKWId = obj.JKWId,
                                        JBahagianId = item.JBahagianId,
                                        Tarikh = obj.Tarikh,
                                        //Kod = "",
                                        Penerima = jenisWaran,
                                        VotId = item.AkCartaId,
                                        Rujukan = obj.NoRujukan,
                                        Debit = item.Amaun
                                    };
                                    await _abBukuVotRepo.Insert(abBukuVotPosting);
                                }
                            }
                            else
                            {
                                if (item.TK == "+")
                                {
                                    //insert into AbBukuVot
                                    AbBukuVot abBukuVotPosting = new AbBukuVot()
                                    {
                                        Tahun = obj.Tahun,
                                        JKWId = obj.JKWId,
                                        JBahagianId = item.JBahagianId,
                                        Tarikh = obj.Tarikh,
                                        //Kod = "",
                                        Penerima = jenisWaran,
                                        VotId = item.AkCartaId,
                                        Rujukan = obj.NoRujukan,
                                        Kredit = item.Amaun
                                    };
                                    await _abBukuVotRepo.Insert(abBukuVotPosting);
                                }
                                else
                                {
                                    //insert into AbBukuVot
                                    AbBukuVot abBukuVotPosting = new AbBukuVot()
                                    {
                                        Tahun = obj.Tahun,
                                        JKWId = obj.JKWId,
                                        JBahagianId = item.JBahagianId,
                                        Tarikh = obj.Tarikh,
                                        //Kod = "",
                                        Penerima = jenisWaran,
                                        VotId = item.AkCartaId,
                                        Rujukan = obj.NoRujukan,
                                        Debit = item.Amaun
                                    };
                                    await _abBukuVotRepo.Insert(abBukuVotPosting);
                                }
                            }


                            // insert into AbBukuVot end

                        }

                        //update posting status in akPO
                        obj.FlPosting = 1;
                        obj.TarikhPosting = DateTime.Now;
                        await _abWaranRepo.Update(obj);

                        //insert applog
                        int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                        _appLog.Insert("Posting", "Posting Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId,modul, syscode, namamodul, user);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya diluluskan.";
                    }
                }
            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "BJ001UT")]
        public async Task<IActionResult> UnPosting(int? id, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                AbWaran obj = await _abWaranRepo.GetById((int)id);

               if (obj != null && obj.AbWaran1 != null)
                {
                    //check
                    // dah ada po atau tidak
                    foreach (var waran in obj.AbWaran1)
                    {
                        var akPO = await _akPORepo.GetAll();

                        foreach (var i in akPO)
                        {
                            var akPO1 = await _context.AkPO1
                                .Where(x => x.AkPOId == i.Id && x.AkCartaId == waran.AkCartaId)
                                .FirstOrDefaultAsync();

                            if (akPO1 != null)
                            {
                                //duplicate id error
                                TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No PO " + i.NoPO;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                    //
                    // dah ada baucer atau tidak
                    foreach (var waran in obj.AbWaran1)
                    {
                        var akPV = await _akPVRepo.GetAll();

                        foreach (var i in akPV)
                        {
                            var akPV1 = await _context.AkPV1
                                .Where(x => x.AkPVId == i.Id && x.AkCartaId == waran.AkCartaId)
                                .FirstOrDefaultAsync();

                            if (akPV1 != null)
                            {
                                //duplicate id error
                                TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No PV " + i.NoPV;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                    //
                    // dah ada resit atau tidak
                    foreach (var waran in obj.AbWaran1)
                    {
                        var akTerima = await _akTerimaRepo.GetAll();

                        foreach (var i in akTerima)
                        {
                            var akTerima1 = await _context.AkTerima1
                                .Where(x => x.AkTerimaId == i.Id && x.AkCartaId == waran.AkCartaId)
                                .FirstOrDefaultAsync();

                            if (akTerima1 != null)
                            {
                                //duplicate id error
                                TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No Resit " + i.NoRujukan;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                    //
                    // dah ada pendahuluan pelbagai atau tidak
                    foreach (var waran in obj.AbWaran1)
                    {
                        var sp = await _spPPRepo.GetAll();

                        foreach (var i in sp)
                        {
                            var sp1 = await _context.SpPendahuluanPelbagai
                                .Where(x => x.Id == i.Id && x.AkCartaId == waran.AkCartaId)
                                .FirstOrDefaultAsync();

                            if (sp1 != null)
                            {
                                //duplicate id error
                                TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No Permohonan " + i.NoPermohonan;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                    //

                    List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(obj.NoRujukan)).ToList();
                    if (abBukuVot == null)
                    {

                        //duplicate id error
                        TempData[SD.Error] = "Data belum diluluskan.";

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
                        obj.FlPosting = 0;
                        obj.TarikhPosting = null;
                        await _abWaranRepo.Update(obj);

                        //insert applog
                        int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                        _appLog.Insert("UnPosting", "UnPosting Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId,modul, syscode, namamodul, user);

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

        // printing Waran by akPO.Id
        [Authorize(Policy = "BJ001P")]
        public async Task<IActionResult> PrintPdf(int id, string syscode)
        {
            AbWaran obj = await _abWaranRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (obj.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(0 - obj.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(obj.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);

            WaranPrintModel data = new WaranPrintModel();

            data.CompanyDetail = await _userService.GetCompanyDetails();
            data.AbWaran = obj;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user?.UserName ?? "";

            //update cetak -> 1
            obj.FlCetak = 1;
            await _abWaranRepo.Update(obj);

            //insert applog
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            _appLog.Insert("Cetak", "Cetak Data", obj.NoRujukan, id, obj.Jumlah, pekerjaId, modul, syscode, namamodul, user);

            //insert applog end

            await _context.SaveChangesAsync();

            var preparedBySignature = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)?.Tandatangan;

            if (preparedBySignature != null)
            {
                data.TandatanganSedia = preparedBySignature;
            }

            return new ViewAsPdf("WaranPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing Waran end
    }
}
