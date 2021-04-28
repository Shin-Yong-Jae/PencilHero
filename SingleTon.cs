using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SingleTon<T> :MonoBehaviour where T : class, new()
{
    protected static T instance = null;
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


  /*  private void DontDestroy()
    {
        DontDestroyOnLoad(gameObject);
    }*/
}



