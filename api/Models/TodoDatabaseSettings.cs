namespace api.Models;

public class TodoDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string TodosCollectionName { get; set; } = null!;
}