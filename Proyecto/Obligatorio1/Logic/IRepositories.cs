﻿namespace Logic;

public interface IRepositories<T>
{
    public void AddToRepository(T item);
    public void RemoveFromRepository(T item);
    public bool ExistsInRepository(T item);
    public T GetFromRepository(int id);
    
}