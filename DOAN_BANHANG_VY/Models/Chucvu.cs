using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Chucvu
{
    [DisplayName("Mã chức vụ")]
    public int Macv { get; set; }
    [DisplayName("Tên chức vụ")]
    public string Ten { get; set; } = null!;
    [DisplayName("Hệ số")]
    public double HeSo { get; set; }

    public virtual ICollection<Nhanvien>? Nhanviens { get; set; } = new List<Nhanvien>();
}
