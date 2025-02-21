using System;
using UniRx;
using Unity.VisualScripting;
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
    LEMON,
    PEACH,
    PINEAPPLE,
    WATERMELON
}

public class FruitsStateManager : MonoBehaviour
{
    public FRUITSTATE state = FRUITSTATE.IDLE;
    public FRUIT fruit = FRUIT.APPLE;

    public GameObject[] fruits;
    public Rigidbody2D rigid;
    private GameManager gManager;

    void Start()
    {
        gManager = GameManager.Instance;
        if (gManager == null) return;

        //Init().SetFruitInfo(fruit);
    }

    public FruitsStateManager Init()
    {
        state = FRUITSTATE.IDLE;
        if (rigid != null) rigid.simulated = false;

        return this;
    }

    public void SetFruitInfo(FRUIT varFruit)
    {
        this.fruit = varFruit;
        int fIndex = (int)fruit;
        //rigid = fruits[fIndex].GetComponent<Rigidbody2D>();

        for (int i = 0; i < fruits.Length - 1; i++)
        {
            if (i == fIndex) fruits[i].SetActive(true);
            else fruits[i].SetActive(false);
        }
        
        rigid = GetComponentInChildren<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var col = collision.gameObject.GetComponent<FruitsStateManager>();
        if (col == null) return;

        if (col.fruit == this.fruit)
        {
            // 둘 중 더 아래에 있는 과일 기준으로 작동(둘 중 하나만 작동되도록 하는 문제도 해결)
            if (col.transform.position.y > this.transform.position.y) return;
            // y 값이 같을 경우 중복 작동하는 경우 해결
            else if (col.transform.position.y == this.transform.position.y && col.transform.position.x > this.transform.position.x) return;
            if (this.fruit == FRUIT.WATERMELON) return;

            Vector2 nextFruitPos = new Vector2((col.transform.position.x + this.transform.position.x) / 2, (col.transform.position.y + this.transform.position.y) / 2);

            col.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            //// Enqueue
            //gManager.fruitsPool.Enqueue(col.gameObject);
            //gManager.fruitsPool.Enqueue(this.gameObject);

            MessageBroker.Default.Publish<InstNewObject>(new InstNewObject(this.fruit, nextFruitPos));
            // == gManager.GetNextFruit(this.fruit, nextFruitPos);
        }

    }
}
