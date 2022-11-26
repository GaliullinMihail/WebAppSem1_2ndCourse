using System.Text.Json;

namespace EnterPool.Http_Server.ServerLogic;

public static class ServerFileHandler
{
    public static ServerSettings ReadJsonSettings(string path)
    {
        if (File.Exists(path))
        {
            return JsonSerializer.Deserialize<ServerSettings>(File.ReadAllBytes(path)) ?? 
                   throw new InvalidDataException("JsonSetting exists but cannot be Deserialize");
        }
        return new ServerSettings();
    }
}