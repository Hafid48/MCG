using UnityEngine;
using Newtonsoft.Json;

namespace MCG.Services
{
    public class PlayerPrefsSaveLoadService : ISaveLoadService
    {
        public delegate void SaveLoadEventHandler(int oldScore, int newScore);

        public bool HasSavedGame => PlayerPrefs.HasKey(KeyGameData);
        private const string KeyGameData = "MCG_GAME_DATA";

        public void SaveGame(GameData data)
        {
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(KeyGameData, json);
            PlayerPrefs.Save();
        }

        public GameData LoadGame()
        {
            if (!HasSavedGame)
                return null;
            string json = PlayerPrefs.GetString(KeyGameData);
            return JsonConvert.DeserializeObject<GameData>(json);
        }
    }
}