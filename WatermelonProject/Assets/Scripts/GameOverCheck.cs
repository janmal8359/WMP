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
    DataManager dManager;

    public TextMeshProUGUI txtCount;
    public GameObject uiResult;
    public Button btnConfirm;
    public TextMeshProUGUI txtScore;

    public float waitTime = 2f;
    public float checkTime = 4f;

    //
    private float colTime = 0f;
    private bool isCheck = false;

    void Start()
    {
        gManager = GameManager.Instance;
        dManager = DataManager.Instance;

        if (btnConfirm == null) return;

        btnConfirm.OnClickAsObservable().Subscribe(_ => {
            gManager.SCORE = 0;
            gManager.trStage.gameObject.SetActive(false);
            gManager.startPage.SetActive(true);
            uiResult.SetActive(false);

            foreach (Transform child in gManager.trStage)
            {
                Destroy(child.gameObject);
            }
        });
    }

    void FixedUpdate()
    {
        if (!isCheck || gManager.state != GAMESTATE.WAIT) return;

        gManager.uiCountDown.SetActive(true);
        int time = (int)((colTime + waitTime + checkTime) - Time.realtimeSinceStartup);
        txtCount.text = time.ToString();

        if (Time.realtimeSinceStartup > colTime + waitTime + checkTime && gManager.state == GAMESTATE.WAIT)
        {
            isCheck = false;
            gManager.state = GAMESTATE.END;
            gManager.uiCountDown.SetActive(false);
            gManager.uiScore.SetActive(false);

            // Save Record To Json
            RankData data = new RankData();
            data.score = gManager.SCORE;
            data.playerName = "guest";

            dManager.SaveDataToJson(data);
        
            ShowResult();
        }

    }

    void ShowResult()
    {
        // Show Result Popup
        if (uiResult != null)
            uiResult.SetActive(true);
            txtScore.text = gManager.SCORE.ToString();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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
        gManager.state = GAMESTATE.PLAYING;
        gManager.uiCountDown.SetActive(false);
        isCheck = false;
    }

}