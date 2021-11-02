using System.Collections.Generic;
using WepApi1.Models;
using WepApi1.Repositories.CategoryRepository;

namespace WepApi1.useCases.ListAllCategories
{
    public class ListAllCategoriesUseCase
    {
        ICategoryRepository categoryRepository;

        public ListAllCategoriesUseCase(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public List<Category> execute()
        {
            var categories = categoryRepository.GetAll();
            return categories;
        }
    }
}
