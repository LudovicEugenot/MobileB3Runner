
[System.Serializable]
public class SavedCurrentRunData
{
    public int currentRunCoinAmount = 0;
    public float currentRunTime = 0f;

    public SavedCurrentRunData(int _currentCoinAmount, float _currentRunTime)
    {
        currentRunCoinAmount = _currentCoinAmount;
        currentRunTime = _currentRunTime;
    }
}
