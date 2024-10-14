using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header(" UI ")]
    [SerializeField] private Button thisButton;
    [SerializeField] private Image stageLockImage;

    public void Congigure(Sprite levelBGSprite, Sprite stageLockSprite, bool unlocked)
    {
        thisButton.GetComponent<Image>().sprite = levelBGSprite;
        stageLockImage.sprite = stageLockSprite;
        thisButton.interactable = unlocked;

        if (!unlocked)
        {
            stageLockImage.rectTransform.offsetMin = Vector2.zero;
            stageLockImage.rectTransform.offsetMax = Vector2.zero;
        }
    }

    public Button GetButton()
    {
        return thisButton;
    }
}
