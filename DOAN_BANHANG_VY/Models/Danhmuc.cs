using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DOAN_BANHANG_VY.Models;

public partial class Danhmuc
{
    [DisplayName("Mã danh mục")]
    public int MaDm { get; set; }
    [DisplayName("Danh mục")]
    public string Ten { get; set; } = null!;

    public virtual ICollection<Mathang>? Mathangs { get; set; } = new List<Mathang>();
}
