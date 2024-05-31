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

	public class NhacungungsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhacungungsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Nhacungungs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nhacungungs.ToListAsync());
        }

        // GET: Admin/Nhacungungs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhacungung = await _context.Nhacungungs
                .FirstOrDefaultAsync(m => m.MaNcu == id);
            if (nhacungung == null)
            {
                return NotFound();
            }

            return View(nhacungung);
        }

        // GET: Admin/Nhacungungs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Nhacungungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNcu,Ten,DienThoai,Email,TinhTrang")] Nhacungung nhacungung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhacungung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhacungung);
        }

        // GET: Admin/Nhacungungs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhacungung = await _context.Nhacungungs.FindAsync(id);
            if (nhacungung == null)
            {
                return NotFound();
            }
            return View(nhacungung);
        }

        // POST: Admin/Nhacungungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNcu,Ten,DienThoai,Email,TinhTrang")] Nhacungung nhacungung)
        {
            if (id != nhacungung.MaNcu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhacungung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhacungungExists(nhacungung.MaNcu))
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
            return View(nhacungung);
        }

        // GET: Admin/Nhacungungs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhacungung = await _context.Nhacungungs
                .FirstOrDefaultAsync(m => m.MaNcu == id);
            if (nhacungung == null)
            {
                return NotFound();
            }

            return View(nhacungung);
        }

        // POST: Admin/Nhacungungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhacungung = await _context.Nhacungungs.FindAsync(id);
            if (nhacungung != null)
            {
                _context.Nhacungungs.Remove(nhacungung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhacungungExists(int id)
        {
            return _context.Nhacungungs.Any(e => e.MaNcu == id);
        }

        public String Kiemtratrangthai()
        {
            string tt = "";
            bool trangthai = true;
            if (trangthai)
            {
                tt = "Hợp tác";
                
            }
            else
               tt="Dừng hợp tác";

            return tt;
        }
    }
}
