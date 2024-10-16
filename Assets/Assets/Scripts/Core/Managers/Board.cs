using UnityEngine;
using System.Collections.Generic;


namespace Nirville.Core
{
    public class Board : MonoBehaviour
    {
        [SerializeField] Canvas boardCanvas;
        [SerializeField] GameObject gridParent;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] CardCollection cardCollection;
        [SerializeField] FlexibleGridLayout gridlayoutWidget;

        int _cardCount;

        List<int> _availableImageIndexes;
        List<int> _availablePositionIndexes;
    	List<CardController> _cardControllers;

        Vector2Int _gridSize = new Vector2Int(2,3);

        private void OnEnable() {
            GameEvents.current.BoardSizeSelect += OnGridSelection;
            GameEvents.current.GameStart += OnGameStart;
            GameEvents.current.GameEnd += OnGameEnd;
            GameEvents.current.NextLevel += OnNextLevel;
        }

        void OnDisable()
        {
            GameEvents.current.BoardSizeSelect -= OnGridSelection;
            GameEvents.current.GameStart -= OnGameStart;
            GameEvents.current.GameEnd -= OnGameEnd;
            GameEvents.current.NextLevel -= OnNextLevel;
        }

        private void Awake() {
        }

        private void Start() {
            OnGridSelection(_gridSize); // default size
        }

        private void OnGameStart()
        {
            _cardControllers = new List<CardController>();

            GenerateAvailableImageIndexes();
            GenerateAvailablePositionIndexes(_cardCount);
            GenerateCards();
        }

        private void OnNextLevel()
        {
            foreach (var go in _cardControllers) Destroy(go.gameObject);
            _availableImageIndexes = null;
            _availablePositionIndexes = null;
            _cardControllers = null;

            _cardControllers = new List<CardController>();

            GenerateAvailableImageIndexes();
            GenerateAvailablePositionIndexes(_cardCount);
            GenerateCards();
        }

        private void OnGameEnd()
        {
            _cardCount = 0;
            foreach(var go in _cardControllers) Destroy(go.gameObject);
            _availableImageIndexes = null;
            _availablePositionIndexes = null;
            _cardControllers = null;
        }

        void OnGridSelection(Vector2Int size)
        {
            _gridSize = size;
            gridlayoutWidget.rows = size.x;
            gridlayoutWidget.columns = size.y;
            SetGridLayouParams(size.x, size.y);
        }

        void SetGridLayouParams(int row, int column)
        {
            GameManager.Instance.BoardGridSize = new Vector2Int(row, column);
            _cardCount = GameManager.Instance.BoardGridSize.x * GameManager.Instance.BoardGridSize.y;
        }
        
        void GenerateCards()
        {
            for(int i = 0; i < _cardCount; i++)
            {
                var card = Instantiate(cardPrefab, gridParent.transform);
                card.transform.name = "Card " + i.ToString();
                _cardControllers.Add(card.GetComponent<CardController>());
            }

            for (int i = 0; i < _cardCount / 2; i++)
            {
                var randomCard = GetRandomAvailableCard();
                SetRandomCardToGrid(randomCard);

                var randomCardMatching = GetCardMatch(randomCard.contentID);
                SetRandomCardToGrid(randomCardMatching);
            }
        }

        public Card GetCardMatch(string contentName)
        {
            foreach (var card in cardCollection.cards)
            {
                if (card.IsMatch(contentName))
                {
                    return card;
                }
            }

            return null;
        }

        public Card GetRandomAvailableCard()
        {
            var random = Random.Range(0, _availableImageIndexes.Count);
            var randomIndex = _availableImageIndexes[random];

            _availableImageIndexes.RemoveAt(random);

            return cardCollection.cards[randomIndex];
        }

        public int GetRandomCardPositionIndex()
        {
            int randomIndex = Random.Range(0, _availablePositionIndexes.Count);
            int randomPosition = _availablePositionIndexes[randomIndex];

            _availablePositionIndexes.RemoveAt(randomIndex);

            return randomPosition;
        }

        void SetRandomCardToGrid(Card randomCard)
        {
            var i = GetRandomCardPositionIndex();
            var cardObj = _cardControllers[i];
            cardObj.SetCard(randomCard);
        }

        void GenerateAvailableImageIndexes()
        {
            _availableImageIndexes = new List<int>();
            int index = cardCollection.cards.Count;

            for (int i = 0; i < index; i++)
            {
                if (i % 2 == 0)
                {
                    _availableImageIndexes.Add(i);
                }
            }
        }

        private void GenerateAvailablePositionIndexes(int cardCount)
        {
            _availablePositionIndexes = new List<int>();

            for (int i = 0; i < cardCount; i++)
            {
                _availablePositionIndexes.Add(i);
            }
        }
    }
}