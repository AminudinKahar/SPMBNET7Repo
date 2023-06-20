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
    public class JTahapAktivitiController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD013";
        public const string namamodul = "Jadual Tahap Aktiviti";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JTahapAktivitiController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }

        // GET: JTahapAktiviti
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JTahapAktiviti.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JTahapAktiviti.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JTahapAktiviti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JTahapAktiviti == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }

            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JTahapAktiviti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JTahapAktiviti jTahapAktiviti, string syscode)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                jTahapAktiviti.Perihal = jTahapAktiviti.Perihal?.ToUpper() ?? "";
                    jTahapAktiviti.UserId = user?.UserName ?? "";
                jTahapAktiviti.TarMasuk = DateTime.Now;
                jTahapAktiviti.SuPekerjaMasukId = pekerjaId;

                _context.Add(jTahapAktiviti);
                _appLog.Insert("Tambah", jTahapAktiviti.Perihal, jTahapAktiviti.Perihal, 0, 0, pekerjaId, modul,syscode,namamodul,user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
            }
            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JTahapAktiviti == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti.FindAsync(id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }
            return View(jTahapAktiviti);
        }

        // POST: JTahapAktiviti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JTahapAktiviti jTahapAktiviti, string syscode)
        {
            if (id != jTahapAktiviti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    var objAsal = await _context.JTahapAktiviti.FirstOrDefaultAsync(x => x.Id == jTahapAktiviti.Id);
                    var perihalAsal = "";

                    if (objAsal != null)
                    {
                        perihalAsal = objAsal.Perihal;
                        jTahapAktiviti.UserId = objAsal.UserId;
                        jTahapAktiviti.TarMasuk = objAsal.TarMasuk;
                        jTahapAktiviti.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                        _context.Entry(objAsal).State = EntityState.Detached;
                    }

                    jTahapAktiviti.Perihal = jTahapAktiviti.Perihal?.ToUpper() ?? "";

                    jTahapAktiviti.UserIdKemaskini = user?.UserName ?? "";
                    jTahapAktiviti.TarKemaskini = DateTime.Now;
                    jTahapAktiviti.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(jTahapAktiviti);

                    if (perihalAsal != jTahapAktiviti.Perihal)
                    {
                        _appLog.Insert("Ubah", perihalAsal + " -> " + jTahapAktiviti.Perihal, jTahapAktiviti.Perihal, id, 0, pekerjaId, modul,syscode,namamodul,user);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JTahapAktivitiExists(jTahapAktiviti.Id))
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
            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JTahapAktiviti == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }

            return View(jTahapAktiviti);
        }

        // POST: JTahapAktiviti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.JTahapAktiviti == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JTahapAktiviti'  is null.");
            }
            var jTahapAktiviti = await _context.JTahapAktiviti.FindAsync(id);
            if (jTahapAktiviti != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                jTahapAktiviti.UserIdKemaskini = user?.UserName ?? "";
                jTahapAktiviti.TarKemaskini = DateTime.Now;
                jTahapAktiviti.SuPekerjaKemaskiniId = pekerjaId;

                _context.JTahapAktiviti.Remove(jTahapAktiviti);
                _appLog.Insert("Hapus", jTahapAktiviti.Perihal, jTahapAktiviti.Perihal, id, 0, pekerjaId,modul,syscode,namamodul,user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _context.JTahapAktiviti.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

            // Batal operation
            obj!.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            obj.FlHapus = 0;
            _context.JTahapAktiviti.Update(obj);

            // Batal operation end
            _appLog.Insert("Rollback", obj.Perihal, obj.Perihal, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JTahapAktivitiExists(int id)
        {
          return (_context.JTahapAktiviti?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
