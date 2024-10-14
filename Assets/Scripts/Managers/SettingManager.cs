using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BonusLibrary;
using TMPro;

public class SettingManager : MonoBehaviour
{
    [Header(" Settings ")]
    private const string keyMenuVol = "MenuVol";
    private const string keySfxVol = "SfxVol";
    private const string keyGameVol = "GameVol";

    private const string keyLanguage = "LanguageKey";

    private int languageIndex;

    [Header(" UI ")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider gameSlider;

    [SerializeField] private Button[] languageButtons;
    [SerializeField] private TextMeshProUGUI languageText;

    [SerializeField] private TextMeshProUGUI[] textObjects;

    [Header("--------  Data --------")]
    private List<LanguageData> languagesData = new List<LanguageData>();
    private LanguageData languageData = new LanguageData();

    [Header("--------  Library --------")]
    DataManage dataManage = new DataManage();
    HugeData hugeData = new HugeData();

    void Start()
    {
        Init();
    }

    private void Init()
    {
        musicSlider.value = dataManage.GetData_Float(keyMenuVol);
        sfxSlider.value = dataManage.GetData_Float(keySfxVol);
        gameSlider.value = dataManage.GetData_Float(keyGameVol);

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

                languageText.text = "English";

                languageIndex = 0;
                languageButtons[0].interactable = false;

                break;

            case "VN":

                for (int i = 0; i < textObjects.Length; i++)
                    textObjects[i].text = languageData.languages_VN[i].letter;

                languageText.text = "Tieng Viet";

                languageIndex = 1;
                languageButtons[1].interactable = false;

                break;
        }
    }

    // 0: menuVol | 1: sfxVol | 2: gameplayVol
    public void SetVolume(int volValue)
    {
        switch (volValue)
        {
            case 0:
                dataManage.SaveData_Float(keyMenuVol, musicSlider.value);
                AudioManager.instance.SetMenuVolume();
                break;

            case 1:
                dataManage.SaveData_Float(keySfxVol, sfxSlider.value);
                AudioManager.instance.SetSfxVolume();
                break;

            case 2:
                dataManage.SaveData_Float(keyGameVol, gameSlider.value);
                AudioManager.instance.SetGameplayVolume();
                break;
        }
    }

    public void ReturnMainMenu()
    {
        AudioManager.instance.PlaySFX(2);
        SceneManager.LoadScene(1);
    }

    public void ChangeLanguage(string direction)
    {
        AudioManager.instance.PlaySFX(2);

        if (direction.Equals("Before"))
        {
            languageIndex++;
            languageText.text = "Viet Nam";

            dataManage.SaveData_String(keyLanguage, "VN");
        }

        else
        {
            languageIndex--;
            languageText.text = "English";

            dataManage.SaveData_String(keyLanguage, "Eng");
        }

        CheckLanguageButtons();
        SetLanguage();
    }

    private void CheckLanguageButtons()
    {
        for (int i = 0; i < languageButtons.Length; i++)
            languageButtons[i].interactable = true;

        if (languageIndex == languageData.languageCount - 1)
            languageButtons[1].interactable = false;

        if (languageIndex == 0)
            languageButtons[0].interactable = false;
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

        languageData = languagesData[4];
    }
}