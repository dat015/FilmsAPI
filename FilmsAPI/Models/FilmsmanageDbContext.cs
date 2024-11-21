using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Models;

public partial class FilmsmanageDbContext : DbContext
{
    public FilmsmanageDbContext()
    {
    }

    public FilmsmanageDbContext(DbContextOptions<FilmsmanageDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DangPhim> DangPhims { get; set; }

    public virtual DbSet<DanhSachDatVeOnline> DanhSachDatVeOnlines { get; set; }

    public virtual DbSet<Ghe> Ghes { get; set; }

    public virtual DbSet<Gium> Gia { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiGhe> LoaiGhes { get; set; }

    public virtual DbSet<LoaiPhim> LoaiPhims { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<Phim> Phims { get; set; }

    public virtual DbSet<PhongChieu> PhongChieus { get; set; }

    public virtual DbSet<QuocGia> QuocGia { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<TinhTrang> TinhTrangs { get; set; }

    public virtual DbSet<Ve> Ves { get; set; }

    public virtual DbSet<XuatChieu> XuatChieus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.

        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Filmsmanage_Db;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DangPhim>(entity =>
        {
            entity.HasKey(e => e.MaDangPhim).HasName("PK__DangPhim__D957C0F5AFEF8FBB");

            entity.ToTable("DangPhim");

            entity.Property(e => e.TenDangPhim).HasMaxLength(255);
        });

        modelBuilder.Entity<DanhSachDatVeOnline>(entity =>
        {
            entity.HasKey(e => e.MaDatVe).HasName("PK__DanhSach__6A32C59383A1A4C1");

            entity.ToTable("DanhSachDatVeOnline");

            entity.Property(e => e.TrangThaiDatVe).HasMaxLength(255);

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.DanhSachDatVeOnlines)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhSachD__MaKha__76969D2E");

            entity.HasOne(d => d.MaPhimNavigation).WithMany(p => p.DanhSachDatVeOnlines)
                .HasForeignKey(d => d.MaPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhSachD__MaPhi__787EE5A0");

            entity.HasOne(d => d.MaXuatChieuNavigation).WithMany(p => p.DanhSachDatVeOnlines)
                .HasForeignKey(d => d.MaXuatChieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhSachD__MaXua__778AC167");
        });

        modelBuilder.Entity<Ghe>(entity =>
        {
            entity.HasKey(e => e.SoGhe).HasName("PK__Ghe__278288CB304CD66C");

            entity.ToTable("Ghe");

            entity.HasOne(d => d.MaLoaiGheNavigation).WithMany(p => p.Ghes)
                .HasForeignKey(d => d.MaLoaiGhe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ghe__MaLoaiGhe__534D60F1");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.Ghes)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ghe__MaPhong__5165187F");

            entity.HasOne(d => d.MaTinhTrangNavigation).WithMany(p => p.Ghes)
                .HasForeignKey(d => d.MaTinhTrang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ghe__MaTinhTrang__52593CB8");
        });

        modelBuilder.Entity<Gium>(entity =>
        {
            entity.HasKey(e => e.MaGia).HasName("PK__Gia__3CD3DE5E55A7377B");

            entity.Property(e => e.SoTien).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.IdLoaiGheNavigation).WithMany(p => p.Gia)
                .HasForeignKey(d => d.IdLoaiGhe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Gia__IdLoaiGhe__628FA481");

            entity.HasOne(d => d.MaDangPhimNavigation).WithMany(p => p.Gia)
                .HasForeignKey(d => d.MaDangPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Gia__MaDangPhim__619B8048");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E55B4A8FE5");

            entity.ToTable("KhachHang");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(50);
            entity.Property(e => e.TenDangNhap).HasMaxLength(100);
        });

        modelBuilder.Entity<LoaiGhe>(entity =>
        {
            entity.HasKey(e => e.MaLoaiGhe).HasName("PK__LoaiGhe__965BB4C1360B3057");

            entity.ToTable("LoaiGhe");

            entity.Property(e => e.TenLoaiGhe).HasMaxLength(255);
        });

        modelBuilder.Entity<LoaiPhim>(entity =>
        {
            entity.HasKey(e => e.MaLoaiPhim).HasName("PK__LoaiPhim__9CA05BEFBB825DCE");

            entity.ToTable("LoaiPhim");

            entity.Property(e => e.TenLoaiPhim).HasMaxLength(255);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.IdNhanVien).HasName("PK__NhanVien__B8294845CE68A773");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap).HasMaxLength(100);
            entity.Property(e => e.TenNhanVien).HasMaxLength(255);

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__MaQuye__5812160E");
        });

        modelBuilder.Entity<Phim>(entity =>
        {
            entity.HasKey(e => e.MaPhim).HasName("PK__Phim__4AC03DE36BE4DB0F");

            entity.ToTable("Phim");

            entity.Property(e => e.AnhDaiDien).HasMaxLength(255);
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MoTaPhim).HasMaxLength(255);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.NoiDungPhim).HasMaxLength(255);

            entity.HasOne(d => d.IdQuocGiaNavigation).WithMany(p => p.Phims)
                .HasForeignKey(d => d.IdQuocGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phim__IdQuocGia__6FE99F9F");

            entity.HasOne(d => d.MaDangPhimNavigation).WithMany(p => p.Phims)
                .HasForeignKey(d => d.MaDangPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phim__MaDangPhim__6EF57B66");

            entity.HasOne(d => d.MaLoaiPhimNavigation).WithMany(p => p.Phims)
                .HasForeignKey(d => d.MaLoaiPhim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phim__MaLoaiPhim__6E01572D");

            entity.HasOne(d => d.MaXuatChieuNavigation).WithMany(p => p.Phims)
                .HasForeignKey(d => d.MaXuatChieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phim__MaXuatChie__6D0D32F4");
        });

        modelBuilder.Entity<PhongChieu>(entity =>
        {
            entity.HasKey(e => e.IdPhongChieu).HasName("PK__PhongChi__77D0B8D0803D4E3B");

            entity.ToTable("PhongChieu");

            entity.Property(e => e.TenPhongChieu).HasMaxLength(255);
        });

        modelBuilder.Entity<QuocGia>(entity =>
        {
            entity.HasKey(e => e.IdQuocGia).HasName("PK__QuocGia__DEA34C5E655B07F0");

            entity.Property(e => e.TenNuoc).HasMaxLength(255);
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PK__Quyen__1D4B7ED471171AAA");

            entity.ToTable("Quyen");

            entity.Property(e => e.TenQuyen).HasMaxLength(255);
        });

        modelBuilder.Entity<TinhTrang>(entity =>
        {
            entity.HasKey(e => e.MaTinhTrang).HasName("PK__TinhTran__89F8F66919D8612C");

            entity.ToTable("TinhTrang");

            entity.Property(e => e.TenTinhTrang).HasMaxLength(255);
        });

        modelBuilder.Entity<Ve>(entity =>
        {
            entity.HasKey(e => e.IdVe).HasName("PK__Ve__B7700A99D7B4E273");

            entity.ToTable("Ve");

            entity.Property(e => e.DonGia).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.IdNhanVienNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.IdNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__IdNhanVien__7D439ABD");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__MaPhong__7B5B524B");

            entity.HasOne(d => d.MaXuatChieuNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.MaXuatChieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__MaXuatChieu__7C4F7684");

            entity.HasOne(d => d.SoGheNavigation).WithMany(p => p.Ves)
                .HasForeignKey(d => d.SoGhe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ve__SoGhe__7E37BEF6");
        });

        modelBuilder.Entity<XuatChieu>(entity =>
        {
            entity.HasKey(e => e.MaXuatChieu).HasName("PK__XuatChie__46080F55C29D9692");

            entity.ToTable("XuatChieu");

            entity.HasOne(d => d.IdPhongChieuNavigation).WithMany(p => p.XuatChieus)
                .HasForeignKey(d => d.IdPhongChieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__XuatChieu__IdPho__5DCAEF64");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
