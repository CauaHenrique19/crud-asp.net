using System.Collections.Generic;
using WepApi1.Models;

namespace WepApi1.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public List<Product> GetByCategory(int categoryId);
        public Product Save(Product product);
        public Product Update(Product product);
        public void Delete(int id);
    }
}
