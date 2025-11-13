using SQLite;

namespace HangTab.Models;
[Table("People")]
public class Person
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2083)]
    public string? ImageUrl { get; set; } = null;

    public bool IsSub { get; set; }
    
    public bool IsDeleted { get; set; }
}
