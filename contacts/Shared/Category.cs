namespace contacts.Shared;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<SubCategory>? SubCategory { get; set; }
}