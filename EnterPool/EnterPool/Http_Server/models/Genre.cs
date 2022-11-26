namespace EnterPool.Http_Server.models;

public class Genre
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }
    public string Example { get; }

    public Genre(int id, string title, string description, string example)
    {
        Id = id;
        Title = title;
        Description = description;
        Example = example;
    }
}