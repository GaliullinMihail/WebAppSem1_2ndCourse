namespace EnterPool.Http_Server.models;

public class Site
{
    public int Id { get; }
    public string Name { get; }
    public string Genre { get; }
    public string Description { get; }
    public string Path { get; }

    public Site(int id, string name, string genre, string description, string path)
    {
        Id = id;
        Name = name;
        Genre = genre;
        Description = description;
        Path = path;
    }
}