using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BonusLibrary;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header(" Elements ")]
    [SerializeField] private AudioSource[] musics;
    [SerializeField] private AudioSource[] sfx;

    [Header(" Settings ")]
    [SerializeField] private int levelMusicToPlay;

    private const string keyMusicVol = "MenuVol";
    private const string keySfxVol = "SfxVol";
    private const string keyGameplayVol = "GameVol";

    [Header("--------  Library --------")]
    DataManage dataManage = new DataManage();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    void Start()
    {
        musics[0].volume = dataManage.GetData_Float(keyMusicVol);

        for (int i = 0; i < sfx.Length; i++)
            sfx[i].volume = dataManage.GetData_Float(keySfxVol);
    }

    public void SetMenuVolume()
    {
        musics[0].volume = dataManage.GetData_Float(keyMusicVol);
    }

    public void SetSfxVolume()
    {
        for (int i = 0; i < sfx.Length; i++)
            sfx[i].volume = dataManage.GetData_Float(keySfxVol);
    }

    public void SetGameplayVolume()
    {
        musics[1].volume = dataManage.GetData_Float(keyGameplayVol);
    }

    public void PlayMusic(int musicToPlay)
    {
        for (int i = 0; i < musics.Length; i++)
            musics[i].Stop();

        musics[musicToPlay].Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }

    public AudioSource[] GetMusics()
    {
        return musics;
    }
}
