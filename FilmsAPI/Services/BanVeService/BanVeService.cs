using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FilmsAPI.Services.BanVeService
{
    public class BanVeService : IBanVeService
    {
        private readonly FilmsDbContext _db;
        public BanVeService(FilmsDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddDetailBillForFoodRangeAsync(List<DetailFood> detailFoods)
        {
            if (detailFoods == null) return false;

            try
            {
                await _db.DetailFoods.AddRangeAsync(detailFoods);
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<Category>> GetFoodCates()
        {
            try
            {
               var cates =await _db.Categories.ToListAsync();
                return (cates == null) ? new List<Category>() : cates;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                return new List<Category>(); 
            }
        }

        public async Task<List<Food>> GetFoodByCate(int cateId)
        {
            try
            {
                var foods = await _db.Foods.Where(fd => fd.CateId == cateId).ToListAsync();
                return foods;
            }
            catch 
            {
                return null;
            }
        }
        public async Task<List<Ve>> GetVeTheoSuatChieu(int maXC)
        {
            try
            {
                var result = await _db.Ves.Where(ve => ve.MaXuatChieu == maXC).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> UpdateStatusVe(List<Ve> ve)
        {
            try
            {
                if (ve == null || !ve.Any()) return false;

                var idVe = ve.Select(p => p.MaVe).ToList();
                var veInDb = await _db.Ves
                    .Where(v => idVe.Contains(v.MaVe))
                    .ToListAsync();

                if (!veInDb.Any())
                {
                    Console.WriteLine("Không tìm thấy vé nào trong DB.");
                    return false;
                }

                foreach (var item in veInDb)
                {
                    var veToUpdate = ve.FirstOrDefault(v => v.MaVe == item.MaVe);
                    if (veToUpdate != null)
                    {
                        item.TrangThai = veToUpdate.TrangThai;
                        _db.Entry(item).Property(v => v.TrangThai).IsModified = true;
                    }
                }

                Console.WriteLine("Bắt đầu lưu thay đổi vào DB.");
                var result = await _db.SaveChangesAsync();
                Console.WriteLine($"Số bản ghi được cập nhật: {result}");

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }


        public async Task<XuatChieu> GetXuatChieuAsync(int id)
        {
            try
            {

                var xuatChieu = await _db.XuatChieus
                    .Where(x => x.MaXuatChieu == id)
                    .Include(x => x.MaPhimNavigation)
                    .Include(x => x.MaPhongNavigation)
                        .ThenInclude(p => p.Ghes)
                        .ThenInclude(p => p.MaLoaiGheNavigation)
                    .FirstOrDefaultAsync();

                if (xuatChieu == null)
                {
                    return null;
                }

                return xuatChieu;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HoaDon> SaveBill(HoaDon hoaDon)
        {
            try
            {
                if (hoaDon == null)
                {
                    return null;
                }
                _db.HoaDons.Add(hoaDon);
                await _db.SaveChangesAsync();
                return hoaDon;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<bool> AddDetailBillRangeAsync(List<ChiTietHoaDon> chiTiet)
        {
            try
            {
                if (chiTiet == null)
                {
                    return false;
                }
                await _db.ChiTietHoaDons.AddRangeAsync(chiTiet);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<KhachHang> GetKhachHangBySdt(string sdt)
        {
            try
            {
                var KH = await _db.KhachHangs.Where(kh => kh.Sdt == sdt).FirstOrDefaultAsync();
                return KH;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<Ve>> GetVeTheoGhe(List<Ghe> listGhe, int maXuatChieu)
        {
            try
            {
                if (listGhe == null || !listGhe.Any())
                {
                    return null; // Trả về null nếu danh sách ghế rỗng
                }

                // Lấy danh sách ID ghế từ listGhe
                var idGhe = listGhe.Select(i => i.MaGhe).ToList();

                // Lấy danh sách vé theo điều kiện
                var listVe = await _db.Ves
                    .Where(ve => idGhe.Contains(ve.MaGhe) && ve.MaXuatChieu == maXuatChieu && ve.TrangThai == false)
                    .ToListAsync();
                if (listVe == null)
                {
                    return null;
                }
                return listVe; // Trả về danh sách vé
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và ghi log nếu cần
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    

        //public async Task<XuatChieu> VebanAsync(List<Ghe> ghe)
        //{
        //    try
        //    {
        //        var gheIds = ghe.Select(g => g.MaGhe).ToList();
        //        var listGhe = await _db.Ghes
        //                  .Where(g => gheIds.Contains(g.MaGhe))
        //                  .ToListAsync();


        //        foreach (var item in listGhe)
        //        {
        //            item.TrangThai = true;
        //        }

        //        await _db.SaveChangesAsync();
        //        return GetXuatChieuAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
