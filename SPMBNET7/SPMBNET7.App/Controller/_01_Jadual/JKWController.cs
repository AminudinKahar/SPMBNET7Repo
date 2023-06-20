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
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces;

namespace SPMBNET7.App.Controller._01_Jadual
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JKWController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD006";
        public const string namamodul = "Jadual Kumpulan Wang";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JKWController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }
        // GET: KW
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JKW.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JKW.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: KW/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kW = await _context.JKW
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kW == null)
            {
                return NotFound();
            }

            return View(kW);
        }

        // GET: KW/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KW/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JKW kW, string syscode)
        {
            if (KodKWExists(kW.Kod) == false)
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    kW.UserId = user?.UserName ?? "";

                    kW.TarMasuk = DateTime.Now;
                    kW.SuPekerjaMasukId = pekerjaId;

                    _context.Add(kW);
                    _appLog.Insert("Tambah", kW.Kod + " - " + kW.Perihal, kW.Kod, 0, 0, pekerjaId, modul, syscode, namamodul, user);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));

                }
            }
            else
            {
                TempData[SD.Error] = "Kod ini telah wujud..!";
            }

            return View(kW);
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: KW/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kW = await _context.JKW.FindAsync(id);
            if (kW == null)
            {
                return NotFound();
            }
            return View(kW);
        }

        // POST: KW/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,JKW kW, string syscode)
        {
            if (id != kW.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    var objAsal = await _context.JKW.FirstOrDefaultAsync(x => x.Id == kW.Id);
                    var kodAsal = objAsal!.Kod;
                    var perihalAsal = objAsal.Perihal;
                    kW.UserId = objAsal.UserId;
                    kW.TarMasuk = objAsal.TarMasuk;
                    kW.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    kW.UserIdKemaskini = user?.UserName ?? "";

                    kW.TarKemaskini = DateTime.Now;
                    kW.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(kW);

                    _appLog.Insert("Ubah", kodAsal + " -> " + kW.Kod + ", " + perihalAsal + " -> " + kW.Perihal + ", ", kW.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KWExists(kW.Id))
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
            return View(kW);
        }

        // GET: KW/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kW = await _context.JKW
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kW == null)
            {
                return NotFound();
            }

            return View(kW);
        }

        // POST: KW/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            var kW = await _context.JKW.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
            
                kW!.UserIdKemaskini = user?.UserName ?? "";
            kW.TarKemaskini = DateTime.Now;
            kW.SuPekerjaKemaskiniId = pekerjaId;

            _context.JKW.Remove(kW);
            _appLog.Insert("Hapus", kW.Kod + " - " + kW.Perihal, kW.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

            var obj = await _context.JKW.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj!.FlHapus = 0;
                obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.JKW.Update(obj);

            // Batal operation end
            _appLog.Insert("Rollback", obj.Kod + " - " + obj.Perihal, obj.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool KWExists(int id)
        {
            return _context.JKW.Any(e => e.Id == id);
        }

        private bool KodKWExists(string kod)
        {
            return _context.JKW.Any(e => e.Kod == kod);
        }
    }
}
