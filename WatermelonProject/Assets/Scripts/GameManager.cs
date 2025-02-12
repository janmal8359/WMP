using System;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GAMESTATE
{
    IDLE,
    PLAYING,
    END
}

public class GameManager : MonoBehaviour
{
    public GAMESTATE state = GAMESTATE.IDLE;

    public GameObject[] fruits;
    public GameObject nextFruit;
    public Transform trFruitsPool;
    public Transform trStage;
    public GameObject startPage;
    public Button btnStart;


    public BoxCollider2D deadLine;

    public int maxIndex = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < fruits.Length - 1; i++)
        {
            if (fruits[i] == null)
            {
                Debug.LogWarning("Fruits is not suitable");
                return;
            }
        }

        // Set Button
        btnStart.OnClickAsObservable().Subscribe(_ => 
        {
            state = GAMESTATE.PLAYING;
            startPage.SetActive(false);

            GetNextFruit();
        });

        MessageBroker.Default.Receive<InstNewObject>().Subscribe(_ =>
        {
            GetNextFruit(_.fruit, _.pos);
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GAMESTATE.PLAYING)
        {
            // Dead 판정
        }
    }

    public void GetNextFruit()
    {
        int fIndex = UnityEngine.Random.Range(0, maxIndex);

        nextFruit = Instantiate(fruits[fIndex], trStage);
    }

    public void GetNextFruit(FRUIT fruitIndex, Vector2 pos)
    {
        int fIndex = (int)fruitIndex;//.ConvertTo<Int32>();

        nextFruit = Instantiate((fruits[++fIndex]), trStage);
        nextFruit.transform.position = pos;
        nextFruit.GetComponent<Rigidbody2D>().simulated = true;
    }
}
