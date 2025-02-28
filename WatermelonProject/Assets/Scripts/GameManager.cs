using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UniRx;
using Unity.VisualScripting;
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
    public GameObject fruit;
    public Transform trFruits;
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


    public float lastPickTime = 0f;

    //
    private int totalScore = 0;


    public int SCORE {
            get {return totalScore;} 
            set 
            {
                totalScore = value;
                txtScore.text = totalScore.ToString();
            }
        }

    public string SAVEPATH {
        get {return Application.dataPath + "/RankData.json";}
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

        // Set Button
        btnStart.OnClickAsObservable().Subscribe(_ => 
        {
            lastPickTime = Time.realtimeSinceStartup;
            state = GAMESTATE.PLAYING;
            trStage.gameObject.SetActive(true);
            startPage.SetActive(false);
            uiCountDown.SetActive(false);
            uiScore.SetActive(true);
        });

        btnRank.OnClickAsObservable().Subscribe(_ =>
        {
            uiRanking.SetActive(true);
            dataManager.SetRank();
        }).AddTo(this);
        
        btnRankClose.OnClickAsObservable().Subscribe(_ =>
        {
            dataManager.ResetRank();
            uiRanking.SetActive(false);
        }).AddTo(this);

        //
        MessageBroker.Default.Receive<InstNewObject>().Subscribe(_ =>
        {
            GetNextFruit(_.fruit, _.pos);
        }).AddTo(this);

        
    }

    public GameObject GetFruit()
    {
        if (PoolManager.Instance != null && PoolManager.Instance.FRUITQUEUE.Count > 0)
        {
            FruitsStateManager newFruit = PoolManager.Instance.DequeueFruit();
            newFruit.gameObject.SetActive(true);
            newFruit.transform.SetParent(trStage);
            newFruit.Init().SetFruitInfo((FRUIT)UnityEngine.Random.Range(0, 2));

            return newFruit.gameObject;
        }
        else
        {
            GameObject newFruit = Instantiate(fruit, trStage);
            newFruit.GetComponent<FruitsStateManager>()?.Init().SetFruitInfo((FRUIT)UnityEngine.Random.Range(0, 2));
            
            return newFruit;
        }
    }

    public GameObject GetNextFruit(FRUIT fruitIndex, Vector2 pos)
    {
        int fIndex = (int)fruitIndex;
        SCORE += ++fIndex * 100;

        if (PoolManager.Instance != null && PoolManager.Instance.FRUITQUEUE.Count > 0)
        {
            FruitsStateManager nextFsm = PoolManager.Instance.FRUITQUEUE.Dequeue();
            nextFsm.gameObject.SetActive(true);
            nextFsm.transform.position = pos;
            nextFsm.transform.SetParent(trStage);
            nextFsm.Init().SetFruitInfo((FRUIT)fIndex);
            nextFsm.rigid.simulated = true;

            return nextFsm.gameObject;
        }
        else
        {
            GameObject nextFruit = Instantiate(fruit, trStage);
            nextFruit.transform.position = pos;
            nextFruit.TryGetComponent<FruitsStateManager>(out FruitsStateManager fsm);
            fsm.Init().SetFruitInfo((FRUIT)fIndex);
            fsm.rigid.simulated = true;

            return nextFruit;
        }
    }
}
