using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Diachi
{
    [DisplayName("Mã địa chỉ")]
    public int MaDc { get; set; }
    [DisplayName("Khách hàng")]
    public int MaKh { get; set; }
    [DisplayName("Địa chỉ cụ thể")]
    public string? DiaChi1 { get; set; }
    [DisplayName("Phường/Xã")]
    public string? PhuongXa { get; set; }
    [DisplayName("Quận/Huyện")]
    public string? QuanHuyen { get; set; }
    [DisplayName("Tỉnh thành")]
    public string? TinhThanh { get; set; }

    public int MacDinh { get; set; }

    public virtual Khachhang? MaKhNavigation { get; set; } = null!;
}
