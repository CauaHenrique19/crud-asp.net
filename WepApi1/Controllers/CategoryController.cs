using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WepApi1.Models;

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
            List<Category> categories = new List<Category>();

            NpgsqlCommand cmdSearch = new NpgsqlCommand("select * from categorias where \"deletadoEm\" isnull order by id", conec.Conectar());
            NpgsqlDataReader reader = cmdSearch.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Category(
                    Convert.ToInt32(reader["id"]), 
                    Convert.ToString(reader["nome"])));
            }

            conec.Desconectar();
            return categories;
        }

        [HttpPost]
        public MessageWithCategory Post(Category category)
        {
            try
            {
                NpgsqlCommand cmdInsert = new NpgsqlCommand("insert into categorias values(default, @name) returning id;", conec.Conectar());

                cmdInsert.Parameters.Clear();
                cmdInsert.Parameters.AddWithValue("@name", category.name);

                category.id = Convert.ToInt32(cmdInsert.ExecuteScalar());

                conec.Desconectar();
                return new MessageWithCategory() { message = "Categoria criada com sucesso!", category = category };
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
