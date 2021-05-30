using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    #region Initialization

    public DanteSkin[] danteSkins;
    [HideInInspector] public SkinnedMeshRenderer danteSMR;

    [HideInInspector] public Skin.SkinType currentRunSkin;

    //code related
    #endregion

    private void Start()
    {
        if (Manager.Instance.playerScript) danteSMR = Manager.Instance.playerScript.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void ChangeDanteSkin(Skin.SkinType skin)
    {
        danteSMR.material = FindDanteSkin(skin);
    }

    public Material FindDanteSkin(Skin.SkinType skinToFind)
    {
        foreach (DanteSkin item in danteSkins) if (item.skinType == skinToFind) return item.materialRelated;

        Debug.LogWarning("Skin \"" + skinToFind.ToString() + "\" not found.");

        return FindDanteSkin(Skin.SkinType.Basic);
    }
}

[System.Serializable]
public struct DanteSkin
{
    public Skin.SkinType skinType;
    public Material materialRelated;
}

public static class Skin
{
    public enum SkinType { Basic, ShieldPower, StPatrick, Oldie, Red, Black }
    public static SkinType CurrentRunSkin()
    {
        return Manager.Instance.Skins.currentRunSkin;
    }
    public static void ChangeDanteSkin(SkinType skinToChangeTo)
    {
        Manager.Instance.Skins.ChangeDanteSkin(skinToChangeTo);
    }

    public static SkinType GetSkinFromString(string skinString)
    {
        return (SkinType)System.Enum.Parse(typeof(SkinType), skinString);
    }

    public static SkinType[] GetSkinArrayFromStringArray(string[] skinString)
    {
        SkinType[] array = new SkinType[skinString.Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = GetSkinFromString(skinString[i]);
        }

        return array;
    }

    #region Store
    public static void BuySkinFromStore(int skinPrice, SkinType skinBought)
    {
        SavedGlobalGameData data = SaveSystem.LoadGlobalData();

        data.globalCoinAmount -= skinPrice;

        SkinType[] skinsOwned = new SkinType[data.skinsOwned.Length + 1];

        for (int i = 0; i < skinsOwned.Length - 1; i++)
        {
            skinsOwned[i] = GetSkinFromString(data.skinsOwned[i]);
        }
        skinsOwned[skinsOwned.Length - 1] = skinBought;

        SaveSystem.SaveGlobalData(data.globalCoinAmount, skinBought, skinsOwned);
    }

    public static void SelectNextRunSkin(SkinType skinToLoadNextRun)
    {
        if (CheckIfSkinIsBought(skinToLoadNextRun))
        {
            SavedGlobalGameData data = SaveSystem.LoadGlobalData();
            SaveSystem.SaveGlobalData(data.globalCoinAmount, skinToLoadNextRun, GetSkinArrayFromStringArray(data.skinsOwned));
        }
    }

    public static bool CheckIfSkinIsBought(SkinType skinToCheck)
    {
        SkinType[] shop = GetSkinArrayFromStringArray(SaveSystem.LoadGlobalData().skinsOwned);
        foreach (SkinType skin in shop)
        {
            if (skin == skinToCheck) return true;
        }
        return false;
    }
    public static bool CheckIfSkinIsBought(string skinToCheck)
    {
        SkinType[] shop = GetSkinArrayFromStringArray(SaveSystem.LoadGlobalData().skinsOwned);
        foreach (SkinType skin in shop)
        {
            if (skin == GetSkinFromString(skinToCheck)) return true;
        }
        return false;
    }
    #endregion
}