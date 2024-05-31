using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DOAN_BANHANG_VY.Models;

public partial class Hoadon
{
    [DisplayName("Mã hóa đơn")]
    public int MaHd { get; set; }
    [DisplayName("Ngày lập")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime Ngay { get; set; }

    [DisplayName("Tổng tiền")]
    public int TongTien { get; set; }
    [DisplayName("Khách hàng")]
    public int MaKh { get; set; }
    [DisplayName("Trạng thái")]
    public int TrangThai { get; set; }

    public virtual ICollection<Cthoadon>? Cthoadons { get; set; } = new List<Cthoadon>();

    public virtual Khachhang? MaKhNavigation { get; set; } = null!;
}
