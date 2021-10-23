using System.ComponentModel.DataAnnotations;

namespace WepApi1.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string name { get; set; }

        [Required(ErrorMessage = "O campo preço é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
        public double price { get; set; }

        [Required(ErrorMessage = "O campo descrição é obrigatório")]
        public string description { get; set; }

        [Required(ErrorMessage = "O campo imagemUrl é obrigatório")]
        public string imageUrl { get; set; }

        [Required(ErrorMessage = "O campo keyImage é obrigatório")]
        public string key_image { get; set; }

        [Required(ErrorMessage = "O campo categoriaId é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "A categoriaId deve ser maior que 0")]
        public int categoryId { get; set; }

        public string categoryName { get; set; }

        public Product(int id, string name, double price, string description, string key_image, string imageUrl, int categoryId, string categoryName)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.description = description;
            this.key_image = key_image;
            this.imageUrl = imageUrl;
            this.categoryId = categoryId;
            this.categoryName = categoryName;
        }

        public Product(int id, string name, double price, string description, string key_image, string imageUrl, int categoryId)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.description = description;
            this.key_image = key_image;
            this.imageUrl = imageUrl;
            this.categoryId = categoryId;
        }

        public Product(int id, string name, double price, string description, string imageUrl, int categoryId, string categoryName)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.description = description;
            this.imageUrl = imageUrl;
            this.categoryId = categoryId;
            this.categoryName = categoryName;
        }

        public Product(string name, double price, string description, string key_image, string imageUrl, int categoryId)
        {
            this.name = name;
            this.price = price;
            this.description = description;
            this.imageUrl = imageUrl;
            this.key_image = key_image;
            this.categoryId = categoryId;
        }
    }
}
