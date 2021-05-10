using UnityEngine;
public static class ObjectsData
{
    //Player
    public static Vector2[] PlayerSpeedOverTime //X = Time, Y = Speed (et faire des points qui évoluent progressivement)
    { get { return new Vector2[] { 
        new Vector2(0, 4), 
        new Vector2(15, 5),
        new Vector2(30, 6)
    }; } } //WIP les faire évoluer en plus longtemps dans le jeu final
    public static Vector2 MaxSpeedGainOverTime { get { return new Vector2(90, 1); } } // en X secondes, combien de vitesse Y le joueur gagne

    //Enemies
    public static float RockFallHeight { get { return 10f; } }

    //Score stuff
    public static int CoinValue { get { return 10; } }

    //Names
    public static string MainMenu { get { return "TitleMenuUI"; } }
    public static string GameScene { get { return "Milestone19_04"; } }
}
