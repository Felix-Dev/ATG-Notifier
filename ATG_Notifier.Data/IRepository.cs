using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATG_Notifier.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();

        T Get(int id);

        T Add(T obj);

        T Update(T obj);

        void Remove(T obj);

        bool Contains(T obj);
    }
}
