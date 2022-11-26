namespace EnterPool.Http_Server.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HttpController : Attribute
{
    public readonly string ControllerName;

    public HttpController(string controllerName)
    {
        ControllerName = controllerName;
    }
}