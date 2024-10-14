using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Glass : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Transform glasses;

    [Header(" Settings ")]
    [SerializeField] private BonusType bonusType;
    [SerializeField] private int bonusAmount;

    private bool hasGateUsed;

    void Start()
    {
        ConfigureGlass();
    }

    private void ConfigureGlass()
    {
        switch (bonusType)
        {
            case BonusType.Addition:
                valueText.text = "+" + bonusAmount;
                break;

            case BonusType.Difference:
                valueText.text = "-" + bonusAmount;
                break;

            case BonusType.Product:
                valueText.text = "x" + bonusAmount;
                break;

            case BonusType.Division:
                valueText.text = "/" + bonusAmount;
                break;
        }
    }

    private void CloseGlasses()
    {
        for (int i = 0; i < glasses.childCount; i++)
        {
            if (glasses.GetChild(i) != null)
                glasses.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasGateUsed)
        {
            GameManager.instance.ApplyBonus(bonusType, bonusAmount);

            CloseGlasses();
            hasGateUsed = true;
        }
    }

    public int GetBonusAmount()
    {
        return bonusAmount;
    }

    public BonusType GetBonusType()
    {
        return bonusType;
    }
}