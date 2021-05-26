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
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavedGlobalGameData data = formatter.Deserialize(stream) as SavedGlobalGameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion

    #region Current Run Data
    public static void SaveCurrentRunData(int coinAmount, float currentSpeed, float currentTime)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ObjectsData.SavedCurrentRunDataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        SavedCurrentRunData data = new SavedCurrentRunData(coinAmount, currentSpeed, currentTime);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SavedCurrentRunData LoadCurrentRunData()
    {
        string path = Application.persistentDataPath + ObjectsData.SavedCurrentRunDataPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavedCurrentRunData data = formatter.Deserialize(stream) as SavedCurrentRunData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    #endregion
}
