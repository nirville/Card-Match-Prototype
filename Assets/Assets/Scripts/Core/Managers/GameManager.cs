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

        internal GameObject[] selectedCards;

        internal int movesCount;
        internal int score;

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

        private void Start() {
            movesCount = 0;
            selectedCards = new GameObject[2];
            DiscardSelection();
        }

        internal void SetSelectedCard(GameObject selection)
        {
            movesCount++;

            if(selectedCards[0] == null)
            {
                selectedCards[0] = selection;
            }
            else if(selectedCards[1] == null)
            {
                selectedCards[1] = selection;
                if(MatchSelectedCards())
                {
                    score++;
                    DisableSelectedCards();
                    DiscardSelection();
                }
                else
                {
                    ResetSelectedCards();
                    DiscardSelection();
                }
            }
        }

        void DiscardSelection()
        {
            selectedCards[0] = null;
            selectedCards[1] = null;
        }

        void ResetSelectedCards()
        {
            if(selectedCards[0] == null || selectedCards[1] == null) return;
            var first = selectedCards[0].GetComponent<CardController>();
            var second = selectedCards[1].GetComponent<CardController>();

            first.ResetCard();
            second.ResetCard();
        }

        void DisableSelectedCards()
        {
            if (selectedCards[0] == null || selectedCards[1] == null) return;
            var first = selectedCards[0].GetComponent<CardController>();
            var second = selectedCards[1].GetComponent<CardController>();

            first.DisableCard();
            second.DisableCard();
        }

        bool MatchSelectedCards()
        {
            var first = selectedCards[0].GetComponent<CardController>();
            var second = selectedCards[1].GetComponent<CardController>();

            return first != null && second != null && first.GetCard().contentID == second.GetCard().contentID;
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