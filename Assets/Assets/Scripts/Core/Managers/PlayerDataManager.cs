using UnityEngine;
using UnityEditor;
using Nirville.Core;
using UnityEngine.Purchasing.MiniJSON;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.IO;

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

    internal Tuple<List<string>, List<int>> LoadJsonData()
    {
        BoardData bd = new BoardData();

        string filePath = Application.persistentDataPath + "/playerdata.json";
        if(File.Exists(filePath))
        {
            var boardData = System.IO.File.ReadAllText(filePath);
            bd = JsonUtility.FromJson<BoardData>(boardData);
            Debug.Log(boardData);

            List<string> cds = new List<string>();
            List<int> idx = new List<int>();

            for (int i = 0; i < bd.cardIDs.Length; i++)
            {
                cds.Add(bd.cardIDs[i]);
                idx.Add(bd.resolvedIndices[i]);
            }

            return Tuple.Create(cds, idx);
        }
        else
            return null;
    }

    internal void SaveToJson(CardController[] cardsC)
    {
        BoardData bd = new BoardData();
        bd.cardIDs = new string[cardsC.Length];
        bd.resolvedIndices = new int[cardsC.Length];
        
        for(int i = 0; i < cardsC.Length; i++)
        {
            bd.cardIDs[i] = cardsC[i].GetCard().contentID;
            if(cardsC[i].IsMatched)
                bd.resolvedIndices[i] = 1;  
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
