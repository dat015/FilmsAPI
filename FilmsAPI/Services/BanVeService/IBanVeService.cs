using FilmsAPI.Models;

namespace FilmsAPI.Services.BanVeService
{
    public interface IBanVeService
    {
        Task<XuatChieu> GetXuatChieuAsync(int id);
        //Task<XuatChieu> VebanAsync(List<Ghe> ghe);
        Task<List<Ve>> GetVeTheoGhe(List<Ghe> listGhe, int maXuatChieu);
        Task<HoaDon> SaveBill(HoaDon hoaDon);
        Task<bool> AddDetailBillRangeAsync(List<ChiTietHoaDon> chiTiet);
        Task<bool> UpdateStatusVe(List<Ve> ve);
        Task<List<Ve>> GetVeTheoSuatChieu(int maXC);
        Task<KhachHang> GetKhachHangBySdt(string sdt);

    }
}
