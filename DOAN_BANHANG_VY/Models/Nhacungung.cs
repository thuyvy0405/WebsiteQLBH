using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Nhacungung
{
    [DisplayName("Mã nhà cung ứng")]
    public int MaNcu { get; set; }
    [DisplayName("Nhà cung ứng")]
    public string Ten { get; set; } = null!;
    [DisplayName("Điện thoại")]
    public string DienThoai { get; set; } = null!;
    [DisplayName("Email")]
    public string Email { get; set; } = null!;
    [DisplayName("Tình trạng")]
    public bool? TinhTrang { get; set; }

    public virtual ICollection<Mathang>? Mathangs { get; set; } = new List<Mathang>();

    public virtual ICollection<Phieunhap>? Phieunhaps { get; set; } = new List<Phieunhap>();
}
