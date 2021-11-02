using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WepApi1.Models;
using WepApi1.Repositories.CategoryRepository;
using WepApi1.useCases;
using WepApi1.useCases.CreateCategory;
using WepApi1.useCases.ListAllCategories;
using WepApi1.useCases.UpdateCategory;

namespace WepApi1.Controllers
{
    [Route("category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        Conexao conec = new Conexao();

        public class MessageWithCategory 
        {
            #nullable disable
            public string message;
            #nullable enable
            public Category? category;
        }

        [HttpGet]
        public List<Category> Get()
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            ListAllCategoriesUseCase listAllCategoriesUseCase = new ListAllCategoriesUseCase(categoryRepository);
            var categories = listAllCategoriesUseCase.execute();
            return categories;
        }

        [HttpPost]
        public MessageWithCategory Post(ICreateCategoryDTO category)
        {
            try
            {
                CategoryRepository categoryRepository = new CategoryRepository();
                CreateCategoryUseCase createCategoryUseCase = new CreateCategoryUseCase(categoryRepository);
                var categoryDb = createCategoryUseCase.execute(category);
                return new MessageWithCategory() { message = "Categoria criada com sucesso!", category = categoryDb };
            }
            catch(NpgsqlException error)
            {
                return new MessageWithCategory() { message = $"Erro ao criar categoria: {error.Message}" };
            }
        }

        [HttpPut]
        public MessageWithCategory Put(IUpdateCategoryDTO category)
        {
            try
            {
                CategoryRepository categoryRepository = new CategoryRepository();
                UpdateCategoryUseCase updateCategoryUseCase = new UpdateCategoryUseCase(categoryRepository);
                var categoryUpdated = updateCategoryUseCase.execute(category);
                return new MessageWithCategory() { message = "Categoria atualizada com sucesso!", category = categoryUpdated };
            }
            catch(NpgsqlException error)
            {
                return new MessageWithCategory() { message = $"Erro ao atualizar categoria: {error.Message}" };
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public string Delete(int id)
        {
            try
            {
                NpgsqlCommand cmdDelete = new NpgsqlCommand($"delete from categorias where id = {id}", conec.Conectar());
                cmdDelete.ExecuteNonQuery();

                conec.Desconectar();
                return $"Categoria excluída com sucesso!";
            }
            catch(NpgsqlException error)
            {
                return $"Erro ao excluir uma categoria: {error.Message}";
            }
        }
    }
}
