using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DOAN_BANHANG_VY.Data;
using DOAN_BANHANG_VY.Models;
using DOAN_BANHANG_VY.DTO;
using QLBTBD.Function;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.DependencyInjection;

namespace QLBH_ASPMVC.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly IPasswordHasher<Khachhang> _passwordHasher;
		private readonly IConfiguration _configuration;
		public HomeController(ApplicationDbContext context, IPasswordHasher<Khachhang> passwordHasher, IConfiguration configuration)
		{
			_context = context;
			_passwordHasher = passwordHasher;
			_configuration = configuration;
		}
        // GET: Home/Details/5
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
            ViewBag.ListMathang = _context.Mathangs.ToList().OrderBy(x => x.MaDm==mathang.MaDm);
            return View(mathang);
        }

       
        private bool MathangExists(int id)
        {
            return _context.Mathangs.Any(e => e.MaMh == id);
        }

          public async Task<IActionResult> ViewCart()
        {
            // Lấy sản phẩm trong giỏ hàng
            var cartItems = GetCartItems();
            Getinfo();
            return View(cartItems);

        }

        public async Task<IActionResult> Checkout()
        {
            Getinfo();
            return View(GetCartItems());
        }
		public async Task<IActionResult> ProductsByManufacturer(string DM)
		{
			Getinfo();
			var products = _context.Mathangs.Where(p => p.MaDmNavigation.Ten.Contains(DM)).Include(p => p.MaDmNavigation);
			return View(await products.ToListAsync());
		}

		public async Task<IActionResult> ProductsByName(string keyword)
		{
			Getinfo();
			var products = _context.Mathangs.Where(p => p.Ten.Contains(keyword)).Include(p => p.MaDmNavigation);
			return View(await products.ToListAsync());
		}

        public async Task<IActionResult> Index(int page = 1, int pageSize = 12, string keyword = null, int DM = 0)
        {

			var query =  _context.Mathangs.Include(m => m.MaDmNavigation)
			.Skip((page - 1) * pageSize)
			.Take(pageSize);
			if(keyword != null)
			{
				query = query.Where(x  => x.Ten.Contains(keyword));
			}
			if(DM != 0)
			{
                query = query.Where(x => x.MaDm.Equals(DM));

            }
            var items = await  query.ToListAsync();
            // Tính toán các thông tin phân trang
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.DanhMucList = await _context.Danhmucs.ToListAsync();
			ViewBag.keyword = keyword;
			ViewBag.DM = DM;
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
                ViewBag.ShowSuccessMessage = true;
            }
            Getinfo();
            return View(items);

        }

        void RemoveCart()
		{
			var session = HttpContext.Session;
			session.Remove("cart");
		}

        public async Task<IActionResult> Profile()
        {
			var khachhang = await _context.Khachhangs.FirstOrDefaultAsync(x => x.Email.Equals(GetKH()));
            if (khachhang == null)
            {
                return View("Login");
            }
            Getinfo();
            ViewBag.DSDH = await GetDonHang(khachhang.MaKh);
            ViewBag.DSDC = await GetDiaChi(khachhang.MaKh);
            return View(khachhang);
        }

        [HttpPost]
        public async Task<IActionResult> capnhatthongtin(Khachhang khachhang)
        {
            if (khachhang == null)
            {
                return View("Login");
            }
            _context.Khachhangs.Update(khachhang);
            await _context.SaveChangesAsync();

            TempData["SUCCESSINFO"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }
        private async Task<bool> VerifyPassword(string matKhauCu, Khachhang khachhang)
        {
            return _passwordHasher.VerifyHashedPassword(khachhang, khachhang.MatKhau, matKhauCu) == PasswordVerificationResult.Success;
        }

        [HttpPost]
        public async Task<IActionResult> capnhatpass(string matkhaucu, string matkhaumoi)
        {
            var khachhangAuth = await _context.Khachhangs.FirstOrDefaultAsync(x => x.Email.Equals(GetKH));
            if (khachhangAuth == null)
            {
                return View("Login");
            }

            if (!await VerifyPassword(matkhaucu, khachhangAuth))
            {
                TempData["ERROR"] = "Mật khẩu cũ của bạn không đúng. Vui lòng nhập lại!";
                return View(nameof(Profile));
            }

            khachhangAuth.MatKhau = _passwordHasher.HashPassword(null, matkhaumoi);
            await _context.SaveChangesAsync();

            TempData["SUCCESS"] = "Cập nhật mật khẩu thành công !";
            return RedirectToAction(nameof(Profile));
        }

        //Đọc danh sách cartitem giỏ hàng từ session
        List<CartItems> GetCartItems()
		{
			var session = HttpContext.Session;
			string jsoncart = session.GetString("cart");
			if (jsoncart != null)
			{
				return JsonConvert.DeserializeObject<List<CartItems>>(jsoncart);
			}
			return new List<CartItems>();
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
				session.SetString("cart", jsonCart);

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
			session.Remove("cart");
		}

		// thêm hàng vào giỏ
		public async Task<IActionResult> AddToCart(int id)

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
					TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return NotFound();
				}
			}
			catch (Exception ex) { return BadRequest("An error occurred while processing the request."); }

		}

		public IActionResult RemoveItem(int id)
		{
			var cart = GetCartItems();
			var item = cart.Find(p => p.Mathang.MaMh == id);
			if (item != null)
			{
				cart.Remove(item);
			}
			SaveCartSession(cart);
			return RedirectToAction(nameof(ViewCart));
		}

		public IActionResult UpdateItem(int id, int quantity)
		{
			var cart = GetCartItems();
			var item = cart.Find(p => p.Mathang.MaMh == id);
			if (item != null)
			{
				item.soluong = quantity;
			}
			SaveCartSession(cart);
			return RedirectToAction(nameof(ViewCart));
		}

        public async Task<IActionResult> diachimacdinh(int id)
        {
			var kh = GetKH();
			if (kh == null)
			{
				return View("Login");
			}
			if(id == null)
			{
				return NotFound();
			}
			var listdiachi = await _context.Diachis.Where(x =>x.MacDinh == 1).ToListAsync();
			foreach(var dc in listdiachi)
			{
				dc.MacDinh = 0;
			}
            await _context.SaveChangesAsync();

            var diachi = await _context.Diachis.FirstOrDefaultAsync(x => x.MaDc.Equals(id));
			diachi.MacDinh = 1;
            _context.Diachis.Update(diachi);
            await _context.SaveChangesAsync();

            TempData["SUCCESSINFO"] = "Cập nhật địa chỉ thành công!";
            return RedirectToAction(nameof(Profile));
        }
        public async Task<IActionResult> xoadiachi(int id)
        {
            var kh = GetKH();
            if (kh == null)
            {
                return View("Login");
            }
            if (id == null)
            {
                return NotFound();
            }
  
            var diachi = await _context.Diachis.FirstOrDefaultAsync(x => x.MaDc.Equals(id));
            _context.Diachis.Remove(diachi);
            await _context.SaveChangesAsync();

            TempData["SUCCESSINFO"] = "Xóa địa chỉ thành công!";
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> DanhmucsanphamAsync(int id, int page = 1, int pageSize = 12)
		{
			// Lọc sản phẩm từ cơ sở dữ liệu theo loại
			var sanpham = await _context.Mathangs.Where(x => x.MaDm.Equals(id))
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			// Tính toán các thông tin phân trang
			var totalItems = sanpham.Count;
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
			
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;
			ViewBag.DanhMucList = _context.Danhmucs.ToListAsync();
			if (TempData.ContainsKey("SuccessMessage"))
			{
				ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
				ViewBag.ShowSuccessMessage = true;
			}
			Getinfo();
			// Trả về danh sách sản phẩm đã lọc
			return View(sanpham);
		}

		public async Task<IActionResult> CreateBill(string hoten, string email, string sdt, string tinhthanh, string quanhuyen, string phuongxa, string diachi, int MaDc = 0)
		{

			try
			{
				// Kiếm tra thông tin khách hàng đã có trên hệ thống chưa??
				var checkkhachhang = await _context.Khachhangs.Where(x => x.Email.Equals(email) || x.DienThoai.Equals(sdt)).FirstOrDefaultAsync();
				var cartlist = GetCartItems();
				var khachhang = new Khachhang();
				//Nếu giỏ hàng khác rỗng
				if (cartlist.Count > 0)
				{
					//Nếu khách hàng chưa mua lần nào
					if (checkkhachhang == null)
					{
						khachhang.Email = email;
						khachhang.DienThoai = sdt;
						khachhang.Ten = hoten;
						string matkhaurand = PasswordHasherFun.GenerateRandomPassword();
						khachhang.MatKhau = _passwordHasher.HashPassword(khachhang, matkhaurand);
						//Xử lý gửi mail cho khách hàng để ddafawngw nhập
						_context.Khachhangs.Add(khachhang);
						await _context.SaveChangesAsync();
						SendPassWordToEmail(email, matkhaurand);
					}
					else
					{
						khachhang = checkkhachhang;
					}
					//Kiếm tra khách hàng đã có địa chỉ chưa
					var listdiachi = khachhang.Diachis.ToList();
					var diachikh = new Diachi();
					// Nếu kh chưa có địa chỉ
					if (listdiachi.Count == 0)
					{
						// cập nhật lại các địa chỉ đã mua hàng thành 0
						foreach (var item in listdiachi)
						{
							item.MacDinh = 0;
						}
						// Tạo địa chỉ mới làm mặc định
						diachikh.DiaChi1 = diachi;
						diachikh.TinhThanh = tinhthanh;
						diachikh.PhuongXa = phuongxa;
						diachikh.QuanHuyen = quanhuyen;
						diachikh.MacDinh = 1;
						diachikh.MaKh = khachhang.MaKh;
						_context.Diachis.Add(diachikh);
						await _context.SaveChangesAsync();// Lưu dịa chỉ
					}
					var hoadon = new Hoadon();
					hoadon.Ngay = DateTime.Now;
					// hoadon.TongTien = Tinhtongtien(cartlist);
					hoadon.MaKh = khachhang.MaKh;
					hoadon.TrangThai = 0;// Chưa xác nhận
					_context.Add(hoadon);
					await _context.SaveChangesAsync();
					int tongtien = 0;
					foreach (var item in cartlist)
					{
						var cthd = new Cthoadon();
						cthd.ThanhTien = item.soluong * item.Mathang.GiaBan;
						cthd.DonGia = item.Mathang.GiaBan;
						cthd.MaHd = hoadon.MaHd;
						cthd.MaMh = item.Mathang.MaMh;
						cthd.Solung = item.soluong;
						tongtien += (item.Mathang.GiaBan * item.soluong);
						_context.Add(cthd);
					}
					hoadon.TongTien = tongtien;
					_context.Update(hoadon);
					await _context.SaveChangesAsync();
					capnhatluotmuaxem(cartlist);
					RemoveCart();
					return View(await _context.Hoadons
								  .Where(x => x.MaHd.Equals(hoadon.MaHd))
								  .Include(m => m.Cthoadons)
									  .ThenInclude(c => c.MaMhNavigation) // Assuming there's a navigation property named MatHang in Cthoadon
								  .FirstOrDefaultAsync());
				}
				else
				{
					return View(nameof(ViewCart));
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
			}
			return BadRequest();
		}
		int Tinhtongtien(List<CartItems> cartlist)
		{
			int tong = 0;
			foreach (var item in cartlist)
			{
				tong += (item.Mathang.GiaBan * item.soluong);
			}
			return tong;
		}

		string GetKH()
		{
			var session = HttpContext.Session;
			string  jsoncart = session.GetString("email");
			if (jsoncart != null)
			{
				return jsoncart;
			}
			return null;
		}
		async void Getinfo()
		{
			ViewData["solg"] = GetCartItems().Count();
			ViewData["email"] = GetKH();
			var kh = GetKH();
			if(GetKH() != null)
			{
                var khachhang = await _context.Khachhangs.FirstOrDefaultAsync(x=> x.Email.Equals(kh));
                ViewData["NOTADDRESS"] = "Địa chỉ mua hàng gần đây";
                ViewBag.khachhang = khachhang;
            }
			else
			{
				ViewBag.khachhang = null;
				ViewBag.DSDC = null;
				ViewBag.DSDH = null;
                ViewData["NOTADDRESS"] = "Bạn chưa có địa chỉ mua hàng!";
            }

        }
		public async Task<List<Diachi>> GetDiaChi(int makh)
		{
            return await _context.Diachis
				.Where(x => x.MaKh == makh).ToListAsync();
        }
        public async Task<List<Hoadon>> GetDonHang(int makh)
        {
            return await _context.Hoadons.Where(x => x.MaKh == makh).Include(m => m.Cthoadons).ThenInclude(mh => mh.MaMhNavigation).ToListAsync() ;
        }
        public IActionResult Login()
		{
			Getinfo();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(string email, string pass)
		{
			var kh = await _context.Khachhangs.FirstOrDefaultAsync(n => n.Email == email);
			if (kh != null && _passwordHasher.VerifyHashedPassword(kh, kh.MatKhau, pass) == PasswordVerificationResult.Success)
			{
				HttpContext.Session.SetString("email", kh.Email);
				return RedirectToAction(nameof(Index));
			}
			Getinfo();
			return RedirectToAction(nameof(Login));
		}
		public IActionResult LogOut()
		{
			HttpContext.Session.SetString("email", "");
			// Đặt ViewData["email"] thành null khi đăng xuất
			ViewData["email"] = null;
			Getinfo();
			// Các hành động khác khi đăng xuất
			return RedirectToAction("Index", "Home");
		}
		public IActionResult Register()
		{
			Getinfo();
			return View();
		}
		[HttpPost]
		public IActionResult Register(string hoten, string sdt, string email, string pass)
		{
			var kh = new Khachhang();
			kh.Email = email;
			kh.Ten = hoten;
			kh.MatKhau = _passwordHasher.HashPassword(null, pass);
			kh.DienThoai = sdt;
			_context.Add(kh);
			_context.SaveChanges();
			Getinfo();
			return RedirectToAction(nameof(Login));
		}
		public void SendPassWordToEmail(string email, string password)
		{
			try
			{
				string emailAddress = _configuration["mail:email"];
				string pass = _configuration["mail:pass"];
				SmtpClient smtp = new SmtpClient();
				smtp.Port = 587;
				smtp.Host = "smtp.gmail.com";
				smtp.EnableSsl = true;
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new NetworkCredential(emailAddress, pass);
				MailMessage messageDetail = new MailMessage();
				messageDetail.From = new MailAddress(emailAddress);
				messageDetail.To.Add(email.Trim());
				messageDetail.Subject = "MẬT KHẨU ĐĂNG NHẬP";
				messageDetail.IsBodyHtml = true;
				messageDetail.Body = "Cảm ơn bạn đã mua sản phẩm tại cửa hàng." +
					"Mật khẩu đăng nhập của bạn là: " + password;
				smtp.Send(messageDetail);
			}
			catch (Exception ex)
			{
			}
		}
		public async void capnhatluotmuaxem(List<CartItems> item)
		{
			foreach(var i in item)
			{
				var sp = _context.Mathangs.FirstOrDefault(x => x.MaMh.Equals(i.Mathang.MaMh));
				sp.LuotMua += 1;
				sp.LuotXem += 1;
				sp.SoLuong -= 1;
				_context.Mathangs.Update(sp);
			}	
			await _context.SaveChangesAsync();
		}
        public async Task<bool> capnhatluotxem(int id)
        {
             var sp = await _context.Mathangs.FirstOrDefaultAsync(x => x.MaMh.Equals(id));
                sp.LuotXem += 1;
                _context.Mathangs.Update(sp);
            await _context.SaveChangesAsync();
			return true;
        }

    }
}
