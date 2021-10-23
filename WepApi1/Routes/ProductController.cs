using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WepApi1.Models;
using WepApi1.Providers;
using WepApi1.Providers.Implementations;
using WepApi1.Repositories.ProductRepository;
using WepApi1.useCases.CreateProduct;
using WepApi1.useCases.DeleteProduct;
using WepApi1.useCases.ListAllProducts;
using WepApi1.useCases.ListAllProductsByCategoryIdUseCase;
using WepApi1.useCases.UpdateProduct;

namespace WepApi1.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ProductRepository productRepository = new ProductRepository();
        AWSUploadProvider awsFileUploader = new AWSUploadProvider();

        [HttpGet]
        public List<Product> Get()
        {
            ListAllProductsUseCase listAllProductsUseCase = new ListAllProductsUseCase(productRepository);
            List<Product> products = listAllProductsUseCase.execute();
            return products;

        }

        [HttpGet]
        [Route("{categoryId}")]
        public List<Product> getById(int categoryId)
        {
            ListAllProductsByCategoryIdUseCase listAllProductsByCategoryIdUseCase = new ListAllProductsByCategoryIdUseCase(productRepository);
            List<Product> products = listAllProductsByCategoryIdUseCase.execute(categoryId);
            return products;
        }

        [HttpPost]
        public async Task<Product> Post([FromForm] IFormFile file, [FromForm] string name, [FromForm] double price, [FromForm] string description, [FromForm] int category_id)
        {
            CreateProductUseCase createProductUseCase = new CreateProductUseCase(productRepository, awsFileUploader);

            IFile fileToUpload = new IFile();
            fileToUpload.name = file.FileName;
            fileToUpload.type = file.ContentType;
            fileToUpload.content = file.OpenReadStream();

            ICreateProductDTO productDTO = new ICreateProductDTO();
            productDTO.name = name;
            productDTO.price = price;
            productDTO.description = description;
            productDTO.category_id = category_id;

            Product returnedProduct = await createProductUseCase.execute(productDTO, fileToUpload);
            return returnedProduct;
        }

        [HttpPut]
        public async Task<Product> Put([FromForm] IFormFile file, [FromForm] int id, [FromForm] string name, [FromForm] double price, [FromForm] string description, [FromForm] string key_image, [FromForm] string image_url, [FromForm] int category_id)
        {
            UpdateProductUseCase updateProductUseCase = new UpdateProductUseCase(productRepository, awsFileUploader);

            IUpdateFile fileToUpdate = new IUpdateFile();
            fileToUpdate.key = key_image;
            fileToUpdate.type = file.ContentType;
            fileToUpdate.content = file.OpenReadStream();

            IUpdateProductDTO productDTO = new IUpdateProductDTO();
            productDTO.id = id;
            productDTO.name = name;
            productDTO.price = price;
            productDTO.description = description;
            productDTO.key_image = key_image;
            productDTO.image_url = image_url;
            productDTO.category_id = category_id;

            Product updatedProduct = await updateProductUseCase.execute(productDTO, fileToUpdate);
            return updatedProduct;
        }

        [HttpDelete]
        public string Delete(IDeleteProductDTO deleteProductDTO)
        {
            DeleteProductUseCase deleteProductUseCase = new DeleteProductUseCase(productRepository, awsFileUploader);
            deleteProductUseCase.execute(deleteProductDTO.key, deleteProductDTO.id);

            return "Produto Excluído com sucesso!";
        }
    }
}
