using System.Collections.Generic;
using WepApi1.Models;
using WepApi1.Repositories.ProductRepository;

namespace WepApi1.useCases.ListAllProducts
{
    public class ListAllProductsUseCase
    {
        IProductRepository productRepository;

        public ListAllProductsUseCase(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public List<Product> execute()
        {
            List<Product> products = productRepository.GetAll();
            return products;
        }
    }
}
