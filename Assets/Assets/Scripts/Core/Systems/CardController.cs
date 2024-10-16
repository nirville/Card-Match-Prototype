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
        float flipSpeed = 4.0f;
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
            StartCoroutine(FullFlip());
        }

        private IEnumerator FullFlip()
        {
            _button.enabled = false;
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

            yield return new WaitForSeconds (1.5f);

            while (transform.localScale.x < 1f)
            {
                cardScale = cardScale + (flipSpeed * Time.deltaTime);
                ChangeScale(cardScale);
                if (flipTolerance > cardScale)
                {
                    backFace.gameObject.SetActive(false);
                    frontFace.gameObject.SetActive(true);
                }
                yield return null;
            }
            ChangeScale(1.0f);
            _button.enabled = true;
        }

        void OnCardInteraction()
        {
            if (_isFlipped) return;
            StartCoroutine(FlipRoutine());
        }

        IEnumerator FlipRoutine()
        {
            _button.enabled = false;
            _isFlipped = true;

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
            _game.SetSelectedCard(this.gameObject);
        }

        IEnumerator ResetCardRoutine()
        {
            _isFlipped = false;
            _button.enabled = true;

            while (transform.localScale.x < 1f)
            {
                cardScale = cardScale + (flipSpeed * Time.deltaTime);
                ChangeScale(cardScale);
                if (flipTolerance > cardScale)
                {
                    backFace.gameObject.SetActive(false);
                    frontFace.gameObject.SetActive(true);
                }
                yield return null;             
            }
            ChangeScale(1.0f);
        }

        public void ChangeScale(float newScale)
        {
            transform.localScale = new Vector3(newScale, 1, 1);
        }

        internal void DisableCard()
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

        internal Card GetCard() => _card;
        internal void ResetCard() => StartCoroutine(ResetCardRoutine());
    }
}