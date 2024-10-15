using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Collection", menuName = "Cards/New Card Collection")]
public class CardCollection : ScriptableObject
{
    public List<Card> cards;
}
