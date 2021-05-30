using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused = true;
    public bool isMainMenu = false;
    [SerializeField] TMPro.TextMeshProUGUI moneyText;

    [Header("Skins")]
    public DanteSkinMenu basicSkin;
    public DanteSkinMenu redSkin;
    public DanteSkinMenu blackSkin;
    public DanteSkinMenu oldSkin;
    public DanteSkinMenu lutinSkin;

    [Header("Preview")]
    [SerializeField] Image basicSkinPreview;
    [SerializeField] Image redSkinPreview;
    [SerializeField] Image blackSkinPreview;
    [SerializeField] Image oldSkinPreview;
    [SerializeField] Image lutinSkinPreview;

    DanteSkinMenu currentPreviewSkin
    {
        get { return _currentPreviewSkin; }
        set
        {
            switch (value.skinRelated)
            {
                case Skin.SkinType.Basic:
                    basicSkinPreview.enabled = true;
                    redSkinPreview.enabled = false;
                    blackSkinPreview.enabled = false;
                    oldSkinPreview.enabled = false;
                    lutinSkinPreview.enabled = false;
                    break;
                case Skin.SkinType.StPatrick:
                    basicSkinPreview.enabled = false;
                    redSkinPreview.enabled = false;
                    blackSkinPreview.enabled = false;
                    oldSkinPreview.enabled = false;
                    lutinSkinPreview.enabled = true;
                    break;
                case Skin.SkinType.Oldie:
                    basicSkinPreview.enabled = false;
                    redSkinPreview.enabled = false;
                    blackSkinPreview.enabled = false;
                    oldSkinPreview.enabled = true;
                    lutinSkinPreview.enabled = false;
                    break;
                case Skin.SkinType.Red:
                    basicSkinPreview.enabled = false;
                    redSkinPreview.enabled = true;
                    blackSkinPreview.enabled = false;
                    oldSkinPreview.enabled = false;
                    lutinSkinPreview.enabled = false;
                    break;
                case Skin.SkinType.Black:
                    basicSkinPreview.enabled = false;
                    redSkinPreview.enabled = false;
                    blackSkinPreview.enabled = true;
                    oldSkinPreview.enabled = false;
                    lutinSkinPreview.enabled = false;
                    break;
                default:
                    break;
            }
            _currentPreviewSkin = value;
        }
    }
    DanteSkinMenu _currentPreviewSkin;

    SavedGlobalGameData data;

    private void Start()
    {
        if (isMainMenu)
        {
            InitSkinMenu();
            InitPreviewSkin();
        }
    }

    private void InitPreviewSkin()
    {
        basicSkinPreview.gameObject.SetActive(true);
        blackSkinPreview.gameObject.SetActive(true);
        redSkinPreview.gameObject.SetActive(true);
        oldSkinPreview.gameObject.SetActive(true);
        lutinSkinPreview.gameObject.SetActive(true);

        currentPreviewSkin = SelectMenuSkinFromSkinType(Skin.GetSkinFromString(data.nextRunSkin));
        // init preview avec current skin
    }

    #region Main Menu
    public void PlayGame()
    {
        //SceneManager.UnloadSceneAsync(ObjectsData.MainMenu);
        SceneManager.LoadScene(LevelLoader.LoadARedLevel());
        Time.timeScale = 1f;
        //Manager.Instance.sound.PlayBGM();
        gamePaused = false;
    }
    #endregion

    #region Shop
    public void InitSkinMenu()
    {
        data = SaveSystem.LoadGlobalData();

        moneyText.text = data.globalCoinAmount.ToString();

        basicSkin.InitSkin(Skin.CheckIfSkinIsBought(basicSkin.skinRelated));
        redSkin.InitSkin(Skin.CheckIfSkinIsBought(redSkin.skinRelated));
        blackSkin.InitSkin(Skin.CheckIfSkinIsBought(blackSkin.skinRelated));
        oldSkin.InitSkin(Skin.CheckIfSkinIsBought(oldSkin.skinRelated));
        lutinSkin.InitSkin(Skin.CheckIfSkinIsBought(lutinSkin.skinRelated));


    }

    public void UpdateMenu()
    {
        data = SaveSystem.LoadGlobalData();

        moneyText.text = data.globalCoinAmount.ToString();
    }

    public void BuyPreviewSkin()
    {
        if (!Skin.CheckIfSkinIsBought(currentPreviewSkin.skinRelated))
        {
            if (currentPreviewSkin.priceValue < data.globalCoinAmount)
            {
                Skin.BuySkinFromStore(currentPreviewSkin.priceValue, currentPreviewSkin.skinRelated);
                Skin.SelectNextRunSkin(currentPreviewSkin.skinRelated);

                SelectMenuSkinFromSkinType(currentPreviewSkin.skinRelated).InitSkin(true);
            }
        }

        UpdateMenu();
    }

    public void SelectPreviewSkin(string skinName)
    {
        currentPreviewSkin = SelectMenuSkinFromSkinType(Skin.GetSkinFromString(skinName));
        if (Skin.CheckIfSkinIsBought(Skin.GetSkinFromString(skinName)))
        {
            Skin.SelectNextRunSkin(Skin.GetSkinFromString(skinName));
        }
    }

    DanteSkinMenu SelectMenuSkinFromSkinType(Skin.SkinType skinType)
    {
        switch (skinType)
        {
            case Skin.SkinType.Basic:
                return basicSkin;
            case Skin.SkinType.StPatrick:
                return lutinSkin;
            case Skin.SkinType.Oldie:
                return oldSkin;
            case Skin.SkinType.Red:
                return redSkin;
            case Skin.SkinType.Black:
                return blackSkin;
            default:
                return basicSkin;
        }
    }
    #endregion 

    #region in game
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Manager.Instance.sound.PauseBGM();
        //SceneManager.LoadScene("PauseMenuScene", LoadSceneMode.Additive);
        gamePaused = true;
    }

    public void Resume()
    {
        //SceneManager.UnloadSceneAsync("PauseMenuScene");
        Time.timeScale = 1f;
        Manager.Instance.sound.PlayBGM();
        gamePaused = false;
    }

    public void ExitGame()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //SceneManager.UnloadSceneAsync("Milestone19_04");
        SaveSystem.ResetCurrentRunData();
        SceneManager.LoadScene(ObjectsData.MainMenu);
        Time.timeScale = 0f;
        //Manager.Instance.sound.StopBGM();
        gamePaused = true;
    }
    #endregion
}

[System.Serializable]
public class DanteSkinMenu
{
    public Skin.SkinType skinRelated;

    [HideInInspector] public bool isBought = false;
    [SerializeField] RectTransform skinRectTrsf;
    [SerializeField] RectTransform skinLock;
    [SerializeField] RectTransform price;
    public int priceValue;
    [HideInInspector] public Image image;

    public void InitSkin(bool _isBought)
    {
        isBought = _isBought;
        image = skinRectTrsf.GetComponentInChildren<Image>();


        skinLock.gameObject.SetActive(!isBought);
        price.gameObject.SetActive(!isBought);

    }
}
