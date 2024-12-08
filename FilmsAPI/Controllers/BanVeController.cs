using FilmsAPI.Services.BanVeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanVeController : ControllerBase
    {
        private IBanVeService banVeService;
        public BanVeController(IBanVeService service)
        {
            banVeService = service;
        }

        [HttpGet("GetXuatChieu/{id}")]
        public async Task<ActionResult> GetXuatChieu(int id)
        {
            try
            {
                var xuatChieu = await banVeService.GetXuatChieuAsync(id);
                if (xuatChieu == null) {
                    return BadRequest(new {Message = "Không tìm thấy suất chiếu" });             
                }
                var listGhe = xuatChieu.MaPhongNavigation?.Ghes.ToList();

                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
    }
}
