namespace MCG.Services
{
    public interface ISaveLoadService
    {
        bool HasSavedGame { get; }

        void SaveGame(GameData data);

        GameData LoadGame();
    }
}