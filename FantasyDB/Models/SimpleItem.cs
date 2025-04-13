namespace FantasyDB.Models
{
    public class SimpleItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ForeignKeyId { get; set; }

        public SimpleItem(int id, string name, int? foreignKeyId = null)
        {
            Id = id;
            Name = name;
            ForeignKeyId = foreignKeyId;
        }
    }
}
