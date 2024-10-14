using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using BonusLibrary;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header(" Settings ")]
    private const string keyLevel = "LevelKey";
    private const string keyLanguage = "LanguageKey";

    [Header(" UI ")]
    [SerializeField] private LevelButton[] levelButtons;
    [SerializeField] private Sprite[] slotSprites;
    [SerializeField] private Sprite[] numberLevels;
    [SerializeField] private Sprite lockButton;

    [SerializeField] private TextMeshProUGUI[] textObjects;

    [Header("--------  Data --------")]
    private List<LanguageData> languagesData = new List<LanguageData>();
    private LanguageData languageData = new LanguageData();

    [Header(" Library ")]
    DataManage dataManage = new DataManage();
    HugeData hugeData = new HugeData();

    void Start()
    {
        int currentLevel = dataManage.GetData_Int(keyLevel) - 4;

        int index = 1;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (index <= currentLevel)
            {
                levelButtons[i].Congigure(slotSprites[0], numberLevels[i], true);

                int sceneIndex = index + 4;
                levelButtons[i].GetButton().onClick.AddListener(() => LoadLevel(sceneIndex));
            }

            else levelButtons[i].Congigure(slotSprites[1], lockButton, false);

            index++;
        }

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

    public void LoadLevel(int sceneIndex)
    {
        AudioManager.instance.PlaySFX(2);
        LoadingManager.instance.LoadScene(sceneIndex);
    }

    public void MainMenu()
    {
        AudioManager.instance.PlaySFX(2);
        SceneManager.LoadScene(1);
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

        languageData = languagesData[2];
    }






    // Put it in each button

    //public void LoadLevel()
    //{
    //    SceneManager.LoadScene(int.Parse(EventSystem.current.currentSelectedGameObject.
    //        GetComponentInChildren<TextMeshProUGUI>().text) + 4);
    //}
}
