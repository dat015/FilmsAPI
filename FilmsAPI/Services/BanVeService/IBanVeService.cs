using FilmsAPI.Models;

namespace FilmsAPI.Services.BanVeService
{
    public interface IBanVeService
    {
        Task<XuatChieu> GetXuatChieuAsync(int id);
        //Task<XuatChieu> VebanAsync(List<Ghe> ghe);
    }
}
