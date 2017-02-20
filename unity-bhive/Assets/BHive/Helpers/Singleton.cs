using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHive.Helpers
{
    public class Singleton<T> where T : class, new()
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}
