using FilmsAPI.Models;

namespace FilmsAPI.Services.FoodService
{
    public interface IFoodService
    {
        Task<bool> Update(Food model);
        Task<bool> Add(Food model);
        Task<bool> Delete(int id);
    }
}
