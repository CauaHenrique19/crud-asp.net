using Npgsql;
using System;
using System.Collections.Generic;
using WepApi1.Models;

namespace WepApi1.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        Conexao conec = new Conexao();

        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            NpgsqlCommand cmdSearch = new NpgsqlCommand("select * from categories order by id", conec.Conectar());
            NpgsqlDataReader reader = cmdSearch.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Category(
                    Convert.ToInt32(reader["id"]),
                    Convert.ToString(reader["name"]),
                    Convert.ToString(reader["color"]),
                    Convert.ToString(reader["icon"])
                ));
            }

            conec.Desconectar();
            return categories;
        }

        public Category Save(Category category)
        {
            NpgsqlCommand cmdInsert = new NpgsqlCommand("insert into categories values(default, @name, @color, @icon) returning id;", conec.Conectar());

            cmdInsert.Parameters.Clear();
            cmdInsert.Parameters.AddWithValue("@name", category.name);
            cmdInsert.Parameters.AddWithValue("@color", category.color);
            cmdInsert.Parameters.AddWithValue("@icon", category.icon);

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
