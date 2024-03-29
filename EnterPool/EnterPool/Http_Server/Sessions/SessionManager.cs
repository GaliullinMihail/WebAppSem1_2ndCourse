﻿using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
namespace EnterPool.Http_Server.Sessions;

public static class SessionManager
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

    public static Guid CreateSession(int accountId, string login, DateTime created)
    {
        var session = new Session(Guid.NewGuid(), accountId, login, created);
        var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));
        _cache.Set(session.Id, session, cacheOptions);
        return session.Id;
    }   

    public static bool CheckSession(Guid id) => _cache.TryGetValue(id, out _);

    public static Session? GetSessionInfo(Guid id) => CheckSession(id) ? _cache.Get(id) as Session : null;

}