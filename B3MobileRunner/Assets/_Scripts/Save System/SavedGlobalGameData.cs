

[System.Serializable]
public class SavedGlobalGameData
{
    public int globalCoinAmount = 0;

    public SavedGlobalGameData(int newGlobalCoinAmount)
    {
        globalCoinAmount = newGlobalCoinAmount;
    }
}