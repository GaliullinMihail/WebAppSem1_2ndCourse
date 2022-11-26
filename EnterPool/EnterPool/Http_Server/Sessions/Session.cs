namespace EnterPool.Http_Server.Sessions;

public class Session
{
    public Guid Id { get; }
    public int AccountId { get; }
    public string Login { get; }
    public DateTime DateTime { get; }

    public Session(Guid id, int accountId, string login, DateTime dateTime)
    {
        Id = id;
        AccountId = accountId;
        Login = login;
        DateTime = dateTime;
    }
    
}