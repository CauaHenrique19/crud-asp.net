using WepApi1.Repositories.CategoryRepository;

namespace WepApi1.useCases.DeleteCategory
{
    public class DeleteCategoryUseCase
    {
        ICategoryRepository categoryRepository;

        public DeleteCategoryUseCase(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public void execute(IDeleteCategoryDTO deleteCategoryDTO)
        {
            categoryRepository.Delete(deleteCategoryDTO.id);
        }
    }
}
