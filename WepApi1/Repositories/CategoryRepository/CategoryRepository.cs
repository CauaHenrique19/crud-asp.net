using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepApi1.Models;

namespace WepApi1.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        Conexao conec = new Conexao();

        public List<Category> GetAll()
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

        public Category Save(Category category)
        {
            NpgsqlCommand cmdInsert = new NpgsqlCommand("insert into categorias values(default, @name) returning id;", conec.Conectar());

            cmdInsert.Parameters.Clear();
            cmdInsert.Parameters.AddWithValue("@name", category.name);

            category.id = Convert.ToInt32(cmdInsert.ExecuteScalar());

            conec.Desconectar();
            return category;
        }

        public Category Update(Category category)
        {
            NpgsqlCommand cmdUpdate = new NpgsqlCommand("update categorias set nome = @name where id = @id", conec.Conectar());

            cmdUpdate.Parameters.Clear();
            cmdUpdate.Parameters.AddWithValue("@name", category.name);
            cmdUpdate.Parameters.AddWithValue("@id", category.id);

            cmdUpdate.ExecuteNonQuery();

            conec.Desconectar();
            return category;
        }

        public void Delete(int id)
        {
            NpgsqlCommand cmdDelete = new NpgsqlCommand($"delete from categorias where id = {id}", conec.Conectar());
            cmdDelete.ExecuteNonQuery();

            conec.Desconectar();
        }
    }
}
