using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoController : MonoBehaviour
{
    //用来存储想要在update上使用的函数委托
    public event UnityAction updateEvent;
    public event UnityAction fixUpdateEvent;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (updateEvent != null)
        {
            updateEvent();
        }
    }
    private void FixedUpdate()
    {
        if (fixUpdateEvent != null)
        {
            fixUpdateEvent();
        }
    }
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }
    public void AddFixUpdateListener(UnityAction fun)
    {
        fixUpdateEvent += fun;
    }
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }

    public void RemoveFixUpdateListener(UnityAction fun)
    {
        fixUpdateEvent -= fun;
    }
}
