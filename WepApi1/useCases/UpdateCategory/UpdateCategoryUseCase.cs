using WepApi1.Models;
using WepApi1.Repositories.CategoryRepository;

namespace WepApi1.useCases.UpdateCategory
{
    public class UpdateCategoryUseCase
    {
        ICategoryRepository categoryRepository;

        public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public Category execute(IUpdateCategoryDTO categoryDTO)
        {
            var categoryEntity = new Category(
                categoryDTO.id,
                categoryDTO.name,
                categoryDTO.color,
                categoryDTO.icon
            );

            var category = categoryRepository.Update(categoryEntity);
            return category;
        }
    }
}
