using Npgsql;
using System;
using System.Collections.Generic;
using WepApi1.Models;

namespace WepApi1.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        Conexao conec = new Conexao();

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            NpgsqlCommand cmdSearch = new NpgsqlCommand(
                    "select " +
                        "products.id, " +
                        "products.name, " +
                        "products.price, " +
                        "products.description, " +
                        "products.key_image, " +
                        "products.image_url, " +
                        "products.category_id, " +
                        "categories.name as category_name " +
                    "from products " +
                    "inner join categories on categories.id = products.category_id " +
                    "order by id;");

            cmdSearch.Connection = conec.Conectar();
            NpgsqlDataReader reader = cmdSearch.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product(
                    Convert.ToInt32(reader["id"]),
                    Convert.ToString(reader["name"]),
                    Convert.ToDouble(reader["price"]),
                    Convert.ToString(reader["description"]),
                    Convert.ToString(reader["key_image"]),
                    Convert.ToString(reader["image_url"]),
                    Convert.ToInt32(reader["category_id"]),
                    Convert.ToString(reader["category_name"])));
            }

            conec.Desconectar();
            return products;
        }

        public List<Product> GetByCategory(int categoryId)
        {
            List<Product> products = new List<Product>();

            NpgsqlCommand cmdSearchById = new NpgsqlCommand(
                "select " +
                    "products.id," +
                    "products.name," +
                    "products.price," +
                    "products.description," +
                    "products.image_url," +
                    "products.key_image," +
                    "products.category_id," +
                    "categories.name as category_name " +
                "from products " +
                "inner join categories on categories.id = products.category_id " +
                $"where products.category_id = {categoryId} " +
                "order by id;", conec.Conectar());

            NpgsqlDataReader reader = cmdSearchById.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product(
                    Convert.ToInt32(reader["id"]),
                    Convert.ToString(reader["name"]),
                    Convert.ToDouble(reader["price"]),
                    Convert.ToString(reader["description"]),
                    Convert.ToString(reader["key_image"]),
                    Convert.ToString(reader["image_url"]),
                    Convert.ToInt32(reader["category_id"]),
                    Convert.ToString(reader["category_name"])));
            }

            conec.Desconectar();
            return products;
        }

        public Product Save(Product product)
        {
            NpgsqlCommand cmdCreate = new NpgsqlCommand($"insert into products values(default, @name, @price, @description, @key_image, @image_url, @category_id) returning id;", conec.Conectar());

            cmdCreate.Parameters.Clear();
            cmdCreate.Parameters.AddWithValue("@name", product.name);
            cmdCreate.Parameters.AddWithValue("@price", product.price);
            cmdCreate.Parameters.AddWithValue("@description", product.description);
            cmdCreate.Parameters.AddWithValue("@key_image", product.key_image);
            cmdCreate.Parameters.AddWithValue("@image_url", product.imageUrl);
            cmdCreate.Parameters.AddWithValue("@category_id", product.categoryId);

            product.id = Convert.ToInt32(cmdCreate.ExecuteScalar());
            conec.Desconectar();

            return product;
        }

        public Product Update(Product product)
        {
            NpgsqlCommand cmdUpdate = new NpgsqlCommand("update products set " +
                "name = @name, " +
                "price = @price, " +
                "description = @description, " +
                "image_url = @image_url, " +
                "key_image = @key_image, " +
                "category_id = @category_id " +
                "where id = @id", conec.Conectar());

            cmdUpdate.Parameters.Clear();
            cmdUpdate.Parameters.AddWithValue("@name", product.name);
            cmdUpdate.Parameters.AddWithValue("@price", product.price);
            cmdUpdate.Parameters.AddWithValue("@description", product.description);
            cmdUpdate.Parameters.AddWithValue("@image_url", product.imageUrl);
            cmdUpdate.Parameters.AddWithValue("@key_image", product.key_image);
            cmdUpdate.Parameters.AddWithValue("@category_id", product.categoryId);
            cmdUpdate.Parameters.AddWithValue("@id", product.id);

            cmdUpdate.ExecuteNonQuery();

            conec.Desconectar();

            return product;
        }

        public void Delete(int id)
        {
            NpgsqlCommand cmdDelete = new NpgsqlCommand($"delete from products where id = {id}", conec.Conectar());
            cmdDelete.ExecuteNonQuery();

            conec.Desconectar();
        }
    }
}
