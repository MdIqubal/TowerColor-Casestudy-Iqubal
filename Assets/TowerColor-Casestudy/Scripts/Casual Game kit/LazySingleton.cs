//Author : Iqubal
//email : iqubal333@gmail.com

using UnityEngine;

namespace PnC.CasualGameKit
{
    /// <summary>
    /// Creates a singleton when required for a class derived from this.
    /// </summary>
    public class LazySingleton<T> : MonoBehaviour where T : Component
    {
        static T instance;

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