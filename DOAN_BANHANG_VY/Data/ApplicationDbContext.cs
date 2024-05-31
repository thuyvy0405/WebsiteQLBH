using System;
using System.Collections.Generic;
using DOAN_BANHANG_VY.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DOAN_BANHANG_VY.Data;

public partial class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Chucvu> Chucvus { get; set; }

    public virtual DbSet<CtPhieunhap> CtPhieunhaps { get; set; }

    public virtual DbSet<Cthoadon> Cthoadons { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Diachi> Diachis { get; set; }

    public virtual DbSet<Donvitinh> Donvitinhs { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Mathang> Mathangs { get; set; }

    public virtual DbSet<Nhacungung> Nhacungungs { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Phieunhap> Phieunhaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=vei-pc\\vydang;Initial Catalog=QLBHTBD;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Chucvu>(entity =>
        {
            entity.HasKey(e => e.Macv).HasName("PK__CHUCVU__2724823EC4FFF1DB");

            entity.ToTable("CHUCVU");

            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<CtPhieunhap>(entity =>
        {
            entity.HasKey(e => e.SoPhieu).HasName("PK__CT_PHIEU__960AAEE267C3B84B");

            entity.ToTable("CT_PHIEUNHAP");

            entity.Property(e => e.MaMh).HasColumnName("MaMH");

            entity.HasOne(d => d.MaMhNavigation).WithMany(p => p.CtPhieunhaps)
                .HasForeignKey(d => d.MaMh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_PHIEUNH__MaMH__48CFD27E");
        });

        modelBuilder.Entity<Cthoadon>(entity =>
        {
            entity.HasKey(e => e.MaCthd).HasName("PK__CTHOADON__1E4FA771D9EA9EF6");

            entity.ToTable("CTHOADON");

            entity.Property(e => e.MaCthd).HasColumnName("MaCTHD");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaMh).HasColumnName("MaMH");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.Cthoadons)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTHOADON__MaHD__3B75D760");

            entity.HasOne(d => d.MaMhNavigation).WithMany(p => p.Cthoadons)
                .HasForeignKey(d => d.MaMh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTHOADON__MaMH__3C69FB99");
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.MaDm).HasName("PK__DANHMUC__2725866E9C57A367");

            entity.ToTable("DANHMUC");

            entity.Property(e => e.MaDm).HasColumnName("MaDM");
            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<Diachi>(entity =>
        {
            entity.HasKey(e => e.MaDc).HasName("PK__DIACHI__27258664DB84AC1E");

            entity.ToTable("DIACHI");

            entity.Property(e => e.MaDc).HasColumnName("MaDC");
            entity.Property(e => e.DiaChi1)
                .HasMaxLength(150)
                .HasColumnName("DiaChi");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.PhuongXa).HasMaxLength(50);
            entity.Property(e => e.QuanHuyen).HasMaxLength(50);
            entity.Property(e => e.TinhThanh).HasMaxLength(60);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Diachis)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DIACHI__MaKH__35BCFE0A");
        });

        modelBuilder.Entity<Donvitinh>(entity =>
        {
            entity.HasKey(e => e.MaDvt).HasName("PK__DONVITIN__3D895AFE59C60EAD");

            entity.ToTable("DONVITINH");

            entity.Property(e => e.MaDvt).HasColumnName("MaDVT");
            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HOADON__2725A6E0267B6DD7");

            entity.ToTable("HOADON");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.Ngay).HasColumnType("datetime");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOADON__MaKH__38996AB5");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KHACHHAN__2725CF1EED5DF40D");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<Mathang>(entity =>
        {
            entity.HasKey(e => e.MaMh).HasName("PK__MATHANG__2725DFD9944B03C1");

            entity.ToTable("MATHANG");

            entity.Property(e => e.MaMh).HasColumnName("MaMH");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LuotMua).HasDefaultValue(0);
            entity.Property(e => e.LuotXem).HasDefaultValue(0);
            entity.Property(e => e.MaDm).HasColumnName("MaDM");
            entity.Property(e => e.MaDvt).HasColumnName("MaDVT");
            entity.Property(e => e.MaNcu).HasColumnName("MaNCU");
            entity.Property(e => e.MoTa).HasMaxLength(1000);
            entity.Property(e => e.Ten).HasMaxLength(100);

            entity.HasOne(d => d.MaDmNavigation).WithMany(p => p.Mathangs)
                .HasForeignKey(d => d.MaDm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MATHANG__MaDM__2D27B809");

            entity.HasOne(d => d.MaDvtNavigation).WithMany(p => p.Mathangs)
                .HasForeignKey(d => d.MaDvt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MATHANG__MaDVT__2E1BDC42");

            entity.HasOne(d => d.MaNcuNavigation).WithMany(p => p.Mathangs)
                .HasForeignKey(d => d.MaNcu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MATHANG__MaNCU__2F10007B");
        });

        modelBuilder.Entity<Nhacungung>(entity =>
        {
            entity.HasKey(e => e.MaNcu).HasName("PK__NHACUNGU__3A185DF98502FE18");

            entity.ToTable("NHACUNGUNG");

            entity.Property(e => e.MaNcu).HasColumnName("MaNCU");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NHANVIEN__2725D70AD02EB3C9");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaCv).HasColumnName("MaCV");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ten).HasMaxLength(100);

            entity.HasOne(d => d.MaCvNavigation).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.MaCv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NHANVIEN__MaCV__412EB0B6");
        });

        modelBuilder.Entity<Phieunhap>(entity =>
        {
            entity.HasKey(e => e.SoPhieu).HasName("PK__PHIEUNHA__960AAEE2B48D5EC2");

            entity.ToTable("PHIEUNHAP");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaNcu).HasColumnName("MaNCU");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayNhap).HasColumnType("datetime");

            entity.HasOne(d => d.MaNcuNavigation).WithMany(p => p.Phieunhaps)
                .HasForeignKey(d => d.MaNcu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PHIEUNHAP__MaNCU__440B1D61");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Phieunhaps)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PHIEUNHAP__MaNV__44FF419A");
        });
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
