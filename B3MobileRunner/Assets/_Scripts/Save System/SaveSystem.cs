using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Global Data
    public static void SaveGlobalData(int newGlobalCoinAmount)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ObjectsData.SavedGlobalDataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        SavedGlobalGameData data = new SavedGlobalGameData(newGlobalCoinAmount);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SavedGlobalGameData LoadGlobalData()
    {
        string path = Application.persistentDataPath + ObjectsData.SavedGlobalDataPath;
        if (!File.Exists(path))
        {
            SaveGlobalData(0);
        }


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        SavedGlobalGameData data = formatter.Deserialize(stream) as SavedGlobalGameData;
        stream.Close();

        return data;
    }
    #endregion

    #region Current Run Data
    public static void SaveCurrentRunData(int coinAmount, float currentSpeed, float currentTime)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ObjectsData.SavedCurrentRunDataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        SavedCurrentRunData data = new SavedCurrentRunData(coinAmount, currentTime);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void ResetCurrentRunData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ObjectsData.SavedCurrentRunDataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        SavedCurrentRunData data = new SavedCurrentRunData(0, 0);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SavedCurrentRunData LoadCurrentRunData()
    {
        string path = Application.persistentDataPath + ObjectsData.SavedCurrentRunDataPath;
        if (!File.Exists(path))
        {
            ResetCurrentRunData();
        }


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        SavedCurrentRunData data = formatter.Deserialize(stream) as SavedCurrentRunData;
        stream.Close();

        return data;
    }
    #endregion

    #region Levels Played Data
    public static void SaveLevelsPlayedData(SavedLevelsPlayedData incData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ObjectsData.SavedLevelsPlayedDataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        SavedLevelsPlayedData goingOutData = new SavedLevelsPlayedData(incData.redLastLevelPlayed, incData.blueLastLevelPlayed, incData.redLevelsPlayed, incData.blueLevelsPlayed);

        formatter.Serialize(stream, goingOutData);
        stream.Close();
    }
    public static void SaveLevelsPlayedData(string redLastLevelPlayed, string blueLastLevelPlayed, string[] redLevelsPlayed, string[] blueLevelsPlayed)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ObjectsData.SavedLevelsPlayedDataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        SavedLevelsPlayedData data = new SavedLevelsPlayedData(redLastLevelPlayed, blueLastLevelPlayed, redLevelsPlayed, blueLevelsPlayed);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SavedLevelsPlayedData LoadLevelsPlayedData()
    {
        string path = Application.persistentDataPath + ObjectsData.SavedLevelsPlayedDataPath;

        if (!File.Exists(path))
        {
            //la première fois qu'on regarde si le fichier existe, on l'initialise
            SaveLevelsPlayedData(
                ObjectsData.RedLevels[ObjectsData.RedLevels.Length - 1],
                ObjectsData.BlueLevels[ObjectsData.BlueLevels.Length - 1],
                new string[0],
                new string[0]);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        SavedLevelsPlayedData data = formatter.Deserialize(stream) as SavedLevelsPlayedData;
        stream.Close();

        return data;
    }
    #endregion
}
