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

	public class MathangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MathangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Mathangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Mathangs.Include(m => m.MaDmNavigation).Include(m => m.MaDvtNavigation).Include(m => m.MaNcuNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Mathangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mathang = await _context.Mathangs
                .Include(m => m.MaDmNavigation)
                .Include(m => m.MaDvtNavigation)
                .Include(m => m.MaNcuNavigation)
                .FirstOrDefaultAsync(m => m.MaMh == id);
            if (mathang == null)
            {
                return NotFound();
            }

            return View(mathang);
        }

        // GET: Admin/Mathangs/Create
        public IActionResult Create()
        {
            ViewData["MaDm"] = new SelectList(_context.Danhmucs, "MaDm", "Ten");
            ViewData["MaDvt"] = new SelectList(_context.Donvitinhs, "MaDvt", "Ten");
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten");
            return View();
        }

        // POST: Admin/Mathangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMh,Ten,GiaGoc,GiaBan,SoLuong,MoTa,HinhAnh,MaDm,MaDvt,MaNcu,LuotXem,LuotMua")] Mathang mathang, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mathang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDm"] = new SelectList(_context.Danhmucs, "MaDm", "Ten", mathang.MaDm);
            ViewData["MaDvt"] = new SelectList(_context.Donvitinhs, "MaDvt", "Ten", mathang.MaDvt);
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten", mathang.MaNcu);
            return View(mathang);
        }

        // GET: Admin/Mathangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mathang = await _context.Mathangs.FindAsync(id);
            if (mathang == null)
            {
                return NotFound();
            }
            ViewData["MaDm"] = new SelectList(_context.Danhmucs, "MaDm", "Ten", mathang.MaDm);
            ViewData["MaDvt"] = new SelectList(_context.Donvitinhs, "MaDvt", "Ten", mathang.MaDvt);
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten", mathang.MaNcu);
            return View(mathang);
        }

        // POST: Admin/Mathangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMh,Ten,GiaGoc,GiaBan,SoLuong,MoTa,HinhAnh,MaDm,MaDvt,MaNcu,LuotXem,LuotMua")] Mathang mathang)
        {
            if (id != mathang.MaMh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mathang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MathangExists(mathang.MaMh))
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
            ViewData["MaDm"] = new SelectList(_context.Danhmucs, "MaDm", "Ten", mathang.MaDm);
            ViewData["MaDvt"] = new SelectList(_context.Donvitinhs, "MaDvt", "Ten", mathang.MaDvt);
            ViewData["MaNcu"] = new SelectList(_context.Nhacungungs, "MaNcu", "Ten", mathang.MaNcu);
            return View(mathang);
        }

        // GET: Admin/Mathangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mathang = await _context.Mathangs
                .Include(m => m.MaDmNavigation)
                .Include(m => m.MaDvtNavigation)
                .Include(m => m.MaNcuNavigation)
                .FirstOrDefaultAsync(m => m.MaMh == id);
            if (mathang == null)
            {
                return NotFound();
            }

            return View(mathang);
        }

        // POST: Admin/Mathangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mathang = await _context.Mathangs.FindAsync(id);
            if (mathang != null)
            {
                _context.Mathangs.Remove(mathang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MathangExists(int id)
        {
            return _context.Mathangs.Any(e => e.MaMh == id);
        }

    }
}
