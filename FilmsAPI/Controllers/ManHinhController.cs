using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models; 
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManHinhController : ControllerBase
    {
        private readonly FilmsDbContext _context; 

        public ManHinhController()
        {
            _context = new FilmsDbContext();
        }

        // Lấy tất cả màn hình
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManHinh>>> GetManHinh()
        {
            var manHinhs = await _context.ManHinhs
                .Include(mh => mh.DangPhims)  // Nếu bạn muốn bao gồm các DangPhim liên quan
                .Include(mh => mh.PhongChieus) // Nếu bạn muốn bao gồm các PhongChieu liên quan
                .ToListAsync();

            return Ok(manHinhs);
        }

        // Lấy màn hình theo MaManHinh
        [HttpGet("{mamanhinh}")]
        public async Task<ActionResult<ManHinh>> GetManHinh(int mamanhinh)
        {
            var manHinh = await _context.ManHinhs
                .Include(mh => mh.DangPhims)  // Bao gồm DangPhim liên quan
                .Include(mh => mh.PhongChieus) // Bao gồm PhongChieu liên quan
                .FirstOrDefaultAsync(mh => mh.MaManHinh == mamanhinh);

            if (manHinh == null)
            {
                return NotFound();
            }

            return Ok(manHinh);
        }

        // Thêm màn hình mới
        [HttpPost]
        public async Task<ActionResult<ManHinh>> PostManHinh(ManHinh manHinh)
        {
            _context.ManHinhs.Add(manHinh);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetManHinh), new { mamanhinh = manHinh.MaManHinh }, manHinh);
        }

        // Cập nhật màn hình
        [HttpPut("{mamanhinh}")]
        public async Task<IActionResult> PutManHinh(int mamanhinh, ManHinh manHinh)
        {
            if (mamanhinh != manHinh.MaManHinh)
            {
                return BadRequest();
            }

            _context.Entry(manHinh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManHinhExists(mamanhinh))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Xóa màn hình
        [HttpDelete("{mamanhinh}")]
        public async Task<IActionResult> DeleteManHinh(int mamanhinh)
        {
            var manHinh = await _context.ManHinhs.FindAsync(mamanhinh);

            if (manHinh == null)
            {
                return NotFound();
            }

            _context.ManHinhs.Remove(manHinh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Kiểm tra sự tồn tại của màn hình
        private bool ManHinhExists(int mamanhinh)
        {
            return _context.ManHinhs.Any(e => e.MaManHinh == mamanhinh);
        }
    }
}
