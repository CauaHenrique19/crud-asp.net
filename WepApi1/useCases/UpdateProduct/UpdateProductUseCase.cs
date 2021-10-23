using System.Threading.Tasks;
using WepApi1.Models;
using WepApi1.Providers;
using WepApi1.Repositories.ProductRepository;

namespace WepApi1.useCases.UpdateProduct
{
    public class UpdateProductUseCase
    {
        IProductRepository productRepository;
        FileUploader awsFileUploader;

        public UpdateProductUseCase(IProductRepository productRepository, FileUploader awsFileUploader)
        {
            this.productRepository = productRepository;
            this.awsFileUploader = awsFileUploader;
        }

        public async Task<Product> execute(IUpdateProductDTO productDTO, IUpdateFile file)
        {
            await awsFileUploader.update(file);

            Product product = new Product(
                productDTO.id,
                productDTO.name,
                productDTO.price,
                productDTO.description,
                productDTO.key_image,
                productDTO.image_url,
                productDTO.category_id
            );

            Product returnedProduct = productRepository.Update(product);
            return returnedProduct;
        }
    }
}
