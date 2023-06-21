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
    public class AkJurnalController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JU001";
        public const string namamodul = "Baucer Jurnal";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkJurnal, int, string> _akJurnalRepo;
        private readonly IRepository<JKW, int, string> _jKWRepo;
        private readonly IRepository<JBahagian, int, string> _bahagianRepo;
        private readonly ListViewIRepository<AkJurnal1, int> _akJurnal1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;
        private readonly IRepository<AkTunaiLejar, int, string> _akTunaiLejarRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVot;
        private readonly UserServices _userService;
        private CartJurnal _cart;

        public AkJurnalController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkJurnal, int, string> akJurnalRepository,
            IRepository<JKW, int, string> jKWRepository,
            IRepository<JBahagian, int, string> bahagianRepository,
            ListViewIRepository<AkJurnal1, int> akJurnal1Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AkTunaiRuncit, int, string> akTunaiRuncitRepository,
            IRepository<AkTunaiLejar, int, string> akTunaiLejarRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            UserServices userService,
            CartJurnal cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akJurnalRepo = akJurnalRepository;
            _jKWRepo = jKWRepository;
            _bahagianRepo = bahagianRepository;
            _akJurnal1Repo = akJurnal1Repository;
            _akCartaRepo = akCartaRepository;
            _akAkaunRepo = akAkaunRepository;
            _akTunaiRuncitRepo = akTunaiRuncitRepository;
            _akTunaiLejarRepo = akTunaiLejarRepository;
            _abBukuVot = abBukuVotRepository;
            _userService = userService;
            _cart = cart;
        }
        private string GetNoRujukan(int JKWId, string Tahun)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == JKWId);

            var kumpulanWang = kw?.Kod ?? "100";
            //var year = DateTime.Now.Year.ToString();
            string prefix = Tahun +"/"+ kumpulanWang+"/";
            int x = 1;
            string noRujukan = prefix + "000";

            var LatestNoRujukan = _context.AkJurnal
                .IgnoreQueryFilters()
                .Where(x => x.NoJurnal.Substring(0, 9) == prefix)
                .Max(x => x.NoJurnal);
            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(9));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            return noRujukan;
        }
        [HttpPost]
        public JsonResult JsonGetKod(AkJurnal data)
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
                    result = GetNoRujukan(data.JKWId,data.Tarikh.ToString("yyyy"));
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        private void PopulateList()
        {
            List<AkTunaiRuncit> tunaiRuncitList = _context.AkTunaiRuncit.Include(x => x.AkTunaiPemegang).OrderBy(b => b.KaunterPanjar).ToList();
            ViewBag.AkTunaiRuncit = tunaiRuncitList;

            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkCarta> cartaList = _context.AkCarta
                .Include(z => z.JKW)
                .Where(x => x.JParas!.Kod=="4")
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = cartaList;

        }
        private void PopulateTable(int? id)
        {
            List<AkJurnal1> akJurnal1Table = _context.AkJurnal1
                .Include(b => b.JBahagianDebit)
                .Include(b => b.JBahagianKredit)
                .Include(b => b.AkCartaDebit)
                .Include(b => b.AkCartaKredit)
                .Where(b => b.AkJurnalId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akJurnal1 = akJurnal1Table;
        }
        private void PopulateCart(AkJurnal akJurnal)
        {
            List<AkJurnal1> akJurnal1Table = _context.AkJurnal1
                .Include(b => b.JBahagianDebit)
                .Include(b => b.JBahagianKredit)
                .Include(b => b.AkCartaDebit)
                .Include(b => b.AkCartaKredit)
                .Where(b => b.AkJurnalId == akJurnal.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkJurnal1 akJurnal1 in akJurnal1Table)
            {
                var bahagianDebit = _context.JBahagian.FirstOrDefault(b => b.Id == akJurnal1.JBahagianDebitId);
                var cartaDebit = _context.AkCarta.FirstOrDefault(b => b.Id == akJurnal1.AkCartaDebitId);

                var bahagianKredit = _context.JBahagian.FirstOrDefault(b => b.Id == akJurnal1.JBahagianKreditId);
                var cartaKredit = _context.AkCarta.FirstOrDefault(b => b.Id == akJurnal1.AkCartaKreditId);

                _cart.AddItem1(
                    akJurnal1.AkJurnalId,
                    akJurnal1.Indeks,
                    (int)akJurnal1.JBahagianDebitId!,
                    bahagianDebit,
                    (int)akJurnal1.AkCartaDebitId!,
                    cartaDebit,
                    (int)akJurnal1.JBahagianKreditId!,
                    bahagianKredit,
                    (int)akJurnal1.AkCartaKreditId!,
                    cartaKredit,
                    akJurnal1.Amaun
                    );
            }
        }

        // populate table from cart ( used when user prompt error when submit form)
        private void PopulateTableFromCart()
        {
            // table 1
            List<AkJurnal1> tbl1 = new List<AkJurnal1>();
            var cart1 = _cart.Lines1.ToList();

            if (cart1 != null && cart1.Count() > 0)
            {

                foreach (var item in cart1)
                {
                    var bahagianDebit = _context.JBahagian.FirstOrDefault(b => b.Id == item.JBahagianDebitId);
                    item.JBahagianDebit = bahagianDebit;
                    var cartaDebit = _context.AkCarta.FirstOrDefault(b => b.Id == item.AkCartaDebitId);
                    item.AkCartaDebit = cartaDebit;

                    var bahagianKredit = _context.JBahagian.FirstOrDefault(b => b.Id == item.JBahagianKreditId);
                    item.JBahagianKredit = bahagianKredit;
                    var cartaKredit = _context.AkCarta.FirstOrDefault(b => b.Id == item.AkCartaKreditId);
                    item.AkCartaKredit = cartaKredit;

                    tbl1.Add(item);
                }
            }
            ViewBag.akJurnal1 = tbl1;
            // table 1 end

            
        }
        // populate table from cart end

        // GET: AkJurnal
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchKw,
            string searchColumn)
        {
            //populate search option

            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            List<SelectListItem> kwSelect = new()
            {
                new SelectListItem() { Text = "-- Pilih Kumpulan Wang --", Value = "" }
            };
            foreach (var q in kwList)
            {
                kwSelect.Add(new SelectListItem() { Text = q.Kod + " - " + q.Perihal, Value = q.Kod });
            }
            if (!string.IsNullOrEmpty(searchKw))
            {
                ViewBag.SearchKw = new SelectList(kwSelect, "Value", "Text", searchKw);
            }
            else
            {
                ViewBag.SearchKw = new SelectList(kwSelect, "Value", "Text", "");
            }

            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Jurnal", Value = "NoJurnal" },
                new SelectListItem() { Text = "Kumpulan Wang", Value = "KW" }
            };
            if (!string.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akJurnal = await _akJurnalRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akJurnal = await _akJurnalRepo.GetAllIncludeDeletedItems();

            }

            if (!string.IsNullOrEmpty(searchString) || !string.IsNullOrEmpty(searchKw)||
                (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoJurnal")
                    {
                        akJurnal = akJurnal.Where(s => s.NoJurnal.ToUpper().Contains(searchString.ToUpper()));
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
                        akJurnal = akJurnal.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                if (!string.IsNullOrEmpty(searchKw))
                {
                    if (searchColumn == "KW")
                    {
                        akJurnal = akJurnal.Where(s => s.JKW!.Kod == searchKw);
                    }
                    ViewBag.SearchKw = new SelectList(kwSelect, "Value", "Text", searchKw);
                }
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "Tarikh");
            }
            return View(akJurnal);
        }

        // GET: AkJurnal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkJurnal == null)
            {
                return NotFound();
            }

            var akJurnal = await _akJurnalRepo.GetByIdIncludeDeletedItems((int)id);

            if (User.IsInRole("User"))
            {
                akJurnal = await _akJurnalRepo.GetById((int)id);
            }


            if (akJurnal == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akJurnal);
        }

        // GET: AkJurnal/Create
        public IActionResult Create()
        {
            ViewBag.NoRujukan = GetNoRujukan(1, DateTime.Now.ToString("yyyy"));
            PopulateList();
            CartEmpty();
            return View();
        }

        // POST: AkJurnal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkJurnal akJurnal, string syscode)
        {
            AkJurnal m = new AkJurnal();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            if (akJurnal.AkTunaiRuncitId != 0)
            {
                m.AkTunaiRuncitId = akJurnal.AkTunaiRuncitId;
                m.FlJenisJurnal = 4;
                m.FlKategoriPenerima = 3;
            }

            decimal amaun = 0;
            foreach (var q in _cart.Lines1.ToArray())
            {
                amaun += q.Amaun;
            };

            if (ModelState.IsValid)
            {
                string noRujukan = GetNoRujukan(akJurnal.JKWId, akJurnal.Tarikh.ToString("yyyy"));
                m.JKWId = akJurnal.JKWId;
                m.JBahagianId = akJurnal.JBahagianId;
                m.NoJurnal = noRujukan;
                m.Tarikh = akJurnal.Tarikh;
                m.JumDebit = amaun;
                m.JumKredit = amaun;
                m.Catatan1 = akJurnal.Catatan1;
                m.Catatan2 = akJurnal.Catatan2;
                m.Catatan3 = akJurnal.Catatan3;
                m.Catatan4 = akJurnal.Catatan4;
                m.IsAKB = akJurnal.IsAKB;
                m.Posting = akJurnal.Posting;
                m.Cetak = akJurnal.Cetak;
                m.FlHapus = akJurnal.FlHapus;
                m.AkJurnal1 = _cart.Lines1.OrderBy(x => x.Indeks).ToList();
                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                await _akJurnalRepo.Insert(m);
                //insert applog
                _appLog.Insert("Tambah", noRujukan, noRujukan, 0, amaun, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end
                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat berjaya ditambah. No jurnal adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }
            ViewBag.NoRujukan = GetNoRujukan(akJurnal.JKWId, akJurnal.Tarikh.ToString("yyyy"));
            PopulateTableFromCart();
            PopulateList();
            return View(akJurnal);
        }

        // GET: AkJurnal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkJurnal == null)
            {
                return NotFound();
            }

            var akJurnal = await _akJurnalRepo.GetById((int)id);
            if (akJurnal.Posting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            akJurnal.JKW = await _jKWRepo.GetById(akJurnal.JKWId);

            if (akJurnal == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(akJurnal);
            return View(akJurnal);
        }

        // POST: AkJurnal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkJurnal akJurnal, string syscode)
        {
            if (id != akJurnal.Id)
            {
                return NotFound();
            }

            decimal amaun = 0;
            var akj1 = _cart.Lines1;
            foreach (var q in akj1)
            {
                amaun += q.Amaun;
            };

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    AkJurnal akJurnalAsal = await _akJurnalRepo.GetById(id);

                    akJurnalAsal.AkJurnal1 = _akJurnal1Repo.GetAll(id).Result.ToList();
                    foreach (AkJurnal1 item in akJurnalAsal.AkJurnal1)
                    {
                        var model = _context.AkJurnal1.FirstOrDefault(q => q.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }
                    var kreditAsal = akJurnalAsal.JumKredit;
                    var debitAsal = akJurnalAsal.JumDebit;
                    var logKredit = "";
                    var logDebit = "";

                    _context.Entry(akJurnalAsal).State = EntityState.Detached;

                    akJurnal.AkJurnal1 = _cart.Lines1.OrderBy(q => q.Indeks).ToList();
                    akJurnal.UserId = akJurnalAsal.UserId;
                    akJurnal.TarMasuk = akJurnalAsal.TarMasuk;
                    akJurnal.SuPekerjaMasukId = akJurnalAsal.SuPekerjaMasukId;
                    akJurnal.Cetak = 0;

                    if (akJurnal.AkTunaiRuncitId != 0)
                    {
                        akJurnal.FlJenisJurnal = 4;
                        akJurnal.FlKategoriPenerima = 3;
                        akJurnal.AkTunaiRuncitId = akJurnal.AkTunaiRuncitId;
                    }

                    akJurnal.UserIdKemaskini = user?.UserName ?? "";
                    akJurnal.TarKemaskini = DateTime.Now;
                    akJurnal.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akJurnal);
                    //insert applog
                    if (kreditAsal != akJurnal.JumKredit)
                    {
                        logKredit = "Kredit : RM " + kreditAsal.ToString() + " -> RM " + akJurnal.JumKredit;
                    }

                    if (debitAsal != akJurnal.JumDebit)
                    {
                        logDebit = "Debit : RM " + debitAsal.ToString() + " -> RM " + akJurnal.JumDebit;
                    }

                    if (logKredit != "" || logDebit != "")
                    {
                        _appLog.Insert("Ubah", "Ubah Data : " + logKredit + "," +logDebit, akJurnal.NoJurnal, id, akJurnal.JumKredit, pekerjaId,modul,syscode,namamodul,user);
                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", akJurnal.NoJurnal, id, akJurnal.JumKredit, pekerjaId,modul,syscode,namamodul,user);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Maklumat berjaya diubah.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkJurnalExists(akJurnal.Id))
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
            PopulateList();
            PopulateTableFromCart();
            return View(akJurnal);
        }

        // GET: AkJurnal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkJurnal == null)
            {
                return NotFound();
            }

            var akJurnal = await _akJurnalRepo.GetById((int)id);

            PopulateTable(id);
            if (akJurnal == null)
            {
                return NotFound();
            }

            return View(akJurnal);
        }

        // POST: AkJurnal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkJurnal == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkJurnal'  is null.");
            }
            var akJurnal = await _context.AkJurnal.FindAsync(id);
            if (akJurnal != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                akJurnal.UserIdKemaskini = user?.UserName ?? "";
                akJurnal.TarKemaskini = DateTime.Now;
                akJurnal.SuPekerjaKemaskiniId = pekerjaId;

                if (akJurnal.Posting == 1)
                {
                    TempData[SD.Error] = "Akses tidak dibenarkan..!";
                    return RedirectToAction(nameof(Index));
                }

                akJurnal.Cetak = 0;
                _context.AkJurnal.Update(akJurnal);
                _context.AkJurnal.Remove(akJurnal);
                _appLog.Insert("Hapus", "Hapus Data", akJurnal.NoJurnal, id, akJurnal.JumKredit, pekerjaId, modul, syscode, namamodul, user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool AkJurnalExists(int id)
        {
          return (_context.AkJurnal?.Any(e => e.Id == id)).GetValueOrDefault();
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
        public async Task<JsonResult> GetCarta(int JBahagianDebitId, int AkCartaDebitId, int JBahagianKreditId, int AkCartaKreditId)
        {
            try
            {
                var cartaDebit = await _context.AkCarta.FirstOrDefaultAsync(b => b.Id == AkCartaDebitId);

                var bahagianDebit = await _context.JBahagian.FirstOrDefaultAsync(b => b.Id == JBahagianDebitId);

                var cartaKredit = await _context.AkCarta.FirstOrDefaultAsync(b => b.Id == AkCartaKreditId);

                var bahagianKredit = await _context.JBahagian.FirstOrDefaultAsync(b => b.Id == JBahagianKreditId);

                return Json(new { result = "OK", cartaDebit = cartaDebit, bahagianDebit = bahagianDebit, cartaKredit = cartaKredit, bahagianKredit = bahagianKredit });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public JsonResult SaveAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal amaun = 0;
                var data = Json(new { });
                // if bahagian kredit, bahagian debit, kod akaun debit, kod akaun kredit is equal, return nothing
                foreach (var item in _cart.Lines1)
                {
                    if (item.AkCartaDebitId == akJurnal1.AkCartaDebitId && item.JBahagianDebitId == akJurnal1.JBahagianDebitId
                        && item.AkCartaKreditId == akJurnal1.AkCartaKreditId && item.JBahagianKreditId == akJurnal1.JBahagianKreditId)
                        return Json(new { result = "ERROR", message = "Carta dan bahagian berikut telah wujud." });
                }

                if (akJurnal1 != null)
                {
                    _cart.AddItem1(
                        akJurnal1.AkJurnalId,
                        akJurnal1.Indeks,
                        (int)akJurnal1.JBahagianDebitId!,
                        null,
                        (int)akJurnal1.AkCartaDebitId!,
                        null,
                        (int)akJurnal1.JBahagianKreditId!,
                        null,
                        (int)akJurnal1.AkCartaKreditId!,
                        null,
                        akJurnal1.Amaun
                        );
                }
                List<AkJurnal1> list = new();
                list = _cart.Lines1.ToList();
                foreach (AkJurnal1 l in list)
                {
                    amaun += l.Amaun;
                }
                data = Json(new { debit = amaun, kredit = amaun });
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal amaun = 0;
                var data = Json(new { });
                if (akJurnal1 != null)
                {
                    _cart.RemoveItem1((int)akJurnal1.JBahagianDebitId!, (int)akJurnal1.AkCartaDebitId!, (int)akJurnal1.JBahagianKreditId!, (int)akJurnal1.AkCartaKreditId!, akJurnal1.Indeks);
                }
                List<AkJurnal1> list = new();
                list = _cart.Lines1.ToList();
                foreach (AkJurnal1 l in list)
                {
                    amaun += l.Amaun;
                }
                data = Json(new { debit = amaun, kredit = amaun });
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal amaun = 0;
                var data = Json(new { });
                if (akJurnal1 != null && akJurnal1.Amaun != 0)
                {

                    akJurnal1.AkCartaDebit = await _context.AkCarta.FirstOrDefaultAsync(x => x.Id == akJurnal1.AkCartaDebitId);
                    akJurnal1.AkCartaKredit = await _context.AkCarta.FirstOrDefaultAsync(x => x.Id == akJurnal1.AkCartaKreditId);
                    await _akJurnal1Repo.Insert(akJurnal1);

                    AkJurnal akJurnal = await _akJurnalRepo.GetById(akJurnal1.AkJurnalId);

                    akJurnal.JumDebit += amaun;
                    akJurnal.JumKredit += amaun;

                    await _akJurnalRepo.Update(akJurnal);
                    await _context.SaveChangesAsync();
                }
                data = Json(new { debit = amaun, kredit = amaun });
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal amaun = 0;
                var data = Json(new { });
                if (akJurnal1 != null)
                {
                    var akJ1 = await _context.AkJurnal1.FirstOrDefaultAsync(
                        x => x.AkCartaDebitId == akJurnal1.AkCartaDebitId
                        && x.AkJurnalId == akJurnal1.AkJurnalId
                        && x.Id == akJurnal1.Id);
                    
                    if (akJ1 != null) _context.AkJurnal1.Remove(akJ1);

                    AkJurnal akJurnal = await _akJurnalRepo.GetById(akJurnal1.AkJurnalId);

                    akJurnal.JumDebit -= amaun;
                    akJurnal.JumKredit -= amaun;

                    await _akJurnalRepo.Update(akJurnal);
                    await _context.SaveChangesAsync();
                }
                data = Json(new { debit = amaun, kredit = amaun });
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> UpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                AkJurnal1 data = await _akJurnal1Repo.GetBy2Id(akJurnal1.AkJurnalId, akJurnal1.Id);
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                _cart.Clear1();

                AkJurnal1 akJ1 = await _akJurnal1Repo.GetById(akJurnal1.Id);
                akJ1.Amaun = akJurnal1.Amaun;
                _context.AkJurnal1.Update(akJ1);
                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAnItemCartAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                AkJurnal1 data = _cart.Lines1.Where(x => x.JBahagianDebitId == akJurnal1.JBahagianDebitId
                                                            && x.AkCartaDebitId == akJurnal1.AkCartaDebitId
                                                            && x.JBahagianKreditId == akJurnal1.JBahagianKreditId
                                                            && x.AkCartaKreditId == akJurnal1.AkCartaKreditId
                                                            && x.Indeks == akJurnal1.Indeks).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveCartAkJurnal1(AkJurnal1ViewModel akJurnal1)
        {
            try
            {
                var akJ1 = _cart.Lines1.Where(x => x.JBahagianDebitId == akJurnal1.JBahagianDebitId
                                                            && x.AkCartaDebitId == akJurnal1.AkCartaDebitId
                                                            && x.JBahagianKreditId == akJurnal1.JBahagianKreditId
                                                            && x.AkCartaKreditId == akJurnal1.AkCartaKreditId).FirstOrDefault();

                if (akJ1 != null)
                {
                    _cart.RemoveItem1(akJurnal1.JBahagianDebitId, akJurnal1.AkCartaDebitId, akJurnal1.JBahagianKreditId, akJurnal1.AkCartaKreditId, akJurnal1.IndeksLama);
                    _cart.AddItem1(
                        akJurnal1.AkJurnalId,
                        akJurnal1.IndeksBaru,
                        akJurnal1.JBahagianDebitId,
                        null,
                        akJurnal1.AkCartaDebitId,
                        null,
                        akJurnal1.JBahagianKreditId,
                        null,
                        akJurnal1.AkCartaKreditId,
                        null,
                        akJurnal1.Amaun
                        );
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> GetAllItemCartAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                List<AkJurnal1> data = _cart.Lines1.OrderBy(x => x.Indeks).ToList();
                foreach (AkJurnal1 item in data)
                {
                    item.AkCartaDebit = await _context.AkCarta.FirstOrDefaultAsync(x => x.Id == item.AkCartaDebitId);
                    item.AkCartaKredit = await _context.AkCarta.FirstOrDefaultAsync(x => x.Id == item.AkCartaKreditId);

                    item.JBahagianDebit =  await _context.JBahagian.FirstOrDefaultAsync(x => x.Id == item.JBahagianDebitId);
                    item.JBahagianKredit = await _context.JBahagian.FirstOrDefaultAsync(x => x.Id == item.JBahagianKreditId);
                }
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

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

                AkJurnal akJurnal = await _akJurnalRepo.GetById((int)id);

                if (akJurnal != null)
                {
                    if (akJurnal.Cetak == 0)
                    {
                        //duplicate id error
                        TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                        return RedirectToAction(nameof(Index));
                    }

                    var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == "JR/"+akJurnal.NoJurnal).FirstOrDefaultAsync();
                    if (akAkaun != null)
                    {
                        //duplicate id error
                        TempData[SD.Error] = "Data gagal diluluskan.";
                    }
                    else
                    {
                        //posting operation start here
                        //insert into akAkaun

                        if (akJurnal.AkJurnal1 != null)
                        {
                            foreach (AkJurnal1 akJurnal1 in akJurnal.AkJurnal1.OrderBy(x => x.Indeks))
                            {
                                AkAkaun akADebit = new AkAkaun();
                                akADebit.NoRujukan = "JR/" + akJurnal.NoJurnal;
                                akADebit.JKWId = akJurnal.JKWId;
                                akADebit.JBahagianId = akJurnal1.JBahagianDebitId;
                                akADebit.Tarikh = akJurnal.Tarikh;
                                akADebit.Tahun = akJurnal.Tarikh.ToString("yyyy");
                                akADebit.AkCartaId1 = (int)akJurnal1.AkCartaDebitId!;
                                akADebit.Debit = akJurnal1.Amaun;
                                akADebit.AkCartaId2 = akJurnal1.AkCartaKreditId;
                                akADebit.Kredit = 0;

                                await _akAkaunRepo.Insert(akADebit);

                                AkAkaun akAKredit = new AkAkaun();
                                akAKredit.NoRujukan = "JR/" + akJurnal.NoJurnal;
                                akAKredit.JKWId = akJurnal.JKWId;
                                akAKredit.JBahagianId = akJurnal1.JBahagianKreditId;
                                akAKredit.Tarikh = akJurnal.Tarikh;
                                akAKredit.Tahun = akJurnal.Tarikh.ToString("yyyy");
                                akAKredit.AkCartaId1 = (int)akJurnal1.AkCartaKreditId!;
                                akAKredit.Debit = 0;
                                akAKredit.AkCartaId2 = akJurnal1.AkCartaDebitId;
                                akAKredit.Kredit = akJurnal1.Amaun;

                                await _akAkaunRepo.Insert(akAKredit);

                            }

                            foreach (AkJurnal1 keVot in akJurnal.AkJurnal1.OrderBy(x => x.Indeks))
                            {
                                if (GetJenisObjek((int)keVot.AkCartaKreditId!) == "B")
                                {
                                    if (keVot.Amaun > 0)
                                    {
                                        AbBukuVot vot = new()
                                        {
                                            Rujukan = "JR/" + akJurnal.NoJurnal,
                                            JKWId = akJurnal.JKWId,
                                            JBahagianId = keVot.JBahagianKreditId,
                                            Tarikh = akJurnal.Tarikh,
                                            VotId = (int)keVot.AkCartaKreditId,
                                            Penerima = akJurnal.Catatan1.Substring(0, akJurnal.Catatan1.Length<200 ? akJurnal.Catatan1.Length : 200),
                                            Debit = keVot.Amaun,
                                            Kredit = keVot.Amaun,
                                            Tanggungan = 0 - keVot.Amaun,
                                            Tahun = akJurnal.Tarikh.Year.ToString()
                                        };
                                        await _abBukuVot.Insert(vot);
                                    }

                                }
                            }


                            // update AkTunaiLejar
                            // had maksimum at AkTunaiRuncit if FlJenisJurnal == 4

                            if (akJurnal.FlJenisJurnal == 4)
                            {
                                if (akJurnal.AkTunaiRuncit != null)
                                {
                                    akJurnal.AkTunaiRuncit.HadMaksimum = akJurnal.AkTunaiRuncit.HadMaksimum - akJurnal.JumDebit;

                                    await _akTunaiRuncitRepo.Update(akJurnal.AkTunaiRuncit);
                                }

                                //find latest baki
                                AkTunaiLejar akT = _context.AkTunaiLejar
                                .Where(x => x.AkTunaiRuncitId == akJurnal.AkTunaiRuncitId)
                                .OrderByDescending(x => x.NoRujukan)
                                .ThenByDescending(x => x.Tarikh)
                                .ThenByDescending(x => x.Id)
                                .FirstOrDefault();

                                decimal bakiAkhir = 0;

                                if (akT != null)
                                {
                                    bakiAkhir = akT.Baki;

                                    if (bakiAkhir < akJurnal.JumDebit)
                                    {
                                        TempData[SD.Warning] = "Baki akhir lejar tunai bagi kod kaunter panjar " + akJurnal.AkTunaiRuncit?.KaunterPanjar + " tidak mencukupi.";
                                        return RedirectToAction(nameof(Index));
                                    }
                                }
                                else
                                {
                                    TempData[SD.Error] = "Baki awal belum dimasukkan ke dalam lejar tunai bagi kod kaunter panjar " + akJurnal.AkTunaiRuncit?.KaunterPanjar + ". Anda perlu membuat baucer pembayaran terlebih dahulu.";
                                    return RedirectToAction(nameof(Index));
                                }

                                //insert into AkTunaiLejar
                                AkTunaiLejar akTunaiLejar = new AkTunaiLejar()
                                {
                                    JKWId = akJurnal.JKWId,
                                    AkTunaiRuncitId = (int)akJurnal.AkTunaiRuncitId!,
                                    Tarikh = akJurnal.Tarikh,
                                    AkCartaId = akJurnal.AkTunaiRuncit!.AkCartaId,
                                    NoRujukan = "JR/" +akJurnal.NoJurnal,
                                    Debit = 0,
                                    Kredit = akJurnal.JumDebit,
                                    Baki = bakiAkhir - akJurnal.JumDebit
                                };
                                // insert into AkTunaiLejar end

                                await _akTunaiLejarRepo.Insert(akTunaiLejar);


                            }
                            // update AkTunaiLejar end
                        }

                        //update posting status in akTerima
                        akJurnal.Posting = 1;
                        akJurnal.TarikhPosting = DateTime.Now;
                        await _akJurnalRepo.Update(akJurnal);
                        //insert applog
                        _appLog.Insert("Posting", "Posting Data", akJurnal.NoJurnal, (int)id, akJurnal.JumKredit, pekerjaId,modul,syscode,namamodul,user);
                        //insert applog end

                        await _context.SaveChangesAsync();
                        TempData[SD.Success] = "Data berjaya diluluskan.";
                    }
                }

            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnPosting(int? id,string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                AkJurnal akJurnal = await _akJurnalRepo.GetById((int)id);

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == "JR/" + akJurnal.NoJurnal).ToList();

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == "JR/"+akJurnal.NoJurnal).ToList();
                if (akAkaun == null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";
                }
                else
                {
                    //unposting operation start here
                    //delete data from akAkaun
                    foreach (AkAkaun item in akAkaun)
                    {
                        await _akAkaunRepo.Delete(item.Id);
                    }

                    foreach (AbBukuVot vot in abBukuVot)
                    {
                        await _abBukuVot.Delete(vot.Id);
                    }
                    // update had maksimum at AkTunaiRuncit if FlJenisJurnal == 4
                    decimal HadMaksimumDitambah = 0;
                    if (akJurnal.AkJurnal1 != null)
                    {
                        foreach (var akJ1 in akJurnal.AkJurnal1)
                        {
                            if (akJ1.Amaun > 0)
                            {
                                HadMaksimumDitambah = HadMaksimumDitambah + akJ1.Amaun;
                            }
                        }
                    }
                    

                    if (akJurnal.FlJenisJurnal == 4)
                    {
                        var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)akJurnal.AkTunaiRuncitId!);

                        akTunaiRuncit.HadMaksimum = akTunaiRuncit.HadMaksimum - HadMaksimumDitambah;

                        await _akTunaiRuncitRepo.Update(akTunaiRuncit);
                    }

                    // update end

                    //delete from tbl AkTunaiLejar
                    if (akJurnal.FlJenisJurnal == 4)
                    {
                        List<AkTunaiLejar> akTunaiLejar = _context.AkTunaiLejar.Where(x => x.NoRujukan == "JR/" + akJurnal.NoJurnal).ToList();

                        if (akTunaiLejar == null)
                        {
                            //duplicate id error
                            TempData[SD.Error] = "Data belum diluluskan.";
                            return RedirectToAction(nameof(Index));
                        }
                        foreach (AkTunaiLejar item in akTunaiLejar)
                        {
                            await _akTunaiLejarRepo.Delete(item.Id);
                        }

                    }

                    //delete from tbl AkTunaiLejar end

                    //update posting status in akTerima
                    akJurnal.Posting = 0;
                    akJurnal.TarikhPosting = null;
                    await _akJurnalRepo.Update(akJurnal);

                    //insert applog
                    _appLog.Insert("UnPosting", "Batal Posting Data", akJurnal.NoJurnal, (int)id, akJurnal.JumKredit, pekerjaId,modul, syscode, namamodul, user);
                    //insert applog end
                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan.";
                    //unposting operation end
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PrintPdf(int id, string syscode)
        {
            var userId = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == userId!.Id)!.SuPekerjaId;

            AkJurnal akJurnal = await _akJurnalRepo.GetByIdIncludeDeletedItems(id);

            JurnalPrintModel data = new JurnalPrintModel();
            var user = "";
            if (akJurnal.UserIdKemaskini == ""||akJurnal.UserIdKemaskini == null)
            {
                user = akJurnal.UserId;
            }
            else
            {
                user = akJurnal.UserIdKemaskini;
            }

            // populate table akJurnal1 based on user interface
            var ringkasan = new List<RingkasanPrintModel>();

            if (akJurnal.AkJurnal1 != null)
            {
                foreach (var item in akJurnal.AkJurnal1)
                {
                    var ringkasanDebit = new RingkasanPrintModel();
                    var bahagianDebit = _context.JBahagian.FirstOrDefault(x => x.Id == item.JBahagianDebitId);
                    var cartaDebit = _context.AkCarta.FirstOrDefault(x => x.Id == item.AkCartaDebitId);

                    ringkasanDebit = new RingkasanPrintModel
                    {
                        Bahagian = bahagianDebit?.Kod ?? "",
                        KodAkaun = cartaDebit?.Kod ?? "",
                        Perihal = cartaDebit?.Perihal ?? "",
                        DebitDecimal = item.Amaun,
                        KreditDecimal = 0
                    };

                    ringkasan.Add(ringkasanDebit);

                    var ringkasanKredit = new RingkasanPrintModel();
                    var bahagianKredit = _context.JBahagian.FirstOrDefault(x => x.Id == item.JBahagianKreditId);
                    var cartaKredit = _context.AkCarta.FirstOrDefault(x => x.Id == item.AkCartaKreditId);

                    ringkasanKredit = new RingkasanPrintModel
                    {
                        Bahagian = bahagianKredit?.Kod ?? "",
                        KodAkaun = cartaKredit?.Kod ?? "",
                        Perihal = cartaKredit?.Perihal ?? "",
                        DebitDecimal = 0,
                        KreditDecimal = item.Amaun
                    };

                    ringkasan.Add(ringkasanKredit);

                }
            }
            

            ringkasan = ringkasan.GroupBy(x => (x.Bahagian, x.KodAkaun))
                    .Select(s => new RingkasanPrintModel
                    {
                        Bahagian = s.First().Bahagian,
                        KodAkaun = s.First().KodAkaun,
                        Perihal = s.First().Perihal,
                        DebitDecimal = s.Sum(d => d.DebitDecimal),
                        KreditDecimal = s.Sum(k => k.KreditDecimal)
                    }).OrderBy(r => r.Bahagian).ThenBy(r => r.KodAkaun).ToList();

            data.Ringkasan = ringkasan;
            // populate table akJurnal1 based on user interface end

            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email!.ToUpper() == user.ToUpper());
            var jumDebitPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akJurnal.JumDebit)).ToUpper();
            var jumKreditPerkataan = ("Ringgit Malaysia " + CalculatePrice.JumlahDalamPerkataan(akJurnal.JumKredit)).ToUpper();
            CompanyDetails company = await _userService.GetCompanyDetails();

            data.Username = namaUser?.Nama ?? "";
            data.AkJurnal = akJurnal;
            data.CompanyDetail = company;
            data.JumlahDebitDalamPerkataan = jumDebitPerkataan;
            data.JumlahKreditDalamPerkataan = jumDebitPerkataan;

            //update cetak -> 1
            akJurnal.Cetak = 1;
            await _akJurnalRepo.Update(akJurnal);
            //insert applog
            _appLog.Insert("Cetak", "Cetak Data", akJurnal.NoJurnal, id, akJurnal.JumKredit, pekerjaId,modul,syscode,namamodul,userId);
            //insert applog end
            await _context.SaveChangesAsync();

            return new ViewAsPdf("JurnalPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "JU001R")]
        public async Task<IActionResult> RollBack(int id,string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _akJurnalRepo.GetByIdIncludeDeletedItems(id);

            // check if already posting redirect back
            if (obj.Posting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Batal operation

            obj.FlHapus = 0;
            obj.Cetak = 0;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;
            _context.AkJurnal.Update(obj);

            // Batal operation end

            //insert applog
            _appLog.Insert("Rollback", "Rolback Data", obj.NoJurnal, (int)id, obj.JumKredit, pekerjaId,modul,syscode,namamodul,user);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private string GetJenisObjek(int id)
        {
            var carta = _context.AkCarta.Include(x => x.JJenis).FirstOrDefault(x => x.Id == id);
            if (carta != null && carta.JJenis != null)
            {
               return carta.JJenis.Kod;
            }
            else
            {
                return "";
            }
        }
    }
}
