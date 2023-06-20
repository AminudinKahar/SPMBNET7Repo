using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class JNegeriController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD007";
        public const string namamodul = "Jadual Negeri";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JNegeriController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }

        // GET: JNegeri
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JNegeri.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JNegeri.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JNegeri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JNegeri == null)
            {
                return NotFound();
            }

            var jNegeri = await _context.JNegeri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jNegeri == null)
            {
                return NotFound();
            }

            return View(jNegeri);
        }

        // GET: JNegeri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JNegeri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( JNegeri negeri, string syscode)
        {
            if (!KodNegeriExists(negeri.Kod))
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    negeri.UserId = user?.UserName ?? "";
                    negeri.TarMasuk = DateTime.Now;
                    negeri.SuPekerjaMasukId = pekerjaId;

                    _context.Add(negeri);
                    _appLog.Insert("Tambah", negeri.Kod + " - " + negeri.Perihal, negeri.Kod, 0, 0, pekerjaId, modul, syscode, namamodul, user);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData[SD.Error] = "Kod telah wujud..!";
            return View(negeri);
        }

        // GET: JNegeri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JNegeri == null)
            {
                return NotFound();
            }

            var jNegeri = await _context.JNegeri.FindAsync(id);
            if (jNegeri == null)
            {
                return NotFound();
            }
            return View(jNegeri);
        }

        // POST: JNegeri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JNegeri negeri, string syscode)
        {
            if (id != negeri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objAsal = await _context.JNegeri.FirstOrDefaultAsync(x => x.Id == negeri.Id);
                    var kodAsal = objAsal!.Kod;
                    var perihalAsal = objAsal.Perihal;
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    negeri.UserId = objAsal.UserId;
                    negeri.TarMasuk = objAsal.TarMasuk;
                    negeri.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    negeri.UserIdKemaskini = user?.UserName ?? "";
                    negeri.TarKemaskini = DateTime.Now;
                    negeri.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(negeri);

                    _appLog.Insert("Ubah", kodAsal + " -> " + negeri.Kod + ", "
                        + perihalAsal + " -> " + negeri.Perihal + ", ", negeri.Kod, id, 0, pekerjaId,modul,syscode,namamodul,user);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JNegeriExists(negeri.Id))
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
            return View(negeri);
        }

        // GET: JNegeri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JNegeri == null)
            {
                return NotFound();
            }

            var jNegeri = await _context.JNegeri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jNegeri == null)
            {
                return NotFound();
            }

            return View(jNegeri);
        }

        // POST: JNegeri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.JNegeri == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JNegeri'  is null.");
            }
            var jNegeri = await _context.JNegeri.FindAsync(id);
            if (jNegeri != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                var negeri = await _context.JNegeri.FindAsync(id);

                negeri!.UserIdKemaskini = user?.UserName ?? "";
                negeri.TarKemaskini = DateTime.Now;
                negeri.SuPekerjaKemaskiniId = pekerjaId;

                _context.JNegeri.Remove(negeri);
                _appLog.Insert("Hapus", negeri.Kod + " - " + negeri.Perihal, negeri.Kod, id, 0, pekerjaId,modul,syscode,namamodul,user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _context.JNegeri.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

            // Batal operation
            obj!.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;
            obj.FlHapus = 0;
            _context.JNegeri.Update(obj);

            // Batal operation end
            _appLog.Insert("Rollback", obj.Kod + " - " + obj.Perihal, obj.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JNegeriExists(int id)
        {
          return (_context.JNegeri?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool KodNegeriExists(string kod)
        {
            return _context.JNegeri.Any(e => e.Kod == kod);
        }
    }
}
