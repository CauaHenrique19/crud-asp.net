using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WepApi1.Models;
using WepApi1.Repositories.CategoryRepository;
using WepApi1.useCases;
using WepApi1.useCases.CreateCategory;
using WepApi1.useCases.ListAllCategories;

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
        [Route("{id}")]
        public MessageWithCategory Put(int id, Category category)
        {
            try
            {
                NpgsqlCommand cmdUpdate = new NpgsqlCommand("update categorias set nome = @name where id = @id", conec.Conectar());

                cmdUpdate.Parameters.Clear();
                cmdUpdate.Parameters.AddWithValue("@name", category.name);
                cmdUpdate.Parameters.AddWithValue("@id", id);

                cmdUpdate.ExecuteNonQuery();

                conec.Desconectar();
                return new MessageWithCategory() { message = "Categoria atualizada com sucesso!", category = category };
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
