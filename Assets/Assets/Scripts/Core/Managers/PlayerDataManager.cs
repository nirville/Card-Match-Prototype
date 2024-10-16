using UnityEngine;
using UnityEditor;
using Nirville.Core;
using UnityEngine.Purchasing.MiniJSON;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour
{
    internal static PlayerDataManager Instance { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    [ContextMenu("Load")]
    internal Dictionary<string, bool> LoadJsonData()
    {
        BoardData bd = new BoardData();

        string filePath = Application.persistentDataPath + "/playerdata.json";
        var boardData = System.IO.File.ReadAllText(filePath);
        bd = JsonUtility.FromJson<BoardData>(boardData);
        Debug.Log(bd);

        Dictionary<string, bool> cds = new Dictionary<string, bool>();
        for (int i = 0; i < bd.cardIDs.Length; i++)
        {
            cds.Add(bd.cardIDs[i], bd.resolvedIndices[i] > 0);
        }

        return cds;
    }

    internal void SaveToJson(CardController[] cardsC)
    {
        BoardData bd = new BoardData();
        bd.score = 10;
        bd.moves = 25;
        bd.cardIDs = new string[cardsC.Length];
        bd.resolvedIndices = new int[cardsC.Length];
        
        for(int i = 0; i < cardsC.Length; i++)
        {
            bd.cardIDs[i] = cardsC[i].GetCard().contentID;
            if(cardsC[i].IsMatched)
                bd.resolvedIndices[i] = i;  
        }

        var bdx = JsonUtility.ToJson(bd);
        string filePath = Application.persistentDataPath + "/playerdata.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, bdx);
    }
}

[System.Serializable]
public class BoardData
{
    public int score;
    public int moves;
    public string[] cardIDs;
    public int[] resolvedIndices;
}
