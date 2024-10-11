using UnityEngine;

[CreateAssetMenu(fileName = "New Card Type", menuName = "Cards/New Card")]
public class Card : ScriptableObject
{
    public string contentID;
    public int height;
    public int width;
    public Sprite contentIMG;
}