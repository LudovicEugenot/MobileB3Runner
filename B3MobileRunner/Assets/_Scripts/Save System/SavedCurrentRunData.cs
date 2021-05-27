
[System.Serializable]
public class SavedCurrentRunData
{
    public int currentRunCoinAmount = 0;
    public float currentRunSpeed = 0f;
    public float currentRunTime = 0f;

    public SavedCurrentRunData(int _currentCoinAmount, float _currentRunSpeed, float _currentRunTime)
    {
        currentRunCoinAmount = _currentCoinAmount;
        currentRunSpeed = _currentRunSpeed;
        currentRunTime = _currentRunTime;
    }
}
