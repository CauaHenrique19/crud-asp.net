using System.Collections.Generic;
using WepApi1.Models;

namespace WepApi1.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category Save(Category category);
        Category Update(Category category);
        void Delete(int id);
    }
}
