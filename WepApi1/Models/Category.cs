namespace WepApi1.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }

        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
