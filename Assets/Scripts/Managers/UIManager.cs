using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BonusLibrary;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header(" Elements ")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject waitPanel;

    public GameObject winPanel;
    public GameObject losePanel;

    [Header(" Settings ")]
    private const string keyGameplayVol = "GameVol";
    private const string keySfxVol = "SfxVol";
    private const string keyLanguage = "LanguageKey";

    [Header(" UI ")]
    [SerializeField] private Slider gameSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private TextMeshProUGUI[] textObjects;

    [Header("--------  Data --------")]
    private List<LanguageData> languagesData = new List<LanguageData>();
    private LanguageData languageData = new LanguageData();

    [Header("--------  Library --------")]
    DataManage dataManage = new DataManage();
    HugeData hugeData = new HugeData();
    AdvertisingManage advertisingManage = new AdvertisingManage();

    void Awake()
    {
        if (instance == null)
            instance = this;

        else Destroy(gameObject);
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        waitPanel.SetActive(true);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);

        SetSliderVolume();
        LoadData();
        SetLanguage();

        advertisingManage.LoadRewardedAd();
    }

    private void SetSliderVolume()
    {
        gameSlider.value = dataManage.GetData_Float(keyGameplayVol);
        sfxSlider.value = dataManage.GetData_Float(keySfxVol);
    }

    private void SetLanguage()
    {
        switch (dataManage.GetData_String(keyLanguage))
        {
            case "Eng":
                for (int i = 0; i < textObjects.Length; i++)
                    textObjects[i].text = languageData.languages_Eng[i].letter;

                break;

            case "VN":
                for (int i = 0; i < textObjects.Length; i++)
                    textObjects[i].text = languageData.languages_VN[i].letter;

                break;
        }
    }

    // 0: Active pausePanel | 1: Return MainMenu | 2: Replay | 3: Continue
    public void PausePanel(int index)
    {
        AudioManager.instance.PlaySFX(2);

        switch (index)
        {
            case 0:
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                break;

            case 1:
                SceneManager.LoadScene(1);
                Time.timeScale = 1;
                break;

            case 2:
                LoadingManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                break;

            case 3:
                pausePanel.SetActive(false);
                Time.timeScale = 1;
                break;
        }
    }

    // 0: Open SettingsPanel | 1: Close SettingsPanel
    public void SettingPanel(int index)
    {
        AudioManager.instance.PlaySFX(2);

        switch (index)
        {
            case 0:
                settingPanel.SetActive(true);
                Time.timeScale = 0;
                break;

            case 1:
                settingPanel.SetActive(false);
                Time.timeScale = 1;
                break;
        }
    }

    // 0: gameplayVol | 1: sfxVol
    public void SetVolume(int volValue)
    {
        switch (volValue)
        {
            case 0:
                dataManage.SaveData_Float(keyGameplayVol, gameSlider.value);
                AudioManager.instance.SetGameplayVolume();
                break;

            case 1:
                dataManage.SaveData_Float(keySfxVol, sfxSlider.value);
                AudioManager.instance.SetSfxVolume();
                break;
        }
    }

    public void NextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex - 4;

        if (currentLevel < 5)
            LoadingManager.instance.LoadScene(currentLevel + 4 + 1);

        else LoadingManager.instance.LoadScene(4);
    }

    public void RewardAds()
    {
        advertisingManage.ShowRewardedAd();
    }

    public void DeactiveWaitPanel()
    {
        waitPanel.SetActive(false);
        GameManager.instance.ChangeGameState(GameState.Playing);
    }


    // DATA LANGUAGE

    private void LoadData()
    {
        hugeData.LoadData();

        LoadLanguageData();
    }

    private void LoadLanguageData()
    {
        languagesData = hugeData.GetLanguageList();

        languageData = languagesData[5];
    }
}