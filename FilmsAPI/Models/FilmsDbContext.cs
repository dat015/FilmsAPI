using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Models;

public partial class FilmsDbContext : DbContext
{
    public FilmsDbContext()
    {
    }

    public FilmsDbContext(DbContextOptions<FilmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<DangPhim> DangPhims { get; set; }

    public virtual DbSet<Ghe> Ghes { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiGhe> LoaiGhes { get; set; }

    public virtual DbSet<LoaiPhim> LoaiPhims { get; set; }

    public virtual DbSet<LoaiVe> LoaiVes { get; set; }

    public virtual DbSet<ManHinh> ManHinhs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<Phim> Phims { get; set; }

    public virtual DbSet<PhongChieu> PhongChieus { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TheLoaiCuaPhim> TheLoaiCuaPhims { get; set; }

    public virtual DbSet<Ve> Ves { get; set; }

    public virtual DbSet<XuatChieu> XuatChieus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.

        => optionsBuilder.UseSqlServer("Server=DESKTOP-CCD18C2\\SQLEXPRESS;Database=FilmsDb;Trusted_Connection=True;TrustServerCertificate=True;");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.MaHd, e.MaVe }).HasName("PK__ChiTietH__C557F7E0730F06C3");

            entity.ToTable("ChiTietHoaDon", tb => tb.HasTrigger("trg_Calculate_ThanhTien_and_TongTien"));

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHoa__MaHD__1BC821DD");

            entity.HasOne(d => d.MaVeNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaVe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHoa__MaVe__1CBC4616");
        });

        modelBuilder.Entity<DangPhim>(entity =>
        {
            entity.HasKey(e => e.MaDangPhim).HasName("PK__DangPhim__D957C0F503EE7BAD");

            entity.ToTable("DangPhim");

            entity.Property(e => e.TenDangPhim).HasMaxLength(100);

            entity.HasOne(d => d.MaManHinhNavigation).WithMany(p => p.DangPhims)
                .HasForeignKey(d => d.MaManHinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DangPhim__MaManH__534D60F1");
        });

        modelBuilder.Entity<Ghe>(entity =>
        {
            entity.HasKey(e => e.MaGhe).HasName("PK__Ghe__3CD3C67B1C4F2A47");

            entity.ToTable("Ghe");

            entity.HasOne(d => d.MaLoaiGheNavigation).WithMany(p => p.Ghes)
                .HasForeignKey(d => d.MaLoaiGhe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ghe__MaLoaiGhe__6B24EA82");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.Ghes)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ghe__MaPhong__6A30C649");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HoaDon__2725A6E0B3AA9AA7");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaNV__17036CC0");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E2CBC27AE");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenKh)
                .HasMaxLength(100)
                .HasColumnName("TenKH");
        });

        modelBuilder.Entity<LoaiGhe>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiGhe__730A5759477DE420");

            entity.ToTable("LoaiGhe");

            entity.Property(e => e.TenLoaiGhe)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoaiPhim>(entity =>
        {
            entity.HasKey(e => e.MaTheLoai).HasName("PK__LoaiPhim__D73FF34A0D58F5F6");

            entity.ToTable("LoaiPhim");

            entity.Property(e => e.TenTheLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<LoaiVe>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiVe__730A5759C6202B2C");

            entity.ToTable("LoaiVe");

            entity.Property(e => e.TenLoaiVe).HasMaxLength(50);
        });

        modelBuilder.Entity<ManHinh>(entity =>
        {
            entity.HasKey(e => e.MaManHinh).HasName("PK__ManHinh__D849392279AAB992");

            entity.ToTable("ManHinh");

            entity.Property(e => e.TenManHinh).HasMaxLength(100);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A80FEEC51");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenNv)
                .HasMaxLength(200)
                .HasColumnName("TenNV");

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__MaQuye__7A672E12");
        });

        modelBuilder.Entity<Phim>(entity =>
        {
            entity.HasKey(e => e.MaPhim).HasName("PK__Phim__4AC03DE3809C5992");

            entity.ToTable("Phim");

            entity.Property(e => e.AnhBia)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.NgayKc).HasColumnName("NgayKC");
            entity.Property(e => e.NgayKt).HasColumnName("NgayKT");
            entity.Property(e => e.TenDaoDien).HasMaxLength(100);
            entity.Property(e => e.TenPhim).HasMaxLength(255);

            entity.HasOne(d => d.MaDangPhimNavigation).WithMany(p => p.Phims)
                .HasForeignKey(d => d.MaDangPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phim__MaDangPhim__5629CD9C");
        });

        modelBuilder.Entity<PhongChieu>(entity =>
        {
            entity.HasKey(e => e.MaPhongChieu).HasName("PK__PhongChi__121FC6E29C522BB8");

            entity.ToTable("PhongChieu", tb => tb.HasTrigger("trg_Calculate_SoHangGhe"));

            entity.Property(e => e.TenPhongChieu).HasMaxLength(100);

            entity.HasOne(d => d.MaManHinhNavigation).WithMany(p => p.PhongChieus)
                .HasForeignKey(d => d.MaManHinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhongChie__MaMan__656C112C");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PK__Quyen__1D4B7ED4B4BF2E67");

            entity.ToTable("Quyen");

            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C6529A3F5D317");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC0A7A43E13").IsUnique();

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoan__MaKH__75A278F5");
        });

        modelBuilder.Entity<TheLoaiCuaPhim>(entity =>
        {
            entity.HasKey(e => new { e.Maphim, e.MaTheLoai }).HasName("PK__LoaiCuaP__0148F99DF6B45DC5");

            entity.ToTable("TheLoaiCuaPhim");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.MaTheLoaiNavigation).WithMany(p => p.TheLoaiCuaPhims)
                .HasForeignKey(d => d.MaTheLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoaiCuaPh__MaThe__628FA481");

            entity.HasOne(d => d.MaphimNavigation).WithMany(p => p.TheLoaiCuaPhims)
                .HasForeignKey(d => d.Maphim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoaiCuaPh__Maphi__619B8048");
        });

        modelBuilder.Entity<Ve>(entity =>
        {
            entity.HasKey(e => e.MaVe).HasName("PK__Ve__2725100F73E6D2F6");

            entity.ToTable("Ve");

            entity.Property(e => e.GiaVe).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");

            entity.HasOne(d => d.MaGheNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaGhe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__MaGhe__00200768");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__MaKH__02084FDA");

            entity.HasOne(d => d.MaLoaiVeNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaLoaiVe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__MaLoaiVe__7F2BE32F");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__MaNV__02FC7413");

            entity.HasOne(d => d.MaXuatChieuNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaXuatChieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__MaXuatChieu__01142BA1");
        });

        modelBuilder.Entity<XuatChieu>(entity =>
        {
            entity.HasKey(e => e.MaXuatChieu).HasName("PK__XuatChie__46080F55B873C3E2");

            entity.ToTable("XuatChieu");

            entity.HasOne(d => d.MaPhimNavigation).WithMany(p => p.XuatChieus)
                .HasForeignKey(d => d.MaPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__XuatChieu__MaPhi__6EF57B66");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.XuatChieus)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__XuatChieu__MaPho__6E01572D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
