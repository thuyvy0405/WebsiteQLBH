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
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DOAN_BANHANG_VY.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]

	public class HoadonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoadonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Hoadons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Hoadons.Include(h => h.MaKhNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Hoadons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadons
                .Include(h => h.MaKhNavigation)
                .Include(cthd => cthd.Cthoadons)
                .ThenInclude(mh => mh.MaMhNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // GET: Admin/Hoadons/Create
        public async Task<IActionResult> Create()
        {
            ViewData["MaKh"] = new SelectList(_context.Khachhangs, "MaKh", "Ten");
            ViewBag.CART = GetCartItems();
            ViewBag.DSSP = await _context.Mathangs.ToListAsync();
            return View();
        }

        // POST: Admin/Hoadons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHd,Ngay,TongTien,MaKh,TrangThai")] Hoadon hoadon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoadon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.Khachhangs, "MaKh", "Ten", hoadon.MaKh);
            return View(hoadon);
        }

        // GET: Admin/Hoadons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadons.FindAsync(id);
            if (hoadon == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.Khachhangs, "MaKh", "Ten", hoadon.MaKh);
            return View(hoadon);
        }

        // POST: Admin/Hoadons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHd,Ngay,TongTien,MaKh,TrangThai")] Hoadon hoadon)
        {
            if (id != hoadon.MaHd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoadon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoadonExists(hoadon.MaHd))
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
            ViewData["MaKh"] = new SelectList(_context.Khachhangs, "MaKh", "Ten", hoadon.MaKh);
            return View(hoadon);
        }

        // GET: Admin/Hoadons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadons
                .Include(h => h.MaKhNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // POST: Admin/Hoadons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoadon = await _context.Hoadons.FindAsync(id);
            if (hoadon != null)
            {
                _context.Hoadons.Remove(hoadon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoadonExists(int id)
        {
            return _context.Hoadons.Any(e => e.MaHd == id);
        }
        List<CartItems> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString("CT");
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItems>>(jsoncart);
            }
            return new List<CartItems>();
        }
        void RemoveCart()
        {
            var session = HttpContext.Session;
            session.Remove("CT");
        }
        //lưu danh sách cartitem giỏ hàng vào session
        bool SaveCartSession(List<CartItems> cartItems)
        {
            try
            {
                // Check if HttpContext is available
                if (HttpContext == null || HttpContext.Session == null)
                {
                    // Handle the situation where HttpContext or Session is not available
                    return false;
                }

                var session = HttpContext.Session;

                // Serialize cartItems to JSON
                string jsonCart = JsonConvert.SerializeObject(cartItems);

                // Save JSON string to the session
                session.SetString("CT", jsonCart);

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting purposes
                Console.Error.WriteLine($"Error saving cart session: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace); // Log the stack trace for more details
                return false;
            }
        }
        // Xóa session giỏ hàng
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove("CT");
        }

        // thêm hàng vào giỏ
        public async Task<IActionResult> AddToCart(int id,DateTime Ngay , int MaKH)
        {
            try
            {
                var mathang = await _context.Mathangs.Include(x => x.MaDmNavigation).FirstOrDefaultAsync(m => m.MaMh == id);
                if (mathang == null)
                {
                    return NotFound("Sản phẩm không tồn tại");
                }
                var mathangdto = new MathangDTO();
                mathangdto.Ten = mathang.Ten;
                mathangdto.MaMh = id;
                mathangdto.GiaBan = mathang.GiaBan;
                mathangdto.GiaGoc = mathang.GiaGoc;
                mathangdto.SoLuong = 1;
                var cart = GetCartItems();
                var item = cart.Find(p => p.Mathang.MaMh == id);
                if (item != null)
                {
                    item.soluong++;
                }
                else
                {
                    cart.Add(new CartItems() { Mathang = mathangdto, soluong = mathangdto.SoLuong });
                }
                var success = SaveCartSession(cart);
                if (success)
                {
                    ViewData["ngaytao"] = Ngay;
                    ViewData["MaKh"] = new SelectList(_context.Khachhangs, "MaKh", "Ten", MaKH != 0? MaKH : null);
                    ViewData["kh"] = MaKH;
                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex) { return BadRequest("An error occurred while processing the request."); }

        }

        public IActionResult RemoveItem(int id, int soluong, DateTime ngaytao, int MaKH)
        {
            var cart = GetCartItems();
            var item = cart.Find(p => p.Mathang.MaMh == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            SaveCartSession(cart);
            ViewData["ngaytao"] = ngaytao;
            ViewData["MaKh"] = new SelectList(_context.Khachhangs, "MaKh", "Ten", MaKH);
            TempData["SuccessMessage"] = "Thêm thành công";
            return RedirectToAction(nameof(Create));
        }

        public IActionResult UpdateItem(int id, int quantity, DateTime ngaytao, int MaKH)
        {
            var cart = GetCartItems();
            var item = cart.Find(p => p.Mathang.MaMh == id);
            if (item != null)
            {
                item.soluong = quantity;
            }
            SaveCartSession(cart);
            ViewData["ngaytao"] = ngaytao;
            ViewData["MaKh"] = new SelectList(_context.Khachhangs, "MaKh", "Ten", MaKH);
            TempData["SuccessMessage"] = "Thêm thành công";
            return RedirectToAction(nameof(Create));
        }

        public IActionResult dashboard()
        {
            List<Hoadon> hoaDons;
            using (var context = new ApplicationDbContext())
            {
                hoaDons = context.Hoadons.ToList();
            }
            return View(hoaDons);
        }

    }
}
