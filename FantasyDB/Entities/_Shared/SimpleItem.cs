namespace FantasyDB.Entities._Shared

{
    public class SimpleItem_deprecated
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ForeignKeyId { get; set; }

        public SimpleItem_deprecated(int id, string name, int? foreignKeyId = null)
        {
            Id = id;
            Name = name;
            ForeignKeyId = foreignKeyId;
        }
    }
}
