using System.Collections.Generic;
using WepApi1.Models;
using WepApi1.Repositories.ProductRepository;

namespace WepApi1.useCases.ListAllProductsByCategoryIdUseCase
{
    public class ListAllProductsByCategoryIdUseCase
    {
        IProductRepository productRepository;

        public ListAllProductsByCategoryIdUseCase(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public List<Product> execute(int categoryId)
        {
            List<Product> products = productRepository.GetByCategory(categoryId);
            return products;
        }
    }
}
