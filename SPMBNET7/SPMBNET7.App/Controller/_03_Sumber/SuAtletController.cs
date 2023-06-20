using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.App.Data;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Carts._02_Akaun;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SPMBNET7.CoreBusiness._Statics;

namespace SPMBNET7.App.Controller._03_Sumber
{
    [Authorize(Roles = Init.allExceptAdminRole)]
    public class SuAtletController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "DF005";
        public const string namamodul = "Daftar Atlet";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<SuAtlet, int, string> _suAtletRepo;
        private readonly IRepository<JNegeri, int, string> _jNegeriRepo;
        private readonly IRepository<JAgama, int, string> _jAgamaRepo;
        private readonly IRepository<JBangsa, int, string> _jBangsaRepo;
        private readonly IRepository<JSukan, int, string> _jSukanRepo;
        private readonly IRepository<JCaraBayar, int, string> _jCaraBayarRepo;
        private CartAtlet _cart;

        public SuAtletController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<SuAtlet, int, string> suAtletRepo,
            IRepository<JNegeri, int, string> jNegeriRepo,
            IRepository<JAgama, int, string> jAgamaRepo,
            IRepository<JBangsa, int, string> jBangsaRepo,
            IRepository<JSukan, int, string> jSukanRepo,
            IRepository<JCaraBayar, int, string> jCaraBayarRepo,
            CartAtlet cart

            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _suAtletRepo = suAtletRepo;
            _jNegeriRepo = jNegeriRepo;
            _jAgamaRepo = jAgamaRepo;
            _jBangsaRepo = jBangsaRepo;
            _jSukanRepo = jSukanRepo;
            _jCaraBayarRepo = jCaraBayarRepo;
            _cart = cart;

        }

        private void PopulateList()
        {
            List<JNegeri> JNegeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = JNegeriList;

            List<JBank> JBankList = _context.JBank.OrderBy(b => b.Kod).ToList();
            ViewBag.JBank = JBankList;

            List<JAgama> JAgamaList = _context.JAgama.OrderBy(b => b.Perihal).ToList();
            ViewBag.JAgama = JAgamaList;

            List<JBangsa> JBangsaList = _context.JBangsa.OrderBy(b => b.Perihal).ToList();
            ViewBag.JBangsa = JBangsaList;

            List<JSukan> JSukanList = _context.JSukan.OrderBy(b => b.Kod).ToList();
            ViewBag.JSukan = JSukanList;

            List<JCaraBayar> JCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = JCaraBayarList;
        }

        private string GetKodAtlet()
        {
            var suP = _suAtletRepo.GetAllIncludeDeletedItems()
                .Result
                .OrderByDescending(s => s.KodAtlet).FirstOrDefault();
            int no = 0;
            if (suP != null)
            {
                if (int.TryParse(suP.KodAtlet, out no))
                {
                    no += 1;
                }
            }
            else
            {
                no = 1;
            }
            return no.ToString("D5");
        }

        //Function Cart Empty
        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.suAtlet1 = new List<int>();
                //ViewBag.spPendahuluanPelbagai2 = new List<int>();
                _cart.Clear1();
                //_cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //Function Cart Empty end

        // GET: SuAtlet
        [Authorize(Policy = "DF005")]
        public async Task<IActionResult> Index()
        {
            var suAtlet = await _suAtletRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                suAtlet = await _suAtletRepo.GetAllIncludeDeletedItems();
            }
            return View(suAtlet);
        }

        // GET: SuAtlet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SuAtlet == null)
            {
                return NotFound();
            }

            var suAtlet = await _suAtletRepo.GetById((int)id);
            if (suAtlet == null)
            {
                return NotFound();
            }

            PopulateList();
            return View(suAtlet);
        }

        // GET: SuAtlet/Create
        [Authorize(Policy = "DF005C")]
        public IActionResult Create()
        {
            ViewBag.KodAtlet = GetKodAtlet();
            PopulateList();
            return View();
        }

        // POST: SuAtlet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "DF005C")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SuAtlet suAtlet, string syscode)
        {

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            SuAtlet m = new SuAtlet();
            if (ICAtletExists(suAtlet.NoKp) == false)
            {
                if (AkaunAtletExists(suAtlet.NoAkaunBank) == false)
                {
                    if (ModelState.IsValid)
                    {
                        //string noRujukan = GetKod(akJurnal.JKWId);
                        if (suAtlet != null)
                        {
                            m.KodAtlet = GetKodAtlet();
                            m.Nama = suAtlet.Nama?.ToUpper() ?? "";
                            m.NoKp = suAtlet.NoKp;
                            m.Alamat1 = suAtlet.Alamat1?.ToUpper() ?? "";
                            m.Alamat2 = suAtlet.Alamat2?.ToUpper() ?? "";
                            m.Alamat3 = suAtlet.Alamat3?.ToUpper() ?? "";
                            m.Poskod = suAtlet.Poskod;
                            m.Bandar = suAtlet.Bandar?.ToUpper() ?? "";
                            m.JNegeriId = suAtlet.JNegeriId;
                            m.JBankId = suAtlet.JBankId;
                            m.Jawatan = suAtlet.Jawatan?.ToUpper() ?? "";
                            m.JSukanId = suAtlet.JSukanId;
                            m.Telefon = suAtlet.Telefon;
                            m.Emel = suAtlet.Emel;
                            m.FlStatus = 1;
                            m.TarikhAktif = suAtlet.TarikhAktif;
                            m.TarikhBerhenti = suAtlet.TarikhBerhenti;
                            //m.FlStatus = suAtlet.FlStatus;
                            m.JAgamaId = suAtlet.JAgamaId;
                            m.JBangsaId = suAtlet.JBangsaId;
                            m.JCaraBayarId = suAtlet.JCaraBayarId;
                            m.NoAkaunBank = suAtlet.NoAkaunBank;
                            m.UserId = user?.UserName ?? "";
                            m.TarMasuk = DateTime.Now;
                            m.SuPekerjaMasukId = pekerjaId;

                            //m.SuTanggungan = _cart.Lines1.ToArray();

                            await _suAtletRepo.Insert(m);

                            //insert applog
                            _appLog.Insert("Tambah", m.KodAtlet + " - " + suAtlet.NoKp, m.KodAtlet, 0, 0, pekerjaId, modul, syscode, namamodul, user);
                            //insert applog end

                            await _context.SaveChangesAsync();

                            TempData[SD.Success] = "Maklumat berjaya ditambah. Kod Atlet adalah " + m.KodAtlet;
                            return RedirectToAction(nameof(Index));
                        }

                    }
                    else
                    {
                        TempData[SD.Error] = "Emel ini telah wujud..!";
                    }

                }
                else
                {
                    TempData[SD.Error] = "No Akaun ini telah wujud..!";
                }

            }
            else
            {
                TempData[SD.Error] = "No Kad Pengenalan ini telah wujud..!";
            }

            ViewBag.KodAtlet = GetKodAtlet();
            PopulateList();
            return View(suAtlet);
        }

        // GET: SuAtlet/Edit/5
        [Authorize(Policy = "DF005E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SuAtlet == null)
            {
                return NotFound();
            }

            var suAtlet = await _suAtletRepo.GetById((int)id);
            if (suAtlet == null)
            {
                return NotFound();
            }

            PopulateList();
            return View(suAtlet);
        }

        // POST: SuAtlet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "DF005E")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SuAtlet suAtlet, string syscode)
        {
            if (id != suAtlet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                    SuAtlet dataAsal = await _suAtletRepo.GetById(id);

                    // list of input that cannot be change
                    //suAtlet.Emel = dataAsal.Emel
                    suAtlet.TarMasuk = dataAsal.TarMasuk;
                    suAtlet.UserId = dataAsal.UserId;
                    suAtlet.NoKp = dataAsal.NoKp;
                    suAtlet.KodAtlet = dataAsal.KodAtlet;
                    suAtlet.FlStatus = dataAsal.FlStatus;
                    var noAkaunAsal = dataAsal.NoAkaunBank;
                    var namaAsal = dataAsal.Nama;
                    suAtlet.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    suAtlet.UserIdKemaskini = user?.UserName ?? "";
                    suAtlet.TarKemaskini = DateTime.Now;
                    suAtlet.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(suAtlet);

                    //insert applog
                    if (namaAsal != suAtlet.Nama || noAkaunAsal != suAtlet.NoAkaunBank)
                    {
                        _appLog.Insert("Ubah", namaAsal + " -> " + suAtlet.Nama
                            + ", " + noAkaunAsal + " -> " + suAtlet.NoAkaunBank, suAtlet.KodAtlet, id, 0, pekerjaId,modul, syscode, namamodul,user);
                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", suAtlet.KodAtlet, id, 0, pekerjaId,modul, syscode, namamodul, user);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuAtletExists(suAtlet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData[SD.Success] = "Data berjaya diubah..!";
                return RedirectToAction(nameof(Index));
            }
            PopulateList();
            return View(suAtlet);
        }

        // GET: SuAtlet/Delete/5
        [Authorize(Policy = "DF005D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SuAtlet == null)
            {
                return NotFound();
            }

            var suAtlet = await _suAtletRepo.GetById((int)id);
            //PopulateTable(id);
            if (suAtlet == null)
            {
                return NotFound();
            }

            return View(suAtlet);
        }

        // POST: SuAtlet/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "DF005D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.SuAtlet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SuAtlet'  is null.");
            }
            var suAtlet = await _context.SuAtlet.FindAsync(id);
            if (suAtlet != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

                suAtlet.UserIdKemaskini = user?.UserName ?? "";
                suAtlet.TarKemaskini = DateTime.Now;
                suAtlet.SuPekerjaKemaskiniId = pekerjaId;

                _context.SuAtlet.Remove(suAtlet);
                _appLog.Insert("Hapus", suAtlet.NoKp + " - " + suAtlet.NoAkaunBank, suAtlet.KodAtlet, id, 0, pekerjaId, modul, syscode, namamodul, user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool SuAtletExists(int id)
        {
          return (_context.SuAtlet?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Policy = "DF005R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.FirstOrDefault(b => b.Id == user!.Id)!.SuPekerjaId;

            var obj = await _suAtletRepo.GetByIdIncludeDeletedItems(id);
            // Batal operation

            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            obj.FlHapus = 0;
            _context.SuAtlet.Update(obj);

            // Batal operation end

            _appLog.Insert("Hapus", obj.NoKp + " - " + obj.NoAkaunBank, obj.KodAtlet, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool ICAtletExists(string kod)
        {
            return _context.SuAtlet.Any(e => e.NoKp == kod && e.FlHapus == 0);
        }

        private bool AkaunAtletExists(string kod)
        {
            return _context.SuAtlet.Any(e => e.NoAkaunBank == kod && e.FlHapus == 0);
        }

        private bool EmelAtletExists(string kod)
        {
            return _context.SuAtlet.Any(e => e.Emel == kod && e.FlHapus == 0);
        }
    }
}
