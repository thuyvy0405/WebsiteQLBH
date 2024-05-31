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

	public class DonvitinhsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonvitinhsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Donvitinhs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donvitinhs.ToListAsync());
        }

        // GET: Admin/Donvitinhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donvitinh = await _context.Donvitinhs
                .FirstOrDefaultAsync(m => m.MaDvt == id);
            if (donvitinh == null)
            {
                return NotFound();
            }

            return View(donvitinh);
        }

        // GET: Admin/Donvitinhs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Donvitinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDvt,Ten")] Donvitinh donvitinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donvitinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donvitinh);
        }

        // GET: Admin/Donvitinhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donvitinh = await _context.Donvitinhs.FindAsync(id);
            if (donvitinh == null)
            {
                return NotFound();
            }
            return View(donvitinh);
        }

        // POST: Admin/Donvitinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDvt,Ten")] Donvitinh donvitinh)
        {
            if (id != donvitinh.MaDvt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donvitinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonvitinhExists(donvitinh.MaDvt))
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
            return View(donvitinh);
        }

        // GET: Admin/Donvitinhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donvitinh = await _context.Donvitinhs
                .FirstOrDefaultAsync(m => m.MaDvt == id);
            if (donvitinh == null)
            {
                return NotFound();
            }

            return View(donvitinh);
        }

        // POST: Admin/Donvitinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donvitinh = await _context.Donvitinhs.FindAsync(id);
            if (donvitinh != null)
            {
                _context.Donvitinhs.Remove(donvitinh);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonvitinhExists(int id)
        {
            return _context.Donvitinhs.Any(e => e.MaDvt == id);
        }
    }
}
