

[System.Serializable]
public class SavedGlobalGameData
{
    public int globalCoinAmount = 0;
    public string nextRunSkin = "Basic";
    //public string[] skinsOwned = { "Basic" };

    public SavedGlobalGameData(int newGlobalCoinAmount, string _nextRunSkin)
    {
        globalCoinAmount = newGlobalCoinAmount;
        nextRunSkin = _nextRunSkin;
        //skinsOwned = _skinsOwned;
    }
}