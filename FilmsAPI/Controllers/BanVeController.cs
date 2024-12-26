using FilmsAPI.Models;
using FilmsAPI.Services.BanVeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanVeController : ControllerBase
    {
        private IBanVeService banVeService;
        private FilmsDbContext _db;
        public BanVeController(IBanVeService service)
        {
            _db = new FilmsDbContext();
            banVeService = service;
        }

        [HttpPost("AddDetailBillForFoodRangeAsync")]
        public async Task<ActionResult> AddDetailBillForFoodRangeAsync([FromBody] List<DetailFood> detailFoods)
        {
            if (detailFoods == null)
            {
                return BadRequest(new { Message = "DetailFoods cannot be null or empty." });
            }

            try
            {
                var result = await banVeService.AddDetailBillForFoodRangeAsync(detailFoods);
                if (!result)
                {
                    return StatusCode(500, new { Message = "An error occurred while processing your request." });
                }

                return Ok(new { Message = "Success for save foods" });
            }
            catch (Exception ex)
            {   
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("GetKhachHangBySDT/{sdt}")]
        public async Task<ActionResult> GetKhachHang(string sdt)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sdt))
                {
                    return BadRequest("Số điện thoại là bắt buộc");
                }

                var result = await banVeService.GetKhachHangBySdt(sdt); // Sử dụng await để đảm bảo API trả về kết quả

                if (result == null)
                {
                    return NotFound("Không tìm thấy khách hàng này"); // Dùng NotFound thay vì BadRequest khi không tìm thấy
                }

                return Ok(result); // Trả về kết quả nếu tìm thấy khách hàng
            }
            catch (Exception ex)
            {
                // Log exception nếu cần và trả về thông báo lỗi chi tiết hơn
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        [HttpPut("UpdateStatusVe")]
        public async Task<ActionResult> UpdateStatusVe([FromBody] List<Ve> ve)
        {
            try
            {
                if (ve == null)
                {
                    return BadRequest("Không có vé nào được tìm thấy");
                }
                var result = await banVeService.UpdateStatusVe(ve);
                if (!result)
                {
                    return BadRequest("Thêm vé không thành công");
                }
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("GetVeBySuatchieu/{id}")]
        public async Task<ActionResult> GetVeTheoXuatChieu(int id)
        {
            try
            {
                var result = await banVeService.GetVeTheoSuatChieu(id);
                if (result == null) return BadRequest("Không tìm thấy vé tương ứng");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("Doan")]
        public async Task<ActionResult> GetDoAn()
        {
            var result = await _db.Foods
                .Include(p => p.Cate)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("GetFoodByCategory/{cateId}")]
        public async Task<ActionResult> GetFoodByCategory(int cateId)
        {
            try
            {
                if (cateId == 0)
                {
                    return BadRequest("Id không tìm thấy");
                }

                var result = await banVeService.GetFoodByCate(cateId);
                if (result == null) return BadRequest("Sản phẩm không tìm thấy");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("GetCateFood")]
        public async Task<ActionResult> GetCateFood()
        {
            try
            {
                var result = await banVeService.GetFoodCates();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("GetXuatChieu/{id}")]
        public async Task<ActionResult> GetXuatChieu(int id)
        {
            try
            {
                var xuatChieu = await banVeService.GetXuatChieuAsync(id);
                if (xuatChieu == null)
                {
                    return BadRequest(new { Message = "Không tìm thấy suất chiếu" });
                }

                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("GetVeTheoGhe/{maXuatChieu}")]
        public async Task<ActionResult> GetVe([FromBody] List<Ghe> listGhe, int maXuatChieu)
        {
            try
            {


                // Chuyển danh sách ID thành danh sách Ghế

                var listVe = await banVeService.GetVeTheoGhe(listGhe, maXuatChieu);
                if (listVe == null || !listVe.Any())
                {
                    return NotFound(new { Message = "Không tìm thấy vé" });
                }

                return Ok(listVe);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

        [HttpPost("AddBill")]
        public async Task<ActionResult> AddBill([FromBody] HoaDon model)
        {
            try
            {
                Console.WriteLine(JsonConvert.SerializeObject(model));

                var hoaDon = await banVeService.SaveBill(model);
                if (hoaDon == null)
                {
                    return BadRequest("Lưu không thành công");
                }

                return Ok(hoaDon);
            }
            catch (Exception ex)
            {
                return BadRequest("Lưu không thành công");

            }
        }


        [HttpPost("AddDetailBillRangeAsync")]
        public async Task<ActionResult> AddDetail([FromBody] List<ChiTietHoaDon> chiTiet)
        {
            try
            {
                // Kiểm tra đầu vào có hợp lệ không
                if (chiTiet == null || chiTiet.Count == 0)
                {
                    return BadRequest("Danh sách chi tiết hóa đơn không hợp lệ.");
                }

                // Gọi service để thêm chi tiết hóa đơn vào cơ sở dữ liệu
                var result = await banVeService.AddDetailBillRangeAsync(chiTiet);

                if (!result)
                {
                    return BadRequest("Lưu chi tiết hóa đơn không thành công.");
                }

                // Trả về kết quả thành công
                return Ok("Chi tiết hóa đơn đã được thêm thành công.");
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi chi tiết nếu có thể (ví dụ: sử dụng ILogger hoặc Console.WriteLine)
                Console.WriteLine($"Lỗi: {ex.Message}");

                // Trả về lỗi chi tiết nếu có exception
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }




    }
}
