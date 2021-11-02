using WepApi1.Models;
using WepApi1.Repositories.CategoryRepository;
using WepApi1.useCases.CreateCategory;

namespace WepApi1.useCases
{
    public class CreateCategoryUseCase
    {
        ICategoryRepository categoryRepository;

        public CreateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public Category execute(ICreateCategoryDTO categoryDTO)
        {
            var categoryEntity = new Category(
                categoryDTO.name,
                categoryDTO.color,
                categoryDTO.icon
            );

            var category = categoryRepository.Save(categoryEntity);
            return category;
        }
    }
}
