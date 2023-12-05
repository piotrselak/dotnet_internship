namespace contacts.Shared;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public SubCategory? SubCategory { get; set; }
}