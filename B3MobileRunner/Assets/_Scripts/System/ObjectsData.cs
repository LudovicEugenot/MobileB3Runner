using UnityEngine;
public struct ObjectsData
{
    //Player
    /// <summary>
    /// X = Time, Y = Speed
    /// </summary>
    public static Vector2[] PlayerSpeedOverTime //X = Time, Y = Speed (et faire des points qui évoluent progressivement)
    {
        get
        {
            return new Vector2[] {
                new Vector2(0, 4),
                new Vector2(15, 5),
                new Vector2(30, 6)
            };
        }
    } //WIP les faire évoluer en plus longtemps dans le jeu final
    public static Vector2 MaxSpeedGainOverTime { get { return new Vector2(90, 1); } } // en X secondes, combien de vitesse Y le joueur gagne


    //Enemies
    public static float RockFallHeight { get { return 11f; } }
    public static float TombstoneMenacingPlayerHeight { get { return 5f; } }


    //Score stuff
    public static int CoinValue { get { return 10; } }


    //Scene names
    public static string MainMenu { get { return "TitleMenuUI"; } }
    public static string[] RedLevels
    {
        get
        {
            return new string[]
            {
                "Scenes/LDs Prefaits/Rouge/Niveau 1-1", //WIP deux fois le premier niveau ?
                "Scenes/LDs Prefaits/Rouge/Niveau 1-2",
                "Scenes/LDs Prefaits/Rouge/Niveau 2",
                "Scenes/LDs Prefaits/Rouge/Niveau 3",
            };
        }
    }
    public static string[] BlueLevels
    {
        get
        {
            return new string[]
            {
                "Scenes/LDs Prefaits/Bleu/Niveau 1",
                "Scenes/LDs Prefaits/Bleu/Niveau 2",
                "Scenes/LDs Prefaits/Bleu/Niveau 3",
                "Scenes/LDs Prefaits/Bleu/Niveau 4"
            };
        }
    }


    //Limites de l'écran en coordonnées monde par rapport à la position de Dante sur l'écran (si la cam bouge au fur et à mesure de la partie : à update WIP)
    public static float ScreenLimitLeft { get { return -7f; } }
    public static float ScreenLimitRight { get { return 17f; } }
    public static float ScreenLimitUp { get { return 11f; } }
    public static float ScreenLimitDown { get { return -3.5f; } }



    //Save system related
    public static string SavedGlobalDataPath { get { return "/myGlobalGameData.weow"; } }
    public static string SavedCurrentRunDataPath { get { return "/myCurrentRunData.zoinks"; } }
    public static string SavedLevelsPlayedDataPath { get { return "/myLevelsPlayed.smartPrograming"; } }
}
