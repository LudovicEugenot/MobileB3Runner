using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static string LoadNextLevel()
    {
        if (LevelIsInRedLevels(SceneManager.GetActiveScene().name))
        {
            return LoadABlueLevel();
        }
        else
        {
            return LoadARedLevel();
        }
    }
    public static string LoadABlueLevel()
    {
        string levelToLoad;
        SavedLevelsPlayedData data = SaveSystem.LoadLevelsPlayedData();

        if (data.blueLevelsPlayed.Length == ObjectsData.BlueLevels.Length - 1)
        {
            data.blueLevelsPlayed = new string[0];

            do
            {
                levelToLoad = ObjectsData.BlueLevels[Random.Range(0, ObjectsData.BlueLevels.Length)];
            } while (levelToLoad == data.blueLastLevelPlayed);
        }
        else
        {
            //set up de l'array temporaire
            string[] levelsPlayed = new string[data.blueLevelsPlayed.Length];
            for (int i = 0; i < levelsPlayed.Length; i++)
            {
                levelsPlayed[i] = data.blueLevelsPlayed[i];
            }

            //on reforme le vrai array
            data.blueLevelsPlayed = new string[levelsPlayed.Length + 1];

            //on recomplète le vrai array
            for (int i = 0; i < levelsPlayed.Length; i++)
            {
                data.blueLevelsPlayed[i] = levelsPlayed[i];
            }
            data.blueLevelsPlayed[data.blueLevelsPlayed.Length - 1] = data.blueLastLevelPlayed;


            //on trouve le bon niveau
            do
            {
                levelToLoad = ObjectsData.BlueLevels[Random.Range(0, ObjectsData.BlueLevels.Length)];
            } while (StringIsInStringArray(levelToLoad, data.blueLevelsPlayed));
        }

        data.blueLastLevelPlayed = levelToLoad;

        SaveSystem.SaveLevelsPlayedData(data);
        return levelToLoad;
    }
    public static string LoadARedLevel()
    {
        string levelToLoad;
        SavedLevelsPlayedData data = SaveSystem.LoadLevelsPlayedData();

        if (data.redLevelsPlayed.Length == ObjectsData.RedLevels.Length - 1)
        {
            data.redLevelsPlayed = new string[0];

            do
            {
                levelToLoad = ObjectsData.RedLevels[Random.Range(0, ObjectsData.RedLevels.Length)];
            } while (levelToLoad == data.redLastLevelPlayed);
        }
        else
        {
            //set up de l'array temporaire
            string[] levelsPlayed = new string[data.redLevelsPlayed.Length];
            for (int i = 0; i < levelsPlayed.Length; i++)
            {
                levelsPlayed[i] = data.redLevelsPlayed[i];
            }

            //on reforme le vrai array
            data.redLevelsPlayed = new string[levelsPlayed.Length + 1];

            //on recomplète le vrai array
            for (int i = 0; i < levelsPlayed.Length; i++)
            {
                data.redLevelsPlayed[i] = levelsPlayed[i];
            }
            data.redLevelsPlayed[data.redLevelsPlayed.Length - 1] = data.redLastLevelPlayed;


            //on trouve le bon niveau
            do
            {
                levelToLoad = ObjectsData.RedLevels[Random.Range(0, ObjectsData.RedLevels.Length)];
            } while (StringIsInStringArray(levelToLoad, data.redLevelsPlayed));
        }

        data.redLastLevelPlayed = levelToLoad;

        SaveSystem.SaveLevelsPlayedData(data);
        return levelToLoad;
    }

    #region Useful methods
    public static bool LevelIsInRedLevels(string currentLevelName)
    {
        for (int i = 0; i < ObjectsData.RedLevels.Length; i++)
        {
            if (ObjectsData.RedLevels[i].Contains(currentLevelName))
            {
                return true;
            }
        }
        for (int i = 0; i < ObjectsData.BlueLevels.Length; i++)
        {
            if (ObjectsData.BlueLevels[i].Contains(currentLevelName))
            {
                return false;
            }
        }
        Debug.LogError("Failed to identify the right level");
        return true;
    }

    public static bool StringIsInStringArray(string stringToCheck, string[] stringArrayToVerify)
    {
        for (int i = 0; i < stringArrayToVerify.Length; i++)
        {
            if (stringArrayToVerify[i] == stringToCheck) return true;
        }
        return false;
    }
    #endregion
}
