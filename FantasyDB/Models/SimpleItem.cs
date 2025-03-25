
namespace FantasyDB.Models
{
    public class SimpleItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SimpleItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
