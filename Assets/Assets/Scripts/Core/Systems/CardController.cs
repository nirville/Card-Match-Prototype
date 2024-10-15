using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Nirville.Core
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] Image frontFace;
        [SerializeField] Image backFace;
        Card _card;
        Button _button;

        GameManager _game;

        float cardScale = 1.0f;
        float flipSpeed = 2.0f;
        float flipTolerance = 0.05f;

        bool _isFlipped;


        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _game = GameManager.Instance;
            _button.onClick.AddListener(() => OnCardInteraction());
        }

        void OnCardInteraction()
        {
            if (_isFlipped) return;
            StartCoroutine(FlipRoutine());
        }

        IEnumerator FlipRoutine()
        {
            while (transform.localScale.x > -1f)
            {
                cardScale = cardScale - (flipSpeed * Time.deltaTime);
                ChangeScale(cardScale);
                if (flipTolerance > cardScale)
                {
                    backFace.gameObject.SetActive(true);
                    frontFace.gameObject.SetActive(false);
                }
                yield return null;
            }
            ChangeScale(-1.0f);
            _isFlipped = true;

            _button.enabled = false;
            _game.LastClickedCardID = transform.name;
            _game.IsSelectionMatchedFound(_card.contentID);
            _game.LastContentRevealed = _card.contentID;
        }

        public void ChangeScale(float newScale)
        {
            transform.localScale = new Vector3(newScale, 1, 1);
        }

        void DisableCard()
        {
            _button.enabled = false;
            backFace.gameObject.SetActive(false);
            frontFace.gameObject.SetActive(false);
        }

        internal void SetCard(Card card)
        {
            _card = card;
            backFace.sprite = _card.contentIMG;
        }
    }
}