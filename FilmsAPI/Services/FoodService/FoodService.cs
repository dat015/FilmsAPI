    using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Services.FoodService
{
    public class FoodService : IFoodService
    {
        private readonly FilmsDbContext _db;
        public FoodService(FilmsDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Add(Food model)
        {

            if (model == null)
            {
                return false;
            }

            try
            {
                var existingFood = await _db.Foods.FirstOrDefaultAsync(f => f.Name == model.Name);
                if (existingFood != null)
                {
                    return false; 
                }

                await _db.Foods.AddAsync(model);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            if (id <= 0)
            {
                return false; 
            }
            
            try
            {
                var food = await _db.Foods.FindAsync(id);
                if (food == null)
                {
                    return false; 
                }

                _db.Foods.Remove(food);
                await _db.SaveChangesAsync(); 

                return true; 
            }
            catch
            {
                return false; 
            }
        }


        public async Task<bool> Update(Food model)
        {
            if (model == null)
            {
                return false;
            }

            try
            {
                var food = await _db.Foods.FindAsync(model.Id);
                if (food == null)
                {
                    return false;
                }

                food.Name = model.Name;
                food.Description = model.Description;
                food.CateId = model.CateId;
                food.Price = model.Price;
                food.ImageUrl = model.ImageUrl;

                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
          
        }
    }
}
