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

namespace DOAN_BANHANG_VY.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CtPhieunhapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CtPhieunhapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CtPhieunhaps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CtPhieunhaps.Include(c => c.MaMhNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/CtPhieunhaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctPhieunhap = await _context.CtPhieunhaps
                .Include(c => c.MaMhNavigation)
                .FirstOrDefaultAsync(m => m.SoPhieu == id);
            if (ctPhieunhap == null)
            {
                return NotFound();
            }

            return View(ctPhieunhap);
        }

        // GET: Admin/CtPhieunhaps/Create
        public IActionResult Create()
        {
            ViewData["MaMh"] = new SelectList(_context.Mathangs, "MaMh", "Ten");
            return View();
        }

        // POST: Admin/CtPhieunhaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoPhieu,MaMh,DonGiaNhap,SoluongNhap,ThanhTien")] CtPhieunhap ctPhieunhap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ctPhieunhap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMh"] = new SelectList(_context.Mathangs, "MaMh", "Ten", ctPhieunhap.MaMh);
            return View(ctPhieunhap);
        }

        // GET: Admin/CtPhieunhaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctPhieunhap = await _context.CtPhieunhaps.FindAsync(id);
            if (ctPhieunhap == null)
            {
                return NotFound();
            }
            ViewData["MaMh"] = new SelectList(_context.Mathangs, "MaMh", "Ten", ctPhieunhap.MaMh);
            return View(ctPhieunhap);
        }

        // POST: Admin/CtPhieunhaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SoPhieu,MaMh,DonGiaNhap,SoluongNhap,ThanhTien")] CtPhieunhap ctPhieunhap)
        {
            if (id != ctPhieunhap.SoPhieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ctPhieunhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CtPhieunhapExists(ctPhieunhap.SoPhieu))
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
            ViewData["MaMh"] = new SelectList(_context.Mathangs, "MaMh", "MaMh", ctPhieunhap.MaMh);
            return View(ctPhieunhap);
        }

        // GET: Admin/CtPhieunhaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ctPhieunhap = await _context.CtPhieunhaps
                .Include(c => c.MaMhNavigation)
                .FirstOrDefaultAsync(m => m.SoPhieu == id);
            if (ctPhieunhap == null)
            {
                return NotFound();
            }

            return View(ctPhieunhap);
        }

        // POST: Admin/CtPhieunhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ctPhieunhap = await _context.CtPhieunhaps.FindAsync(id);
            if (ctPhieunhap != null)
            {
                _context.CtPhieunhaps.Remove(ctPhieunhap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CtPhieunhapExists(int id)
        {
            return _context.CtPhieunhaps.Any(e => e.SoPhieu == id);
        }
    }
}
