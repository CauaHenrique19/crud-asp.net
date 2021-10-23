using WepApi1.Providers;
using WepApi1.Repositories.ProductRepository;

namespace WepApi1.useCases.DeleteProduct
{
    public class DeleteProductUseCase
    {
        IProductRepository productRepository;
        FileUploader awsFileUploader;

        public DeleteProductUseCase(IProductRepository productRepository, FileUploader awsFileUploader)
        {
            this.productRepository = productRepository;
            this.awsFileUploader = awsFileUploader;
        }

        public async void execute(string key, int id)
        {
            await awsFileUploader.delete(key);
            productRepository.Delete(id);
        }
    }
}
