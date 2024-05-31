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
    public class CthoadonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CthoadonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Cthoadons

        // GET: Admin/Cthoadons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cthoadon = await _context.Cthoadons
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaMhNavigation)
                .FirstOrDefaultAsync(m => m.MaCthd == id);
            if (cthoadon == null)
            {
                return NotFound();
            }

            return View(cthoadon);
        }

        // GET: Admin/Cthoadons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cthoadon = await _context.Cthoadons.FindAsync(id);
            if (cthoadon == null)
            {
                return NotFound();
            }
            ViewData["MaHd"] = new SelectList(_context.Hoadons, "MaHd", "MaHd", cthoadon.MaHd);
            ViewData["MaMh"] = new SelectList(_context.Mathangs, "MaMh", "Ten", cthoadon.MaMh);
            return View(cthoadon);
        }

        // POST: Admin/Cthoadons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCthd,MaHd,MaMh,DonGia,Solung,ThanhTien")] Cthoadon cthoadon)
        {
            if (id != cthoadon.MaCthd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cthoadon.ThanhTien = cthoadon.Solung * cthoadon.DonGia;

                    _context.Update(cthoadon);
                    await _context.SaveChangesAsync();
                    var cthdUpdate = _context.Cthoadons.Where(x => x.MaHd.Equals(cthoadon.MaHd)).ToList();
                    int tong = 0;
                    foreach( var item in cthdUpdate)
                    {
                        tong += item.Solung * item.DonGia;
                    }
                    var hoadon = await _context.Hoadons.FirstOrDefaultAsync(x => x.MaHd == cthoadon.MaHd);
                    hoadon.TongTien = tong;
                    _context.Update(cthoadon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CthoadonExists(cthoadon.MaCthd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Lấy giá trị của Mahd từ cthoadon
                int mahd1 = cthoadon.MaHd;

                // Tạo một đối tượng RouteValueDictionary để chứa thông tin chuyển hướng
                var routeValues1 = new RouteValueDictionary
                {   
                { "controller", "HoaDons" },   // Controller muốn chuyển hướng đến
                { "action", "Details" },       // Action muốn chuyển hướng đến
                { "id", mahd1 }                  // Tham số id
                };

                // Sử dụng RedirectToAction với đối số routeValues
                return RedirectToAction("Details", "HoaDons", routeValues1);
            }
            ViewData["MaHd"] = new SelectList(_context.Hoadons, "MaHd", "MaHd", cthoadon.MaHd);
            ViewData["MaMh"] = new SelectList(_context.Mathangs, "MaMh", "Ten", cthoadon.MaMh);
            // Lấy giá trị của Mahd từ cthoadon
            int mahd = cthoadon.MaHd;

            // Tạo một đối tượng RouteValueDictionary để chứa thông tin chuyển hướng
            var routeValues = new RouteValueDictionary
            {
                { "controller", "HoaDons" },   // Controller muốn chuyển hướng đến
                { "action", "Details" },       // Action muốn chuyển hướng đến
                { "id", mahd }                  // Tham số id
            };

            // Sử dụng RedirectToAction với đối số routeValues
            return RedirectToAction("Details", "HoaDons", routeValues);
        }

        // GET: Admin/Cthoadons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cthoadon = await _context.Cthoadons
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaMhNavigation)
                .FirstOrDefaultAsync(m => m.MaCthd == id);
            if (cthoadon == null)
            {
                return NotFound();
            }

            return View(cthoadon);
        }

        // POST: Admin/Cthoadons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cthoadon = await _context.Cthoadons.FindAsync(id);
            if (cthoadon != null)
            {
                _context.Cthoadons.Remove(cthoadon);
            }
            int mahd = cthoadon.MaHd;
            await _context.SaveChangesAsync();
            var cthdUpdate = _context.Cthoadons.Where(x => x.MaHd.Equals(cthoadon.MaHd)).ToList();
            int tong = 0;
            foreach (var item in cthdUpdate)
            {
                tong += item.Solung * item.DonGia;
            }
            var hoadon = await _context.Hoadons.FirstOrDefaultAsync(x => x.MaHd == cthoadon.MaHd);
            hoadon.TongTien = tong;
            _context.Update(cthoadon);
            await _context.SaveChangesAsync();
            // Lấy giá trị của Mahd từ cthoadon

            // Tạo một đối tượng RouteValueDictionary để chứa thông tin chuyển hướng
            var routeValues = new RouteValueDictionary
            {
                { "controller", "HoaDons" },   // Controller muốn chuyển hướng đến
                { "action", "Details" },       // Action muốn chuyển hướng đến
                { "id", mahd }                  // Tham số id
            };

            // Sử dụng RedirectToAction với đối số routeValues
            return RedirectToAction("Details", "HoaDons", routeValues);
        }

        private bool CthoadonExists(int id)
        {
            return _context.Cthoadons.Any(e => e.MaCthd == id);
        }
    }
}
