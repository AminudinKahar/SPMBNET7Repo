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
    public class JBankController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string modul = "JD004";
        public const string namamodul = "Jadual Bank";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JBankController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }

        // GET: JBank
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JBank.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JBank.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JBank/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JBank == null)
            {
                return NotFound();
            }

            var jBank = await _context.JBank
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBank == null)
            {
                return NotFound();
            }

            return View(jBank);
        }

        // GET: JBank/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JBank/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JBank bank, string syscode)
        {
            if (KodBankExists(bank.Kod) == false)
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    bank.SuPekerjaMasukId = pekerjaId ?? 1;
                    bank.UserId = user?.UserName ?? "";
                    bank.TarMasuk = DateTime.Now;

                    _context.Add(bank);
                    _appLog.Insert("Tambah", bank.Kod + " - " + bank.Nama, bank.Kod, 0, 0, pekerjaId, modul, syscode, namamodul, user);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData[SD.Error] = "Kod ini telah wujud..!";
            }
            return View(bank);
        }

        // GET: JBank/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JBank == null)
            {
                return NotFound();
            }

            var jBank = await _context.JBank.FindAsync(id);
            if (jBank == null)
            {
                return NotFound();
            }
            return View(jBank);
        }

        // POST: JBank/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,JBank bank, string syscode)
        {
            if (id != bank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    var objAsal = await _context.JBank.FirstOrDefaultAsync(x => x.Id == bank.Id);
                    var kodAsal = objAsal!.Kod;
                    var perihalAsal = objAsal.Nama;
                    var kodEFTAsal = objAsal.KodEFT;
                    bank.UserId = objAsal.UserId;
                    bank.TarMasuk = objAsal.TarMasuk;
                    bank.SuPekerjaMasukId = pekerjaId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    bank.UserIdKemaskini = user?.UserName ?? "";
                    bank.TarKemaskini = DateTime.Now;
                    bank.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(bank);

                    _appLog.Insert("Ubah", kodAsal + " -> " + bank.Kod  + ", "
                        + perihalAsal + " -> " + bank.Nama + ", "
                        + kodEFTAsal + " -> " + bank.KodEFT + ", ", bank.Kod, id, 0, pekerjaId,modul,syscode,namamodul,user);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JBankExists(bank.Id))
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
            return View(bank);
        }

        // GET: JBank/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JBank == null)
            {
                return NotFound();
            }

            var jBank = await _context.JBank
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBank == null)
            {
                return NotFound();
            }

            return View(jBank);
        }

        // POST: JBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            if (_context.JBank == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JBank'  is null.");
            }
            var bank = await _context.JBank.FindAsync(id);
            if (bank != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
                bank.UserIdKemaskini = user?.UserName ?? "";
                bank.TarKemaskini = DateTime.Now;
                bank.SuPekerjaKemaskiniId = pekerjaId;

                _context.JBank.Remove(bank);
                _appLog.Insert("Hapus", bank.Kod + " - " + bank.Nama, bank.Kod, id, 0, pekerjaId,modul,syscode,namamodul,user);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
                
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var obj = await _context.JBank.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;
            obj!.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            // Batal operation

            obj.FlHapus = 0;
            _context.JBank.Update(obj);

            // Batal operation end
            _appLog.Insert("Rollback", obj.Kod + " - " + obj.Nama, obj.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JBankExists(int id)
        {
          return (_context.JBank?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool KodBankExists(string kod)
        {
            return _context.JBank.Any(e => e.Kod == kod);
        }
    }
}
