using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATG_Notifier.Data.Services
{
    interface IModelDataService<T>
    {
        IQueryable<T> Get();

        T Get(int id);

        T Update(T obj);

        void Remove(T obj);

        bool Contains(T obj);
    }
}
