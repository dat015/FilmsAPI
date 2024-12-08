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
