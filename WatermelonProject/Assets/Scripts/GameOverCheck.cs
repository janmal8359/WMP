using System;
using System.IO;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCheck : MonoBehaviour
{
    GameManager gManager;

    public TextMeshProUGUI txtCount;
    public GameObject uiRank;
    public Button btnConfirm;

    public float waitTime = 2f;
    public float checkTime = 4f;


    private float colTime = 0f;
    private bool isCheck = false;
    private bool isConfirm = false;

    void Start()
    {
        gManager = GameManager.Instance;

        if (btnConfirm == null) return;
        btnConfirm.OnClickAsObservable().Subscribe(_ => {

            isCheck = false;
            gManager.SCORE = 0;
            gManager.trStage.gameObject.SetActive(false);
            gManager.state = GAMESTATE.END;
            gManager.startPage.SetActive(true);
            gManager.count.SetActive(false);
        });
    }

    void FixedUpdate()
    {
        if (!isCheck) return;

        gManager.count.SetActive(true);
        int time = (int)((colTime + waitTime + checkTime) - Time.realtimeSinceStartup);
        txtCount.text = time.ToString();

        if (Time.realtimeSinceStartup > colTime + waitTime + checkTime)
        {
        RankRecord record = new RankRecord();
        record.score = gManager.SCORE;
        record.playerName = "guest";

        gManager.SaveRankDataToJson(record);
        
            ShowResult();
            //isCheck = false;
            //gManager.SCORE = 0;
            //gManager.trStage.gameObject.SetActive(false);
            //gManager.state = GAMESTATE.END;
            //gManager.startPage.SetActive(true);
            //gManager.count.SetActive(false);
        }

    }

    void ShowResult()
    {
        // Show Result Popup
        if (uiRank != null)
            uiRank.SetActive(true);

        // Save Record To Json
        RankRecord record = new RankRecord();
        record.score = gManager.SCORE;
        record.playerName = "guest";

        gManager.SaveRankDataToJson(record);
        //string saveData = JsonUtility.ToJson(record);
        //File.WriteAllText(gManager.SAVEPATH, saveData);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        colTime = Time.realtimeSinceStartup;
        gManager.state = GAMESTATE.WAIT;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << this.gameObject.layer) & collision.gameObject.layer) < 0) return;
        if (Time.realtimeSinceStartup < colTime + waitTime) return;
        if (gManager == null) return;

        isCheck = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.LogError("Exit");
        gManager.state = GAMESTATE.PLAYING;
        gManager.count.SetActive(false);
        isCheck = false;
    }

}