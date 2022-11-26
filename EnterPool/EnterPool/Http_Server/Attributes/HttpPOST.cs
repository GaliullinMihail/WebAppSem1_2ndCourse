namespace EnterPool.Http_Server.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class HttpPOST: Attribute
{
    public readonly string MethodUri;

    public HttpPOST(string methodUri = "")
    {
        MethodUri = methodUri;
    }
}