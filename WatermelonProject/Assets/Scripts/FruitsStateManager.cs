using UniRx;
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
    public FRUIT fruit = FRUIT.APPLE;

    public Rigidbody2D rigid;

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var col = collision.gameObject.GetComponent<FruitsStateManager>();
        if (col == null) return;

        // 두 개의 오브젝트가 충돌해서 하위 행동이 2번 일어나서 계속 합쳐지는 현상이 있어서
        // 어떻게 한 번만 작동되게 할것인지 고려가 필요함.

        if (col.fruit == this.fruit)
        {
            Debug.Log("Crash Same Fruit");
            Vector2 nextFruitPos = new Vector2((col.transform.position.x + this.transform.position.x) / 2, (col.transform.position.y + this.transform.position.y) / 2);

            Debug.Log("Erase Collision Fruits");
            col.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            Debug.Log("Create New Fruit");
            MessageBroker.Default.Publish<InstNewObject>(new InstNewObject(this.fruit, nextFruitPos));
        }

    }
}
