using MCG.Model.Domain.Cards;
using MCG.Model.Domain.Game;
using UnityEngine;
using UnityEngine.UI;

namespace MCG.View
{
    public class CardView : MonoBehaviour
    {
        public ICard Card { get; private set; }
        private Button _cardButton;
        private Image _cardImage;
        private IMemoryGame _game;
        private BoardView _boardView;

        private void Awake()
        {
            _cardButton = GetComponent<Button>();
            _cardImage = transform.GetChild(0).GetComponent<Image>();
        }

        public void Initialize(ICard card, IMemoryGame game, BoardView boardView)
        {
            Card = card;
            _game = game;
            _boardView = boardView;
            switch (card.State)
            {
                case CardState.Empty:
                    SetAsInactive();
                    break;
                case CardState.Revealed:
                case CardState.Matched:
                    FlipToFront();
                    break;
                case CardState.Hidden:
                    FlipToBack();
                    break;
            }
        }

        public void OnCardButtonClicked()
        {
            _game.SelectCard(Card);
        }

        public void SetAsInactive()
        {
            _cardButton.interactable = false;
            _cardImage.enabled = false;
        }

        public void FlipToFront()
        {
            _cardImage.sprite = Card.Definition.Sprite;
        }

        public void FlipToBack()
        {
            _cardImage.sprite = _boardView.CardBackSprite;
        }
    }
}
