using UnityEngine;

[CreateAssetMenu(fileName = "New Card Type", menuName = "Cards/New Card")]
public class Card : ScriptableObject
{
    public string contentID;
    public Sprite contentIMG;

    public bool IsMatch(string val)
    {
        val = val.ToLower();
        return (val == contentID);
    }
}