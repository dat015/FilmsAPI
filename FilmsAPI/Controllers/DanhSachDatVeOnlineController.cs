using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhSachDatVeOnlineController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;
        public DanhSachDatVeOnlineController()
        {
            _db = new FilmsmanageDbContext();
        }

        [HttpGet("{maPhim}", Name = "GetDanhSachDatVe")]
        public async Task<IActionResult> GetDanhSachDatVe(int maPhim)
        {
            try
            {
                var ds = await _db.DanhSachDatVeOnlines
                    .Where(d => d.MaPhim == maPhim)
                    .ToListAsync();
                return Ok(ds);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateDS")]
        public async Task<IActionResult> UpdateDS(int id, [FromBody] DanhSachDatVeOnline dto)
        {
            try
            {
                var ds = await _db.DanhSachDatVeOnlines.FindAsync(id);
                if (ds == null) return NotFound();

                ds.MaKhachHang = dto.MaKhachHang;
                ds.MaXacNhan = dto.MaXacNhan;
                ds.MaPhim = dto.MaPhim;
                ds.SoLuongVeThuong = dto.SoLuongVeThuong;
                ds.SoLuongVeVip = dto.SoLuongVeVip;
                ds.TrangThaiDatVe = dto.TrangThaiDatVe;
                ds.MaXuatChieu = dto.MaXuatChieu;

                await _db.SaveChangesAsync();
             
                return Ok(new { Message = "Cập nhật thành công!", UpdatedObject =  await GetDanhSachDatVe(ds.MaPhim) });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}", Name = "DeleteDS")]
        public async Task<IActionResult> DeleteDS(int id)
        {
            try
            {
                var ds = await _db.DanhSachDatVeOnlines.FindAsync(id);
                if (ds == null) return NotFound();

                _db.DanhSachDatVeOnlines.Remove(ds);

                await _db.SaveChangesAsync();
                return Ok(new { Message = "Xóa thành công!", UpdatedObject = await GetDanhSachDatVe(ds.MaPhim) });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost(Name = "AddDS")]
        public async Task<IActionResult> AddDS([FromBody] DanhSachDatVeOnline dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { Message = "Dữ liệu không hợp lệ!" });
                }

                _db.DanhSachDatVeOnlines.Add(dto);

                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetDanhSachDatVe", new { maPhim = dto.MaPhim }, dto);
            }
            catch
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi thêm dữ liệu!" });
            }
        }

    }
}
