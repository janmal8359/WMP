using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class RankData
{
    public string playerName;
    public int score;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance {get; private set;}

    public RankData rankData;
    public RankDataCell rankCell;
    public Transform trRank;
    public ContentsAutoCell autoCell;

    private string path;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        
        Instance = this;
    }

    void Start()
    {
        path = Path.Combine(Application.dataPath, "RankData.json");
        //      = Application.dataPath + "/RankData.json"

        if (!File.Exists(path)) return;

        List<RankData> datas = new List<RankData>();
        datas = DeserializedJsonData(File.ReadAllText(path));

        // Set Rank Data
    }

    public void SaveDataToJson(RankData rankData)
    {
        string newJsonData = JsonUtility.ToJson(rankData);
        string jsonData = "";

        if (File.Exists(path))
        {
            string loadJsonData = File.ReadAllText(path);

            StringBuilder sb = new StringBuilder(loadJsonData);

            sb.Append("/\n");
            sb.Append(newJsonData);
            jsonData = sb.ToString();
        
            Debug.LogWarning(loadJsonData + "\n" + newJsonData + "\n" + jsonData);
        }

        else
        {
            jsonData = newJsonData;
        }

        File.WriteAllText(path, jsonData);
    }

    public RankData LoadDataFromJson()
    {
        string jsonData = File.ReadAllText(path);
        rankData = JsonUtility.FromJson<RankData>(jsonData);

        return rankData;
    }

    public List<RankData> DeserializedJsonData(string jsonData)
    {
        string[] jsons = jsonData.Split(char.Parse("/"));

        List<RankData> datas = new List<RankData>();

        foreach (var data in jsons)
        {
            datas.Add(JsonUtility.FromJson<RankData>(data));
        }

        return datas;
    }
    
    public void ResetRank()
    {
        if (autoCell.scv.content.childCount > 0)
        {
            foreach (Transform item in autoCell.scv.content)
            {
                Destroy(item.gameObject);
            }
        }

    }

    public void SetRank()
    {
        ResetRank();

        CreateRankCell(DeserializedJsonData(File.ReadAllText(path)));
        autoCell.AutoCelling();
    }

    void CreateRankCell(List<RankData> datas)
    {
        datas.Sort(CompareDatasByScore);

        foreach (var data in datas)
        {
            var cell = Instantiate(rankCell, trRank);
            cell.txtName.text = data.playerName;
            cell.txtScore.text = data.score.ToString();
        }
    }

    // Sort Datas by Score;
    private static int CompareDatasByScore(RankData x, RankData y)
    {
        if (x == null)
        {
            if (y == null) return 0;

            else return -1;
        }

        else
        {
            if (y == null) return 1;

            int val = y.score.CompareTo(x.score);

            if (val != 0) return val;
            else return 0;
        }
    }
}
