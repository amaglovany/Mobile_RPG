using UnityEngine;

namespace TDC
{
    public class Singleton<T> where T : new()
    {
        protected static T instance;

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

        public static void Delete()
        {
            Debug.Log("Deleting Singleton with type: " + typeof(T));
            instance = default(T);
        }
    }
}