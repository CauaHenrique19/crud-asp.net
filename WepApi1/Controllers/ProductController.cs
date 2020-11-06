using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using WepApi1.Models;

namespace WepApi1.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        Conexao conec = new Conexao();

        public class MessageWithProduct
        {
            #nullable disable
            public string message;
            #nullable enable
            public Product? product;
        }

        [HttpGet]
        public List<Product> Get()
        {
            List<Product> products = new List<Product>();

            NpgsqlCommand cmdSearch = new NpgsqlCommand(
                "select " + 
                    "produtos.id," +
                    "produtos.nome," +
                    "produtos.preco," +
                    "produtos.descricao," +
                    "produtos.\"imagemUrl\"," +
                    "produtos.\"categoriaId\"," + 
                    "categorias.nome as \"categoriaNome\"" +
                "from produtos " +
                "inner join categorias on categorias.id = produtos.\"categoriaId\" " + 
                "where produtos.\"deletadoEm\" isnull " +
                "order by id;", conec.Conectar());
            NpgsqlDataReader reader = cmdSearch.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product(
                    Convert.ToInt32(reader["id"]), 
                    Convert.ToString(reader["nome"]), 
                    Convert.ToDouble(reader["preco"]), 
                    Convert.ToString(reader["descricao"]), 
                    Convert.ToString(reader["imagemUrl"]), 
                    Convert.ToInt32(reader["categoriaId"]),
                    Convert.ToString(reader["categoriaNome"])));
            }

            conec.Desconectar();
            return products;
        }

        [HttpGet]
        [Route("{categoryId}")]
        public List<Product> getById(int categoryId)
        {
            List<Product> products = new List<Product>();

            NpgsqlCommand cmdSearchById = new NpgsqlCommand(
                "select " +
                    "produtos.id," +
                    "produtos.nome," +
                    "produtos.preco," +
                    "produtos.descricao," +
                    "produtos.\"imagemUrl\"," +
                    "produtos.\"categoriaId\"," +
                    "categorias.nome as \"categoriaNome\"" +
                "from produtos " +
                "inner join categorias on categorias.id = produtos.\"categoriaId\" " +
                $"where produtos.\"categoriaId\" = {categoryId} and produtos.\"deletadoEm\" isnull " +
                "order by id;", conec.Conectar());

            NpgsqlDataReader reader = cmdSearchById.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product(
                    Convert.ToInt32(reader["id"]),
                    Convert.ToString(reader["nome"]),
                    Convert.ToDouble(reader["preco"]),
                    Convert.ToString(reader["descricao"]),
                    Convert.ToString(reader["imagemUrl"]),
                    Convert.ToInt32(reader["categoriaId"]),
                    Convert.ToString(reader["categoriaNome"])));
            }

            conec.Desconectar();
            return products;
        }

        [HttpPost]
        public MessageWithProduct Post(Product p)
        {
            try
            {
                NpgsqlCommand cmdCreate = new NpgsqlCommand($"insert into produtos values(default, @name, @price, @description, @imageUrl, @categoryId) returning id;", conec.Conectar());
                
                cmdCreate.Parameters.Clear();
                cmdCreate.Parameters.AddWithValue("@name", p.name);
                cmdCreate.Parameters.AddWithValue("@price", p.price);
                cmdCreate.Parameters.AddWithValue("@description", p.description);
                cmdCreate.Parameters.AddWithValue("@imageUrl", p.imageUrl);
                cmdCreate.Parameters.AddWithValue("@categoryId", p.categoryId);

                if (ModelState.IsValid)
                {
                    p.id = Convert.ToInt32(cmdCreate.ExecuteScalar());
                    
                    NpgsqlCommand cmdSearchCategoryName = new NpgsqlCommand($"select nome from categorias where id = {p.categoryId};", conec.Conectar());
                    NpgsqlDataReader reader = cmdSearchCategoryName.ExecuteReader();
                    
                    reader.Read();
                    
                    p.categoryName = Convert.ToString(reader["nome"]);
                    conec.Desconectar();
                }

                return new MessageWithProduct() { message = $"Produto cadastrado com sucesso!", product = p };
            }
            catch(NpgsqlException error)
            {
                return new MessageWithProduct() { message = $"Erro ao cadastrar um produto: ${error.Message}" };
            }
        }

        [HttpPut]
        [Route("{id}")]
        public MessageWithProduct Put(int id, Product p)
        {
            try
            {
                NpgsqlCommand cmdUpdate = new NpgsqlCommand("update produtos set " +
                    "nome = @name, " +
                    "preco = @price, " +
                    "descricao = @description, " +
                    "\"imagemUrl\" = @imageUrl, " +
                    "\"categoriaId\" = @categoryId " +
                    "where id = @id", conec.Conectar());

                cmdUpdate.Parameters.Clear();
                cmdUpdate.Parameters.AddWithValue("@name", p.name);
                cmdUpdate.Parameters.AddWithValue("@price", p.price);
                cmdUpdate.Parameters.AddWithValue("@description", p.description);
                cmdUpdate.Parameters.AddWithValue("@imageUrl", p.imageUrl);
                cmdUpdate.Parameters.AddWithValue("@categoryId", p.categoryId);
                cmdUpdate.Parameters.AddWithValue("@id", id);

                if (ModelState.IsValid)
                {
                    cmdUpdate.ExecuteNonQuery();

                    NpgsqlCommand cmdSearchCategoryName = new NpgsqlCommand($"select nome from categorias where id = {p.categoryId};", conec.Conectar());
                    NpgsqlDataReader reader = cmdSearchCategoryName.ExecuteReader();

                    reader.Read();

                    p.id = id;
                    p.categoryName = Convert.ToString(reader["nome"]);

                    conec.Desconectar();
                }
                return new MessageWithProduct() { message = $"Produto atualizado com sucesso!", product = p };
            }
            catch(NpgsqlException error)
            {
                return new MessageWithProduct() { message = $"Erro ao atualizar um produto: {error.Message}" };
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public string Delete(int id)
        {
            try
            {
                NpgsqlCommand cmdDelete = new NpgsqlCommand($"delete from produtos where id = {id}", conec.Conectar());
                cmdDelete.ExecuteNonQuery();

                conec.Desconectar();
                return "Produto excluído com sucesso!";
            }
            catch(NpgsqlException error)
            {
                return $"Erro ao excluir um produto: {error.Message}";
            }
        }
    }
}
