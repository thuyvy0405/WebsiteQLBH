using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Donvitinh
{
    [DisplayName("Mã Đơn vị tính")]
    public int MaDvt { get; set; }
    [DisplayName("Đơn vị tính")]
    public string Ten { get; set; } = null!;

    public virtual ICollection<Mathang>? Mathangs { get; set; } = new List<Mathang>();
}
