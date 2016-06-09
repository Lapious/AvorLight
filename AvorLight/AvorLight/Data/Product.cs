using SQLite;

namespace AvorLight.Data
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public float? StartingPrice { get; set; }
        public float? Price { get; set; }
        public float? SpecialPrice { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
    }
}
