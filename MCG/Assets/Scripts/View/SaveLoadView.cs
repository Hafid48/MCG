using MCG.Model.Domain.Game;
using UnityEngine;
using UnityEngine.UI;

namespace MCG.View
{
    public class SaveLoadView : MonoBehaviour
    {
        private Button _saveButton;
        private Button _loadButton;
        private IMemoryGame _game;

        private void Awake()
        {
            _saveButton = transform.GetChild(0).GetComponent<Button>();
            _loadButton = transform.GetChild(1).GetComponent<Button>();
        }

        public void Initialize(IMemoryGame game)
        {
            _game = game;
            _game.OnGameReset += OnGameReset;
            _game.OnGameEnded += OnGameEnded;
        }

        private void OnGameReset()
        {
            _saveButton.interactable = true;
            _loadButton.interactable = true;
        }

        private void OnGameEnded()
        {
            _saveButton.interactable = false;
            _loadButton.interactable = false;
        }

        public void OnSaveButtonClicked()
        {
            _game.SaveGame();
        }

        public void OnLoadButtonClicked()
        {
            _game.LoadGame();
        }
    }
}