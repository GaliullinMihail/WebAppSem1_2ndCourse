namespace EnterPool.Http_Server.models;

public class FavoriteSite
{
    public int Id { get; }
    public string Site { get; }
    public string Genre { get; }

    public FavoriteSite(int id , string site, string genre)
    {
        Id = id;
        Site = site;
        Genre = genre;
    }
}