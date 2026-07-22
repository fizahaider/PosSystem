using System;
using System.Collections.Generic;
using System.Text;
using POSSystem.Models;
using POSSystem.Helpers;

namespace POSSystem.Services
{
    public class Repository<T>
    {
        List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return items;
        }
    }
}
