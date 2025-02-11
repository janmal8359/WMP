using UnityEngine;

public enum FRUITSTATE
{
    IDLE,
    DROP
}

public enum FRUIT
{
    APPLE = 0,
    ORANGE,
    KIWI,
    
}

public class FruitsStateManager : MonoBehaviour
{
    public FRUITSTATE state = FRUITSTATE.IDLE;

    public Rigidbody2D rigid;

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
    }

}
