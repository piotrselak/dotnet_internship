using contacts.Shared;

namespace contacts.Client.Domain;

public class AllCategoriesWithSub
{
    public Dictionary<Category, IEnumerable<SubCategory>> CategoryDictionary;
}