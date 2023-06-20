using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPMBNET7.Sumber.Data;
using SPMBNET7.CoreBusiness.Models.Administrations;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.Infrastructure.Interfaces.Common;
using SPMBNET7.Infrastructure.Interfaces;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using SPMBNET7.CoreBusiness._Statics;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SPMBNET7.Sumber.Controller._01_Jadual
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JPenyemakController : Microsoft.AspNetCore.Mvc.Controller
    {
        public const string syscode = "SPPB";
        public const string modul = "JD011";
        public const string namamodul = "Jadual Penyemak";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<JPenyemak, int, string> _penyemakRepo;

        public JPenyemakController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog,
            IRepository<JPenyemak, int, string> penyemakRepo)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
            _penyemakRepo = penyemakRepo;
        }

        // GET: JPenyemak
        public async Task<IActionResult> Index()
        {
            var obj = await _penyemakRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _penyemakRepo.GetAllIncludeDeletedItems();
            }

            return View(obj);
        }

        // GET: JPenyemak/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JPenyemak == null)
            {
                return NotFound();
            }

            var jPenyemak = await _penyemakRepo.GetById((int)id);
            if (jPenyemak == null)
            {
                return NotFound();
            }
            PopulateList();
            return View(jPenyemak);
        }
        private void PopulateList()
        {
            List<SuPekerja> pekerjaList = _context.SuPekerja.ToList();
            ViewBag.SuPekerja = pekerjaList;
        }
        // GET: JPenyemak/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: JPenyemak/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JPenyemak jPenyemak)
        {
            JPenyemak m = new JPenyemak();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            var pekerja = await _context.SuPekerja.FirstOrDefaultAsync(x => x.Id == jPenyemak.SuPekerjaId);

            if (IsSuPekerjaExists(jPenyemak.SuPekerjaId) == true)
            {
                TempData[SD.Error] = "Penyemak ini telah wujud..!";
                PopulateList();
                return View(jPenyemak);
            }

            if (jPenyemak.MinAmaun > jPenyemak.MaksAmaun)
            {
                TempData[SD.Error] = "Amaun Minimum lebih besar dari Amaun Maksimum..!";
                PopulateList();
                return View(jPenyemak);
            }

            if (ModelState.IsValid)
            {
                if (jPenyemak != null && jPenyemak.SuPekerjaId != 0 && pekerja != null)
                {
                    m.SuPekerjaId = jPenyemak.SuPekerjaId;
                    m.MinAmaun = jPenyemak.MinAmaun;
                    m.MaksAmaun = jPenyemak.MaksAmaun;
                    m.IsBelian = jPenyemak.IsBelian;
                    m.IsNotaMinta = jPenyemak.IsNotaMinta;
                    m.IsPendahuluan = jPenyemak.IsPendahuluan;
                    m.IsPO = jPenyemak.IsPO;
                    m.IsPV = jPenyemak.IsPV;
                    m.IsInvois = jPenyemak.IsInvois;
                    m.IsLaporanBukuVot = jPenyemak.IsLaporanBukuVot;
                    m.TarMasuk = DateTime.Now;
                    m.UserId = user?.UserName ?? "";
                    m.SuPekerjaMasukId = pekerjaId;

                    await _penyemakRepo.Insert(m);

                    //insert applog
                    _appLog.Insert("Tambah", pekerja.NoGaji + " - " + pekerja.NoKp, pekerja.NoGaji, 0, 0, pekerjaId,modul,syscode,namamodul,user);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    //CartEmpty();
                    TempData[SD.Success] = "Maklumat Penyemak berjaya ditambah";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData[SD.Error] = "Sila isi ruangan yang bertanda (*)..!";
            PopulateList();
            return View(jPenyemak);
        }

        // GET: JPenyemak/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JPenyemak == null)
            {
                return NotFound();
            }

            var jPenyemak = await _penyemakRepo.GetById((int)id);
            if (jPenyemak == null)
            {
                return NotFound();
            }
            PopulateList();
            return View(jPenyemak);
        }

        // POST: JPenyemak/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,JPenyemak jPenyemak)
        {
            if (id != jPenyemak.Id)
            {
                return NotFound();
            }

            if (jPenyemak.MinAmaun > jPenyemak.MaksAmaun)
            {
                TempData[SD.Error] = "Amaun Minimum lebih besar dari Amaun Maksimum..!";
                PopulateList();
                return View(jPenyemak);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                    var objAsal = await _context.JPenyemak.Include(x => x.SuPekerja).FirstOrDefaultAsync(x => x.Id == id);
                    jPenyemak.UserId = objAsal.UserId;
                    jPenyemak.TarMasuk = objAsal.TarMasuk;
                    jPenyemak.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                    jPenyemak.SuPekerjaId = objAsal.SuPekerjaId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    var objPekerja = await _context.SuPekerja.FirstOrDefaultAsync(x => x.Id == jPenyemak.SuPekerjaId);

                    jPenyemak.UserIdKemaskini = user?.UserName ?? "";
                    jPenyemak.TarKemaskini = DateTime.Now;
                    jPenyemak.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(jPenyemak);
                    //insert applog

                    _appLog.Insert("Ubah", objPekerja.NoGaji + " - " + objPekerja.NoKp, objPekerja.NoGaji, id, 0, pekerjaId,modul,syscode,namamodul,user);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JPenyemakExists(jPenyemak.Id))
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
            return View(jPenyemak);
        }

        // GET: JPenyemak/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JPenyemak == null)
            {
                return NotFound();
            }

            var jPenyemak = await _penyemakRepo.GetById((int)id);
            if (jPenyemak == null)
            {
                return NotFound();
            }

            return View(jPenyemak);
        }

        // POST: JPenyemak/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JPenyemak == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JPenyemak'  is null.");
            }
            var jPenyemak = await _penyemakRepo.GetById(id);
            if (jPenyemak != null)
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                jPenyemak.UserIdKemaskini = user?.UserName ?? "";
                jPenyemak.TarKemaskini = DateTime.Now;
                jPenyemak.SuPekerjaKemaskiniId = pekerjaId;

                await _penyemakRepo.Delete(id);
                //insert applog
                _appLog.Insert("Hapus", jPenyemak.SuPekerja.NoGaji + " - " + jPenyemak.SuPekerja.NoKp, jPenyemak.SuPekerja.NoGaji, id, 0, pekerjaId,modul,syscode, namamodul,user);

                //insert applog end

                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool JPenyemakExists(int id)
        {
          return (_context.JPenyemak?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool IsSuPekerjaExists(int id)
        {
            return _context.JPenyemak.Any(e => e.SuPekerjaId == id);
        }
    }
}
