using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance {get; private set;}

    public Queue<FruitsStateManager> FRUITQUEUE {get; private set;} = new Queue<FruitsStateManager>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void EnqueueFruit(FruitsStateManager fsmObject)
    {
        if (FRUITQUEUE == null) return;

        FRUITQUEUE.Enqueue(fsmObject);
        fsmObject.transform.SetParent(this.transform);
        fsmObject.gameObject.SetActive(false);
    }

    public FruitsStateManager DequeueFruit()
    {
        if (FRUITQUEUE == null) return null;
        
        return FRUITQUEUE.Dequeue();
    }
}
