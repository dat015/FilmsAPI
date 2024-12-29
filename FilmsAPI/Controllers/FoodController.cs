using FilmsAPI.Filters;
using FilmsAPI.Models;
using FilmsAPI.Services.BanVeService;
using FilmsAPI.Services.FoodService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]

    public class FoodController : ControllerBase
    {
        private IFoodService sv;
        private FilmsDbContext _db;
        public FoodController(IFoodService service)
        {
            _db = new FilmsDbContext();
            sv = service;
        }

        [HttpPut("UpdateFood")]
        public async Task<ActionResult> Update([FromBody] Food model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Food is null!" });
            }

            try
            {
                var result = await sv.Update(model);
                if (!result)
                {
                    return BadRequest(new { Message = "Sửa không thành công" });
                }
                return Ok(new { Message = "Sửa thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });


            }
        }
        [HttpPost("AddFood")]
        public async Task<ActionResult> Add([FromBody] Food model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Food is null!" });
            }

            try
            {
                var result = await sv.Add(model);
                if (!result)
                {
                    return BadRequest(new { Message = "Thêm món ăn không thành công" });
                }
                return Ok(new { Message = "Thêm món ăn thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("DeleteFood/{id}")]
        public async Task<bool> Delete(int id)
        {
            if (id <= 0)
            {
                return false; 
            }

            try
            {
                var result = await sv.Delete(id); 
                return result; 
            }
            catch (Exception)
            {
                return false; 
            }
        }



    }
}
