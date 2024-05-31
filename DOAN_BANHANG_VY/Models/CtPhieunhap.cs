using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class CtPhieunhap
{
    [DisplayName("Số phiếu")]
    public int SoPhieu { get; set; }
    [DisplayName("Mã mặt hàng")]
    public int MaMh { get; set; }
    [DisplayName("Đơn giá nhập")]
    public int DonGiaNhap { get; set; }
    [DisplayName("Số lượng nhập")]
    public int? SoluongNhap { get; set; }
    [DisplayName("Thành tiền")]
    public int ThanhTien { get; set; }

    public virtual Mathang? MaMhNavigation { get; set; } = null!;
}
