using UnityEngine;

namespace WhalesAndGames.MapGame.Singletons
{
    /// <summary>
    /// Defines a singleton behaviour which extends Monobehaviour to be it's own instance type.
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }

                return instance;
            }
        }
    }
}