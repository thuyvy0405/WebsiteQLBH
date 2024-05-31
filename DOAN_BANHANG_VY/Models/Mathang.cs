using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DOAN_BANHANG_VY.Models;

public partial class Mathang
{
    [DisplayName("Mã mặt hàng")]
    public int MaMh { get; set; }
    [DisplayName("Tên mặt hàng")]

    public string Ten { get; set; } = null!;
    [DisplayName("Giá bán")]
    public int GiaGoc { get; set; }
    [DisplayName("Giá bán giảm giá")]
    public int GiaBan { get; set; }
    [DisplayName("Mô tả")]
    public string? MoTa { get; set; }
    [DisplayName("Hình ảnh")]
    public string? HinhAnh { get; set; }
    [DisplayName("Danh mục")]
    public int MaDm { get; set; }
    [DisplayName("Đơn vị tính")]
    public int MaDvt { get; set; }
    [DisplayName("Nhà cung cấp")]
    public int MaNcu { get; set; }
    [DisplayName("Lược xem")]
    public int? LuotXem { get; set; }
    [DisplayName("Lượng mua")]
    public int? LuotMua { get; set; }
    [DisplayName("Số lượng")]
    public int? SoLuong { get; set; }

    public virtual ICollection<CtPhieunhap>? CtPhieunhaps { get; set; } = new List<CtPhieunhap>();

    public virtual ICollection<Cthoadon>? Cthoadons { get; set; } = new List<Cthoadon>();

    public virtual Danhmuc? MaDmNavigation { get; set; } = null!;

    public virtual Donvitinh? MaDvtNavigation { get; set; } = null!;

    public virtual Nhacungung? MaNcuNavigation { get; set; } = null!;
}
