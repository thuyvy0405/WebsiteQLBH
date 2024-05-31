using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Nhanvien
{
    [DisplayName("Mã nhân viên")]
    public int MaNv { get; set; }
    [DisplayName("Tên nhân viên")]
    public string Ten { get; set; } = null!;
    [DisplayName("Chức vụ")]
    public int MaCv { get; set; }
    [DisplayName("Điện thoại")]
    public string DienThoai { get; set; } = null!;

    public string Email { get; set; } = null!;
    [DisplayName("Mật khẩu")]
    public string MatKhau { get; set; } = null!;

    public virtual Chucvu? MaCvNavigation { get; set; } = null!;

    public virtual ICollection<Phieunhap>? Phieunhaps { get; set; } = new List<Phieunhap>();
}
