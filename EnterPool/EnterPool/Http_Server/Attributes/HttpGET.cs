namespace EnterPool.Http_Server.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class HttpGET: Attribute
{
    public readonly string MethodUri;

    public HttpGET(string methodUri = "")
    {
        MethodUri = methodUri;
    }
}