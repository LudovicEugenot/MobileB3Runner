
[System.Serializable]
public class SavedLevelsPlayedData
{
    public string redLastLevelPlayed = "";
    public string blueLastLevelPlayed = "";

    public string[] redLevelsPlayed = new string[0];
    public string[] blueLevelsPlayed = new string[0];

    public SavedLevelsPlayedData(string _redLastLevelPlayed, string _blueLastLevelPlayed, string[] _redLevelsPlayed, string[] _blueLevelsPlayed)
    {
        redLastLevelPlayed = _redLastLevelPlayed;
        blueLastLevelPlayed = _blueLastLevelPlayed;
        redLevelsPlayed = _redLevelsPlayed;
        blueLevelsPlayed = _blueLevelsPlayed;
    }
}