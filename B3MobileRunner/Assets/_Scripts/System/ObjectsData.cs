using UnityEngine;
public static class ObjectsData
{
    //Player
    public static Vector2[] PlayerSpeedOverTime //X = Time, Y = Speed (et faire des points qui évoluent progressivement)
    { get { return new Vector2[] { 
        new Vector2(0, 4), 
        new Vector2(15, 6),
        new Vector2(30, 7)
    }; } }
    public static Vector2 MaxSpeedGainOverTime { get { return new Vector2(90, 1); } } // en X temps, combien de vitesse Y le joueur gagne

    //Enemies
    public static float RockFallHeight { get { return 10f; } }

    //Score stuff
    public static int CoinValue { get { return 10; } }
}
