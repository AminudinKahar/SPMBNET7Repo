using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness._Statics;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;

namespace SPMBNET7.Sumber.Controller._01_Jadual
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JBahagianController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string syscode = "SPPB";
        public const string modul = "JD002";
        public const string namamodul = "Jadual Bahagian";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<JBahagian, int, string> _jBahagianRepo;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JBahagianController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IRepository<JBahagian, int, string> jBahagianRepo,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _jBahagianRepo = jBahagianRepo;
            _appLog = appLog;
        }

        // GET: JBahagian
        public async Task<IActionResult> Index()
        {
            var obj = await _jBahagianRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _jBahagianRepo.GetAllIncludeDeletedItems();
            }
            return View(obj);
        }

        // GET: JBahagian/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _jBahagianRepo.GetById((int)id);

            if (jBahagian == null)
            {
                return NotFound();
            }

            return View(jBahagian);
        }

        // GET: JBahagian/Create
        public IActionResult Create()
        {

            // get latest no rujukan running number  
            var ptj = _context.JPTJ.FirstOrDefault();

            var kumpulanWang = ptj.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JBahagian
                        .IgnoreQueryFilters()
                        .Where(x => x.JPTJ.Kod == ptj.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(1));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;

            List<JPTJ> ptjlist = _context.JPTJ.ToList();

            ViewBag.JPtj = ptjlist;
            return View();
        }

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
                    var ptj = _context.JPTJ.FirstOrDefault(x => x.Id == data);

                    var kumpulanWang = ptj.Kod;
                    string prefix = kumpulanWang;
                    int x = 1;
                    string noRujukan = prefix + "00";

                    var LatestNoRujukan = _context.JBahagian
                                .IgnoreQueryFilters()
                                .Where(x => x.JPTJ.Kod == ptj.Kod)
                                .Max(x => x.Kod);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(1));
                        x++;
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }

                    result = noRujukan;
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // POST: JBahagian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JBahagian jBahagian)
        {
            JBahagian m = new JBahagian();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            var ptj = _context.JPTJ.FirstOrDefault(x => x.Id == jBahagian.JPTJId);

            var kumpulanWang = ptj.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JBahagian
                        .IgnoreQueryFilters()
                        .Where(x => x.JPTJ.Kod == ptj.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(1));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            if (jBahagian != null && jBahagian.JPTJId != 0)
            {

                if (ModelState.IsValid)
                {
                    if (ptj.JKWId != null)
                        m.JKWId = (int)ptj.JKWId;

                    m.JPTJId = jBahagian.JPTJId;
                    m.Kod = noRujukan;
                    m.Perihal = jBahagian?.Perihal?.ToUpper() ?? "";
                    m.UserId = user?.UserName ?? "";
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    _context.Add(m);
                    _appLog.Insert("Tambah",m.Kod + " - " + m.Perihal, m.Kod, 0, 0, pekerjaId,modul, syscode, namamodul, user);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }

            List<JKW> list = _context.JKW.ToList();
            ViewBag.NoRujukan = noRujukan;
            ViewBag.JKw = list;

            List<JPTJ> ptjList = _context.JPTJ.ToList();
            ViewBag.JPtj = ptjList;
            return View(jBahagian);
        }

        // GET: JBahagian/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _context.JBahagian.FindAsync(id);
            if (jBahagian == null)
            {
                return NotFound();
            }
            List<JKW> list = _context.JKW.ToList();
            List<JPTJ> ptjList = _context.JPTJ.ToList();

            ViewBag.JKw = list;
            ViewBag.JPtj = ptjList;
            return View(jBahagian);
        }

        // POST: JBahagian/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JBahagian jBahagian, int JKWId)
        {
            if (id != jBahagian.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    JBahagian jBahagianAsal = await _context.JBahagian.FindAsync(id);

                    // list of input that cannot be change
                    jBahagian.JKWId = jBahagianAsal.JKWId;
                    jBahagian.JPTJId = jBahagianAsal.JPTJId;
                    jBahagian.Kod = jBahagianAsal.Kod;
                    jBahagian.TarMasuk = jBahagianAsal.TarMasuk;
                    jBahagian.UserId = jBahagianAsal.UserId;
                    var perihalAsal = jBahagianAsal.Perihal;
                    // list of input that cannot be change end
                    _context.Entry(jBahagianAsal).State = EntityState.Detached;

                    jBahagian.Perihal = jBahagian.Perihal?.ToUpper()?? "";

                    jBahagian.UserIdKemaskini = user?.UserName ?? "";
                    jBahagian.TarKemaskini = DateTime.Now;
                    jBahagian.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(jBahagian);

                    if (perihalAsal != jBahagian.Perihal)
                    {
                        _appLog.Insert("Ubah", perihalAsal + " -> " + jBahagian.Perihal, jBahagian.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);
                    }
                    else
                    {
                        _appLog.Insert("Ubah", "Ubah Data", jBahagian.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JBahagianExists(jBahagian.Id))
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
            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list;
            return View(jBahagian);
        }

        // GET: JBahagian/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _jBahagianRepo.GetById((int)id);

            if (jBahagian == null)
            {
                return NotFound();
            }

            return View(jBahagian);
        }

        // POST: JBahagian/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jBahagian = await _context.JBahagian.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            jBahagian.UserIdKemaskini = user?.UserName ?? "";
            jBahagian.TarKemaskini = DateTime.Now;
            jBahagian.SuPekerjaKemaskiniId = pekerjaId;

            _context.JBahagian.Remove(jBahagian);
            _appLog.Insert("Hapus", jBahagian.Kod + " - " + jBahagian.Perihal, jBahagian.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _jBahagianRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            obj.UserIdKemaskini = user?.UserName ?? "";
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;
            // Batal operation

            obj.FlHapus = 0;
            _context.JBahagian.Update(obj);

            //await AddLogAsync("Rollback", obj.Kod + " - " + obj.Perihal, 0);
            // Batal operation end
            _appLog.Insert("Rollback", obj.Kod + " - " + obj.Perihal, obj.Kod, id, 0, pekerjaId, modul, syscode, namamodul, user);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JBahagianExists(int id)
        {
            return _context.JBahagian.Any(e => e.Id == id);
        }
    }
}
