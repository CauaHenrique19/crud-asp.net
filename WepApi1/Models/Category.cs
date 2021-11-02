using System.ComponentModel.DataAnnotations;

namespace WepApi1.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string name { get; set; }

        [Required(ErrorMessage = "O campo color é obrigatório")]
        public string color { get; set; }

        [Required(ErrorMessage = "O campo icon é obrigatório")]
        public string icon { get; set; }

        public Category(int id, string name, string color, string icon)
        {
            this.id = id;
            this.name = name;
            this.color = color;
            this.icon = icon;
        }

        public Category(string name, string color, string icon)
        {
            this.name = name;
            this.color = color;
            this.icon = icon;
        }
    }
}
