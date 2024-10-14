using BonusLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject exitPanel;

    [Header(" Settings ")]
    private const string keyLevel = "LevelKey";
    private const string keyLanguage = "LanguageKey";

    [Header(" UI ")]
    [SerializeField] private TextMeshProUGUI[] textObjects;

    [Header(" Data")]
    [SerializeField] private List<ItemInfo> itemInfo = new List<ItemInfo>();
    [SerializeField] private List<LanguageData> languagesData = new List<LanguageData>();

    private LanguageData languageData = new LanguageData();

    [Header(" Library ")]
    DataManage dataManage = new DataManage();
    HugeData hugeData = new HugeData();

    void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!AudioManager.instance.GetMusics()[0].isPlaying)
            AudioManager.instance.PlayMusic(0);

        LoadData();
        SetLanguage();
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

    public void ChangeScene(int sceneIndex)
    {
        AudioManager.instance.PlaySFX(2);
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartGame()
    {
        AudioManager.instance.PlaySFX(2);
        LoadingManager.instance.LoadScene(dataManage.GetData_Int(keyLevel));
    }

    public void ExitPanel(int YONindex)
    {
        AudioManager.instance.PlaySFX(2);

        switch (YONindex)
        {
            case 0:
                Application.Quit();
                break;

            case 1:
                exitPanel.SetActive(true);
                break;

            case 2:
                exitPanel.SetActive(false);
                break;
        }
    }

    private void LoadData()
    {
        hugeData.CreateData(itemInfo, languagesData);

        hugeData.LoadData();

        LoadLanguageData();
    }

    private void LoadLanguageData()
    {
        languagesData = hugeData.GetLanguageList();

        languageData = languagesData[0];
    }
}