using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Khachhang
{
    [DisplayName("Mã khách hàng")]
    public int MaKh { get; set; }
    [DisplayName("Tên khách hàng")]
    public string Ten { get; set; } = null!;
    [DisplayName("Điện thoại")]
    public string DienThoai { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    [DisplayName("Mật khẩu")]
    public string MatKhau { get; set; } = null!;

    public virtual ICollection<Diachi>? Diachis { get; set; } = new List<Diachi>();

    public virtual ICollection<Hoadon>? Hoadons { get; set; } = new List<Hoadon>();
}
