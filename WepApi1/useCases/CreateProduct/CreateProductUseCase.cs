using System.Threading.Tasks;
using WepApi1.Models;
using WepApi1.Providers;
using WepApi1.Repositories.ProductRepository;

namespace WepApi1.useCases.CreateProduct
{
    public class CreateProductUseCase
    {
        IProductRepository productRepository;
        FileUploader awsFileUploader;

        public CreateProductUseCase(IProductRepository productRepository, FileUploader awsFileUploader)
        {
            this.productRepository = productRepository;
            this.awsFileUploader = awsFileUploader;
        }

        public async Task<Product> execute(ICreateProductDTO productDTO, IFile file)
        {   
            IUploadedFile uploadedFile = await awsFileUploader.upload(file);
            productDTO.key_image = uploadedFile.Key;
            productDTO.image_url = uploadedFile.Location;

            Product product = new Product(
                productDTO.name,
                productDTO.price,
                productDTO.description,
                productDTO.key_image,
                productDTO.image_url,
                productDTO.category_id
            );

            Product returnedProduct = this.productRepository.Save(product);
            return returnedProduct;
        }
    }
}
