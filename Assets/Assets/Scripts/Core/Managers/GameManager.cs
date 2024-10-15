using UnityEngine;

namespace Nirville.Core
{
    public class GameManager : MonoBehaviour 
    {
        internal static GameManager Instance {get; private set;}

        public string LastContentRevealed { get; set; }
        public string LastClickedCardID {get; set;}

        public Vector2Int BoardGridSize { get; set;}

        public bool isLogMessages;

        private void Awake() 
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        internal bool IsSelectionMatchedFound(string newCard)
        {
            return (LastContentRevealed == newCard);
        }

        public void Logger(string msg)
        {
            if(isLogMessages)
                Debug.Log(msg);
        }
    }
}