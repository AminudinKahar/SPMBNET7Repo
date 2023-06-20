﻿using System;
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
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness._Statics;
using Rotativa.AspNetCore;
using SPMBNET7.App.Pages.PrintModels._02_Akaun;

namespace SPMBNET7.App.Controller._02_Akaun
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AkTunaiRuncitController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "DF004";
        public const string namamodul = "Daftar P. Tunai Runcit";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly CustomIRepository<string, int> _customRepo;
        private CartTunaiRuncit _cart;

        public AkTunaiRuncitController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTunaiRuncit, int, string> akTunaiRuncitRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            CustomIRepository<string, int> customRepo,
            CartTunaiRuncit cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
            _akTunaiRuncitRepo = akTunaiRuncitRepository;
            _customRepo = customRepo;
            _cart = cart;
        }

        [Authorize(Policy = "DF004")]
        public async Task<IActionResult> Index()
        {
            var akTunaiRuncit = await _akTunaiRuncitRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                akTunaiRuncit = await _akTunaiRuncitRepo.GetAllIncludeDeletedItems();
            }

            List<AkTunaiRuncitViewModel> viewModel = new List<AkTunaiRuncitViewModel>();

            foreach (AkTunaiRuncit item in akTunaiRuncit)
            {
                decimal baki = await _customRepo.GetBalanceFromKaunterPanjar("BAKI AWAL", item.Id);

                viewModel.Add(new AkTunaiRuncitViewModel
                {
                    Id = item.Id,
                    KodKW = item.JKW?.Kod ?? "",
                    KodRujukan = item.KaunterPanjar,
                    KodAkaun = item.AkCarta?.Kod ?? "",
                    Perihal = item.AkCarta?.Perihal ?? "",
                    BakiLejarPanjar = baki,
                    FlHapus = item.FlHapus
                });
            }
            return View(viewModel);
        }

        // GET: AkTunaiRuncit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AkTunaiRuncit == null)
            {
                return NotFound();
            }

            var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);

            if (akTunaiRuncit == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);

            return View(akTunaiRuncit);
        }

        private void PopulateTableFromCart()
        {
            // table 1
            List<AkTunaiPemegang> tbl1 = new List<AkTunaiPemegang>();
            var cart1 = _cart.Lines1.ToList();

            if (cart1 != null && cart1.Count() > 0)
            {
                foreach (var item in cart1)
                {
                    tbl1.Add(item);
                }
            }
            ViewBag.akTunaiPemegang = tbl1;
            // table 1 end

        }
        // populate table from cart end

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Kod).ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas!.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

            List<SuPekerja> suPekerjaList = _context.SuPekerja
                .OrderBy(b => b.NoGaji).ToList();
            ViewBag.SuPekerja = suPekerjaList;


        }

        private void PopulateTable(int? id)
        {
            List<AkTunaiPemegang> akTunaiPemegangTable = _context.AkTunaiPemegang
                .Include(b => b.SuPekerja)
                .Where(b => b.AkTunaiRuncitId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTunaiPemegang = akTunaiPemegangTable;

            // baki awal
            List<AkTunaiLejar> tunaiLejar = _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit!.Id == id && b.Rekup == "BAKI AWAL")
                .OrderBy(b => b.Tarikh)
                .ToList();

            // rekupan
            List<AkTunaiLejar> tunaiLejarRekup = _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit!.Id == id && b.Rekup != "BAKI AWAL" && b.Rekup != null)
                .OrderBy(b => b.Rekup).ThenBy(b => b.Tarikh)
                .ToList();

            tunaiLejar.AddRange(tunaiLejarRekup);
            // belum rekup
            List<AkTunaiLejar> tunaiLejarBelumRekup = _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit!.Id == id && b.Rekup == null)
                .OrderBy(b => b.Tarikh)
                .ToList();

            tunaiLejar.AddRange(tunaiLejarBelumRekup);

            ViewBag.akTunaiLejar = tunaiLejar;

        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akTunaiPemegang = new List<int>();
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // GET: AkTunaiRuncit/Create
        [Authorize(Policy = "DF004C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault();

            var kumpulanWang = kw?.Kod ?? "100";
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.AkTunaiRuncit
                        .Where(x => x.JKW!.Kod == kumpulanWang)
                        .Max(x => x.KaunterPanjar);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;
            PopulateList();
            CartEmpty();
            return View();
        }

        // function json get no rujukan (running number)
        [HttpPost]
        public JsonResult JsonGetKod(int data)
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
                    var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

                    var kumpulanWang = kw?.Kod ?? "100";
                    string prefix = kumpulanWang;
                    int x = 1;
                    string noRujukan = prefix + "00";

                    var LatestNoRujukan = _context.AkTunaiRuncit
                                .Where(x => x.JKW!.Kod == kumpulanWang)
                                .Max(x => x.KaunterPanjar);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(3));
                        x++;
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }

                    result = noRujukan;

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

        //function get latest date rekup (noPV) in tunai lejar
        public async Task<JsonResult> GetLastDateRekupInTunaiLejar(int id)
        {
            try
            {
                // cari baucer yang tak direkup lagi paling latest
                var result = await _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit!.Id == id && b.Rekup == null && b.NoRujukan.Contains("PV"))
                .OrderBy(b => b.Tarikh)
                .FirstOrDefaultAsync();

                if (result == null)
                {
                    result = await _context.AkTunaiLejar
                                    .Include(b => b.AkTunaiRuncit)
                                    .Where(b => b.AkTunaiRuncit!.Id == id && b.Rekup == "BAKI AWAL")
                                    .OrderBy(b => b.Tarikh)
                                    .FirstOrDefaultAsync();

                    if (result == null)
                    {
                        return Json(new { result = "ERROR" });
                    }
                }

                var tarikh = result.Tarikh.ToString("yyyy-MM-dd");

                return Json(new { result = "OK", tarikh = tarikh, record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //function get latest date rekup (noPV) in tunai lejar end

        // get list of no rekup based on AkTunaiRuncitId
        public JsonResult GetListOfNoRekup(int id)
        {
            try
            {
                // cari baucer yang tak direkup lagi paling latest
                var result = (from tbl1 in _context.AkTunaiLejar
                            .Where(x => x.AkTunaiRuncitId == id && x.Rekup != "BAKI AWAL" && x.Rekup != null).ToList()
                              select new
                              {
                                  tbl1.Rekup
                              }).GroupBy(x => x.Rekup).Select(x => x.First());

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        // get list of no rekup based on AkTunaiRuncitId end

        public JsonResult GetSuPekerja(SuPekerja suPekerja)
        {
            try
            {
                var result = _context.SuPekerja.Where(b => b.Id == suPekerja.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public JsonResult SaveAkTunaiPemegang(AkTunaiPemegang akTunaiPemegang)
        {

            try
            {
                if (akTunaiPemegang != null)
                {
                    _cart.AddItem1(akTunaiPemegang.AkTunaiRuncitId,
                                   akTunaiPemegang.SuPekerjaId);
                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTunaiPemegang(AkTunaiPemegang akTunaiPemegang)
        {

            try
            {
                if (akTunaiPemegang != null)
                {

                    _cart.RemoveItem1(akTunaiPemegang.SuPekerjaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkTunaiRuncit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkTunaiRuncit akTunaiRuncit, decimal Baki, DateTime? TarikhBaki, string syscode)
        {
            AkTunaiRuncit m = new AkTunaiRuncit();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == akTunaiRuncit.JKWId);

            var kumpulanWang = kw?.Kod ?? "100";
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.AkTunaiRuncit
                        .Where(x => x.JKW!.Kod == kumpulanWang)
                        .Max(x => x.KaunterPanjar);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end
            if (ModelState.IsValid)
            {
                m.JKWId = akTunaiRuncit.JKWId;
                m.AkCartaId = akTunaiRuncit.AkCartaId;
                m.KaunterPanjar = noRujukan;
                m.Catatan = akTunaiRuncit.Catatan;
                m.UserId = user?.UserName ?? "";
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkTunaiPemegang = _cart.Lines1.ToArray();

                await _akTunaiRuncitRepo.Insert(m);

                //insert applog
                _appLog.Insert("Tambah", m.KaunterPanjar, m.KaunterPanjar, 0, 0, pekerjaId,modul,syscode,namamodul,user);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan kaunter panjar adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }
            PopulateList();
            PopulateTableFromCart();
            return View(akTunaiRuncit);
        }

        // GET: AkTunaiRuncit/Edit/5
        [Authorize(Policy = "DF004E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AkTunaiRuncit == null)
            {
                return NotFound();
            }

            var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);
            if (akTunaiRuncit == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akTunaiRuncit);
            return View(akTunaiRuncit);
        }

        private void PopulateCartFromDb(AkTunaiRuncit akTunaiRuncit)
        {
            List<AkTunaiPemegang> akTunaiPemegangTable = _context.AkTunaiPemegang
                .Include(b => b.SuPekerja)
                .Where(b => b.AkTunaiRuncitId == akTunaiRuncit.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkTunaiPemegang akTunaiPemegang in akTunaiPemegangTable)
            {
                _cart.AddItem1(akTunaiPemegang.AkTunaiRuncitId,
                               akTunaiPemegang.SuPekerjaId);
            }

            ViewBag.akTunaiPemegang = akTunaiPemegangTable;

        }

        // POST: AkTunaiRuncit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkTunaiRuncit akTunaiRuncit, decimal Baki, DateTime? TarikhBaki, string syscode)
        {
            if (id != akTunaiRuncit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                    AkTunaiRuncit akTunaiRuncitAsal = await _akTunaiRuncitRepo.GetById(id);

                    // list of input that cannot be change
                    akTunaiRuncit.JKWId = akTunaiRuncitAsal.JKWId;
                    akTunaiRuncit.KaunterPanjar = akTunaiRuncitAsal.KaunterPanjar;
                    akTunaiRuncit.HadMaksimum = akTunaiRuncitAsal.HadMaksimum;
                    akTunaiRuncit.AkCartaId = akTunaiRuncitAsal.AkCartaId;
                    akTunaiRuncit.TarMasuk = akTunaiRuncitAsal.TarMasuk;
                    akTunaiRuncit.UserId = akTunaiRuncitAsal.UserId;
                    // list of input that cannot be change end

                    if (akTunaiRuncitAsal.AkTunaiPemegang != null)
                    {
                        foreach (AkTunaiPemegang item in akTunaiRuncitAsal.AkTunaiPemegang)
                        {
                            var model = _context.AkTunaiPemegang.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                    }
                    
                    _context.Entry(akTunaiRuncitAsal).State = EntityState.Detached;

                    akTunaiRuncit.AkTunaiPemegang = _cart.Lines1.ToList();

                    akTunaiRuncit.UserIdKemaskini = user?.UserName ?? "";
                    akTunaiRuncit.TarKemaskini = DateTime.Now;
                    akTunaiRuncit.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akTunaiRuncit);

                    //insert applog
                    _appLog.Insert("Ubah", "Ubah Data", akTunaiRuncit.KaunterPanjar, id, 0, pekerjaId, modul, syscode, namamodul,user);
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTunaiRuncitExists(akTunaiRuncit.Id))
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
            TempData[SD.Warning] = "Data tidak lengkap. Sila cuba sekali lagi.";
            PopulateList();
            PopulateTableFromCart();
            return View(akTunaiRuncit);
        }

        // GET: AkTunaiRuncit/Delete/5
        [Authorize(Policy = "DF004D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AkTunaiRuncit == null)
            {
                return NotFound();
            }

            var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);
            if (akTunaiRuncit == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akTunaiRuncit);
        }

        // POST: AkTunaiRuncit/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "DF004D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.AkTunaiRuncit == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AkTunaiRuncit'  is null.");
            }

            // check if already made a transaction in tunaiCV
            var akTunaiCV = await _context.AkTunaiCV.Where(b => b.AkTunaiRuncitId == id).FirstOrDefaultAsync();

            if (akTunaiCV != null)
            {
                TempData[SD.Error] = "Data terkait dengan No CV " +akTunaiCV.NoCV+ ". Data gagal dihapuskan..!";
                return RedirectToAction(nameof(Index));
            }
            // check end

            // check if already made a transaction in akPV
            var akPV = await _context.AkPV.Where(b => b.AkTunaiRuncitId == id).FirstOrDefaultAsync();

            if (akPV != null)
            {
                TempData[SD.Error] = "Data terkait dengan No Baucer " + akPV.NoPV + ". Data gagal dihapuskan..!";
                return RedirectToAction(nameof(Index));
            }
            // check end

            var akTunaiRuncit = await _context.AkTunaiRuncit.FindAsync(id);
            if (akTunaiRuncit != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                akTunaiRuncit.UserIdKemaskini = user?.UserName ?? "";
                akTunaiRuncit.TarKemaskini = DateTime.Now;
                akTunaiRuncit.SuPekerjaKemaskiniId = pekerjaId;

                _context.AkTunaiRuncit.Remove(akTunaiRuncit);

                //insert applog
                _appLog.Insert("Hapus", "Hapus Data", akTunaiRuncit.KaunterPanjar, id, 0, pekerjaId, modul, syscode, namamodul, user);
                //insert applog end


                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "DF004R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _akTunaiRuncitRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // rollback operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkTunaiRuncit.Update(obj);

            // rollback operation end

            //insert applog
            _appLog.Insert("Posting", "Posting Data", obj.KaunterPanjar, id, 0, pekerjaId, modul, syscode, namamodul, user);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        // rekup function
        [Authorize(Policy = "DF001T")]
        public async Task<IActionResult> Rekup(int? id, string tarikhDari, string tarikhHingga, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;
                var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);
                DateTime date1 = DateTime.Parse(tarikhDari);
                DateTime date2 = DateTime.Parse(tarikhHingga).AddHours(23.99);

                // check if date 2 less than date 1
                if (date2 < date1)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Tarikh Hingga tidak boleh kurang dari Tarikh Dari.";
                    return RedirectToAction(nameof(Index));
                }
                // check end
                // step:
                //1. cari latest no rekup.
                //2. define running number untuk no rekup.
                //3. ambil latest no baucer dan list of tunai keluar yang tiada no rekup(range tarikhDari -> tarikhHingga) ikut input user.
                //4. update latest no rekup untuk list of (3) ikut running number (2)

                // 1
                // cari latest no rekup
                var LatestTunaiLejarRekup = _context.AkTunaiLejar
                    .Include(b => b.AkTunaiRuncit)
                    .Where(b => b.AkTunaiRuncit!.Id == id && b.Rekup != null)
                    .OrderByDescending(b => b.Rekup).ThenByDescending(b => b.Tarikh)
                    .FirstOrDefault();

                // 2
                // define running number 
                var year = DateTime.Now.ToString("yyyy");
                string prefix = year + "/";
                int x = 1;
                string noRekup = prefix + "0000";

                // kalau tiada
                if (LatestTunaiLejarRekup == null)
                {
                    // cari baki awal (sebab tak pernah buat rekupan lagi)
                    //LatestTunaiLejarRekup = await _context.AkTunaiLejar
                    //    .Include(b => b.AkTunaiRuncit)
                    //    .Where(b => b.AkTunaiRuncit.Id == id && b.Rekup == "BAKI AWAL")
                    //    .OrderByDescending(b => b.Rekup).ThenByDescending(b => b.Tarikh)
                    //    .FirstOrDefaultAsync();

                    noRekup = string.Format("{0:" + prefix + "0000}", x);
                }
                else
                {
                    x = int.Parse(LatestTunaiLejarRekup.Rekup.Substring(5));
                    x++;
                    noRekup = string.Format("{0:" + prefix + "0000}", x);
                }
                // 1 & 2 end


                List<AkTunaiLejar> tunaiLejarBelumRekup = await _context.AkTunaiLejar
                    .Include(b => b.AkTunaiRuncit)
                    .Where(b => b.AkTunaiRuncit!.Id == id && b.Rekup == null &&
                    b.Tarikh >= date1 && b.Tarikh <= date2)
                    .OrderBy(b => b.Tarikh)
                    .ToListAsync();

                if (tunaiLejarBelumRekup.Count == 0)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Tiada tunai keluar untuk direkup.";

                }
                else
                {
                    decimal jumlahRekupan = 0;
                    //unposting operation start here
                    //delete data from akTunaiLejar
                    foreach (AkTunaiLejar item in tunaiLejarBelumRekup)
                    {
                        jumlahRekupan = jumlahRekupan + item.Kredit;
                        //var tunaiLejar = await _context.AkTunaiLejar.Where(b => b.Id == item.Id).FirstOrDefaultAsync();
                        item.Rekup = noRekup;
                        _context.Update(item);
                    }

                    //update posting status in akTunaiCV
                    akTunaiRuncit.UserIdKemaskini = user?.UserName ?? "";
                    akTunaiRuncit.TarKemaskini = DateTime.Now;
                    akTunaiRuncit.SuPekerjaKemaskiniId = pekerjaId;

                    await _akTunaiRuncitRepo.Update(akTunaiRuncit);

                    //insert applog
                    _appLog.Insert("Rekup", "Rekup Data", akTunaiRuncit.KaunterPanjar + " - No Rekup : " + noRekup, (int)id, jumlahRekupan, pekerjaId,modul,syscode,namamodul,user);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Rekupan berjaya. No Rekup yang berdaftar adalah " + noRekup;
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // rekup function end

        // printing Rekupan Tunai Runcit
        [Authorize(Policy = "DF001P")]
        public async Task<IActionResult> PrintPdf(int id, string kodKaunter, string rekup, string syscode)
        {
            if (rekup == null)
            {
                TempData[SD.Error] = "Tiada pilihan rekupan";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var userManager = _userManager.GetUserAsync(User);

                var user = _context.applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.Email == userManager.Result!.Email);

                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                var rekupanList = (from tblTunaiLejar in _context.AkTunaiLejar
                                       .Include(x => x.AkTunaiRuncit)
                                       .Where(x => x.AkTunaiRuncitId == id && x.Rekup == rekup).ToList()
                                   join tblTunaiCV in _context.AkTunaiCV
                                       .Include(x => x.AkTunaiCV1).ThenInclude(x => x.AkCarta).ToList()
                                   on tblTunaiLejar.NoRujukan equals tblTunaiCV.NoCV into tblTunaiLejarTblTunaiCV
                                   from tbl_1 in tblTunaiLejarTblTunaiCV.DefaultIfEmpty()
                                   select new
                                   {
                                       Tarikh = tblTunaiLejar.Tarikh,
                                       Butiran = tbl_1?.Penerima ?? string.Empty,
                                       NoRujukan = tblTunaiLejar.NoRujukan,
                                       Debit = tblTunaiLejar.Debit,
                                       Kredit = tblTunaiLejar.Kredit,
                                       Baki = tblTunaiLejar.Baki
                                   }).OrderBy(x => x.Tarikh).ToList();


                RekupTunaiRuncitPrintModel data = new RekupTunaiRuncitPrintModel();

                List<Rekupan> rekupans = new List<Rekupan>();


                foreach (var item in rekupanList)
                {
                    rekupans.Add(
                        new Rekupan
                        {
                            Tarikh = item.Tarikh,
                            Butiran = item.Butiran,
                            NoRujukan = item.NoRujukan,
                            Debit = item.Debit,
                            Kredit = item.Kredit,
                            Baki = item.Baki
                        }
                        );
                }

                data.RekupanList = rekupans;

                CompanyDetails company = new CompanyDetails();
                data.CompanyDetail = company;
                if (User.IsInRole("SuperAdmin"))
                {
                    data.Penyedia = user?.UserName ?? "";
                }
                else
                {
                    data.Penyedia = user?.SuPekerja?.Nama ?? "";
                }
                data.NoRekup = rekup;

                string customSwitches = string.Format(" --header-html  \"{0}\" " +
                                       "--header-spacing \"-12\" " +
                                       "--header-font-size \"10\" " +
                                       "--footer-center \"[page]/[toPage]\" " +
                                       "--footer-font-size \"7\" --footer-spacing 1",
                                       Url.Action("Header", "AkTunaiRuncit",
                                       new
                                       {
                                           NoRekup = rekup,
                                           KodKaunter = kodKaunter
                                       },
                                       "https"));

                //insert applog
                _appLog.Insert("Cetak", "Cetak Rekupan", "Kod Kaunter Panjar : " + kodKaunter + ", No Rekup : " + rekup, id, 0, pekerjaId,modul,syscode,namamodul,user);

                //insert applog end
                await _context.SaveChangesAsync();

                //return View("TunaiRuncitPrintPdf");
                return new ViewAsPdf("TunaiRuncitPrintPdf", data)
                {
                    PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                    CustomSwitches = customSwitches,
                    //CustomSwitches = "--footer-center \"  Tarikh: " +
                    //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                    //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                };
            }

        }
        // printing Rekupan Tunai Runcit end

        [AllowAnonymous]
        public ActionResult Header(RekupTunaiRuncitPrintModel reportModel)
        {
            return View(reportModel);
        }
        private bool AkTunaiRuncitExists(int id)
        {
          return (_context.AkTunaiRuncit?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
