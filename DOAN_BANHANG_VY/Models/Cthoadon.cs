using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Cthoadon
{
    [DisplayName("Mã chi tiết")]
    public int MaCthd { get; set; }
    [DisplayName("Mã hóa đơn")]
    public int MaHd { get; set; }
    [DisplayName("Mã mặt hàng")]
    public int MaMh { get; set; }
    [DisplayName("Đơn giá")]
    public int DonGia { get; set; }
    [DisplayName("Số lượng")]
    public int Solung { get; set; }
    [DisplayName("Thành tiền")]
    public int ThanhTien { get; set; }

    public virtual Hoadon? MaHdNavigation { get; set; } = null!;

    public virtual Mathang? MaMhNavigation { get; set; } = null!;
}
