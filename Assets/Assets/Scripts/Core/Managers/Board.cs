using UnityEngine;
using System.Collections.Generic;
using System;


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

        Tuple<List<string>, List<int>> cardsSaved;

        Vector2Int _gridSize = new Vector2Int(2,3);

        bool _isSavedBoardAvailable;

        private void OnEnable() {
            GameEvents.current.BoardSizeSelect += OnGridSelection;
            GameEvents.current.GameStart += OnGameStart;
            GameEvents.current.GameEnd += OnGameEnd;
            GameEvents.current.NextLevel += OnNextLevel;
            GameEvents.current.SaveGame += SaveCurrentBoard;
        }

        void OnDisable()
        {
            GameEvents.current.BoardSizeSelect -= OnGridSelection;
            GameEvents.current.GameStart -= OnGameStart;
            GameEvents.current.GameEnd -= OnGameEnd;
            GameEvents.current.NextLevel -= OnNextLevel;
            GameEvents.current.SaveGame -= SaveCurrentBoard;
        }

        private void SaveCurrentBoard() {
            PlayerDataManager.Instance.SaveToJson(_cardControllers.ToArray());
        }

        private void Start() 
        {
            OnGridSelection(_gridSize); // default size
            cardsSaved = PlayerDataManager.Instance.LoadJsonData();

            if(cardsSaved != null)
            {
                GameManager.Instance.ui.PlayText("resume");
                _isSavedBoardAvailable = true;
            }
        }

        private void OnGameStart()
        {
            if(_isSavedBoardAvailable)
            {
                LoadPreviousLevel();
            }
            else
            {
                _cardControllers = new List<CardController>();

                GenerateAvailableImageIndexes();
                GenerateAvailablePositionIndexes(_cardCount);
                GenerateCards();
            }
        }

        private void LoadPreviousLevel()
        {
            var cardStrings = new List<string>();
            var cardIndices = new List<int>();

            cardStrings = cardsSaved.Item1;
            cardIndices = cardsSaved.Item2;


            var cardCountingInSaveGame = cardStrings.Count;

            switch (cardCountingInSaveGame)
            {
                default:
                    SetGridLayouParams(2, 3);
                break;
            }

            _availableImageIndexes = null;
            _availablePositionIndexes = null;
            _cardControllers = null;

            _cardControllers = new List<CardController>();

            for (int i = 0; i < cardStrings.Count; i++)
            {
                var card = Instantiate(cardPrefab, gridParent.transform);
                card.transform.name = "Card " + i.ToString();
                _cardControllers.Add(card.GetComponent<CardController>());
            }

            for(int i = 0; i < cardIndices.Count; i++)
            {
                var matching = GetCardMatch(cardStrings[i]);
                _cardControllers[i].SetCard(matching);
                if(cardIndices[i] > 0)
                    _cardControllers[i].DisableCard();
            }
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
            var random = UnityEngine.Random.Range(0, _availableImageIndexes.Count);
            var randomIndex = _availableImageIndexes[random];

            _availableImageIndexes.RemoveAt(random);

            return cardCollection.cards[randomIndex];
        }

        public int GetRandomCardPositionIndex()
        {
            int randomIndex = UnityEngine.Random.Range(0, _availablePositionIndexes.Count);
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