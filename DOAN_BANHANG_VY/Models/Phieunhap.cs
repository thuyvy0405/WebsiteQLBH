using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Phieunhap
{
    [DisplayName("Số phiếu")]
    public int SoPhieu { get; set; }
    [DisplayName("Ngày nhập")]
    public DateTime NgayNhap { get; set; }
    [DisplayName("Tổng tiền")]
    public int TongTien { get; set; }
    [DisplayName("Ghi chú")]
    public string? GhiChu { get; set; }
    [DisplayName("Nhà cung ứng")]
    public int MaNcu { get; set; }
    [DisplayName("Nhân viên")]
    public int MaNv { get; set; }

    public virtual Nhacungung? MaNcuNavigation { get; set; } = null!;

    public virtual Nhanvien? MaNvNavigation { get; set; } = null!;
}
