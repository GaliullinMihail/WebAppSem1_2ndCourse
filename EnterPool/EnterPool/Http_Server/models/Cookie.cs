namespace EnterPool.Http_Server.models;

public class Cookie
{
    public string Guid { get; }
    public int IdAcc { get;}

    public Cookie(string guid, int id)
    {
        Guid = guid;
        IdAcc = id;
    }
}