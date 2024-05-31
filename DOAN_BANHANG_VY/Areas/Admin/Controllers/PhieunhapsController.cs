using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOAN_BANHANG_VY.Data;
using DOAN_BANHANG_VY.Models;
using Microsoft.AspNetCore.Authorization;
using DOAN_BANHANG_VY.DTO;
using Newtonsoft.Json;

namespace DOAN_BANHANG_VY.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]

	public class PhieunhapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhieunhapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Phieunhaps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Phieunhaps.Include(p => p.MaNcuNavigation).Include(p => p.MaNvNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Phieunhaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieunhap = await _context.Phieunhaps
                .Include(p => p.MaNcuNavigation)
                .Include(p => p.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.SoPhieu == id);
            if (phieunhap == null)
            {
                return NotFound();
            }

            return View(phieunhap);
        }

        // GET: Admin/Phieunhaps/Create
        public IActionResult Create()
        {
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten");
            ViewData["MaNv"] = new SelectList(_context.Nhanviens, "MaNv", "Ten");
            return View();
        }

        // POST: Admin/Phieunhaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoPhieu,NgayNhap,TongTien,GhiChu,MaNcu,MaNv")] Phieunhap phieunhap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phieunhap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten", phieunhap.MaNcu);
            ViewData["MaNv"] = new SelectList(_context.Nhanviens, "MaNv", "Ten", phieunhap.MaNv);
            return View(phieunhap);
        }

        // GET: Admin/Phieunhaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieunhap = await _context.Phieunhaps.FindAsync(id);
            if (phieunhap == null)
            {
                return NotFound();
            }
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten", phieunhap.MaNcu);
            ViewData["MaNv"] = new SelectList(_context.Nhanviens, "MaNv", "Ten", phieunhap.MaNv);
            return View(phieunhap);
        }

        // POST: Admin/Phieunhaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SoPhieu,NgayNhap,TongTien,GhiChu,MaNcu,MaNv")] Phieunhap phieunhap)
        {
            if (id != phieunhap.SoPhieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieunhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieunhapExists(phieunhap.SoPhieu))
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
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten", phieunhap.MaNcu);
            ViewData["MaNv"] = new SelectList(_context.Nhanviens, "MaNv", "Ten", phieunhap.MaNv);
            return View(phieunhap);
        }

        // GET: Admin/Phieunhaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieunhap = await _context.Phieunhaps
                .Include(p => p.MaNcuNavigation)
                .Include(p => p.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.SoPhieu == id);
            if (phieunhap == null)
            {
                return NotFound();
            }

            return View(phieunhap);
        }

        // POST: Admin/Phieunhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieunhap = await _context.Phieunhaps.FindAsync(id);
            if (phieunhap != null)
            {
                _context.Phieunhaps.Remove(phieunhap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhieunhapExists(int id)
        {
            return _context.Phieunhaps.Any(e => e.SoPhieu == id);
        }
     
    }
}
