
[System.Serializable]
public class SavedCurrentRunData
{
    public int currentRunCoinAmount = 0;
    public float currentRunTime = 0f;
    public bool isCurrentlyInvincible = false; //si plus d'états qu'invincible ou non, en faire un string

    public SavedCurrentRunData(int _currentCoinAmount, float _currentRunTime, bool _isInvincible)
    {
        currentRunCoinAmount = _currentCoinAmount;
        currentRunTime = _currentRunTime;
        isCurrentlyInvincible = _isInvincible;
    }
}
