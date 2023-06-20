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
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.Infrastructure.Carts._03_Sumber;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using System.Security.Claims;
using SPMBNET7.CoreBusiness._Statics;

namespace SPMBNET7.Sumber.Controller._03_Sumber
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class SuPekerjaController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "DF001";
        public const string namamodul = "Daftar Anggota";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly IRepository<JNegeri, int, string> _jNegeriRepo;
        private readonly IRepository<JAgama, int, string> _jAgamaRepo;
        private readonly IRepository<JBangsa, int, string> _jBangsaRepo;
        private readonly ListViewIRepository<SuTanggunganPekerja, int> _suTanggunganRepo;
        private readonly IRepository<JCaraBayar, int, string> _jCaraBayarRepo;
        private CartPekerja _cart;

        public SuPekerjaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<SuPekerja, int, string> suPekerjaRepo,
            IRepository<JNegeri, int, string> jNegeriRepo,
            IRepository<JAgama, int, string> jAgamaRepo,
            IRepository<JBangsa, int, string> jBangsaRepo,
            ListViewIRepository<SuTanggunganPekerja, int> suTanggunganRepo,
            IRepository<JCaraBayar, int, string> jCaraBayarRepo,
            CartPekerja cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _suPekerjaRepo = suPekerjaRepo;
            _jNegeriRepo = jNegeriRepo;
            _jAgamaRepo = jAgamaRepo;
            _jBangsaRepo = jBangsaRepo;
            _suTanggunganRepo = suTanggunganRepo;
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

            List<JCaraBayar> JCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = JCaraBayarList;

        }

        private string GetNoGaji()
        {
            var suP = _suPekerjaRepo.GetAllIncludeDeletedItems()
                .Result
                .OrderByDescending(s => s.NoGaji).FirstOrDefault();
            int no = 0;
            if (suP != null)
            {
                if (int.TryParse(suP.NoGaji, out no))
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

        private void PopulateTable(int? id)
        {
            List<SuTanggunganPekerja> suTanggungan = _context.SuTanggunganPekerja.Where(b => b.SuPekerjaId == id).ToList();
            ViewBag.suTanggungan = suTanggungan;
        }

        private void PopulateTableFromCart()
        {
            List<SuTanggunganPekerja> suTanggungan = _cart.Lines1.ToList();
            ViewBag.suTanggungan = suTanggungan;
        }

        private JsonResult CartEmpty()
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
        private void PopulateCart(SuPekerja suPekerja)
        {
            List<SuTanggunganPekerja> suTanggungan = _context.SuTanggunganPekerja
                .Where(b => b.SuPekerjaId == suPekerja.Id)
                .ToList();
            foreach (SuTanggunganPekerja suT in suTanggungan)
            {
                _cart.AddItem1(
                    suT.SuPekerjaId,
                    suT.Nama,
                    suT.Hubungan,
                    suT.NoKP
                    );
            }
        }

        [Authorize(Policy = "DF001")]
        // GET: SuPekerja
        public async Task<IActionResult> Index()
        {
            var suPekerja = await _suPekerjaRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                suPekerja = await _suPekerjaRepo.GetAllIncludeDeletedItems();
            }
            return View(suPekerja);
        }

        // GET: SuPekerja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SuPekerja == null)
            {
                return NotFound();
            }

            var suPekerja = await _suPekerjaRepo.GetById((int)id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);
            return View(suPekerja);
        }

        [Authorize(Policy = "DF001C")]
        // GET: SuPekerja/Create
        public IActionResult Create()
        {
            ViewBag.nogaji = GetNoGaji();
            PopulateList();
            CartEmpty();
            return View();
        }

        [Authorize(Policy = "DF001C")]
        // POST: SuPekerja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SuPekerja suPekerja, string syscode)
        {
            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            SuPekerja m = new SuPekerja();
            if (ICPekerjaExists(suPekerja.NoKp) == false)
            {
                if (AkaunPekerjaExists(suPekerja.NoAkaunBank) == false)
                {
                    if (EmelPekerjaExists(suPekerja.Emel) == false)
                    {
                        if (suPekerja.Emel != null)
                        {
                            if (ModelState.IsValid)
                            {
                                //string noRujukan = GetKod(akJurnal.JKWId);
                                if (suPekerja != null)
                                {
                                    m.NoGaji = GetNoGaji();
                                    m.Nama = suPekerja.Nama?.ToUpper()?? "";
                                    m.NoKp = suPekerja.NoKp;
                                    m.Alamat1 = suPekerja.Alamat1?.ToUpper()?? "";
                                    m.Alamat2 = suPekerja.Alamat2?.ToUpper()?? "";
                                    m.Alamat3 = suPekerja.Alamat3?.ToUpper()?? "";
                                    m.Poskod = suPekerja.Poskod;
                                    m.Bandar = suPekerja.Bandar?.ToUpper()?? "";
                                    m.JNegeriId = suPekerja.JNegeriId;
                                    m.JBankId = suPekerja.JBankId;
                                    m.Jawatan = suPekerja.Jawatan?.ToUpper()?? "";
                                    m.Emel = suPekerja.Emel;
                                    m.NoAkaunBank = suPekerja.NoAkaunBank;
                                    m.UserId = username;
                                    m.TarMasuk = DateTime.Now;
                                    m.SuPekerjaMasukId = pekerjaId;

                                    m.SuTanggungan = _cart.Lines1.ToArray();

                                    await _suPekerjaRepo.Insert(m);

                                    //insert applog
                                    _appLog.Insert("Tambah", m.NoGaji + " - " + suPekerja.NoKp, m.NoGaji, 0, 0, pekerjaId,modul,syscode,namamodul,user);
                                    //insert applog end

                                    await _context.SaveChangesAsync();

                                    CartEmpty();
                                    TempData[SD.Success] = "Maklumat berjaya ditambah. No Gaji yang didaftar adalah " + m.NoGaji;
                                    return RedirectToAction(nameof(Index));
                                }
                            }
                        }
                        else
                        {
                            TempData[SD.Error] = "Sila isi ruangan Emel..!";
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

            ViewBag.nogaji = GetNoGaji();
            PopulateList();
            return View(suPekerja);
        }

        [Authorize(Policy = "DF001E")]
        // GET: SuPekerja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SuPekerja == null)
            {
                return NotFound();
            }

            var suPekerja = await _suPekerjaRepo.GetById((int)id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(suPekerja);
            return View(suPekerja);
        }

        [Authorize(Policy = "DF001E")]
        // POST: SuPekerja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SuPekerja suPekerja, string syscode)
        {
            if (id != suPekerja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    SuPekerja dataAsal = await _suPekerjaRepo.GetById(id);

                    // list of input that cannot be change
                    suPekerja.Emel = dataAsal.Emel;
                    suPekerja.TarMasuk = dataAsal.TarMasuk;
                    suPekerja.UserId = dataAsal.UserId;
                    suPekerja.NoKp = dataAsal.NoKp;
                    suPekerja.NoGaji = dataAsal.NoGaji;
                    var noAkaunAsal = dataAsal.NoAkaunBank;
                    var namaAsal = dataAsal.Nama;
                    suPekerja.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    // list of input to uppercase
                    suPekerja.Nama = suPekerja.Nama?.ToUpper()?? "";
                    suPekerja.Alamat1 = suPekerja.Alamat1?.ToUpper()?? "";
                    suPekerja.Alamat2 = suPekerja.Alamat2?.ToUpper()?? "";
                    suPekerja.Alamat3 = suPekerja.Alamat3?.ToUpper()?? "";
                    suPekerja.Bandar = suPekerja.Bandar?.ToUpper()?? "";
                    suPekerja.Jawatan = suPekerja.Jawatan?.ToUpper()?? "";
                    // list of input to uppercase end

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    suPekerja.UserIdKemaskini = user?.UserName ?? "";
                    suPekerja.TarKemaskini = DateTime.Now;
                    suPekerja.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(suPekerja);

                    //insert applog
                    if (namaAsal != suPekerja.Nama || noAkaunAsal != suPekerja.NoAkaunBank)
                    {
                        _appLog.Insert("Ubah", namaAsal + " -> " + suPekerja.Nama
                            + ", " + noAkaunAsal + " -> " + suPekerja.NoAkaunBank, suPekerja.NoGaji, id, 0, pekerjaId,modul,syscode,namamodul,user);
                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", suPekerja.NoGaji, id, 0, pekerjaId,modul,syscode,namamodul,user);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuPekerjaExists(suPekerja.Id))
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
            PopulateTableFromCart();
            return View(suPekerja);
        }

        [Authorize(Policy = "DF001D")]
        // GET: SuPekerja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SuPekerja == null)
            {
                return NotFound();
            }

            var suPekerja = await _suPekerjaRepo.GetById((int)id);
            PopulateTable(id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            return View(suPekerja);
        }

        [Authorize(Policy = "DF001D")]
        // POST: SuPekerja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.SuPekerja == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SuPekerja'  is null.");
            }
            

            var suPekerja = await _context.SuPekerja.FindAsync(id);
            if (suPekerja != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                suPekerja.UserIdKemaskini = user?.UserName ?? "";
                suPekerja.TarKemaskini = DateTime.Now;
                suPekerja.SuPekerjaKemaskiniId = pekerjaId;

                _context.SuPekerja.Remove(suPekerja);
                _appLog.Insert("Hapus", suPekerja.NoKp + " - " + suPekerja.NoAkaunBank, suPekerja.NoGaji, id, 0, pekerjaId,modul,syscode,namamodul,user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                return RedirectToAction(nameof(Index));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuPekerjaExists(int id)
        {
          return (_context.SuPekerja?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Policy = "DF001R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _suPekerjaRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // Batal operation

            obj.FlHapus = 0;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.SuPekerja.Update(obj);

            // Batal operation end
            _appLog.Insert("Rollback", obj.NoKp + " - " + obj.NoAkaunBank, obj.NoGaji, id, 0, pekerjaId,modul,syscode,namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool ICPekerjaExists(string kod)
        {
            return _context.SuPekerja.Any(e => e.NoKp == kod);
        }

        private bool AkaunPekerjaExists(string kod)
        {
            return _context.SuPekerja.Any(e => e.NoAkaunBank == kod);
        }

        private bool EmelPekerjaExists(string kod)
        {
            return _context.SuPekerja.Any(e => e.Emel == kod);
        }
    }
}
