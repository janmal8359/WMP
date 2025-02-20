using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public enum GAMESTATE
{
    IDLE,
    PLAYING,
    WAIT,   // 게임 오버 체크 확인
    END
}

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance {get; private set;}
    DataManager dataManager;

    public GAMESTATE state = GAMESTATE.IDLE;

    public GameObject[] fruits;
    public GameObject nextFruit;
    public Transform trFruits;
    [HideInInspector]
    public Queue<GameObject> fruitsPool = new Queue<GameObject>();
    public Transform trStage;
    public GameObject startPage;
    public Button btnStart;
    public GameObject uiRanking;
    public Button btnRank;
    public Button btnRankClose;
    public GameObject uiScore;
    public TextMeshProUGUI txtScore;
    public GameObject uiCountDown;


    public BoxCollider2D deadLine;

    public int maxIndex = 2;
    public float lastPickTime = 0f;

    private int totalScore = 0;

    private List<GameObject> tmpList = new List<GameObject>();

    public int SCORE {
            get {return totalScore;} 
            set 
            {
                totalScore = value;
                txtScore.text = totalScore.ToString();
            }
        }

    public string SAVEPATH {
        get {return Application.persistentDataPath + "/RankData.json";}
        private set{}
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dataManager = DataManager.Instance;
        if (dataManager == null) return;

        for (int i = 0; i < fruits.Length - 1; i++)
        {
            if (fruits[i] == null)
            {
                //Debug.LogWarning("Fruits is not suitable");
                return;
            }
        }

        // Set Button
        btnStart.OnClickAsObservable().Subscribe(_ => 
        {

            if (tmpList.Count > 0)
            {
                foreach(GameObject child in tmpList)
                    Destroy(child);
            }

            lastPickTime = Time.realtimeSinceStartup;
            state = GAMESTATE.PLAYING;
            trStage.gameObject.SetActive(true);
            startPage.SetActive(false);
            uiCountDown.SetActive(false);
            uiScore.SetActive(true);

            GetNextFruit();
        });

        btnRank.OnClickAsObservable().Subscribe(_ =>
        {
            uiRanking.SetActive(true);
            dataManager.SetRank();
        }).AddTo(this);
        
        btnRankClose.OnClickAsObservable().Subscribe(_ =>
        {
            uiRanking.SetActive(false);
        }).AddTo(this);

        MessageBroker.Default.Receive<InstNewObject>().Subscribe(_ =>
        {
            GetNextFruit(_.fruit, _.pos);
        }).AddTo(this);

        
    }

    public void GetNextFruit()
    {
        int fIndex = UnityEngine.Random.Range(0, maxIndex);

        //if (fruitsPool.Count > 0)
        //{
        //    nextFruit = fruitsPool.Dequeue();
        //}
        nextFruit = Instantiate(fruits[fIndex], trStage);
        tmpList.Add(nextFruit);
    }

    public void GetNextFruit(FRUIT fruitIndex, Vector2 pos)
    {
        int fIndex = (int)fruitIndex;//.ConvertTo<Int32>();
        SCORE += (fIndex + 1) * 100;
        Debug.Log(fIndex);

        nextFruit = Instantiate((fruits[++fIndex]), trStage);
        nextFruit.transform.position = pos;
        nextFruit.GetComponent<Rigidbody2D>().simulated = true;
        tmpList.Add(nextFruit);
    }

}
