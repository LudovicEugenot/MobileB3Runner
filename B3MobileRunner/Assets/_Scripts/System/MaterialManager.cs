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
        danteSMR = Manager.Instance.playerScript.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void ChangeDanteSkin(Skin.SkinType skin)
    {
        danteSMR.material = FindDanteSkin(skin);
    }

    public Material FindDanteSkin(Skin.SkinType skinToFind)
    {
        foreach (DanteSkin item in danteSkins) if (item.skinType == skinToFind) return item.materialRelated;

        Debug.LogWarning("Skin " + skinToFind.ToString() + " not found.");

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
    public enum SkinType { Basic, ShieldPower, StPatrick }
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
        Debug.Log(skinString);
        return (SkinType) System.Enum.Parse(typeof(SkinType), skinString);
    }
}