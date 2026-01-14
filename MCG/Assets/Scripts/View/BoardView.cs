using MCG.Model.Data;
using MCG.Model.Domain.Cards;
using MCG.Model.Domain.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MCG.View
{
    [RequireComponent(typeof(AudioSource))]
    public class BoardView : MonoBehaviour
    {
        [field: SerializeField]
        public int Rows { get; private set; } = 4;
        [field: SerializeField]
        public int Columns { get; private set; } = 4;
        [field: SerializeField]
        public CardDefinition[] CardDefinitions { get; private set; }
        [field: SerializeField]
        public Sprite CardBackSprite { get; private set; }
        [SerializeField]
        private CardView _cardViewPrefab;
        [SerializeField]
        private AudioClip _revealCardSound;
        [SerializeField]
        private AudioClip _matchCardSound;
        [SerializeField]
        private AudioClip _mismatchCardSound;
        [SerializeField]
        private AudioClip _gameEndedSound;
        [SerializeField]
        private float _mismatchCardDelay = 0.25f;
        private Transform _boardParent;
        private GridLayoutGroup _gridLayout;
        private AudioSource _audioSource;
        private IMemoryGame _game;
        private readonly Dictionary<ICard, CardView> _cardViews = new Dictionary<ICard, CardView>();

        private void Awake()
        {
            _boardParent = transform;
            _gridLayout = GetComponent<GridLayoutGroup>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(IMemoryGame game)
        {
            _game = game;
            SetupBoardUI();
            SubscribeToGameEvents();
        }

        public void SetupBoardUI()
        {
            foreach (CardView cardView in _cardViews.Values)
                Destroy(cardView.gameObject);
            _cardViews.Clear();
            ResizeGrid();
            foreach (ICard card in _game.Board.Cards)
            {
                CardView cardViewInstance = Instantiate(_cardViewPrefab, _boardParent);
                cardViewInstance.Initialize(card, _game, this);
                _cardViews.Add(card, cardViewInstance);
            }
        }

        private void ResizeGrid()
        {
            RectTransform parentRect = _boardParent as RectTransform;
            float spacingX = _gridLayout.spacing.x;
            float spacingY = _gridLayout.spacing.y;
            // Calculate max cell size
            float cellWidth = (parentRect.rect.width - spacingX * (Rows - 1)) / Rows;
            float cellHeight = (parentRect.rect.height - spacingY * (Columns - 1)) / Columns;
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = Rows;
            // Force square
            float cellSize = Mathf.Min(cellWidth, cellHeight);
            _gridLayout.cellSize = new Vector2(cellSize, cellSize);
            // Calculate extra space to center the grid
            float totalGridWidth = cellSize * Rows + spacingX * (Rows - 1);
            float totalGridHeight = cellSize * Columns + spacingY * (Columns - 1);
            int paddingX = Mathf.RoundToInt((parentRect.rect.width - totalGridWidth) / 2);
            int paddingY = Mathf.RoundToInt((parentRect.rect.height - totalGridHeight) / 2);
            // Apply padding
            _gridLayout.padding.left = paddingX;
            _gridLayout.padding.right = paddingX;
            _gridLayout.padding.top = paddingY;
            _gridLayout.padding.bottom = paddingY;
        }

        private void SubscribeToGameEvents()
        {
            _game.OnCardRevealed += OnCardRevealed;
            _game.OnCardMatched += OnCardsMatched;
            _game.OnCardMismatched += OnCardMismatched;
            _game.OnGameStarted += OnGameStarted;
            _game.OnGameReset += OnGameReset;
            _game.OnGameEnded += OnGameEnded;
        }

        private void UnsubscribeFromGameEvents()
        {
            _game.OnCardRevealed -= OnCardRevealed;
            _game.OnCardMatched -= OnCardsMatched;
            _game.OnCardMismatched -= OnCardMismatched;
            _game.OnGameStarted -= OnGameStarted;
            _game.OnGameReset -= OnGameReset;
            _game.OnGameEnded -= OnGameEnded;
        }

        private void OnCardRevealed(ICard card)
        {
            if (_cardViews.TryGetValue(card, out CardView cardView))
                cardView.FlipToFront();
            if (_revealCardSound)
                _audioSource.PlayOneShot(_revealCardSound);
        }

        private void OnCardsMatched(ICard firstCard, ICard secondCard)
        {
            if (_cardViews.TryGetValue(firstCard, out CardView firstCardView))
                firstCardView.PlayMatchEffect();
            if (_cardViews.TryGetValue(secondCard, out CardView secondCardView))
                secondCardView.PlayMatchEffect();
            if (_audioSource.isPlaying)
                _audioSource.Stop();
            if (_matchCardSound)
                _audioSource.PlayOneShot(_matchCardSound);
            //TODO : add score
        }

        private void OnCardMismatched(ICard firstCard, ICard secondCard)
        {
            StartCoroutine(HideCardsCoroutine(firstCard, secondCard, _mismatchCardDelay));
        }

        private IEnumerator HideCardsCoroutine(ICard first, ICard second, float delay)
        {
            yield return new WaitForSeconds(delay);
            first.Hide();
            second.Hide();
            if (_cardViews.TryGetValue(first, out CardView firstView))
                firstView.FlipToBack();
            if (_cardViews.TryGetValue(second, out CardView secondView))
                secondView.FlipToBack();
            if (_mismatchCardSound)
                _audioSource.PlayOneShot(_mismatchCardSound);
        }

        private void OnGameStarted()
        {

        }

        private void OnGameReset()
        {

        }

        private void OnGameEnded()
        {
            
        }
    }
}