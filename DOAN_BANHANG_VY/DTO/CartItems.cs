using System.ComponentModel.DataAnnotations;

namespace DOAN_BANHANG_VY.DTO
{
	public class CartItems
	{
		public MathangDTO Mathang { get; set; }
		public int soluong { get; set; }
		public DiachikhachangDTO diachikhachang { get; set; }
	}
	public class MathangDTO
	{
		public int MaMh { get; set; }

		[Display(Name = "Tên Sản Phẩm")]
		[Required]
		public string Ten { get; set; } = null!;

		[Display(Name = "Giá Góc")]
		[Required]
		public int GiaGoc { get; set; }

		[Display(Name = "Giá Bán")]
		[Required]
		public int GiaBan { get; set; }

		[Display(Name = "Số Lượng")]
		[Required]
		public int SoLuong { get; set; }
		[Display(Name = "Hình Ảnh")]
		[Required]
		public string? HinhAnh { get; set; }
	}

	public class DiachikhachangDTO
	{
		public string Hoten { get; set; }
		public string Email { get; set; }
		public string Sdt { get; set; }
		public string Tinhthanh { get; set; }
		public string Quanhuyen { get; set; }
		public string Phuongxa { get; set; }
		public string Diachi { get; set; }
	}
}
