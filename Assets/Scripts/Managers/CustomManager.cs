using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BonusLibrary;
using TMPro;

public class CustomManager : MonoBehaviour
{
    [Header("-------- Elements --------")]
    [SerializeField] private GameObject[] hats;
    [SerializeField] private GameObject[] weapons;

    [SerializeField] private GameObject[] itemPanels;
    [SerializeField] private GameObject itemPanel;

    [SerializeField] private GameObject[] panelObjects;

    [SerializeField] private Material[] colors;
    [SerializeField] private Material colorOrigin;

    [SerializeField] private SkinnedMeshRenderer rendererCharacter;

    [SerializeField] private Animator equipPanelAnim;

    [Header("--------  Settings --------")]
    private int hatIndex = -1;
    private int weaponIndex = -1;
    private int colorIndex = -1;

    private int itemPanelsActive;

    private const string keyHat = "HatKey";
    private const string keyWeapon = "WeaponKey";
    private const string keyColor = "ColorKey";

    private const string keyScore = "ScoreKey";
    private const string keyLanguage = "LanguageKey";

    private string purchaseLG;
    private string purchasedLG;

    [Header("--------  UI --------")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI purchaseText;

    [SerializeField] private Button[] hatButtons;
    [SerializeField] private Button[] weaponButtons;
    [SerializeField] private Button[] colorButtons;

    [SerializeField] private Button[] itemButtons;

    [SerializeField] private TextMeshProUGUI[] textObjects;

    [Header("--------  Data --------")]
    [SerializeField] private List<ItemInfo> itemInfo = new List<ItemInfo>();
    [SerializeField] private List<LanguageData> languagesData = new List<LanguageData>();

    private LanguageData languageData = new LanguageData();

    [Header("--------  Library --------")]
    DataManage dataManage = new DataManage();
    HugeData hugeData = new HugeData();

    void Start()
    {
        LoadDataItems(-1);

        ClosePanels();
        DisplayScoreText(dataManage.GetData_Int(keyScore));

        LoadData();

        SetLanguage();
    }

    private void DisplayScoreText(int currentScore)
    {
        scoreText.text = currentScore.ToString();

        dataManage.SaveData_Int(keyScore, currentScore);
    }

    private void SetLanguage()
    {
        switch (dataManage.GetData_String(keyLanguage))
        {
            case "Eng":
                for (int i = 0; i < textObjects.Length; i++)
                    textObjects[i].text = languageData.languages_Eng[i].letter;

                purchaseLG = languageData.languages_Eng[5].letter;
                purchasedLG = languageData.languages_Eng[8].letter;

                break;

            case "VN":
                for (int i = 0; i < textObjects.Length; i++)
                    textObjects[i].text = languageData.languages_VN[i].letter;

                purchaseLG = languageData.languages_VN[5].letter;
                purchasedLG = languageData.languages_VN[8].letter;

                break;
        }
    }


    // -------------- CHANGE ITEMS --------------

    public void ChangeHatCustom(string direction)
    {
        AudioManager.instance.PlaySFX(2);

        if (direction.Equals("Before"))
        {
            if (hatIndex >= 0)
                hats[hatIndex].SetActive(false);

            hatIndex++;

            hats[hatIndex].SetActive(true);

            itemText.text = itemInfo[hatIndex].itemName;
        }

        else
        {
            hats[hatIndex].SetActive(false);
            hatIndex--;

            if (hatIndex == -1)
                itemText.text = "No Item";

            else
            {
                hats[hatIndex].SetActive(true);
                itemText.text = itemInfo[hatIndex].itemName;
            }
        }

        // On/Off Direction Button
        CheckHatButtons();

        CheckItemIsPurchased(hatIndex);
    }

    private void CheckHatButtons()
    {
        for (int i = 0; i < hatButtons.Length; i++)
            hatButtons[i].interactable = true;

        if (hatIndex == hats.Length - 1)
            hatButtons[1].interactable = false;

        if (hatIndex == -1)
            hatButtons[0].interactable = false;
    }

    public void ChangeWeaponCustom(string direction)
    {
        AudioManager.instance.PlaySFX(2);

        if (direction.Equals("Before"))
        {
            if (weaponIndex >= 0)
                weapons[weaponIndex].SetActive(false);

            weaponIndex++;

            weapons[weaponIndex].SetActive(true);

            itemText.text = itemInfo[weaponIndex + 3].itemName;

            CheckItemIsPurchased(weaponIndex + 3);
        }

        else
        {
            weapons[weaponIndex].SetActive(false);
            weaponIndex--;

            if (weaponIndex == -1)
            {
                itemText.text = "No Item";

                itemButtons[0].interactable = false;

                CheckItemIsPurchased(weaponIndex);
            }

            else
            {
                weapons[weaponIndex].SetActive(true);
                itemText.text = itemInfo[weaponIndex + 3].itemName;

                CheckItemIsPurchased(weaponIndex + 3);
            }
        }

        // On/Off Direction Button
        CheckWeaponButtons();
    }

    private void CheckWeaponButtons()
    {
        for (int i = 0; i < weaponButtons.Length; i++)
            weaponButtons[i].interactable = true;

        if (weaponIndex == weapons.Length - 1)
            weaponButtons[1].interactable = false;

        if (weaponIndex == -1)
            weaponButtons[0].interactable = false;
    }

    public void ChangeColorCustom(string direction)
    {
        AudioManager.instance.PlaySFX(2);

        Material[] mats = rendererCharacter.materials;

        if (direction.Equals("Before"))
        {
            colorIndex++;

            mats[0] = colors[colorIndex];

            rendererCharacter.materials = mats;

            itemText.text = itemInfo[colorIndex + 6].itemName;

            CheckItemIsPurchased(colorIndex + 6);
        }

        else
        {
            colorIndex--;

            if (colorIndex == -1)
            {
                itemText.text = "No Item";
                mats[0] = colorOrigin;

                CheckItemIsPurchased(colorIndex);
            }

            else
            {
                itemText.text = itemInfo[colorIndex + 6].itemName;
                mats[0] = colors[colorIndex];

                CheckItemIsPurchased(colorIndex + 6);
            }

            rendererCharacter.materials = mats;
        }

        // On/Off Direction Button
        CheckColorButtons();
    }

    private void CheckColorButtons()
    {
        for (int i = 0; i < colorButtons.Length; i++)
            colorButtons[i].interactable = true;

        if (colorIndex == colors.Length - 1)
            colorButtons[1].interactable = false;

        if (colorIndex == -1)
            colorButtons[0].interactable = false;
    }

    // -------------- END --------------






    public void ClickItemPanel(int groupItemIndex)
    {
        AudioManager.instance.PlaySFX(2);

        OpenPanels();
        LoadDataItems(groupItemIndex);

        itemPanelsActive = groupItemIndex;

        itemPanels[groupItemIndex].SetActive(true);
    }

    public void BackItemSelect()
    {
        AudioManager.instance.PlaySFX(2);

        ClosePanels();
        LoadDataItems(itemPanelsActive);

        itemPanelsActive = -1;

        foreach (GameObject itemPanel in itemPanels)
            itemPanel.SetActive(false);
    }

    private void OpenPanels()
    {
        panelObjects[0].SetActive(true);
        panelObjects[1].SetActive(true);

        itemPanel.SetActive(false);
    }

    private void ClosePanels()
    {
        panelObjects[0].SetActive(false);
        panelObjects[1].SetActive(false);

        itemPanel.SetActive(true);
    }

    private void CheckItemIsPurchased(int itemIndex)
    {
        if (itemIndex == -1)
        {
            purchaseText.text = purchasedLG;

            itemButtons[0].interactable = false;
            itemButtons[1].interactable = true;
        }

        else
        {
            if (!itemInfo[itemIndex].purchaseStatus)
            {
                purchaseText.text = itemInfo[itemIndex].price + "-" + purchaseLG;

                itemButtons[1].interactable = false;

                if (dataManage.GetData_Int(keyScore) < itemInfo[itemIndex].price)
                    itemButtons[0].interactable = false;

                else itemButtons[0].interactable = true;
            }

            else
            {
                purchaseText.text = purchasedLG;

                itemButtons[0].interactable = false;
                itemButtons[1].interactable = true;
            }
        }
    }

    public void PurchaseItem()
    {
        AudioManager.instance.PlaySFX(3);

        int currentScore = 0;
        int itemIndex = 0;

        switch (itemPanelsActive)
        {
            case 0:
                itemIndex = hatIndex;
                break;

            case 1:
                itemIndex = weaponIndex + 3;
                break;

            case 2:
                itemIndex = colorIndex + 6;
                break;
        }

        itemInfo[itemIndex].purchaseStatus = true;

        currentScore = dataManage.GetData_Int(keyScore) - itemInfo[itemIndex].price;

        CheckItemIsPurchased(itemIndex);

        DisplayScoreText(currentScore);
    }

    public void SaveItem()
    {
        AudioManager.instance.PlaySFX(4);

        switch (itemPanelsActive)
        {
            case 0:
                dataManage.SaveData_Int(keyHat, hatIndex);
                break;

            case 1:
                dataManage.SaveData_Int(keyWeapon, weaponIndex);
                break;

            case 2:
                dataManage.SaveData_Int(keyColor, colorIndex);
                break;
        }

        if (!equipPanelAnim.GetBool("Equipped"))
            equipPanelAnim.SetBool("Equipped", true);
    }

    public void ReturnMainMenu()
    {
        AudioManager.instance.PlaySFX(2);

        hugeData.SaveData(itemInfo);
        SceneManager.LoadScene(1);
    }







    // DATA ITEM

    private void LoadDataItems(int groupItemIndex)
    {
        switch (groupItemIndex)
        {
            case 0:
                HatData();
                break;

            case 1:
                WeaponData();
                break;

            case 2:
                ColorData();
                break;

            default:

                HatData();
                WeaponData();
                ColorData();
                break;
        }
    }

    private void HatData()
    {
        if (dataManage.GetData_Int(keyHat) == -1)
        {
            hatIndex = -1;

            foreach (GameObject hat in hats)
                hat.SetActive(false);

            foreach (Button itemButton in itemButtons)
                itemButton.interactable = false;

            itemText.text = "No Item";

            CheckItemIsPurchased(-1);
        }

        else
        {
            hatIndex = dataManage.GetData_Int(keyHat);
            hats[hatIndex].SetActive(true);

            itemText.text = itemInfo[hatIndex].itemName;

            CheckItemIsPurchased(hatIndex);
        }

        CheckHatButtons();
    }

    private void WeaponData()
    {
        if (dataManage.GetData_Int(keyWeapon) == -1)
        {
            weaponIndex = -1;

            foreach (GameObject weapon in weapons)
                weapon.SetActive(false);

            foreach (Button itemButton in itemButtons)
                itemButton.interactable = false;

            itemText.text = "No Item";

            CheckItemIsPurchased(-1);
        }

        else
        {
            weaponIndex = dataManage.GetData_Int(keyWeapon);
            weapons[weaponIndex].SetActive(true);

            itemText.text = itemInfo[weaponIndex + 3].itemName;

            CheckItemIsPurchased(weaponIndex + 3);
        }

        CheckWeaponButtons();
    }

    private void ColorData()
    {
        Material[] mats = rendererCharacter.materials;

        if (dataManage.GetData_Int(keyColor) == -1)
        {
            colorIndex = -1;

            foreach (Button itemButton in itemButtons)
                itemButton.interactable = false;

            colorIndex = -1;
            itemText.text = "No Item";

            mats[0] = colorOrigin;

            CheckItemIsPurchased(-1);
        }

        else
        {
            colorIndex = dataManage.GetData_Int(keyColor);
            mats[0] = colors[colorIndex];

            itemText.text = itemInfo[colorIndex + 6].itemName;

            CheckItemIsPurchased(colorIndex + 6);
        }

        rendererCharacter.materials = mats;

        CheckColorButtons();
    }

    // -------------- END --------------





    // DATA LANGUAGE

    private void LoadData()
    {
        hugeData.LoadData();

        itemInfo = hugeData.GetItemList();

        LoadLanguageData();
    }

    private void LoadLanguageData()
    {
        languagesData = hugeData.GetLanguageList();

        languageData = languagesData[1];
    }
}