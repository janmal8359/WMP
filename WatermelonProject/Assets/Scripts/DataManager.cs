using System.IO;
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

    private string path;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        
        Instance = this;
    }

    public void SaveDataToJson(RankData rankData)
    {
        string jsonData = JsonUtility.ToJson(rankData);
        path = Path.Combine(Application.dataPath, "RankData.json");
        //      = Application.dataPath + "/RankData.json"
        File.WriteAllText(path, jsonData);
    }

    public RankData LoadDataToJson()
    {
        string jsonData = File.ReadAllText(path);
        rankData = JsonUtility.FromJson<RankData>(jsonData);

        return rankData;
    }
}
