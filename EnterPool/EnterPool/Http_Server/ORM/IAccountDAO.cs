﻿using EnterPool.Http_Server.models;

namespace EnterPool.Http_Server.ORM;

public interface IAccountDAO
{
    public IEnumerable<Account> GetAll();
    public Account? GetById(int id);
    public void Insert(string login, string password);
}