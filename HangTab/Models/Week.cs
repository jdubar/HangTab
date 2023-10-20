using SQLite;

namespace HangTab.Models
{
    public class Week
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int BowlerId { get; set; }
        public int Hangings { get; set; }
    }
}
