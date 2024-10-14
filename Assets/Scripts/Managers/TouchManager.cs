using BonusLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchManager : MonoBehaviour
{
    public static TouchManager instance;

    [Header(" Elements ")]
    [SerializeField] private Fader fader;

    [Header(" Settings ")]
    private const string keyLevel = "LevelKey";

    public bool isTouch;

    [Header(" Library ")]
    DataManage dataManage = new DataManage();

    void Awake()
    {
        if (instance == null)
            instance = this;

        else Destroy(gameObject);

        dataManage.CheckCurrentLevel(keyLevel);
    }

    void Start()
    {
        AudioManager.instance.PlayMusic(0);
    }

    public void OpenMenuScreen()
    {
        if (!isTouch) return;

        StartCoroutine(WaitMainMenu());

        AudioManager.instance.PlaySFX(2);
    }

    IEnumerator WaitMainMenu()
    {
        yield return fader.FadeOutCo(2f);

        MainMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
